using UnityEngine;
using System.Collections;

public class MenuButtonManager : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
	
	}

	public void LoadLevel(string levelName){
		Application.LoadLevel(levelName);
	}

	public void Exit(){
		Application.Quit();
	}
}
