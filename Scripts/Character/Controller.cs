using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinRise
{
	public class Controller : MonoBehaviour
	{
		public BoxCollider boxCollider;
		public CharacterInput characterInput;
		public CharacterAnimation characterAnimation;
		public CharacterMovement characterMovement;

		void Start ()
		{
			boxCollider = AddComponentMenu (BoxCollider);
		}


		void Update ()
		{
			characterInput.InputUpdate ();
			characterAnimation.AnimationUpdate ();
			characterMovement.MoveUpdate ();
		}
	}
}
