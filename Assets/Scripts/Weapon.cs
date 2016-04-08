using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	private LineRenderer lineRend;

	public float normalReloadingTime = 0.1f;
	public float specialReloadingTime = 0.5f;
	private float nextReloadTime;

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
		//SE STA RICARICANDO
		if (isReloading) {
			//IF IT'S PASSED ENOUGH TIME
			if(Time.time > nextReloadTime){
				isReloading = false; //WE'RE NOT RELOADING ANYMORE
			}
			//IF THE LINE RENDERER IS ACTIVE AND ENOUGH TIME PASSED
			if(lineRend.enabled && Time.time > nextReloadTime-0.07f)
				lineRend.enabled = false;
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
				randDmg = Random.Range(25, 40);
				nextReloadTime = Time.time + specialReloadingTime;
				lineRend.SetColors (Color.blue, Color.blue);
				break;
			default:
				Debug.LogError("Weapon - WEAPON ID NOT FOUND : " + weaponID);
				break;

			}

			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit, 100)) {
				if(hit.transform.GetComponent<IDamageable>() != null){
					IDamageable damageableObj = hit.transform.GetComponent<IDamageable>();
					damageableObj.Damage(randDmg);
				}
			}
			lineRend.enabled = true;
			isReloading = true;
		}
	}

}
