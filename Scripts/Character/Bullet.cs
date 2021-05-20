using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public int Speed;
	Vector3 lastPos;

	public GameObject decal;

	public GameObject metalHitEffeck;
	public GameObject sandHitEffeck;
	public GameObject stoneHitEffeck;
	public GameObject[] meatHitEffeck;
	public GameObject woodHitEffeck;

	void Start ()
	{
		lastPos = transform.position;
		Destroy (gameObject, 10);
	}


	void Update ()
	{
		transform.Translate (Vector3.forward * Speed * Time.deltaTime);

		RaycastHit hit;

		Debug.DrawLine (lastPos, transform.position);
		if (Physics.Linecast (lastPos, transform.position, out hit)) {
			if (hit.collider.sharedMaterial != null) {
				string materialName = hit.collider.sharedMaterial.name;
				switch (materialName) {
				case"Metal":
					SpawnDecal (hit, metalHitEffeck);
					break;
				case"Sand":
					SpawnDecal (hit, sandHitEffeck);
					break;
				case"Stone":
					SpawnDecal (hit, stoneHitEffeck);
					break;
				case "Wood":
					SpawnDecal (hit, woodHitEffeck);
					break;
				case"Meat":
					SpawnDecal (hit, meatHitEffeck [Random.Range (0, meatHitEffeck.Length)]);
					break;
				}
			}
			Destroy (gameObject);
		}
		lastPos = transform.position;

	}

	void SpawnDecal (RaycastHit hit, GameObject prefab)
	{
		GameObject spawndecal = GameObject.Instantiate (prefab, hit.point, Quaternion.LookRotation (hit.normal));
		spawndecal.transform.SetParent (hit.collider.transform);
		Destroy (spawndecal.gameObject, 10);
	}
		

}
