using UnityEngine;

public class ProjectileScript : MonoBehaviour{
  public GameObject target;
  float speed = 0.75f;
  Vector3 moveVector;

  void Start() {
    if(target != null){
      Vector3 targetPos = target.transform.position;
      moveVector = targetPos - this.transform.position;
      moveVector = moveVector.normalized * speed;
    }
  }

  void FixedUpdate() {
    this.transform.position += moveVector;
  }
}
