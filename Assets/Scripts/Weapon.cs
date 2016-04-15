using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	private LineRenderer lineRend;

	public int maxDistance = 50;
	public float normalReloadingTime = 0.1f;
	public float specialReloadingTime = 0.5f;
	public float lineRendTimeToDie = 0.05f;
	public float imprecision = 1f;
	private float nextLineRendDie;
	private float nextReloadTime;

	private Vector3 miraPos = new Vector3 (0, -0.3f, 1);
	private Vector3 noMiraPos = new Vector3 (0.5f, -0.4f, 1);

	private bool mira = false;
	public bool Mira{
		get{ return mira; }
		set{ mira = value; OnMiraChanged(); }
	}

	private bool isReloading = false;
	public bool IsReloading{
		get { return isReloading; }
	}
	
	void Start(){
		if(GetComponent<LineRenderer>()){
			lineRend = GetComponent<LineRenderer> ();
			lineRend.enabled = false;
		}else{
			Debug.LogError("Weapon - MISSING LINE RENDERER");
		}

	}

	void Update(){
		//IF IT'S RELOADING
		if (isReloading) {
			//IF IT'S PASSED ENOUGH TIME
			if(Time.time > nextReloadTime){
				isReloading = false; //WE'RE NOT RELOADING ANYMORE
			}
			//IF THE LINE RENDERER IS ACTIVE AND ENOUGH TIME PASSED
			if(lineRend.enabled && Time.time > nextLineRendDie)
				lineRend.enabled = false;
		}


	}

	private void OnMiraChanged(){
		if (mira) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, miraPos, Time.deltaTime * 10);
		} else {
			transform.localPosition = Vector3.Lerp(transform.localPosition, noMiraPos, Time.deltaTime * 10);
		}
	}

	public void Shoot(int weaponID){
		if(!isReloading){
			int randDmg = 0;

			switch(weaponID){
			case 1:
				 randDmg = Random.Range(10, 15);
				nextReloadTime = Time.time + normalReloadingTime;
				lineRend.SetColors (Color.yellow, Color.yellow);
				break;
			case 2:
				randDmg = Random.Range(100, 150);
				nextReloadTime = Time.time + specialReloadingTime;
				lineRend.SetColors (Color.blue, Color.blue);
				break;
			default:
				Debug.LogError("Weapon - WEAPON ID NOT FOUND : " + weaponID);
				break;

			}

			RaycastHit hit;
			if (Physics.Raycast (transform.position,transform.forward, out hit)) {
				if(hit.transform != transform.root){
					if(hit.transform.GetComponent<IDamageable>() != null){
						IDamageable damageableObj = hit.transform.GetComponent<IDamageable>();
						damageableObj.Damage(randDmg);
					}
				}
				lineRend.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)));
			}else{
				lineRend.SetPosition(1, -(transform.position + transform.forward * 50));
			}

			nextLineRendDie = Time.time + lineRendTimeToDie;
			lineRend.enabled = true;
			isReloading = true;
		}
	}

}
