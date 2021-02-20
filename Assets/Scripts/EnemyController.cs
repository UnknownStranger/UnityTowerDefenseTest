using System;

using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour {
  Vector2 currentPosition;
  //Enemy is in prefab facing to the right of the play screen as the starting angle.
  Vector3 facingAngle;
  string pathTileName = "towerDefense_tilesheet_93";
  Vector2 vector = new Vector2 (0, 0);
  float speed = 0.05f;
  int health = 20;
  Vector3 sunPos = new Vector3(-11f, 7f, 0f);

  public float distanceTraveled{ private set; get; }

  [SerializeField]
  Tilemap map;
  [SerializeField]
  GameObject shadow;

  void OnTriggerEnter2D(Collider2D other) {
    if(other.tag.ToLower() == "projectile"){
      this.health--;
      Destroy(other.gameObject);
    }
  }

  void Start () {
    map = GameObject.Find ("/Grid/Tilemap").GetComponent<Tilemap> ();
    currentPosition = this.transform.position;
    distanceTraveled = 0f;
  }

  void FixedUpdate () {
    MoveEnemy ();
    if(this.health <=0){
      Destroy(this.gameObject);
    }
  }

  void MoveEnemy () {
    if (!CheckPosition ()) {
      AdjustAngle ();
    }

    currentPosition += vector * speed;
    distanceTraveled += speed;
    this.transform.SetPositionAndRotation (currentPosition, Quaternion.Euler (facingAngle));
    Vector3 temp = (this.transform.position - sunPos);
    temp.Normalize();
    var sPos = new Vector2(temp.x, temp.y);
    shadow.transform.SetPositionAndRotation(currentPosition + sPos * .25f,  Quaternion.Euler (facingAngle));
  }

  bool CheckPosition () {
    facingAngle = this.transform.rotation.eulerAngles;

    //Vector3Int checkPosition = new Vector3Int ((int) (currentPosition.x - vector.x * 0.5f), (int) (currentPosition.y - vector.y * 0.5f), 0);
    Vector3Int checkPosition = Vector3Int.FloorToInt(currentPosition - vector * 0.5f);

    #region Checking facing angle and determining position of tile to check
    if (facingAngle.z == 0) {
      //facing right
      checkPosition += Vector3Int.right;
      vector = Vector2.right;
    } else if (facingAngle.z == 270 || facingAngle.z == -90) {
      //facing down
      checkPosition += Vector3Int.down;
      vector = Vector2.down;
    } else if (facingAngle.z == 180) {
      //facing left
      checkPosition += Vector3Int.left;
      vector = Vector2.left;
    } else if (facingAngle.z == 90 || facingAngle.z == -270) {
      //facing up
      checkPosition += Vector3Int.up;
      vector = Vector2.up;
    }
    #endregion
    return map.GetTile (checkPosition).name == pathTileName;
  }

  void AdjustAngle () {
    float rotation = (this.transform.rotation.eulerAngles.z - 90) % 360;
    this.transform.rotation = Quaternion.Euler (0f, 0f, rotation);
    if (!CheckPosition ()) {
      rotation = (rotation - 180) % 360;
      this.transform.rotation = Quaternion.Euler (0f, 0f, rotation);
      CheckPosition ();
    }
  }
}