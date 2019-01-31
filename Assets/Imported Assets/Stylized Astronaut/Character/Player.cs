using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;

	public float speed = 5;
	public float turnSpeed = 400;
    public float gravity = 20.0f;
    private Vector2 input;
    private float angle;
    private Quaternion targetRotation;
    private Transform cam;
    private bool isRotating;


	void Start () {
		controller = GetComponent <CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        GetInput();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1)
        {
            isRotating = false;
            anim.SetInteger("AnimationPar", 0);
            return;
        }

        CalculateDirection();
        Rotate();
        Move();
        anim.SetInteger("AnimationPar", 1);

    }

    // input based on horizontal and vertical input
    void GetInput() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    // direction relative to camera's rotation
    void CalculateDirection() {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
    }

    // rotate towards calculated angle
    void Rotate() {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        if ( transform.rotation == targetRotation ) {
            isRotating = false;
        } else {
            isRotating = true;
        }
    }

    // player moves along forward axis
    void Move() {
        if (!isRotating) {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
