using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
	private Animator anim;
	private CharacterIK characterIK;
	private CharacterInput characterInput;

	public Transform targetLook;
	public GameObject mCamera;
	public Transform rHand;

	public WeaponConfigs firstWeapon;
	public WeaponConfigs secondWeapon;

	public  GameObject objWeapon;
	Weapon activeWeapon;

	void Start ()
	{
		Init ();
	}

	void Init ()
	{
		anim = GetComponent<Animator> ();
		characterIK = GetComponent<CharacterIK> ();
		characterInput = GetComponent<CharacterInput> ();
	}

	public void InventoryUpdate ()
	{
		Ray ray = new Ray (mCamera.transform.position, mCamera.transform.forward * 5f);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.tag == "Item") {
				if (Input.GetKeyDown (KeyCode.E)) {
					Instantiate (firstWeapon.prefabItems, hit.collider.transform.position, hit.collider.transform.rotation);
					DestroyWeapon ();
					firstWeapon = hit.collider.GetComponent<Item> ().weaponConfigsItem;
					characterInput.SelWeapon = 2;
					anim.SetTrigger ("Select");
					Destroy (hit.collider.gameObject);
				}
			}
		}

	}


	public void SelectWeapon (int selWeapon)
	{
		if (selWeapon == 1) {
			DestroyWeapon ();
		}
		if (selWeapon == 2) {
			objWeapon = Instantiate (firstWeapon.WeaponPrefabs);
			activeWeapon = objWeapon.GetComponent<Weapon> ();
			objWeapon.transform.parent = rHand;
			objWeapon.transform.localPosition = firstWeapon.Weapon_pos;
			objWeapon.transform.localRotation = Quaternion.Euler (firstWeapon.Weapon_rot);

			characterIK.r_hand.localPosition = firstWeapon.rHandPos;
			Quaternion rotRight = Quaternion.Euler (firstWeapon.rHandRot.x, firstWeapon.rHandRot.y, firstWeapon.rHandRot.z);
			characterIK.r_hand.localRotation = rotRight;

			activeWeapon.targetLook = targetLook;
			activeWeapon.cameraMain = mCamera;
			characterInput.weapon = activeWeapon;
			characterIK.l_hand_target = activeWeapon.LeftHand;

			anim.SetBool ("Weapon", true);
			anim.SetInteger ("WeaponTipe", 2);
		}
		if (selWeapon == 3) {
			objWeapon = Instantiate (secondWeapon.WeaponPrefabs);
			activeWeapon = objWeapon.GetComponent<Weapon> ();
			objWeapon.transform.parent = rHand;
			objWeapon.transform.localPosition = secondWeapon.Weapon_pos;
			objWeapon.transform.localRotation = Quaternion.Euler (secondWeapon.Weapon_rot);

			characterIK.r_hand.localPosition = secondWeapon.rHandPos;
			Quaternion rotRight = Quaternion.Euler (secondWeapon.rHandRot.x, secondWeapon.rHandRot.y, secondWeapon.rHandRot.z);
			characterIK.r_hand.localRotation = rotRight;

			activeWeapon.targetLook = targetLook;
			activeWeapon.cameraMain = mCamera;
			characterInput.weapon = activeWeapon;
			characterIK.l_hand_target = activeWeapon.LeftHand;

			anim.SetBool ("Weapon", true);
			anim.SetInteger ("WeaponTipe", 1);
		}
	}

	public void DestroyWeapon ()
	{
		characterInput.weapon = null;
		characterIK.l_hand_target = null;
		Destroy (objWeapon);
		anim.SetBool ("Weapon", false);
		anim.SetInteger ("WeaponTipe", 0);
	}

}
