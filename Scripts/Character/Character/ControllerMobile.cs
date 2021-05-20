using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMobile : MonoBehaviour
{

	[Header ("Конвасы мобильного управления относительно состояния персонажа ")]
	public GameObject[] AllConvas;
	[Space (5)]
	public GameObject[] locomotionStats;
	[Space (5)]
	public GameObject[] battleStats;


	private CharacterInput characterInput;
	private CharacterState characterStatus;

	void Start ()
	{
		characterStatus = GetComponent<CharacterState> ();
		characterInput = GetComponent<CharacterInput> ();
	}

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
			if (!characterStatus.isAimingMove) {
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
