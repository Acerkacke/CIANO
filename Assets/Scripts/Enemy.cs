using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable {
	private int health;
	public int Health{
		get{
			return health;
		}
		protected set{
			health = value;
		}
	}
	public float minDistance = 3.5f;
	public int initialHealth = 40;
	public float speed = 5;
	public int damage = 10;
	private GameObject playerGo;
	private Player player;
	private bool canMove = true;

	void Start () {
		health = initialHealth;
		playerGo = GameObject.FindGameObjectWithTag("Player");
		player = Player.Instance;
	}
	
	void Update () {
		if (canMove) {
			if (Vector3.Distance (gameObject.transform.position, playerGo.transform.position) > minDistance)
				transform.Translate ((playerGo.transform.position - transform.position).normalized * Time.deltaTime * speed);
			else {
				player.Damage (Random.Range (damage-5, damage+5));
				StartCoroutine(wait(1));
			}
		}
	}

	public void Damage(int i){
		health -= i;
		if (health <= 0) {
			player.addScore (5);
			Destroy(this.gameObject);
		}
	}
	private IEnumerator wait(float t){
		canMove = false;
		yield return new WaitForSeconds (t);
		canMove = true;
	}
}
