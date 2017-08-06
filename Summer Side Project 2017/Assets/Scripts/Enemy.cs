using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp;
    bool dead = false;

    private ParticleController particle;

	// Use this for initialization
	void Start () {
        particle = FindObjectOfType<ParticleController>();
	}

    public void GetHit(int damage)
    {
        hp = hp - damage;
        if (hp < 1) hp = 0;
        // Update HP Bar.

        if (hp == 0 && !dead)
        {
            StartCoroutine(Die());
            dead = true;
        }
    }

    IEnumerator Die()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<DeathTouchHazard>().hazardous = false;
        particle.DeathParticleRelease(this.gameObject, true);
        yield return new WaitForSeconds(particle.deathParticle.GetComponent<ParticleSystem>().main.duration);
        DestroyObject(gameObject);

    }
}
