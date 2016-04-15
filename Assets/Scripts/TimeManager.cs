using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public static TimeManager Instance;

	private bool isGoingOn;

	void Start () {
		if(Instance == null){
			Instance = this;
		}else{
			Destroy(this);
		}
		isGoingOn = true;
	}

	void Update () {
	
	}

	public void SwitchTime(){
		if(isGoingOn){
			StopTime(); 
		}else{ 
			NormalTime();
		}
	}

	public void StopTime(){
		Time.timeScale = 0;
		isGoingOn = false;
	}

	public void NormalTime(){
		Time.timeScale = 1;
		isGoingOn = true;
	}

	public void DoubleTime(){
		Time.timeScale = Time.timeScale * 2;
	}

	public void HalfTime(){
		Time.timeScale = Time.timeScale / 2;
	}

	public void SlowmoTime(){
		Time.timeScale = 0.2f;
	}
}
