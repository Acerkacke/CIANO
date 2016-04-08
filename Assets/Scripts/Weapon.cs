using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	private LineRenderer ln;
	private bool isReloading = false;
	private float t;
	private float reloadTime;

	public bool IsReloading{
		get { return isReloading; }
	}

	void Start(){
		ln = gameObject.GetComponent<LineRenderer> ();
		ln.enabled = false;
	}

	void Update(){
		if (isReloading) {
			t += Time.deltaTime;
			if(t > reloadTime)
				isReloading = false;
			if(ln.enabled == true && t > 0.05f)
				ln.enabled = false;
		}
	}

	public void ShootOne(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 100)) {
			IDamageable d = hit.collider.gameObject.GetComponent<IDamageable>();
			if(d != null){
				d.Damage(Random.Range(10, 15));
			}
		}
		t = 0;
		reloadTime = 0.1f;
		ln.SetColors (Color.yellow, Color.yellow);
		ln.enabled = true;
		isReloading = true;
	}

	public void ShootTwo(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 100)) {
			IDamageable d = hit.collider.gameObject.GetComponent<IDamageable>();
			if(d != null){
				d.Damage(Random.Range(60, 80));
			}
		}
		t = 0;
		reloadTime = 0.5f;
		ln.SetColors (Color.blue, Color.blue);
		ln.enabled = true;
		isReloading = true;
	}

}
