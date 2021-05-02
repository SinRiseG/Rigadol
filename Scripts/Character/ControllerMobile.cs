using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMobile : MonoBehaviour
{


	[Header ("Mobile State")]
	public bool battle;

	[Header ("Convas For Mobile")]
	public GameObject[] AllConvas;
	public GameObject[] locomotionStats;
	public GameObject[] battleStats;

	[Header ("Components")]
	public CharacterInput characterInput;
	public CharacterState characterStatus;


	void Update ()
	{
		allActions ();
		ConvasLocomotion ();
	}

	void allActions ()
	{
		if (characterInput.Mobile) {
			for (int i = 0; i < AllConvas.Length; i++) {
				AllConvas [i].SetActive (true);
			}
		} else {
			for (int i = 0; i < AllConvas.Length; i++) {
				AllConvas [i].SetActive (false);
			}
		}
	}

	void ConvasLocomotion ()
	{
		if (characterInput.Mobile) {
			if (!characterStatus.isBattle) {
				for (int i = 0; i < battleStats.Length; i++) {
					battleStats [i].SetActive (false);
				}
				for (int i = 0; i < locomotionStats.Length; i++) {
					locomotionStats [i].SetActive (true);
				}
			} else {
				for (int i = 0; i < battleStats.Length; i++) {
					battleStats [i].SetActive (true);
				}
				for (int i = 0; i < locomotionStats.Length; i++) {
					locomotionStats [i].SetActive (false);
				}
			}
		}

	}
		
}
