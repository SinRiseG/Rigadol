using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCheck : MonoBehaviour
{


	public CharacterInput characterInput;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (true);
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (false);
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (true);
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (true);
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (false);
	}

	void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject == characterInput.gameObject)
			return;

		characterInput.OnFlyCheck (true);
	}
}
