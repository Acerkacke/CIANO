using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SlowUpdateCaller : MonoBehaviour {

	public int everyHowManyFrames;
	public bool usesFixedUpdate = false;
	private List<ISlowUpdate> calls = new List<ISlowUpdate>();
	private int frames;

	void Start () {
		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()){
			if(go.GetComponent<ISlowUpdate>() != null){
				calls.Add(go.GetComponent<ISlowUpdate>());
			}
		}
	}

	void Update () {
		if(!usesFixedUpdate){
			if(calls.Count > 0){
				if(frames % everyHowManyFrames == 0){
					foreach(ISlowUpdate call in calls){
						call.SlowUpdate();
					}
				}
				frames++;
			}
		}
	}

	void FixedUpdate(){
		if(usesFixedUpdate){
			if(calls.Count > 0){
				if(frames % everyHowManyFrames == 0){
					foreach(ISlowUpdate call in calls){
						call.SlowUpdate();
					}
				}
				frames++;
			}
		}
	}
}
