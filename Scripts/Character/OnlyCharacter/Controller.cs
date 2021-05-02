using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
	private CharacterInput characterInput;
	private CharacterAnimation characterAnimation;
	private CharacterMovement characterMovement;

	void Start ()
	{
		characterInput = GetComponent<CharacterInput> ();
		characterAnimation = GetComponent<CharacterAnimation> ();
		characterMovement = GetComponent <CharacterMovement> ();
	}

	void Update ()
	{
		characterInput.InputUpdate ();
		characterAnimation.AnimationUpdate ();
		characterMovement.MoveUpdate ();
	}
}
