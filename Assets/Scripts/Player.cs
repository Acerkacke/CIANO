using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable,ISlowUpdate,ISecondUpdate{
	
	private int score = 0;
	public int Score{
		get{
			return score;
		}
		protected set{
			score = value;
			OnScoreChanged();
		}
	}
	public float normalSpeed = 10f;
	private float speed;
	public float jumpSpeed = 8f;
	public float mouseSensibility = 1.5f;

	public static Player Instance;

	public float startRehealAfter = 4f;
	private float startRehealAt;
	public int rehealPerSecond;
	private bool hasToReheal = false;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController characterController;
	public GameObject cam;
	private Weapon weapon;
	public int startHealth = 50;
	private int health;
	public int Health{
		get{
			return health;
		}
		protected set{
			int lastHealth = health;
			health = value;
			if(value > lastHealth){
				OnHealthChanged(false);
			}else{
				OnHealthChanged(true);
			}
		}
	}



	public Text healthText;
	public Text scoreText;
	public Image damagePanel;

	void Start(){
		health = startHealth;
		characterController = GetComponent<CharacterController>();
		//cam = gameObject.GetComponentInChildren<Camera> ().gameObject;
		Cursor.visible = false;
		weapon = gameObject.GetComponentInChildren<Weapon> ();


		if(Instance == null){
			Instance = this;
		}else{
			Debug.LogError("Player - TWO INSTANCES FOUND, FIX THIS");
		}
	}

	void Update() {
		Shoot();
		Move();
		View ();
	}

	void Shoot(){
		if (Input.GetMouseButton (0)) {
			if(!weapon.IsReloading){
				weapon.Shoot(1);
			}
		}
		if (Input.GetMouseButton (1)) {
			/*if(!weapon.IsReloading){
				weapon.Shoot(2);
			}*/
			weapon.Mira = true;
		} else {
			weapon.Mira = false;
		}
	}

	void Move(){
		//RUN
		if (Input.GetKey (KeyCode.LeftShift))
			speed = normalSpeed * 2;
		else
			speed = normalSpeed;

		if (characterController.isGrounded) {
			moveDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

		}
		moveDirection.y -= 9.81f * Time.deltaTime;
		characterController.Move(moveDirection * Time.deltaTime);
	}

	void View(){
		cam.transform.Rotate (new Vector3(-Input.GetAxis("Mouse Y") * mouseSensibility, 0, 0));
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensibility, 0));
	}

	public void Damage(int i){
		if (i > 0) {
			Health -= i;
			hasToReheal = true;
			startRehealAt = Time.time + startRehealAfter;
			if (Health <= 0) {
				Health = 0;
				Destroy (this);
			}
		}
	}

	void OnHealthChanged(bool hasLostHealth){
		healthText.text = "Health: " + health;
		if (hasLostHealth) {
			damagePanel.color = new Color (1, 0, 0, 0.3f);
		}
	}

	void OnScoreChanged(){
		scoreText.text= "Score: " + score;
	}

	public void addScore(int s){
		Score += s;
	}

	public void SlowUpdate(){
		if (damagePanel.color.a > 0) {
			damagePanel.color = new Color(1,0,0,damagePanel.color.a-0.02f);
		}
	}

	public void SecondUpdate(){
		Reheal ();
	}

	void Reheal(){
		if(hasToReheal){
			if(Time.time >= startRehealAt){
				if(health < startHealth){
					Health+=rehealPerSecond;
				}else{
					hasToReheal = false;
				}
			}
		}
	}
}