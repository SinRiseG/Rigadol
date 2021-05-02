using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangCheck : MonoBehaviour
{

	public CharacterInput characterInput;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Helpers") {
			characterInput.OnHangCheck (true);
			characterInput.OnHangTransform (other.transform);
			characterInput.OnHelpersPosition (other.GetComponent<HangHelpers> ().Positionhelperst);

		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Helpers") {
			characterInput.OnHangCheck (false);
			characterInput.OnHangTransform (null);
			characterInput.OnHelpersPosition (other.GetComponent<HangHelpers> ().Positionhelperst);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Helpers") {
			characterInput.OnHangCheck (true);
			characterInput.OnHangTransform (other.transform);
			characterInput.OnHelpersPosition (other.GetComponent<HangHelpers> ().Positionhelperst);
		}

	}


}
