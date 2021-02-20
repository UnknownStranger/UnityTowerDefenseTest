using System.Collections.Generic;

using UnityEngine;

public class TowerController : MonoBehaviour {

  [SerializeField]
  GameObject barrelLeft, barrelRight, muzzleFlash, projectile;

  List<GameObject> inRange = new List<GameObject> ();
  GameObject target;

  float updateTime = 0f;
  float firingDelay = 0f;
  float muzzleTimer = 0f;
  SpriteRenderer flashRenderer;
  bool shootLeft = true;

  void OnTriggerEnter2D (Collider2D other) {
    if (other.tag.ToLower () == "enemy") {
      if (target == null) {
        target = other.gameObject;
      }
      inRange.Add (other.gameObject);
    }
  }

  void OnTriggerExit2D (Collider2D other) {
    if (other.tag.ToLower () == "enemy") {
      inRange.Remove (other.gameObject);
      UpdateTarget ();
    }
  }

  void Start () {
    flashRenderer = muzzleFlash.GetComponent<SpriteRenderer> ();
  }

  void Update () {
    if (target != null) {
      FaceTarget (target);
      if (firingDelay <= 0) {
        Attack (target);
        firingDelay = 0.2f;
      } else {
        firingDelay -= Time.deltaTime;
      }
    }

    if (muzzleTimer >= 0) {
      flashRenderer.enabled = true;
      muzzleTimer -= Time.deltaTime;
    } else {
      flashRenderer.enabled = false;
    }
  }

  void UpdateTarget () {
    if (inRange.Count > 0) {
      target = inRange[inRange.Count - 1];
      for (int i = inRange.Count - 1; i > 0; i--) {
        GameObject e = inRange[i];
        if (e != null && GetDistanceTraveled (e) > GetDistanceTraveled (target)) {
          target = e;
        } else if (e == null) {
          inRange.RemoveAt (i);
        }
      }
    } else { target = null; }
  }

  float GetDistanceTraveled (GameObject e) {
    return e.GetComponent<EnemyController> ().distanceTraveled;
  }

  void FaceTarget (GameObject t) {
    Vector3 dir = t.transform.position - this.transform.position;
    float offset = -90;
    float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + offset;
    this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
  }

  void Attack (GameObject t) {
    if (shootLeft) {
      ShootLeft ();
      shootLeft = !shootLeft;
    } else {
      ShootRight ();
      shootLeft = !shootLeft;
    }
  }

  void ShootLeft () {
    muzzleFlash.transform.position = barrelLeft.transform.position;
    muzzleTimer = 0.05f;
    SpawnProjectile (barrelLeft.transform.position);
  }
  void ShootRight () {
    muzzleFlash.transform.position = barrelRight.transform.position;
    muzzleTimer = 0.05f;
    SpawnProjectile (barrelRight.transform.position);
  }

  void SpawnProjectile (Vector3 p) {
    GameObject t = Instantiate (projectile);
    t.transform.position = p;
    t.GetComponent<ProjectileScript> ().target = target;
  }
}