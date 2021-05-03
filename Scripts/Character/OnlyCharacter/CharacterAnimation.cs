using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

	private Animator anim;
	private CharacterInput characterInput;
	private CharacterState characterStatus;
	private CharacterMovement characterMovement;

	[Header ("Чистота перехода анимации.")]
	public float dampTime;
	[Space (5)]
	[Header ("Время свободного подения персонажа.")]
	public float flyTime;

	bool lowDown;
	bool longDown;
	bool hardDown;

	bool jlow;
	bool jlong;
	bool jhard;

	[HideInInspector]
	public bool jump;
	[HideInInspector]
	public float moveAmound;
	[HideInInspector]
	public float t;
	[HideInInspector]
	public float timejump;
	[HideInInspector]
	public float hanguptime;

	void Start ()
	{
		anim = GetComponent<Animator> ();
		characterInput = GetComponent<CharacterInput> ();
		characterStatus = GetComponent<CharacterState> ();
		characterMovement = GetComponent<CharacterMovement> ();
	}



	public void AnimationUpdate ()
	{
		AnimationLocomotion ();
		BattleAnimationUpdate ();
		BattleLocomotionAnimationUpdate ();
		DownUpdate ();
		FlyUpdate ();
		CrouchUpdate ();
		OnWallUpdate ();
	}

	void AnimationLocomotion ()
	{
		moveAmound = (Mathf.Abs (characterInput.Horizontal) + Mathf.Abs (characterInput.Vertical));
		if (!characterStatus.isSprint) {
			
			moveAmound = Mathf.Clamp (moveAmound, 0, 0.7f);
		} else {
			moveAmound = Mathf.Clamp (moveAmound, 0, 1);
		}
		anim.SetFloat ("MoveAmound", moveAmound, dampTime, Time.deltaTime);


	}

	void BattleAnimationUpdate ()
	{
		if (characterStatus.isBattle) {
			anim.SetBool ("Battle", true);
		} else {
			anim.SetBool ("Battle", false);
		}
	}

	void BattleLocomotionAnimationUpdate ()
	{
		anim.SetFloat ("vertical", characterInput.Vertical, dampTime, Time.deltaTime);
		anim.SetFloat ("horizontal", characterInput.Horizontal, dampTime, Time.deltaTime);
	}


	public void AttackUpdate ()
	{
		anim.SetTrigger ("test");

	}

	void FlyUpdate ()
	{
		if (!characterMovement.HaveOnWall) {
			if (!characterStatus.isGroundet) {
				anim.SetBool ("Fly", true);
			} else {
				anim.SetBool ("Fly", false);
			}
		} else {
			anim.SetBool ("Fly", false);
		}
	}

	void DownUpdate ()
	{
		if (!characterStatus.isGroundet) {
			flyTime += Time.deltaTime;
			t = 0;
		} else {
			t += Time.deltaTime;	
			if (flyTime < 1f && flyTime > 0.5f) {
				lowDown = true;
				longDown = false;
				hardDown = false;
			} else if (flyTime >= 1f && flyTime < 1.5f) {
				lowDown = false;
				longDown = true;
				hardDown = false;
			} else if (flyTime >= 1.5f) {
				lowDown = false;
				longDown = false;
				hardDown = true;
			}
			if (t >= 0.1) {
				flyTime = 0;
				lowDown = false;
				longDown = false;
				hardDown = false;
			}
		}
		anim.SetBool ("LowDown", lowDown);
		anim.SetBool ("LongDown", longDown);
		anim.SetBool ("HardDown", hardDown);

	}

	void CrouchUpdate ()
	{
		anim.SetBool ("Crouch", characterStatus.isCrouch);
	}

	void OnWallUpdate ()
	{
		anim.SetBool ("OnWall", characterMovement.OnWallAnimation);
		anim.SetBool ("HangJump", characterMovement.hangJumpAnimation);
		anim.SetBool ("UpWall", characterMovement.UpWallAnimation);
		if (characterMovement.OnWallCanLocomtion) {
			if (!characterMovement.CanOnWallMoveLeft) {
				characterInput.Horizontal = Mathf.Clamp (characterInput.Horizontal, 0, 1);
			} else {
				characterInput.Horizontal = Mathf.Clamp (characterInput.Horizontal, -1, 1);
			}
			if (!characterMovement.CanOnWallMoveRight) {
				characterInput.Horizontal = Mathf.Clamp (characterInput.Horizontal, -1, 0);
			} else {
				characterInput.Horizontal = Mathf.Clamp (characterInput.Horizontal, -1, 1);
			}
			anim.SetFloat ("OnWallH", characterInput.Horizontal);
		}
	}
}
