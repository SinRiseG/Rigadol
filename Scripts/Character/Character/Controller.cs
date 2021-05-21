using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
	private CharacterInput characterInput;
	private CharacterAnimation characterAnimation;
	private CharacterMovement characterMovement;
	private CharacterInventory characterInventory;

	void Start ()
	{
		characterInput = GetComponent<CharacterInput> ();
		characterAnimation = GetComponent<CharacterAnimation> ();
		characterMovement = GetComponent <CharacterMovement> ();
		characterInventory = GetComponent<CharacterInventory> ();
	}

	void Update ()
	{
		characterInput.InputUpdate ();
		characterAnimation.AnimationUpdate ();
		characterMovement.MoveUpdate ();
		characterInventory.InventoryUpdate ();
	}
}
