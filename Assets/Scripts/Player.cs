using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable {
	private static int score = 0;
	public float speed = 10.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private GameObject cam;
	private Weapon weapon;
	private float health = 50;
	private Text vita;
	private static Text scoreT;

	void Start(){
		controller = GetComponent<CharacterController>();
		cam = gameObject.GetComponentInChildren<Camera> ().gameObject;
		Cursor.visible = false;
		weapon = gameObject.GetComponentInChildren<Weapon> ();
		Text[] testi = gameObject.GetComponentsInChildren<Text> ();
		foreach(Text t in testi){
			if(t.gameObject.name == "Vita")
				vita = t;
			if(t.gameObject.name == "Score")
				scoreT = t;
		}
	}

	void Update() {
		if (Input.GetMouseButton (0)) {
			if(!weapon.IsReloading){
				weapon.ShootOne();
			}
		}
		if (Input.GetMouseButton (1)) {
			if(!weapon.IsReloading){
				weapon.ShootTwo();
			}
		}

		if (controller.isGrounded) {
			moveDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

		View ();
	}

	void View(){
		cam.transform.Rotate (new Vector3(-Input.GetAxis("Mouse Y") * 1.5f, 0, 0));
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * 1.5f, 0));
	}

	public void Damage(int i){
		health -= i;
		vita.text = "Health: " + health.ToString ();
		if (health < 1)
			Destroy (this);
	}

	public static void addScore(int s){
		score += s;
		scoreT.text = "Score: " + score.ToString ();
	}
}