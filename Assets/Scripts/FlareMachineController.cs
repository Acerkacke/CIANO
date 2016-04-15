using UnityEngine;
using System.Collections;

public class FlareMachineController : MonoBehaviour {

	private MachineController controller;
	public LensFlare flareLight;
	void Start(){
		if(controller == null){
			controller = GetComponent<MachineController>();
		}
		controller.AddOnStateChanged(OnStateChanged);
		flareLight.color = Color.green;
	}

	void OnStateChanged(MachineController.MachineState state){
		switch(state){
		case MachineController.MachineState.Patrolling:
			flareLight.color = Color.green;
			break;
		case MachineController.MachineState.Alert:
			flareLight.color = Color.yellow;
			break;
		case MachineController.MachineState.Attack:
			flareLight.color = Color.red;
			break;
		}
	}

}
