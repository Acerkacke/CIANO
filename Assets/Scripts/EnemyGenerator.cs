using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour, IDamageable {
	public GameObject enemyPrefab;
	private float t;
	public int health = 150;
	public float spawnTime = 8f;

	void Start () {
		t = 0f;

	}
	
	void Update () {
		t += Time.deltaTime;
		if (t >= spawnTime) {
			t = 0f;
			Instantiate(enemyPrefab, transform.position + transform.forward * 2, Quaternion.identity);
		}
	}

	public void Damage(int i){
		health -= i;
		if (health <= 0) {
			Player.Instance.addScore(Random.Range(45,55));
			Destroy(this.gameObject);
		}
	}
}
