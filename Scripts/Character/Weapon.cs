using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

	public WeaponConfigs weaponConfig;
	public Transform shotPoint;
	public Transform targetLook;

	public GameObject cameraMain;
	public GameObject decal;
	public GameObject bullet;

	public ParticleSystem muzzleFlesh;
	AudioSource audioSource;
	public AudioClip shootClip;

	public GameObject Shell;
	public Transform shellPosition;

	public Transform LeftHand;

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void Update ()
	{
		shotPoint.LookAt (targetLook);

		Vector3 origin = shotPoint.position;
		Vector3 dir = targetLook.position;

		//RaycastHit hit;



		//decal.SetActive (false);
		Debug.DrawLine (origin, dir, Color.black);
		Debug.DrawLine (cameraMain.transform.position, dir, Color.black);
//
//		if (Physics.Linecast (origin, dir, out hit)) {
//			//decal.SetActive (true);
//			decal.transform.position = hit.point + hit.normal * 0.01f;
//			decal.transform.rotation = Quaternion.LookRotation (-hit.normal);
//		}
	}

	public void Shoot ()
	{
		Instantiate (bullet, shotPoint.position, shotPoint.rotation);
		audioSource.PlayOneShot (shootClip);
		muzzleFlesh.Play ();

		AddShell ();
	}

	void AddShell ()
	{
		GameObject newShell = Instantiate (Shell);
		newShell.transform.position = shellPosition.position;

		Quaternion rot = shellPosition.rotation;
		newShell.transform.rotation = rot;

		newShell.transform.parent = null;
		newShell.GetComponent<Rigidbody> ().AddForce (-newShell.transform.forward * Random.Range (80, 120));
		Destroy (newShell, 10);
	}
}
