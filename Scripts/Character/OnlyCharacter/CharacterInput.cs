using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
	//Компаненты приватные
	private CharacterState characterState;
	private CharacterAnimation characterAnimation;
	private CharacterMovement characterMovement;
	//Переменная вертикального движения
	[HideInInspector]
	public float Vertical;
	//переменная горизонтального движения
	[HideInInspector]
	public float Horizontal;

	//переменная джижения мыши вертикально
	[HideInInspector]
	public float MouseX;
	//переменная движенния мыши горизонтально
	[HideInInspector]
	public float MouseY;
	//Айди кнопки битвы
	int battleID;
	//Показатель состояния персонажа битвы
	bool battle;
	//айди кнопки аттаки
	int attackID;
	//айди кнопки бега
	int sprintID;
	//айди кнопки присяда
	int crouchID;
	//Показатель состояния персонажа приседа
	bool crouch;
	//Показатель состояния прыжка
	[HideInInspector]
	public bool jump;
	[HideInInspector]
	public float time;
	[HideInInspector]
	public float timeJump;

	public bool isJumpDown;

	[Space (10)]
	[Header ("Переключения управления на мобильое.")]
	public bool Mobile;
	[Header ("Мобильные компоненты управления.")]
	//для мобильного упровления чувствительность
	[Header ("Чувствительность мобильной камеры.")]
	public float FixedMouse;
	[Space (5)]
	[Header ("Джойстик для передвижения.")]
	public MoviJoystick moveJoystick;
	[Space (5)]
	[Header ("Джойстик для движения камерой.")]
	public FixedTouchFild touchFild;

	//Метод старт в котором я переменным указываю компонент других скриптов для дальнеёшего их использования
	void Start ()
	{
		characterState = GetComponent<CharacterState> ();
		characterAnimation = GetComponent<CharacterAnimation> ();
		characterMovement = GetComponent<CharacterMovement> ();
	}
	// главный метод работы инпута , в него указываю все свои методы
	public void InputUpdate ()
	{
		// компонент не обходимый не зависимо от вида упровления мобильного или пк 
		CheckBattleOrCrouchUpdate ();
		JumpUpdate ();
		BattelUpdate ();
		SprintUpdate ();
		JumpDownOnWallUpdate ();
		if (!Mobile) {
			// метод упровления с пк
			InputPC ();
		} else {
			// метод управления с мобильного 
			InputMobile ();
		}
	}

	// регион упровления пк

	#region PCInput

	//центральынй метод мобильного управления в котором задействованы все матоды относящиеся к
	// пк управлению
	void InputPC ()
	{
		PCLocomotionUpdate ();
		PCJumpUpdate ();
		PCCrouchUpdate ();
		if (!characterMovement.OnWall) {
			PCBattelUpdate ();
			PCAttackUpdate ();
			PCSprintState ();
		}
	}
	// стандартное управления
	void PCLocomotionUpdate ()
	{
		Vertical = Input.GetAxis ("Vertical");
		Horizontal = Input.GetAxis ("Horizontal");

		MouseX = Input.GetAxis ("Mouse X");
		MouseY = Input.GetAxis ("Mouse Y");
	}
	// режим битвы
	void PCBattelUpdate ()
	{
		if (!characterState.isCrouchEmpty) {
			if (Input.GetMouseButtonDown (1)) {
				battleID += 1;
			} 
		}
	}
	//метод быстрого бега
	void PCSprintState ()
	{
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			sprintID += 1;
		}
	}
	// метод аттаки
	void PCAttackUpdate ()
	{

		if (characterState.isBattle) {
			if (Input.GetMouseButtonDown (0)) {
				characterAnimation.AttackUpdate ();
			}

		}
	}
	//метод прыжка
	void PCJumpUpdate ()
	{
		if (!characterState.isBattle) {
			if (!characterState.isCrouchEmpty) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					jump = true;

				}
			}
		}
	}
	//метод прыжка
	void PCCrouchUpdate ()
	{
		if (!characterState.OnWall) {
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
		} else {
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				isJumpDown = true;
			}
		}
	}

	#endregion

	#region MobileInput

	// центральный скрип обработки управления с мобильного
	void InputMobile ()
	{
		MoboilLocomotionUpdate ();
		MCrouchUpdate ();
	}

	void MoboilLocomotionUpdate ()
	{
		Vertical = moveJoystick.inputStick.y;
		Horizontal = moveJoystick.inputStick.x;

		MouseX = touchFild.touchDist.x / FixedMouse;
		MouseY = touchFild.touchDist.y / FixedMouse;
	}

	public void OnBattleState ()
	{
		if (!characterState.isCrouchEmpty) {
			battleID += 1;
		}
	}

	public void OnAttackState ()
	{
		if (characterState.isGroundet) {
			characterAnimation.AttackUpdate ();
		}
	}

	public void OnJompState ()
	{
		jump = true;
	}

	public void OnSprintState ()
	{
		sprintID += 1;
	}

		
	//	void SprintUpdate ()
	//	{
	//		if (characterState.isGroundet) {
	//			if (diveID != 0) {
	//				time += Time.deltaTime;
	//				characterState.isDive = true;
	//				if (time > 0.1) {
	//					diveID = 0;
	//					time = 0;
	//					characterState.isDive = false;
	//				}
	//
	//			}
	//		} else {
	//			diveID = 0;
	//			characterState.isDive = false;
	//		}
	//	}
	//
	//	public void SprintState ()
	//	{
	//		if (characterState.isGroundet) {
	//			diveID += 1;
	//		}
	//	}

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
		if (!characterState.OnWall) {
			if (!battle) {
				if (characterState.isGroundet) {
					if (!characterState.isBattle) {
						crouchID += 1;
					}
				}
			}
		} else {
			isJumpDown = true;
		}
	}

	#endregion


	void JumpUpdate ()
	{
		if (jump) {
			timeJump += Time.deltaTime;
			if (timeJump > 0.1) {
				jump = false;
				timeJump = 0f;
			}
		}
		if (!characterState.isGroundet && !characterState.OnWall) {
			jump = false;
			timeJump = 0;
		}
		characterState.isJump = jump;
	}

	void JumpDownOnWallUpdate ()
	{
		if (isJumpDown) {
			timeJump += Time.deltaTime;
			if (timeJump > 0.1) {
				isJumpDown = false;
				timeJump = 0f;
			}
		}
	}

	void BattelUpdate ()
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

	void SprintUpdate ()
	{
		if (characterState.isGroundet && !characterState.isCrouchEmpty && !characterState.isCrouch && !characterState.OnWall && characterAnimation.moveAmound != 0) {
			if (sprintID == 0)
				characterState.isSprint = false;
			else if (sprintID == 1)
				characterState.isSprint = true;
			else
				sprintID = 0;
		} else {
			sprintID = 0;
			characterState.isSprint = false;
		}
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


	//Регион работы чекеров ...

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


	#endregion
}
