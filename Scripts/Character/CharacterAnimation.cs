using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

	public Animator anim;
	public CharacterInput characterInput;
	public CharacterState characterStatus;
	public CharacterMovement characterMovement;


	public float dampTime;

	public float flyTime;

	bool lowDown;
	bool longDown;
	bool hardDown;

	bool jlow;
	bool jlong;
	bool jhard;

	public bool jump;

	float moveAmound;
	public float t;
	public float timejump;
	public float hanguptime;

	public void AnimationUpdate ()
	{
		AnimationLocomotion ();
		BattleAnimationUpdate ();
		BattleLocomotionAnimationUpdate ();
		DiveUpdate ();
		DownUpdate ();
		FlyUpdate ();
		jumpbool ();
		CrouchUpdate ();
		HangUpdate ();
	}

	void AnimationLocomotion ()
	{
		moveAmound = (Mathf.Abs (characterInput.Horizontal) + Mathf.Abs (characterInput.Vertical));
		moveAmound = Mathf.Clamp (moveAmound, 0, 1);

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

	void DiveUpdate ()
	{
		if (characterStatus.isDive) {
			anim.SetTrigger ("dive");
		}
		anim.SetBool ("tDive", characterStatus.isDive);
	}

	public void JumpUpdate ()
	{
		if (characterStatus.isGroundet) {
			if (moveAmound != 0f) {
				jump = true;
			} else {
				jump = true;
			}

		}
		if (characterStatus.isHang) {
			if (moveAmound != 0f) {
				jump = true;
			} else {
				jump = true;
			}
		}


	}

	void jumpbool ()
	{
		if (jump) {
			timejump += Time.deltaTime;
			if (timejump > 0.1) {
				jump = false;
				timejump = 0;
			}
		}
		characterStatus.isJump = jump;
	}

	void FlyUpdate ()
	{
		
		if (!characterStatus.isGroundet) {
			anim.SetBool ("Fly", true);
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

	//test
	void HangUpdate ()
	{
		anim.SetBool ("Hang", characterStatus.isHang);
		anim.SetBool ("HangJump", Input.GetKeyDown (KeyCode.Space));
	}
}
