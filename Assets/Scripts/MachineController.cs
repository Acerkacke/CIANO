using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MachineController : MonoBehaviour,IDamageable{

	public enum MachineState
	{
		Resting,
		Patrolling,
		Alert,
		Attack
	}
	public GameObject testa;
	//NAV MESH AGENT
	private NavMeshAgent NMA;
	//PATROLLING (sorvegliare)
	public float restingTime = 5;
	private float nextMove;
	//PLAYER
	public GameObject player;
	//STATE
	private MachineState state;
	public MachineState State{
		get{
			return state;
		}
		protected set{
			state = value;
			if(OnMachineStateChanged != null){
				OnMachineStateChanged(state);
			}
			OnStateChanged(value);
		}
	}
	public MachineState initialState;
	//FINDING THE PLAYER
	public float maxDistanceRest = 12;
	public float maxDistancePatrol = 10;
	public float maxDistanceAlert = 8;
	public float maxDistanceAttack = 10;
	//TARGETING
	public bool isPlayerNear = false;
	private float timeLastSeen;
	private Vector3 lastSeenPos;
	public float losingAlertAfter = 5;
	//SHOOTING
	public int damage = 10;
	public float frequency = 2;
	private float nextShot;
	//ROTATING THE TURRET
	public float turretRotationSpeed = 2.5f;
	//DAMAGE
	public int initialHealth = 50;
	private int currHealth;

	//LINERENDER
	private LineRenderer shootLine;
	private float disableLineTime;

	//EVENTS
	private Action<MachineState> OnMachineStateChanged;

	void Start () {

		if (GetComponent<NavMeshAgent> ()) {
			NMA = GetComponent<NavMeshAgent> ();
		} else {
			Debug.LogError("MachineController - UNASSIGNED NAV MESH AGENT");
		}
		if (player == null) {
			if(GameObject.FindGameObjectWithTag("Player") != null){
				player = GameObject.FindGameObjectWithTag("Player");
			}else{
				Debug.LogError("MachineController - UNASSIGNED PLAYER");
			}
		}
		if (testa == null) {
			Debug.LogError("MachineController - UNASSIGNED TURNING HEAD");
		}
		//SET THE INSPECTOR HEALTH
		currHealth = initialHealth;
		//SET THE INSPECTOR STATE
		state = initialState;
		//NEXT MOVE IN
		nextMove = Time.time + restingTime;

		shootLine = testa.GetComponentInChildren<LineRenderer> ();
		shootLine.enabled = false;

		PatrolMove();
	}

	private int lessFreqUpdate = 0;

	void Update () {

		switch (State) {
		case MachineState.Patrolling:
			Patrol();
			break;
		case MachineState.Alert:
			Alert();
			break;
		case MachineState.Attack:
			Attack();
			break;
		}

		if (lessFreqUpdate % 15 == 0) {
			LessFrequentUpdate();
		}
		lessFreqUpdate++;
	}

	void LessFrequentUpdate(){
		switch (State) {
		case MachineState.Resting:
			CheckPlayerDistance(maxDistanceRest);
			break;
		case MachineState.Patrolling:
			CheckPlayerDistance(maxDistancePatrol);
			break;
		case MachineState.Alert:
			CheckPlayerDistance(maxDistanceAlert);
			break;
		case MachineState.Attack:
			CheckPlayerDistance(maxDistanceAttack);
			break;
		}
	}

	void CheckPlayerDistance(float maxDist){
		if (player != null) {
			//RAYCAST
			Vector3 direction = (player.transform.position - transform.position).normalized;
			Ray ray = new Ray( transform.position, direction);
			RaycastHit hit;
			//Debug.DrawLine(transform.position, direction*10,Color.red,0.2f);
			if (Physics.Raycast(ray,out hit)){
				if(hit.transform.tag == "Player"){ 
					//Debug.Log("Player distance " + hit.distance + " state: " + state);
					if(hit.distance <= maxDist){
						//HIT! HE'S NEAR
						if(State != MachineState.Attack){
							//Debug.Log("Player near, upgrade state");
							isPlayerNear = true;
							UpgradeState();
						}
						lastSeenPos = hit.point;
						timeLastSeen = Time.time;
					}else{
						checkTooFar();
					}
				}else{
					//IF ENOUGH TIME IS PASSED SINCE THE LAST TIME THE PLAYER WAS SEEN
					checkTooFar();
				}
			}
		}
	}

	void checkTooFar(){
		if(isPlayerNear){
			//Debug.Log("Time.time : " + Time.time + " Losing after : " + (timeLastSeen+losingAlertAfter));
			if(Time.time > timeLastSeen+losingAlertAfter){
				//Debug.Log("Player too far, degrade state");
				timeLastSeen = Time.time;
				if(State == MachineState.Alert){
					goCheckLastSeen();
					isPlayerNear = false;
				}
				DegradeState();
			}
		}
	}

	void goCheckLastSeen(){
		Debug.Log ("Checking last seen position");
		NMA.SetDestination (lastSeenPos);
	}

	void Attack(){
		if (isPlayerNear) {
			//ROTATE THE HEAD TOWARDS THE ENEMY
			Quaternion neededRotation = Quaternion.LookRotation(player.transform.position - testa.transform.position);
			Quaternion interpolatedRotation = Quaternion.Slerp(testa.transform.rotation, neededRotation, Time.deltaTime * turretRotationSpeed);
			testa.transform.rotation = interpolatedRotation;
			Shoot();
		}
	}

	void Shoot(){
		if (Time.time >= nextShot) {
			nextShot = Time.time + frequency;
			RaycastHit hit;
			if (Physics.Raycast (testa.transform.position, testa.transform.forward, out hit, maxDistanceAttack)) {
				if (hit.transform.GetComponent<IDamageable> () != null) {
					hit.transform.GetComponent<IDamageable> ().Damage (UnityEngine.Random.Range (damage - 2, damage + 3));

				} else {
					//HIT A WALL OR SOMETHING
				}
				shootLine.SetPosition(0,shootLine.transform.position);
				shootLine.SetPosition(1,hit.point);
			}
			shootLine.enabled = true;
			disableLineTime = Time.time + 0.4f;
		} else {
			if(Time.time > disableLineTime){
				shootLine.enabled = false;
			}
		}
	}

	void Alert(){
		//Tipo accendi luci fai cose gira la testa
		testa.transform.Rotate (new Vector3(0,turretRotationSpeed,0));
	}

	//PATROLLING
	void Patrol(){
		if (Time.time >= nextMove) {
			if(NMA != null){
				PatrolMove();
				//NOT UPDATING NEXT MOVE HERE BECAUSE WHAT IF HE IS STILL MOVING
			}else{
				Debug.LogError("MachineController - UNASSIGNED NAV MESH AGENT");
			}
		}
	}

	void PatrolMove(){
		if (NMA.remainingDistance != Mathf.Infinity &&  NMA.pathStatus == NavMeshPathStatus.PathComplete && NMA.remainingDistance == 0) {
			float randX = UnityEngine.Random.Range (transform.position.x - 10, transform.position.x + 10);
			float randZ = UnityEngine.Random.Range (transform.position.z - 10, transform.position.z + 10);
			NMA.SetDestination (new Vector3 (randX, 0, randZ));
			nextMove = Time.time + restingTime;
		}
	}

	void UpgradeState(){
		switch (State) {
		case MachineState.Resting:
			State = MachineState.Patrolling;
			break;
		case MachineState.Patrolling:
			State = MachineState.Alert;
			break;
		case MachineState.Alert:
			nextShot = Time.time + 0.5f;
			State = MachineState.Attack;
			break;
		}
		//Debug.Log ("Upgraded State: " + state);
	}

	void DegradeState(){
		switch (State) {
		case MachineState.Alert:
			state = MachineState.Patrolling;
			break;
		case MachineState.Attack:
			state = MachineState.Alert;
			break;
		}
		//Debug.Log ("Degraded State: " + state);
	}

	void OnStateChanged(MachineState state){

	}

	public void Damage(int i){
		currHealth -= i;
		if (currHealth <= 0) {
			Destroy(gameObject);
		}
	}

	public void AddOnStateChanged(Action<MachineState> action){
		OnMachineStateChanged += action;
	}
	public void RemoveOnStateChanged(Action<MachineState> action){
		OnMachineStateChanged -= action;
	}
}
