using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTouchHazard : MonoBehaviour {

    public LevelManager level;
    public bool hazardous = true;

	// Use this for initialization
	void Start () {
        level = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collided)
    {
        if (collided.name == "Player" && hazardous)
        {
            HitSpikes();
        }
    }

    void HitSpikes()
    {
        StartCoroutine(level.RespawnPlayer());
    }
}
