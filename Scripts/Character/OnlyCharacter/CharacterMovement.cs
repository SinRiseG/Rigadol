using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[Header ("Переменная перемещения камеры.")]
	public Transform cameraTransform;

	private CharacterState characterStatus;
	private CharacterInput characterInput;
	private CharacterAnimation anim;
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
	[Header ("Чекер полёта персонажа в перёд.")]
	public GameObject FlyCheck;
	[Header ("Лист хелперов.")]
	public List<Collider> helpers;
	[Space (5)]
	[Header ("Бокс колайдер для клипинга.")]
	public BoxCollider BoxCliping;
	[Space (5)]
	[Header ("Скорость прыжка на стене")]
	public float SpeedOnWallJump;
	public float SpeedOnWallDown;
	[Space (5)]
	[Header ("Выполнил подъём на стену.")]
	public bool getUp;
	[Space (5)]
	[Header ("Выполнил спуск со стену.")]
	public bool getDown;
	[HideInInspector]
	public bool OnWall;
	[HideInInspector]
	public bool OnWallJump;
	[HideInInspector]
	public bool isJump;
	[HideInInspector]
	public bool isUp;
	[HideInInspector]
	public bool isJumpDown;
	[HideInInspector]
	public bool DownJump;

	public bool isDown;
	[HideInInspector]
	public bool isFlyClimping;
	[HideInInspector]
	public bool CanOnWallMoveRight;
	[HideInInspector]
	public bool CanOnWallMoveLeft;
	[HideInInspector]
	public Collider col;
	[HideInInspector]
	public Collider ColMove;
	[HideInInspector]
	public Vector3 newPoint;
	[HideInInspector]
	public bool OnWallCanLocomtion;
	[HideInInspector]
	public bool OnWallAnimation;
	[HideInInspector]
	public bool UpWallAnimation;
	[HideInInspector]
	public bool hangJumpAnimation;
	[HideInInspector]
	public bool HaveOnWall;

	float t;

	public int DownInit;

	Vector3 startPos;
	Vector3 targetPos;
	[Space (5)]
	[Header ("Растояние от стены в клипинг системе.")]
	public float offSetFromWall = 0.3f;
	[Space (5)]
	[Header ("Растояние без стены в клипинг системе.")]
	public float offSetWihtOutWall;
	[Space (5)]
	[Header ("Скорость вращения персонажа на стене.")]
	public float OnWallRotation;
	[Space (5)]
	[Header ("Спуск персонажа по У для коректного зацепления.")]
	public float offSetOnWall;
	[Space (5)]
	[Header ("Размер колайдара клипинга в обычном состоянии.")]
	public float boxPosOnGround;
	[Space (5)]
	[Header ("Размер колайдера на стене.")]
	public float boxPosOnWall;

	Transform helpClimp;

	Vector3 moveCurrend;
	float moveAmound;

	public bool WorH;

	void Awake ()
	{
		Init ();
	}

	void Init ()
	{
		rg = GetComponent<Rigidbody> ();
		characterStatus = GetComponent<CharacterState> ();
		characterInput = GetComponent<CharacterInput> ();
		anim = GetComponent<CharacterAnimation> ();

		helpers = new List<Collider> ();

		helpClimp = new GameObject ().transform;
		helpClimp.name = "ClimpHelpers";
	}

	void  OnTriggerEnter (Collider col)
	{
		if (!isFlyClimping) {
			if (col.tag == "Helpers" && col.isTrigger) {
				int index = helpers.FindIndex (x => x.gameObject == col.gameObject);
				if (index == -1) {
					HaveOnWall = true;
					helpers.Add (col);
				}
			
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.tag == "Helpers" && col.isTrigger) {
			int index = helpers.FindIndex (x => x.gameObject == col.gameObject);
			if (index != -1) {
				HaveOnWall = false;
				helpers.Remove (col);
			}
		}
	}

	public void MoveUpdate ()
	{
		if (!OnWall) {
			DownInit = 0;
			LocomotionUpdate ();
			RotationNormal ();
			JumpUdpate ();
			UpdateCrouch ();
			FlyUpdate ();
		}
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
		if (helpers.Count == 0) {
			moveAmound = (Mathf.Abs (characterInput.Horizontal) + Mathf.Abs (characterInput.Vertical));
			moveAmound = Mathf.Clamp (moveAmound, 0, 1);
			if (!characterStatus.isGroundet) {
				if (!characterStatus.isFlyForwardEmpty) {
					moveCurrend = new Vector3 (0f, 0f, FlyCurrend * moveAmound);
				} else {
					moveCurrend = new Vector3 (0f, 0f, 0f);
				}
			} else {
				moveCurrend = new Vector3 (0f, 0f, 0f);
				if (characterStatus.isJump) {
					rg.velocity = JumpPower * Vector3.up;
				}
			}
		} else if (helpers.Count != 0) {
			if (characterStatus.isJump) {
				rg.velocity = 0f * Vector3.up;
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
		if (characterStatus.isGroundet && !OnWall) {
			FlyCheck.SetActive (false);
			characterStatus.isFlyForwardEmpty = false;
			BoxCliping.enabled = true;
			isFlyClimping = false;
		} else if (!characterStatus.isGroundet && !OnWall) {
			FlyCheck.SetActive (true);
			BoxCliping.enabled = false;
			isFlyClimping = true;
			helpers.Clear ();
		} else if (!characterStatus.isGroundet && OnWall) {
			FlyCheck.SetActive (false);
			BoxCliping.enabled = true;
			characterStatus.isFlyForwardEmpty = false;
			isFlyClimping = false;
		}
	}

	void ClimpingSystemUpdate ()
	{
		characterStatus.OnWall = OnWall;
		if (!isFlyClimping) {
			if (!isJump && !isUp) {
				if (characterStatus.isJump) {
					if (OnWall && !Physics.Raycast (transform.position + Vector3.up * (Mathf.Abs (offSetOnWall) + 0.2f), transform.forward, 1)) {
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
							if (!OnWall) {
								if (Physics.Raycast (ray, out hit, 2.5f)) {
									ColMove = col;
									newPoint = new Vector3 (hit.point.x, col.bounds.max.y, hit.point.z) - col.transform.forward * offSetFromWall + Vector3.up * offSetOnWall;
									isJump = true;
									locomotionCollider.enabled = false;
									crouchCollider.enabled = false;
									rg.useGravity = false;
									OnWall = true;
									OnWallAnimation = true;
								}
							} else {
								if (Physics.Raycast (ray, out hit, 1.5f)) {
									ColMove = col;
									newPoint = new Vector3 (hit.point.x, col.bounds.max.y, hit.point.z) - col.transform.forward * offSetFromWall + Vector3.up * offSetOnWall;
									isJump = true;
									hangJumpAnimation = true;
								}
							}
						}
					}
				}
			}
			if (characterInput.isJumpDown && OnWall && !isJumpDown) {
				
				for (int i = 0; i < helpers.Count; i++) {
					if (Quaternion.Angle (transform.rotation, helpers [i].transform.rotation) < 50) {
						if (helpers [i].bounds.max.y < ColMove.bounds.max.y) {
							col = helpers [i];
						}
					}
				}
				if (col.bounds.max.y != ColMove.bounds.max.y) {
					Ray ray = new Ray (
						          new Vector3 (transform.position.x, col.bounds.max.y - .1f, transform.position.z),
						          new Vector3 (col.bounds.center.x, col.bounds.max.y - .1f, col.bounds.center.z) - new Vector3 (transform.position.x, col.bounds.max.y - .1f, transform.position.z)
					          );
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 2.5f)) {
						ColMove = col;
						newPoint = new Vector3 (hit.point.x, col.bounds.max.y, hit.point.z) - col.transform.forward * 0.35f + Vector3.up * offSetOnWall;
						isJumpDown = true;
						DownJump = false;
					}
				} else if (col.bounds.max.y == ColMove.bounds.max.y) {
					isDown = true;
					OnWallAnimation = false;
				}
			}

			if (isJump && !isUp) {
				if (Vector3.Distance (transform.position, new Vector3 (newPoint.x, newPoint.y, newPoint.z)) > .02f) {
					transform.rotation = Quaternion.Slerp (transform.rotation, ColMove.transform.rotation, 5f * Time.deltaTime);
					transform.position = Vector3.Lerp (transform.position, new Vector3 (newPoint.x, newPoint.y, newPoint.z), SpeedOnWallJump * Time.deltaTime);
				} else {
					isJump = false;
					hangJumpAnimation = false;


				}
			} else if (isJumpDown) {
				if (Vector3.Distance (transform.position, new Vector3 (transform.position.x, newPoint.y, newPoint.z)) > .02f) {
					transform.rotation = Quaternion.Slerp (transform.rotation, ColMove.transform.rotation, 5f * Time.deltaTime);
					transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, newPoint.y, newPoint.z), SpeedOnWallDown * Time.deltaTime);
				} else {
					isJumpDown = false;
					DownJump = false; 


				}
			} else if (OnWall && isUp) {
				if (getUp) {
					rg.useGravity = true;
					locomotionCollider.enabled = true;
					OnWall = false;
					isJump = false;
					isUp = false;
					getUp = false;
				}
			} else if (OnWall && isDown) {
				if (getDown) {
					rg.useGravity = true;
					locomotionCollider.enabled = true;
					OnWall = false;
					isJump = false;
					isUp = false;
					isDown = false;
					OnWallAnimation = false;
					helpers.Clear ();
				}
			}
			if (OnWall && !isJump && !isUp && !isJumpDown) {
				OnWallCanLocomtion = true;
				if (ColMove != null) {
					RaycastHit hitLeft;
					if (Physics.Raycast (transform.position + transform.up * (Mathf.Abs (offSetOnWall) - 0.2f) + transform.right * -0.5f + transform.forward * -2f, transform.forward, out hitLeft, 5f)) {
						{
							if (hitLeft.collider.tag == "Helpers") {
								if (characterInput.Horizontal <= 0) {
									CanOnWallMoveLeft = true;
									ColMove = hitLeft.collider;
								} else {
									CanOnWallMoveLeft = false;
								}
							} else {
								CanOnWallMoveLeft = false;
							}
						}
					}
					Debug.DrawRay (transform.position + transform.up * (Mathf.Abs (offSetOnWall) - 0.2f) + transform.right * -0.5f + transform.forward * -1f, transform.forward * 5f, Color.red);
					RaycastHit hitRight;
					if (Physics.Raycast (transform.position + transform.up * (Mathf.Abs (offSetOnWall) - 0.2f) + transform.right * 0.5f + transform.forward * -2f, transform.forward, out hitRight, 5f)) {
						if (hitRight.collider.tag == "Helpers") {
							if (characterInput.Horizontal >= 0) {
								CanOnWallMoveRight = true;
								ColMove = hitRight.collider;
							} else {
								CanOnWallMoveRight = false;
							}
						} else {
							CanOnWallMoveRight = false;
						}
					}
					Debug.DrawRay (transform.position + transform.up * (Mathf.Abs (offSetOnWall) - 0.2f) + transform.right * 0.5f + transform.forward * -1f, transform.forward * 5f, Color.red);

					if (!Physics.Raycast (transform.position + Vector3.up * (Mathf.Abs (offSetOnWall) - 1f), transform.forward, 1)) {
						WorH = true;
					} else {
						WorH = false;
					}

					Debug.DrawRay (transform.position + transform.up * (Mathf.Abs (offSetOnWall) - 1f), transform.forward * 1f, Color.red);

					float TransformForWall = 0f;

					if (WorH) {
						TransformForWall = offSetWihtOutWall;
					} else {
						TransformForWall = offSetFromWall;
					}


					Vector3 origin = transform.position;
					origin.y += (Mathf.Abs (offSetOnWall) - 0.2f);
					Vector3 dirpos = transform.forward;
					RaycastHit hitPos;
					if (Physics.Raycast (origin, dirpos, out hitPos, 1)) {
						helpClimp.transform.rotation = Quaternion.LookRotation (-hitPos.normal);
						startPos = transform.position;
						targetPos = hitPos.point + (hitPos.normal * TransformForWall) + transform.up * offSetOnWall;
						t = 0; 
					}

					t += Time.deltaTime;
					if (t > 1)
						t = 1;
					Vector3 tp = Vector3.Lerp (startPos, targetPos, t);
					transform.position = new Vector3 (tp.x, transform.position.y, tp.z);
					transform.rotation = Quaternion.Slerp (transform.rotation, helpClimp.transform.rotation, OnWallRotation * Time.deltaTime);
				}
			} else {
				OnWallCanLocomtion = false;
			}
			DebugLineCliping ();
		}
	}

	void  DebugLineCliping ()
	{
		if (!OnWall) {
			BoxCliping.center = new Vector3 (BoxCliping.center.x, boxPosOnGround, BoxCliping.center.z);
		} else {
			BoxCliping.center = new Vector3 (BoxCliping.center.x, boxPosOnWall, BoxCliping.center.z);
		}
			
	}

	Vector3 PosWithOffSet (Vector3 origin, Vector3 target)
	{
		Vector3 direction = origin - target;
		direction.Normalize ();
		Vector3 offset = direction * offSetFromWall;
		return target + offset;
	}

	void FixedUpdate ()
	{
		if (!OnWall) {
			rg.MovePosition (rg.position + transform.TransformDirection (moveCurrend) * Time.fixedDeltaTime);
		}
	}
}