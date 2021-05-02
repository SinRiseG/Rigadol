using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
	public CharacterState characterState;
	public CharacterAnimation characterAnimation;
	public CharacterMovement characterMovement;

	public float Vertical;
	public float Horizontal;

	public float MouseX;
	public float MouseY;


	int battleID;
	bool battle;
	int attackID;
	int diveID;
	int crouchID;
	bool crouch;
	public float time;

	public bool Mobile;
	[Header ("Mobile Controll")]
	public MoviJoystick moveJoystick;
	public FixedTouchFild touchFild;


	public float FixedMouse;

	public void InputUpdate ()
	{
		CheckBattleOrCrouchUpdate ();

		if (!Mobile) {
			InputPC ();
		} else {
			InputMobile ();
		}
	}


	#region PCInput

	void InputPC ()
	{
		PCLocomotionUpdate ();
		if (!characterMovement.OnWall) {
			PCBattelUpdate ();
			PCAttackUpdate ();
			PCJumpUpdate ();
			PCCrouchUpdate ();
		}
	}

	void PCLocomotionUpdate ()
	{
		Vertical = Input.GetAxis ("Vertical");
		Horizontal = Input.GetAxis ("Horizontal");

		MouseX = Input.GetAxis ("Mouse X");
		MouseY = Input.GetAxis ("Mouse Y");
	}

	void PCBattelUpdate ()
	{
		
		if (characterState.isGroundet) {
			if (!characterState.isCrouchEmpty) {
				if (Input.GetMouseButtonDown (1)) {
					battleID += 1;
				} 

				if (battleID == 0) {
					characterState.isBattle = false;
				} else if (battleID == 1) {
					characterState.isBattle = true;
				} else {
					battleID = 0;
				}
			}
		} else {
			characterState.isBattle = false;
		}

	}


	void PCAttackUpdate ()
	{

		if (characterState.isBattle) {
			if (Input.GetMouseButtonDown (0)) {
				characterAnimation.AttackUpdate ();
			}

		}
	}

	void PCJumpUpdate ()
	{
		if (!characterState.isBattle) {
			if (!characterState.isCrouchEmpty) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					characterAnimation.JumpUpdate ();

				}
			}
		}
	}

	void PCCrouchUpdate ()
	{
		if (!battle) {
			if (characterState.isGroundet) {
				if (!characterState.isBattle) {
					if (Input.GetKeyDown (KeyCode.LeftControl)) {
						crouchID += 1;
					}
				}
			}
		}

		if (crouchID == 0) {
			characterState.isCrouch = false;
		} else if (crouchID == 1) {
			characterState.isCrouch = true;
		} else {
			crouchID = 0;
		}
	}

	#endregion

	#region MobileInput

	void InputMobile ()
	{
		MoboilLocomotionUpdate ();
		MBattleUpdate ();
		MDiveUpdate ();
		MCrouchUpdate ();
	}

	void MoboilLocomotionUpdate ()
	{
		Vertical = moveJoystick.inputStick.y;
		Horizontal = moveJoystick.inputStick.x;

		MouseX = touchFild.touchDist.x / FixedMouse;
		MouseY = touchFild.touchDist.y / FixedMouse;
	}

	#region BattelControll

	void MBattleUpdate ()
	{
		if (characterState.isGroundet) {
			if (battleID == 0) {
				characterState.isBattle = false;
			} else if (battleID == 1) {
				characterState.isBattle = true;
			} else {
				battleID = 0;
			}
		} else {
			battleID = 0;
			characterState.isBattle = false;
		}
	}

	public void OnBattleState ()
	{
		if (!characterState.isCrouchEmpty) {
			battleID += 1;
		}
	}

	#endregion

	#region AttackControll


	public void OnAttackState ()
	{
		if (characterState.isGroundet) {
			characterAnimation.AttackUpdate ();
		}
	}



	#endregion

	#region Jump

	public void OnJompState ()
	{
		if (characterState.isGroundet) {
			if (!characterState.isCrouchEmpty) {
				characterAnimation.JumpUpdate ();
			}
		}
	}

	#endregion

	#region MDive

	void MDiveUpdate ()
	{
		if (characterState.isGroundet) {
			if (diveID != 0) {
				time += Time.deltaTime;
				characterState.isDive = true;
				if (time > 0.1) {
					diveID = 0;
					time = 0;
					characterState.isDive = false;
				}

			}
		} else {
			diveID = 0;
			characterState.isDive = false;
		}
	}

	public void OnDiveState ()
	{
		if (characterState.isGroundet) {
			diveID += 1;
		}
	}

	#endregion

	#region MCrouch

	void MCrouchUpdate ()
	{
		if (crouchID == 0) {
			characterState.isCrouch = false;
		} else if (crouchID == 1) {
			characterState.isCrouch = true;
		} else {
			crouchID = 0;
		}
	}

	public void OnCrouchState ()
	{
		if (!battle) {
			if (characterState.isGroundet) {
				if (!characterState.isBattle) {
					crouchID += 1;
				}
			}
		}
	}

	#endregion

	#endregion

	#region Checkers

	public void OnGroundCheck (bool _ground)
	{
		characterState.isGroundet = _ground;
	}

	public void OnCrouchCheck (bool _crouchEmpty)
	{
		characterState.isCrouchEmpty = _crouchEmpty;
	}

	public void OnFlyCheck (bool _flyCheck)
	{
		characterState.isFlyForwardEmpty = _flyCheck;
	}
	//test
	public void OnHangCheck (bool _hang)
	{
		characterState.isHang = _hang;
	}

	public void OnHangTransform (Transform _trans)
	{
		characterMovement.hangPos = _trans;
	}

	public void OnHelpersPosition (Transform _hangPosition)
	{
		characterMovement.hangPosnNow = _hangPosition;
	}

	public void OnCanUpHang (bool _canHang)
	{
		characterState.CanHangUp = _canHang;
	}

	public void OnWeakUpHang (bool _canUp)
	{
		//kbholjbn
	}

	public void OnUpHangTransform (Transform _UpHang)
	{
		characterMovement.HangUp = _UpHang;
	}

	public void OnUpHangPosition (Transform _UpHangPosition)
	{
		characterMovement.HangPosUp = _UpHangPosition;
	}

	void CheckBattleOrCrouchUpdate ()
	{
		if (!characterState.isCrouchEmpty) {
			if (characterState.isBattle) {
				crouchID = 0;
				battle = true;
			} else {
				battle = false;
			}
		} else {
			battle = false;
			battleID = 0;
			crouchID = 1;
		}
	}



	#endregion
}
