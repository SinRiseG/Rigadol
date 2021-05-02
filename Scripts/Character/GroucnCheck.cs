using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroucnCheck : MonoBehaviour
{

	public CharacterInput characterInput;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (true);
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (false);
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (true);
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (true);
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (false);
	}

	void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnGroundCheck (true);
	}
}

