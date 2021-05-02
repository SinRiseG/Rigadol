using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{


	public bool isGroundet;
	public bool isSprint;
	public bool isBattle;
	public bool isAttack;
	public bool isDive;
	public bool isJump;
	public bool isCrouch;
	public bool isWall;
	public bool isCrouchEmpty;
	public bool isFlyForwardEmpty;
	//test
	[Header ("Wall")]
	public bool isHang;
	public bool CanHangUp;
}
