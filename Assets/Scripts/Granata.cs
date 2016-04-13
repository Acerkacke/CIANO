using UnityEngine;
using System.Collections;

public class Granata : MonoBehaviour {
	private Rigidbody rb;
	private float timeToExplode;
	private float t;
	private int damage;
	private int radius;
	public int launchForce;
	
	void Update () {
		t += Time.deltaTime;
		if (t > timeToExplode) {
			Collider[] hitted = Physics.OverlapSphere(transform.position, radius);
			foreach(Collider obj in hitted){
				IDamageable d = obj.gameObject.GetComponent<IDamageable>();
				if(d != null){
					d.Damage(damage);
				}
			}
			Destroy(this.gameObject);
		}
	}

	public void Set(Vector3 dir, float time, int r, int dam){
		rb = gameObject.GetComponent<Rigidbody> ();
		timeToExplode = time;
		damage = dam;
		radius = r;
		Debug.Log (dir);
		rb.AddForce (dir.x * launchForce, (dir.y + 0.5f) * launchForce, dir.z * launchForce, ForceMode.Impulse);
	}

	public float TimeToExplode{
		get { return timeToExplode; }
	}
}
