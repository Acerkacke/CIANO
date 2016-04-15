using UnityEngine;
using System.Collections;

public class GameCanvasController : MonoBehaviour {

	public static GameCanvasController GCC;

	private TimeManager timeManager;

	// Use this for initialization
	void Start () {
		if(GCC == null){
			GCC = this;
		}else{
			Destroy(this);
		}
		timeManager = TimeManager.Instance;
	}
	

}
