using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

	public CharacterInput characterInput;
	public CharacterAnimation characterAnimation;
	public CharacterMovement characterMovement;

	void Update ()
	{
		characterInput.InputUpdate ();
		characterAnimation.AnimationUpdate ();
		characterMovement.MoveUpdate ();
	}
}
