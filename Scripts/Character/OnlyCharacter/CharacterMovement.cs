using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[Header ("Переменная перемещения камеры.")]
	public Transform cameraTransform;

	private CharacterState characterStatus;
	private CharacterInput characterInput;
	private Rigidbody rg;
	[Space (5)]
	[Header ("Переменная поворота персонажа.")]
	public float speedRotation;

	[HideInInspector]
	public Vector3 rotationDirection;
	[HideInInspector]
	public Vector3 moveDirection;

	[Space (5)]
	[Header ("Переменная силы прыжка персонажа.")]
	public float JumpPower;
	[Space (5)]
	[Header ("Переменная силы полета в перёд персонажа.")]
	public float FlyCurrend;
	[Space (5)]
	[Header ("Переменные для приседа персонажа.")]
	[Header ("Колизия персонажа в стоячем положении.")]
	public CapsuleCollider locomotionCollider;
	[Space (5)]
	[Header ("Колизия персонажа в сидячем положении.")]
	public CapsuleCollider crouchCollider;
	[Space (5)]
	[Header ("Чекер персонажа отвечающий за проверку нахождения над головой объектов.")]
	public GameObject CrouchCheck;
	[HideInInspector]
	public int CrouchIt = 0;
	[Space (5)]
	[Header ("Чекер полёта персонажа в перёд")]
	public GameObject FlyCheck;

	public List<Collider> helpers;
	public bool OnWall;
	public bool isJump;
	public bool isUp;
	public Collider col;
	//Куда двигаемся
	public Collider ColMove;
	public Vector3 newPoint;
	public bool OnWallCanLocomtion;
	public bool OnWallAnimation;
	public bool UpWallAnimation;
	public bool hangJumpAnimation;
	public bool test;
	public float dir;
	//	[Space (10)]
	//	[Header ("Система лазанья по стенам")]
	//	public float ComeTime;
	//	[Header ("Скорость подъема на преграда")]
	//	public float SpeedUp;
	//
	//	private bool Stay;
	//	private bool jump;
	//	private bool Wall;
	//public bool OnWall;
	//	public bool OnJump;
	//
	//	//Зацепление сейчас
	//	public Transform hangPos;
	//	public Transform hangPosnNow;
	//
	//	public Transform ShangPos;
	//	public Transform ShangPosNow;
	//	//Верхняя точка
	//	public Transform HangUp;
	//	public Transform HangPosUp;
	//
	//	public Transform ShandUp;
	//
	//	public Vector3 HangVector;

	Vector3 moveCurrend;
	float moveAmound;

	void Awake ()
	{
		rg = GetComponent<Rigidbody> ();
		characterStatus = GetComponent<CharacterState> ();
		characterInput = GetComponent<CharacterInput> ();
		helpers = new List<Collider> ();
	}

	void  OnTriggerEnter (Collider col)
	{
		if (col.tag == "Helpers" && col.isTrigger) {
			int index = helpers.FindIndex (x => x.gameObject == col.gameObject);
			if (index == -1) {
				test = true;
				helpers.Add (col);
			}
			
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.tag == "Helpers" && col.isTrigger) {
			int index = helpers.FindIndex (x => x.gameObject == col.gameObject);
			if (index != -1) {
				test = false;
				helpers.Remove (col);
			}
		}
	}

	public void MoveUpdate ()
	{
		if (!OnWall) {
			LocomotionUpdate ();
			RotationNormal ();
			JumpUdpate ();
			UpdateCrouch ();
			FlyUpdate ();
		}
//		for (int i = 0; i < helpers.Count; i++) {
//			if (helpers [i] == 0)
//				test = false;
//			else
//				test = true;
//		}
		ClimpingSystemUpdate ();
	}

	void LocomotionUpdate ()
	{
		Vector3 moveDir = cameraTransform.forward * characterInput.Vertical;

		moveDir += cameraTransform.right * characterInput.Horizontal;
		moveDir.Normalize ();
		moveDirection = moveDir;
		rotationDirection = cameraTransform.forward;
	}

	void RotationNormal ()
	{
		if (!characterStatus.isBattle) {
			rotationDirection = moveDirection;
		}
		Vector3 targetDir = rotationDirection;
		targetDir.y = 0;

		if (targetDir == Vector3.zero)
			targetDir = transform.forward;

		Quaternion lookDir = Quaternion.LookRotation (targetDir);
		Quaternion targetRot = Quaternion.Slerp (transform.rotation, lookDir, Time.deltaTime * speedRotation);
		transform.rotation = targetRot;
	}

	void JumpUdpate ()
	{
		if (!test) {
			moveAmound = (Mathf.Abs (characterInput.Horizontal) + Mathf.Abs (characterInput.Vertical));
			moveAmound = Mathf.Clamp (moveAmound, 0, 1);

			if (!characterStatus.isGroundet) {
				if (!characterStatus.isFlyForwardEmpty) {
					moveCurrend = new Vector3 (0f, 0f, FlyCurrend * moveAmound);
				} else {
					moveCurrend = new Vector3 (0f, 0f, 0f);
				}
			} else if (characterStatus.isGroundet) {
				moveCurrend = new Vector3 (0f, 0f, 0f);
			}
			if (characterStatus.isJump) {
				//rg.AddForce (transform.up * JumpPower);
				rg.velocity = JumpPower * Vector3.up;
			}
		}
	}

	void UpdateCrouch ()
	{
		if (characterStatus.isCrouch || characterStatus.isCrouchEmpty) {
			if (characterStatus.isGroundet) {
				locomotionCollider.enabled = false;
				crouchCollider.enabled = true;
				CrouchCheck.SetActive (true);
			}
		} else if (!characterStatus.isCrouch && !characterStatus.isCrouchEmpty) {
			locomotionCollider.enabled = true;
			crouchCollider.enabled = false;
			CrouchCheck.SetActive (false);
		}

	}

	void FlyUpdate ()
	{
		if (characterStatus.isGroundet) {
			FlyCheck.SetActive (false);
			characterStatus.isFlyForwardEmpty = false;
		} else {
			if (characterStatus.isHang) {
				FlyCheck.SetActive (false);
				characterStatus.isFlyForwardEmpty = false;
			} else {
				if (moveAmound != 0) {
					FlyCheck.SetActive (true);
				}
			}
		}
	}

	void ClimpingSystemUpdate ()
	{
		if (!isJump && !isUp) {
			if (Input.GetKeyUp (KeyCode.Space)) {
				if (OnWall && !Physics.Raycast (transform.position + Vector3.up * 2.1f, transform.forward, 1)) {
					isUp = true;
					newPoint = transform.position + Vector3.up * 2.1f + transform.forward * .5f;
					OnWallAnimation = false;
					UpWallAnimation = true;

				} else {
					col = null;
					for (int i = 0; i < helpers.Count; i++) {
						if (Quaternion.Angle (transform.rotation, helpers [i].transform.rotation) < 50) {
							if (col == null)
								col = helpers [i];
							else if (helpers [i].bounds.max.y > col.bounds.max.y)
								col = helpers [i];
						}
					}
					if (col != null) {
						Ray ray = new Ray (
							          new Vector3 (transform.position.x, col.bounds.max.y - .1f, transform.position.z),
							          new Vector3 (col.bounds.center.x, col.bounds.max.y - .1f, col.bounds.center.z) - new Vector3 (transform.position.x, col.bounds.max.y - .1f, transform.position.z)
						          );
						RaycastHit hit;
						if (Physics.Raycast (ray, out hit, 2.5f)) {
							ColMove = col;
							newPoint = new Vector3 (hit.point.x, col.bounds.max.y, hit.point.z) - col.transform.forward * 0.35f + Vector3.up * -1.91f;
							isJump = true;
							locomotionCollider.enabled = false;
							crouchCollider.enabled = false;
							rg.useGravity = false;
							if (!OnWall) {
								OnWall = true;
								OnWallAnimation = true;
							} else {
								hangJumpAnimation = true;
							}
						}
					}
				}
			}
		}
		if (isJump && !isUp) {
			if (Vector3.Distance (transform.position, new Vector3 (transform.position.x, newPoint.y, newPoint.z)) > .02f) {
				transform.rotation = Quaternion.Slerp (transform.rotation, ColMove.transform.rotation, 5f * Time.deltaTime);
				transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, newPoint.y, newPoint.z), 5f * Time.deltaTime);
			} else {
				isJump = false;
				hangJumpAnimation = false;
			}
		} else if (OnWall && isUp) {
			if (Vector3.Distance (transform.position, new Vector3 (transform.position.x, newPoint.y, newPoint.z)) > .02f) {
				if (transform.position.y < newPoint.y - .1f) {
					transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, newPoint.y, transform.position.z), 10f * Time.deltaTime);
				} else {
					transform.position = Vector3.Slerp (transform.position, newPoint, 5f * Time.deltaTime);
				}
			} else {
				rg.useGravity = true;
				locomotionCollider.enabled = true;
				OnWall = false;
				isJump = false;
				isUp = false;
			}
		}
		if (OnWall && !isJump && !isUp) {
			if (characterInput.Horizontal != 0) {
				if (characterInput.Horizontal > 0) {
					dir = .5f;
				} else if (characterInput.Horizontal < 0) {
					dir = -.5f;
				}
				if (ColMove != null) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position + transform.up * 1.87f + transform.right * dir, transform.forward, out hit, 1f)) {
						if (hit.collider == ColMove) {
							OnWallCanLocomtion = true;
						} else {
							OnWallCanLocomtion = false;
						}
					}
					Debug.DrawRay (transform.position + transform.up * 1.87f + transform.right * dir, transform.forward, Color.red);

				}
			}
		} else {
			OnWallCanLocomtion = false;
		}
	}

	//	#region HangUpdate
	//
	//	void HangUpdate ()
	//	{
	//		HangConfigs ();
	//		HangOnWallBool ();
	//		HangWall ();
	//	}
	//
	//	void HangConfigs ()
	//	{
	//		if (characterStatus.isWall) {
	//			rg.useGravity = false;
	//		} else {
	//			rg.useGravity = true;
	//		}
	//	}
	//
	//	void HangOnWallBool ()
	//	{
	//		if (characterStatus.isHang) {
	//			characterStatus.isWall = true;
	//			OnWall = true;
	//		} else if (!characterStatus.isHang && ComeTime == 0) {
	//			characterStatus.isWall = false;
	//			OnWall = false;
	//		}
	//	}
	//
	//	void HangWall ()
	//	{
	//		if(OnWall)
	//		{
	//			transform.position = Vector3.Slerp(transform.position,new Vector3 (hangPosnNow))
	//		}
	//	}
	//
	//	#endregion



	void FixedUpdate ()
	{
		if (!OnWall) {
			rg.MovePosition (rg.position + transform.TransformDirection (moveCurrend) * Time.fixedDeltaTime);
		}
	}

}