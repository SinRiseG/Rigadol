using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{

	[Header ("Персонаж на земле.")]
	public bool isGroundet;
	[Header ("Персонаж в быстром беге.")]
	public bool isSprint;
	[Header ("Прицеливание.")]
	public bool isAiming;
	[Header ("Передвижения в прицеливании")]
	public bool isAimingMove;
	[Header ("Персонаж аттакует.")]
	public bool isShoot;
	[Header ("Персонаж в прыжке.")]
	public bool isJump;
	[Header ("Персонаж в присяди.")]
	public bool isCrouch;
	[Header ("Персонаж на стене.")]
	public bool OnWall;
	[Header ("Персонаж не может встать.")]
	public bool isCrouchEmpty;
	[Header ("Персонаж не может лететь в перёд.")]
	public bool isFlyForwardEmpty;

}
