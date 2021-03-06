using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
	//Компаненты приватные
	private CharacterState characterState;
	private CharacterAnimation characterAnimation;
	private CharacterMovement characterMovement;
	private CharacterInventory characterInventory;
	private Animator anim;

	public Weapon weapon;
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
	[HideInInspector]
	public bool isJumpDown;

	[Space (10)]
	[Header ("Переключения управления на мобильое.")]
	public bool Mobile;
	[Header ("Дебаг прицела.")]
	public bool AimingDebug;
	//для мобильного упровления чувствительность
	[Header ("Чувствительность мобильной камеры.")]
	public float FixedMouse;
	[Space (5)]
	[Header ("Джойстик для передвижения.")]
	public MoviJoystick moveJoystick;
	[Space (5)]
	[Header ("Джойстик для движения камерой.")]
	public FixedTouchFild touchFild;
	[Space (5)]
	[Header ("Какое оружие нажато.")]
	public int SelWeapon = 1;


	public bool opportunityToAim;
	public float distance;

	//Метод старт в котором я переменным указываю компонент других скриптов для дальнеёшего их использования
	void Start ()
	{
		characterState = GetComponent<CharacterState> ();
		characterAnimation = GetComponent<CharacterAnimation> ();
		characterMovement = GetComponent<CharacterMovement> ();
		characterInventory = GetComponent<CharacterInventory> ();
		anim = GetComponent<Animator> ();
	}
	// главный метод работы инпута , в него указываю все свои методы
	public void InputUpdate ()
	{
		// компонент не обходимый не зависимо от вида упровления мобильного или пк 
		//CheckBattleOrCrouchUpdate ();
		JumpUpdate ();
		BattelUpdate ();
		SprintUpdate ();
		//ShootUpdate ();
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
			PCAimingUpdate ();
			PCAttackUpdate ();
			PCSprintState ();
			PCInputSelectWeapon ();
		}
	}
	// стандартное управления
	void PCLocomotionUpdate ()
	{
		Vertical = Input.GetAxis ("Vertical");
		Horizontal = Input.GetAxis ("Horizontal");
		if (!characterInventory.InventoryOpen) {
			MouseX = Input.GetAxis ("Mouse X");
			MouseY = Input.GetAxis ("Mouse Y");
		} else {
			MouseX = 0;
			MouseY = 0;
		}
	}
	// режим битвы
	void PCAimingUpdate ()
	{
		if (anim.GetBool ("Weapon")) {
			if (!AimingDebug) {
				if (Input.GetMouseButton (1) && opportunityToAim) {
					characterState.isAiming = true;
					characterState.isAimingMove = true;
				}
				if (Input.GetMouseButton (1) && !opportunityToAim) {
					characterState.isAiming = false;
					characterState.isAimingMove = true;
				}
				if (!Input.GetMouseButton (1)) {
					characterState.isAiming = false;
					characterState.isAimingMove = false;
				}
			} else {
				characterState.isAiming = true;
				characterState.isAimingMove = true;
			}
		}
	}
	//Проверка и нажатия кнопок для смены оружия
	public void PCInputSelectWeapon ()
	{
		if (!anim.GetBool ("Aiming")) {
			if (Input.GetKeyDown (KeyCode.Alpha1) && SelWeapon != 1) {
				SelWeapon = 1;
				anim.SetTrigger ("Select");
			}
			if (Input.GetKeyDown (KeyCode.Alpha2) && SelWeapon != 2) {
				SelWeapon = 2;
				anim.SetTrigger ("Select");
			}
			if (Input.GetKeyDown (KeyCode.Alpha3) && SelWeapon != 3) {
				SelWeapon = 3;
				anim.SetTrigger ("Select");
			}
		}

	}
	//метод для анимации
	public void SelectWeapon ()
	{
		characterInventory.DestroyWeapon ();
		characterInventory.SelectWeapon (SelWeapon);
	}

	//метод быстрого бега
	void PCSprintState ()
	{
		if (!characterState.isCrouch) {
			characterState.isSprint = Input.GetKey (KeyCode.LeftShift);
		} else {
			characterState.isSprint = false;
		}
	}
	// метод аттаки
	void PCAttackUpdate ()
	{
		if (Input.GetMouseButtonDown (0) && Input.GetMouseButton (1) && opportunityToAim) {
			weapon.Shoot ();
		}
	}
	//метод прыжка
	void PCJumpUpdate ()
	{
		if (!characterState.isAimingMove) {
			if (!characterState.isCrouchEmpty) {
				//if (Input.GetKeyDown (KeyCode.Space)) {
				//jump = true;
				characterState.isJump = Input.GetKeyDown (KeyCode.Space);
				//}
			}
		}
	}
	//метод прыжка
	void PCCrouchUpdate ()
	{
		if (!characterState.OnWall) {
			if (characterState.isGroundet) {	 
				if (Input.GetKeyDown (KeyCode.LeftControl)) {
					crouchID += 1;
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
			//characterAnimation.AttackUpdate ();
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

		
	//		void SprintUpdate ()
	//		{
	//			if (characterState.isGroundet) {
	//				if (diveID != 0) {
	//					time += Time.deltaTime;
	//					characterState.isDive = true;
	//					if (time > 0.1) {
	//						diveID = 0;
	//						time = 0;
	//						characterState.isDive = false;
	//					}
	//
	//				}
	//			} else {
	//				diveID = 0;
	//				characterState.isDive = false;
	//			}
	//		}
	//
	//		public void SprintState ()
	//		{
	//			if (characterState.isGroundet) {
	//				diveID += 1;
	//			}
	//		}

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
					if (!characterState.isAimingMove) {
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
		RayCastAiming ();
//		if (opportunityToAim) {
//			if (characterState.isGroundet) {
//				if (battleID == 0) {
//					characterState.is = false;
//				} else if (battleID == 1) {
//					characterState.isBattle = true;
//				} else {
//					battleID = 0;
//				}
//			} else {
//				battleID = 0;
//				characterState.isBattle = false;
//			}
//		} else {
//			battleID = 0;
//			characterState.isBattle = false;
//		}
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

	//	void CheckBattleOrCrouchUpdate ()
	//	{
	//		if (!characterState.isCrouchEmpty) {
	//			if (characterState.isBattle) {
	//				crouchID = 0;
	//				battle = true;
	//			} else {
	//				battle = false;
	//			}
	//		} else {
	//			battle = false;
	//			battleID = 0;
	//			crouchID = 1;
	//		}
	//	}

	//	void ShootUpdate ()
	//	{
	//		if (characterState.isAttack)
	//			weapon.Shoot ();
	//	}

	void RayCastAiming ()
	{
		Debug.DrawLine (transform.position + transform.up * 1.4f, characterInventory.targetLook.position, Color.green);

		distance = Vector3.Distance (transform.position + transform.up * 1.4f, characterInventory.targetLook.position);
		if (distance > 1.5f) {
			opportunityToAim = true;
		} else {
			opportunityToAim = false;
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
