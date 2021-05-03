using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{

	[Header ("Персонаж на земле.")]
	public bool isGroundet;
	[Header ("Персонаж в быстром беге.")]
	public bool isSprint;
	[Header ("Персонаж в битве.")]
	public bool isBattle;
	[Header ("Персонаж аттакует.")]
	public bool isAttack;
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
