using UnityEngine;
using System.Collections;

public class Granate_Launchr : MonoBehaviour {
	public int granatesQta;
	public GameObject granata;
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.G) && granatesQta > 0) {
			GameObject g = Instantiate(granata, transform.position + transform.forward * 2, Quaternion.identity) as GameObject;
			g.GetComponent<Granata>().Set(transform.forward, 3, 5, 40);
			granatesQta --;
		}
	}
}
