using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable {
	int health = 40;
	public float speed = 5;
	private GameObject player;
	private bool isMoving = true;

	void Start () {
		player = GameObject.Find ("Player");
	}
	
	void Update () {
		if (isMoving) {
			if (Vector3.Distance (this.gameObject.transform.position, player.transform.position) > 5)
				transform.Translate (-(transform.position - player.transform.position).normalized * Time.deltaTime * speed);
			else {
				player.GetComponent<IDamageable> ().Damage (Random.Range (5, 13));
				StartCoroutine(wait(1));
			}
		}
	}

	public void Damage(int i){
		health -= i;
		if (health <= 0) {
			//transform.position = new Vector3(0, 1, 0);
			//health = 40;
			Player.addScore (5);
			Destroy(this.gameObject);
		}
	}
	private IEnumerator wait(float t){
		isMoving = false;
		yield return new WaitForSeconds (t);
		isMoving = true;
	}
}
