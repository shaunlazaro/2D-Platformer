using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    public GameObject deathParticle;
    public GameObject respawnParticle;
    public GameObject jumpParticle;
    public GameObject bulletHitParticle;
    public GameObject bulletExplodeParticle;
    
    public GameObject dashFinishParticle;

    public void DeathParticleRelease(GameObject deadObject, bool isEnemy)
    {
        //  TODO:
        //  - Do particle effects for dead NPC.
        //  - Do particle effects for dead enemy.
        //
        Instantiate(deathParticle, deadObject.transform.position, deathParticle.transform.rotation);
    }
    public void DeathParticleRelease(PlayerController deadPlayer)
    {
        Instantiate(deathParticle, deadPlayer.transform.position, deathParticle.transform.rotation);
    }

    //Used as a spawn animation for NPCs.
    public void RespawnParticleRelease(GameObject deadObject, bool isEnemy)
    {

        //  TODO:
        //  - Do particle effects for spawning enemy.
        //  - Do particle effects for spawning NPC.
        //  ~~~

        if (isEnemy)
            Instantiate(respawnParticle, deadObject.transform.position, respawnParticle.transform.rotation);
        else
            Instantiate(respawnParticle, deadObject.transform.position, respawnParticle.transform.rotation);
    }
    //Overload specifically for the player.
    public void RespawnParticleRelease(PlayerController deadPlayer)
    {
        Instantiate(respawnParticle, deadPlayer.transform.position, respawnParticle.transform.rotation);
    }

    public void JumpParticleRelease(Transform groundCheck)
    {
        Debug.Log("GroundCheckPosition: " + groundCheck.position);
        Instantiate(jumpParticle, groundCheck.position, jumpParticle.transform.rotation);
    }
    public void JumpParticleRelease(PlayerController player)
    {
        Instantiate(jumpParticle, player.transform.position, jumpParticle.transform.rotation);
    }

    public void BulletHitParticleRelease(GameObject thingHit)
    {
        Instantiate(bulletHitParticle, thingHit.transform.position, bulletHitParticle.transform.rotation);
    }

    public void BulletExplodeParticleRelease(GameObject thingHit)
    {
        Instantiate(bulletExplodeParticle, thingHit.transform.position, bulletExplodeParticle.transform.rotation);
    }


    public void DashParticleRelease (Vector3 endingPosition)
    {
        Instantiate(dashFinishParticle, endingPosition, dashFinishParticle.transform.rotation);
    }
}
