using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvectoryField : MonoBehaviour
{

	public CharacterInventory characterInventory;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Item") {
			characterInventory.itemOnTheGround.Add (other.transform.GetComponent<Item> ());
			characterInventory.ItGroundetUpdate ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Item") {
			characterInventory.itemOnTheGround.Remove (other.transform.GetComponent<Item> ());
			characterInventory.ItGroundetUpdate ();
		}
	}
}
