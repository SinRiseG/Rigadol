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
	//айди кнопки кувырка
	int diveID;
	//айди кнопки присяда
	int crouchID;
	//Показатель состояния персонажа приседа
	bool crouch;
	//Показатель состояния прыжка
	[HideInInspector]
	public bool jump;
	[HideInInspector]
	public float time;

	[Space (10)]
	[Header ("Переключения управления на мобильое ")]
	public bool Mobile;
	[Header ("Мобильные компоненты управления")]
	//для мобильного упровления чувствительность
	[Header ("Чувствительность мобильной камеры ")]
	public float FixedMouse;
	[Space (5)]
	[Header ("Джойстик для передвижения")]
	public MoviJoystick moveJoystick;
	[Space (5)]
	[Header ("Джойстик для движения камерой")]
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
		if (!characterMovement.OnWall) {
			PCBattelUpdate ();
			PCAttackUpdate ();
			PCCrouchUpdate ();
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

	// метод аттаки
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
					jump = true;

				}
			}
		}
	}
	//метод прыжка
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

	// центральный скрип обработки управления с мобильного
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
		jump = true;
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

	void JumpUpdate ()
	{
		characterState.isJump = jump;
		float time = 0f;
		if (jump) {
			time += Time.deltaTime;
			if (time > 0.1) {
				jump = false;
				time = 0f;
			}
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
