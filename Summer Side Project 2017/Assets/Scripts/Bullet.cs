using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private bool isColliding = false;
    public int bulletDamage;

    void Start()
    {
        bulletDamage = FindObjectOfType<PlayerStatsManager>().BulletDamage;
    }

    // Allows user to aim and select starting point.
    public void Fire(Vector2 startingPoint, Vector2 destination, float desiredMagnitude)
    {
        float xLength = destination.x - startingPoint.x;
        float yLength = destination.y - startingPoint.y;
        float currentMagnitude = Mathf.Sqrt(xLength * xLength + yLength * yLength);

        xLength = xLength / currentMagnitude * desiredMagnitude;
        yLength = yLength / currentMagnitude * desiredMagnitude;

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(xLength,yLength);
    }

    // Moves in a straight line from spawn.
    public void Fire(float speed)
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

    void OnBecameInvisible()
    {
        DestroyObject(gameObject, gameObject.GetComponent<TrailRenderer>().time);
    }

    void OnTriggerEnter2D(Collider2D thingHit)
    {
        // Exits if the bullet already hit something or passes through another bullet.
        if (isColliding || thingHit.gameObject.GetComponent<Bullet>() != null) return;
        isColliding = true;
        
        if(thingHit.tag == "Enemy")
        {
            thingHit.gameObject.GetComponent<Enemy>().GetHit(bulletDamage);
        }

        if (thingHit.tag != "Pass Through")
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            DestroyObject(gameObject, gameObject.GetComponent<TrailRenderer>().time);
            GameObject.FindGameObjectWithTag("Level Manager").GetComponent<ParticleController>().BulletHitParticleRelease(gameObject);
        }
        else
            isColliding = false;
    }
}
