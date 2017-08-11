using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour {

    public GameObject enemyToSpawn;
    public int localScaleX;

	// Use this for initialization
	void Start () {
        SpawnEnemy(localScaleX);
	}

    public void SpawnEnemy(int localScalex = 1)
    {
        GameObject enemy = Instantiate(enemyToSpawn, this.gameObject.transform);
        enemy.transform.localScale = new Vector3(localScalex, 1, 1);
    }
}
