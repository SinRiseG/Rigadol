using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchCheck : MonoBehaviour
{

	public CharacterInput characterInput;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;
		characterInput.OnCrouchCheck (true);
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnCrouchCheck (false);
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnCrouchCheck (true);
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnCrouchCheck (true);
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnCrouchCheck (false);
	}

	void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnCrouchCheck (true);
	}
}
