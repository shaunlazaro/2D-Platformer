using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckpoint;
    private PlayerController player;
    private ParticleController particle;

    public float delayBeforeRespawn;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        particle = FindObjectOfType<ParticleController>();
	}
	
    public IEnumerator RespawnPlayer()
    {
        particle.DeathParticleRelease(player.gameObject);
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.ResetShootingCooldown();
        yield return new WaitForSeconds(delayBeforeRespawn);
        player.enabled = true;
        player.GetComponent<Renderer>().enabled = true;
        player.transform.position = currentCheckpoint.transform.position;

        particle.RespawnParticleRelease(player);
        yield break;
    }

}
