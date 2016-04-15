using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SlowUpdateCaller : MonoBehaviour {

	public int everyHowManyFrames = 15;
	public int timesSlowerUpdate = 2;
	public int timesSlowestUpdate = 2;
	public bool usesFixedUpdate = false;
	private List<ISlowUpdate> SlowUpdatecalls = new List<ISlowUpdate>();
	private List<ISlowerUpdate> SlowerUpdatecalls = new List<ISlowerUpdate>();
	private List<ISlowestUpdate> SlowestUpdatecalls = new List<ISlowestUpdate>();
	private List<ISecondUpdate> SecondUpdatecalls = new List<ISecondUpdate>();

	private int nextSlowUpdateFrameCheck;
	private int nextSlowerUpdateFrameCheck;
	private int nextSlowestUpdateFrameCheck;
	private int frames = 1;
	public static SlowUpdateCaller Instance;

	void Start () {
		if(Instance == null){
			Instance = this;
		}else{
			Destroy(gameObject);
			//Debug.Log("SlowUpdateCaller - Destroyed");
		}
		getSlowUpdateScripts();
		getSlowerUpdateScripts();
		getSlowestUpdateScripts();
		getSecondUpdateScripts ();

		DontDestroyOnLoad(this);
	}

	void getSlowUpdateScripts(){
		SlowUpdatecalls.Clear();
		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()){
			if(go.GetComponent<ISlowUpdate>() != null){
				SlowUpdatecalls.Add(go.GetComponent<ISlowUpdate>());
			}
		}
	}

	void getSlowerUpdateScripts(){
		SlowerUpdatecalls.Clear();
		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()){
			if(go.GetComponent<ISlowerUpdate>() != null){
				SlowerUpdatecalls.Add(go.GetComponent<ISlowerUpdate>());
			}
		}
	}

	void getSlowestUpdateScripts(){
		SlowestUpdatecalls.Clear();
		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()){
			if(go.GetComponent<ISlowestUpdate>() != null){
				SlowestUpdatecalls.Add(go.GetComponent<ISlowestUpdate>());
			}
		}
	}

	void getSecondUpdateScripts(){
		SecondUpdatecalls.Clear();
		foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>()){
			if(go.GetComponent<ISecondUpdate>() != null){
				SecondUpdatecalls.Add(go.GetComponent<ISecondUpdate>());
			}
		}
	}

	void Update () {
		if(!usesFixedUpdate){
			callSlowUpdate();
			callSlowerUpdate();
			callSlowestUpdate();
			callSecondUpdate();
			frames++;
		}
	}

	void FixedUpdate(){
		if(usesFixedUpdate){
			callSlowUpdate();
			callSlowerUpdate();
			callSlowestUpdate();
			callSecondUpdate();
			frames++;
		}
	}

	void callSlowUpdate(){
		if(SlowUpdatecalls.Count > 0){
			if(frames >= nextSlowUpdateFrameCheck){
				nextSlowUpdateFrameCheck = frames + everyHowManyFrames;
				foreach(ISlowUpdate call in SlowUpdatecalls){
					call.SlowUpdate();
				}
			}
		}
	}

	void callSlowerUpdate(){
		if(SlowerUpdatecalls.Count > 0){
			if(frames >= nextSlowerUpdateFrameCheck){
				nextSlowerUpdateFrameCheck = frames + everyHowManyFrames * timesSlowerUpdate;
				foreach(ISlowerUpdate call in SlowerUpdatecalls){
					call.SlowerUpdate();
				}
			}
		}
	}
	void callSlowestUpdate(){
		if(SlowestUpdatecalls.Count > 0){
			if(frames >= nextSlowestUpdateFrameCheck){
				nextSlowestUpdateFrameCheck = frames + everyHowManyFrames * timesSlowerUpdate * timesSlowestUpdate;
				foreach(ISlowestUpdate call in SlowestUpdatecalls){
					call.SlowestUpdate();
				}
			}
		}
	}

	void callSecondUpdate(){
		if(SecondUpdatecalls.Count > 0){
			if(Time.time % 1 == 0){
				foreach(ISecondUpdate call in SecondUpdatecalls){
					call.SecondUpdate();
				}
			}
		}
	}

	void OnLevelWasLoaded(int level){
		Debug.Log("Loaded level " + level);
		getSlowUpdateScripts();
		getSlowerUpdateScripts();
		getSlowestUpdateScripts();
	}
}
