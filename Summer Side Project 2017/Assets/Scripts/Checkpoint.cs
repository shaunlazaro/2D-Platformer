﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public LevelManager level;

	// Use this for initialization
	void Start () {
        level = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.name == "Player")
        {
            level.currentCheckpoint = this.gameObject;
        }
    }

}
