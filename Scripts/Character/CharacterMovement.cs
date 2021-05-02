using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

	public Transform cameraTransform;
	public CharacterState characterStatus;
	public CharacterInput characterInput;

	public float speedRotation;

	public Vector3 rotationDirection;
	public Vector3 moveDirection;

	public float JumpPower;
	public float FlyCurrend;

	[Header ("CrouchUpdate")]
	public CapsuleCollider locomotionCollider;
	public CapsuleCollider crouchCollider;
	public GameObject CrouchCheck;
	public int CrouchIt = 0;

	[Header ("FlyCheck")]
	public GameObject FlyCheck;

	[Header ("hang")]
	public float ComeTime;
	public float SpeedUp;

	private bool Stay;
	private bool jump;
	private bool Wall;
	public bool OnWall;
	public bool OnJump;

	//Зацепление сейчас
	public Transform hangPos;
	public Transform hangPosnNow;

	public Transform ShangPos;
	public Transform ShangPosNow;
	//Верхняя точка
	public Transform HangUp;
	public Transform HangPosUp;

	public Transform ShandUp;

	public Vector3 HangVector;



	Rigidbody rg;
	Vector3 moveCurrend;
	float moveAmound;

	void Awake ()
	{
		rg = GetComponent<Rigidbody> ();
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
		HangUpdate ();
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
		if (!characterStatus.isHang) {
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

	#region HangUpdate

	void HangUpdate ()
	{
		HangConfigs ();
		HangOnWallBool ();
		HangWall ();
	}

	void HangConfigs ()
	{
		if (characterStatus.isWall) {
			rg.useGravity = false;
		} else {
			rg.useGravity = true;
		}
	}

	void HangOnWallBool ()
	{
		if (characterStatus.isHang) {
			characterStatus.isWall = true;
			OnWall = true;
		} else if (!characterStatus.isHang && ComeTime == 0) {
			characterStatus.isWall = false;
			OnWall = false;
		}
	}

	void HangWall ()
	{
		Wall = characterStatus.isWall;
		if (Wall) {
			if (ComeTime == 0) {
				transform.position = new Vector3 (transform.position.x, hangPosnNow.position.y, hangPosnNow.position.z);
				transform.rotation = Quaternion.Slerp (transform.rotation, hangPosnNow.rotation, 5 * Time.deltaTime);
			} else {
				characterStatus.isWall = true;
				OnWall = true;
			}
			if (Input.GetKey (KeyCode.Space)) {
				ComeTime += Time.deltaTime;
				jump = true;
				ShangPosNow = HangPosUp;
			}
			if (jump) {
				if (characterStatus.CanHangUp) {
					transform.position = Vector3.Slerp (transform.position, new Vector3 (transform.position.x, ShangPosNow.position.y, ShangPosNow.position.z), SpeedUp * Time.deltaTime);
					transform.rotation = Quaternion.Slerp (transform.rotation, ShangPosNow.rotation, SpeedUp * Time.deltaTime);
					if (hangPosnNow == ShangPosNow) {
						ComeTime = 0;
						jump = false;
					}
				} else {
					ComeTime = 0;
					jump = false;
				}
			}
		}

	}

	#endregion



	void FixedUpdate ()
	{
		if (!OnWall) {
			rg.MovePosition (rg.position + transform.TransformDirection (moveCurrend) * Time.fixedDeltaTime);
		}
	}

}