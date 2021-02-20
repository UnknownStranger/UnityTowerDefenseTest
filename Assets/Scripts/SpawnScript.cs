using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SpawnScript : MonoBehaviour {
  [SerializeField]
  GameObject enemy, spawnMarker, goalMarker;

  Vector2 spawnPosition;
  int waveSize = 100, enemiesSpawned = 0;

  List<GameObject> enemyList = new List<GameObject> ();
  float spawnTimer = 0;

  void Start () {
    spawnPosition = spawnMarker.transform.position;
    // SpawnEnemy ();
  }

  void FixedUpdate () {
    spawnTimer += Time.deltaTime;
    UpdateEnemies ();
  }

  void UpdateEnemies () {
    if (spawnTimer > 0.5f && enemiesSpawned < waveSize) {
      enemiesSpawned++;
      SpawnEnemy ();
    }

    // enemyList = (from e in enemyList where e != null select e).ToList ();
    enemyList = enemyList.Where(e => e != null).ToList();
    foreach (var e in enemyList) {
      Vector3 distance = e.transform.position - goalMarker.transform.position;
      if (Math.Abs (distance.x) < 1 && Math.Abs (distance.y) < 1) {
        DeleteEnemy (e);
      }
    };
  }

  void SpawnEnemy () {
    spawnTimer = 0;
    GameObject newEnemy = Instantiate (enemy);

    newEnemy.transform.position = spawnPosition;
    enemyList.Add (newEnemy);
  }

  void DeleteEnemy (GameObject e) {
    int indexOfEnemy = enemyList.IndexOf (e);
    UnityEngine.Object.Destroy (enemyList[indexOfEnemy]);
    enemyList.RemoveAt (indexOfEnemy);
  }
}