using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour, IDamageable {
	public GameObject enemy;
	private float t;
	public int health = 150;

	void Start () {
		t = 0f;

	}
	
	void Update () {
		t += Time.deltaTime;
		if (t > 4) {
			t = 0f;
			Instantiate(enemy, transform.position + transform.forward * 2, Quaternion.identity);
		}
	}

	public void Damage(int i){
		health -= i;
		if (health < 1) {
			Player.addScore(56);
			Destroy(this.gameObject);
		}
	}
}
