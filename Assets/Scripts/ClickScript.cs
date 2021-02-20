using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickScript : MonoBehaviour {
  #region assigned in editor
  [Header("Script variables")]
  [SerializeField] [Tooltip("Current Map")]
  Tilemap map;
  [SerializeField] [Tooltip("Tower to be built")]
  GameObject tower;
  #endregion

  List<GameObject> towers = new List<GameObject>();
  int totalTowers = 0;
  int towerLimit = 10;

  void Update () {
    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
    Vector3 worldPoint = r.GetPoint(-r.origin.z / r.direction.z);
    
    Vector3Int tilePos = map.WorldToCell(worldPoint);
    if (Input.GetMouseButtonDown (0)) {
      string tileName = map.GetTile(tilePos).name;
      if(tileName == "towerDefense_tilesheet_24" && totalTowers < towerLimit){
        BuildTower(tilePos);
      }
    }
  }

  void BuildTower (Vector3 p) {
    bool canBuild = true;
    foreach (GameObject t in towers)
    {
        if(t.transform.position == p){
          canBuild = false;
        }
    }
    if(canBuild){
      //instantiate a tower at position p and add it to the list
      GameObject t = Instantiate(tower);
      t.transform.position = p + new Vector3(0.5f,0.5f,0);
      towers.Add(t);
      totalTowers++;
    }
  }
}