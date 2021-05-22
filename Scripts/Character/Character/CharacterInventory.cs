using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
	private Animator anim;
	private CharacterIK characterIK;
	private CharacterInput characterInput;
	[Space (3)]
	[Header ("Открыт инвентарь или нет.")]
	public bool InventoryOpen;
	[Space (3)]
	[Header ("Трансформ targetLook, цели за которой следит персонаж.")]
	public Transform targetLook;
	[Space (3)]
	[Header ("Камера персонажа.")]
	public GameObject mCamera;
	[Space (3)]
	[Header ("Трансформ правой руки.")]
	public Transform rHand;
	[Space (3)]
	[Header ("Список вещей в инвенторе.")]
	public List<Item> item = new List<Item> ();
	[Space (3)]
	[Header ("Список вещей на земле.")]
	public List<Item> itemOnTheGround = new List<Item> ();
	[Space (3)]
	[Header ("Игровой объект инвентарь.")]
	public GameObject inventory;
	[Space (3)]
	[Header ("Объект ячейки.")]
	public GameObject cell;
	[Space (3)]
	[Header ("Родители присвоения ячеек.")]
	public Transform parentCell;
	[Space (2)]
	public Transform parentCellGround;
	[Space (2)]
	public Transform firstWeaponCell;
	[Space (2)]
	public Transform secondWeaponCell;
	[Space (3)]
	[Header ("Первый слот оружия.")]
	public WeaponConfigs firstWeapon;
	[Space (3)]
	[Header ("Второй слот оружия.")]
	public WeaponConfigs secondWeapon;
	[Space (3)]
	[Header ("Объект созданный в руках.")]
	public  GameObject objWeapon;

	public SphereCollider sphereCollider;
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
		InvectoryActive ();
		Ray ray = new Ray (mCamera.transform.position, mCamera.transform.forward * 5f);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.tag == "Item") {
				if (Input.GetKeyDown (KeyCode.E)) {
					if (hit.collider.GetComponent<Item> ().typeItem == "Weapon") {
						Instantiate (firstWeapon.prefabItems, hit.collider.transform.position, hit.collider.transform.rotation);
						DestroyWeapon ();
						firstWeapon = hit.collider.GetComponent<Item> ().weaponConfigsItem;
						characterInput.SelWeapon = 2;
						anim.SetTrigger ("Select");
						Destroy (hit.collider.gameObject);
					} else if (hit.collider.GetComponent<Item> ().typeItem == "Other") {
						item.Add (hit.collider.GetComponent<Item> ());
						Destroy (hit.transform.gameObject);
					}


					sphereCollider.enabled = false;
					itemOnTheGround.Clear ();
					sphereCollider.enabled = true;
					SortItem ();
					ItGroundetUpdate ();
				}
			}
		}

	}

	public void SortItem ()
	{
		for (int i = 0; i < item.Count; i++) {
			for (int j = 1 + 1; j < item.Count; j++) {
				if (item [i].number > item [j].number) {
					Item t = item [i];
					item [i] = item [j];
					item [j] = t;
				}
			}
		}
		for (int i = 0; i < parentCell.childCount; i++) {
			if (parentCell.transform.childCount > 0) {
				Destroy (parentCell.transform.GetChild (i).gameObject);
			}	
		}

		if (firstWeapon == null) {
			if (firstWeaponCell.childCount > 0) {
				Destroy (firstWeaponCell.transform.GetChild (0).gameObject);
				DestroyWeapon ();
			}
		}

		if (secondWeapon == null) {
			if (secondWeaponCell.childCount > 0) {
				Destroy (secondWeaponCell.transform.GetChild (0).gameObject);
				DestroyWeapon ();
			}
		}

		inventory.SetActive (true);

		int count = item.Count;
		for (int i = 0; i < count; i++) {

			Item it = item [i];

			GameObject newCell = Instantiate (cell);
			Drag drag;
			newCell.transform.SetParent (parentCell);
			newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.spritaPath);
			drag = newCell.GetComponent<Drag> ();
			drag.item = it;
			drag.typeList = "Inventory";
			newCell.transform.GetChild (1).GetComponent<Text> ().text = drag.item.nameItem;
		}
	}

	public void crutchSortItem ()
	{
		Invoke ("SortItem", 0.01f);
	}

	public void InvectoryActive ()
	{
		if (Input.GetKeyDown (KeyCode.I)) {
			if (inventory.activeSelf) {
				inventory.SetActive (false);
				InventoryOpen = false;
				for (int i = 0; i < parentCell.childCount; i++) {
					if (parentCell.transform.childCount > 0) {
						Destroy (parentCell.transform.GetChild (i).gameObject);
					}	
				}
			} else {
				InventoryOpen = true;
				SortItem ();
			}
		}
	}

	public void ItGroundetUpdate ()
	{
		for (int i = 0; i < itemOnTheGround.Count; i++) {
			for (int j = 1 + 1; j < itemOnTheGround.Count; j++) {
				if (itemOnTheGround [i].number > itemOnTheGround [j].number) {
					Item t = itemOnTheGround [i];
					itemOnTheGround [i] = itemOnTheGround [j];
					itemOnTheGround [j] = t;
				}
			}
		}

		int count = itemOnTheGround.Count;

		for (int i = 0; i < parentCellGround.childCount; i++) {
			if (parentCellGround.transform.childCount > 0) {
				Destroy (parentCellGround.transform.GetChild (i).gameObject);
			}

		}
		for (int i = 0; i < count; i++) {
			Item it = itemOnTheGround [i];

			GameObject newCell = Instantiate (cell);
			Drag drag;
			newCell.transform.SetParent (parentCellGround);
			newCell.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.spritaPath);
			drag = newCell.GetComponent<Drag> ();
			drag.item = it;
			drag.typeList = "Ground";
			newCell.transform.GetChild (1).GetComponent<Text> ().text = drag.item.nameItem;
		}
	}

	public void RemoveItem (Drag drag)
	{
		Item it = drag.item;
		GameObject newObj = Instantiate<GameObject> (Resources.Load<GameObject> (it.prefabPath));
		newObj.transform.position = transform.position + transform.forward + transform.up;
		Destroy (drag.gameObject);
		item.Remove (it);
  
	}

	public void UseItem (Drag drag)
	{
		print ("Use: " + drag.item.nameItem);
	}

	public void TakeItem (Drag drag)
	{
		item.Add (drag.item);
		Destroy (drag.item.gameObject);

		sphereCollider.enabled = false;
		itemOnTheGround.Clear ();
		sphereCollider.enabled = true;
		SortItem ();
		ItGroundetUpdate ();
	}

	public void TakeWeapon (Drag drag)
	{
		Item it = drag.item;

		if (firstWeaponCell.childCount > 0) {
			GameObject newObj = Instantiate<GameObject> (Resources.Load<GameObject> (firstWeaponCell.GetChild (0).GetComponent<Drag> ().item.prefabPath));
			newObj.transform.position = transform.position + transform.forward + transform.up;

			Drag dragFW = firstWeaponCell.GetChild (0).GetComponent<Drag> ();

			dragFW.item.typeItem = "First Weapon";
			dragFW.typeList = "Ground";

			dragFW.item = drag.item;
			dragFW.item.typeItem = "Use W";
			dragFW.typeList = "Inventory";
			firstWeapon = dragFW.item.weaponConfigsItem;
			firstWeaponCell.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.spritaPath);
			Destroy (drag.item.gameObject);
			DestroyWeapon ();
			SelectWeapon (2);
		} else {
			GameObject newCell = Instantiate (cell);
			newCell.transform.SetParent (firstWeaponCell);
			Drag newDrag = newCell.GetComponent<Drag> ();
			newDrag.item = drag.item;
			newDrag.item.typeItem = "Use W";
			newDrag.typeList = "Inventory";
			firstWeapon = drag.item.weaponConfigsItem;
			firstWeaponCell.GetChild (0).transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load<Sprite> (it.spritaPath);

			Destroy (drag.item.gameObject);
		}

		sphereCollider.enabled = false;
		itemOnTheGround.Clear ();
		sphereCollider.enabled = true;
		SortItem ();
		ItGroundetUpdate ();
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
