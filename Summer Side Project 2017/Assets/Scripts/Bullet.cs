using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Allows user to aim and select starting point.
    public void Fire(Vector2 startingPoint, Vector2 destination, float speed)
    {
        float distanceToX = destination.x - startingPoint.x;
        float distanceToY = destination.y - startingPoint.y;
        float totalDistance = Mathf.Abs(distanceToX) + Mathf.Abs(distanceToY);

        float xPercent = distanceToX / totalDistance;
        float yPercent = distanceToY / totalDistance;

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * xPercent, speed * yPercent);
    }

    // Moves in a straight line from spawn.
    public void Fire(float speed)
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }

    void OnBecameInvisible()
    {
        DestroyObject(gameObject);
    }
}
