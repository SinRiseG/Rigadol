using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{

	private Animator anim;
	private CharacterMovement characterMovement;
	private CharacterInventory characterInventory;
	private CharacterState characterState;
	[Header ("Таргет лук за которым будет следить ИК.")]
	public Transform targetLook;
	[HideInInspector]
	public Transform l_hand;

	public Transform l_hand_target;
	[HideInInspector]
	public Transform r_hand;

	public Quaternion lh_rot;

	public float rh_Weight;
	public float lh_Weight;

	public Transform shoulter;
	public Transform aimPivot;

	public bool LeftWeight;

	void Start ()
	{
		Init ();
		ShoulterCreate ();
	}

	void Init ()
	{
		//Указываем поеск компонентов
		anim = GetComponent<Animator> ();
		characterMovement = GetComponent<CharacterMovement> ();
		characterInventory = GetComponent<CharacterInventory> ();
		characterState = GetComponent<CharacterState> ();
	}

	void ShoulterCreate ()
	{
		//находим кость и назначаем ей объект
		shoulter = anim.GetBoneTransform (HumanBodyBones.RightShoulder).transform;
		// создаём пивот и позиции рук 
		aimPivot = new GameObject ().transform;
		aimPivot.name = "aim Pivot";
		aimPivot.transform.parent = transform;

		r_hand = new GameObject ().transform;
		r_hand.name = "Right Hand";
		r_hand.transform.parent = aimPivot;

		l_hand = new GameObject ().transform;
		l_hand.name = "Left Hand";
		l_hand.transform.parent = aimPivot;

	}

	void Update ()
	{
		if (l_hand_target != null) {
			lh_rot = l_hand_target.rotation;
			l_hand.position = l_hand_target.position;
		}

		if (anim.GetInteger ("WeaponTipe") >= 2) {
			lh_Weight = 1;
			if (characterState.isAiming) {
				rh_Weight += Time.deltaTime * 4;
			} else {
				rh_Weight -= Time.deltaTime * 4;
			}
			rh_Weight = Mathf.Clamp (rh_Weight, 0, 1);
			lh_Weight = Mathf.Clamp (lh_Weight, 0, 1);
		} else {
			if (characterState.isAiming) {
				rh_Weight += Time.deltaTime * 4;
				lh_Weight += Time.deltaTime * 4;
			} else {
				rh_Weight -= Time.deltaTime * 4;
				lh_Weight -= Time.deltaTime * 4;
			}
			rh_Weight = Mathf.Clamp (rh_Weight, 0, 1);
			lh_Weight = Mathf.Clamp (lh_Weight, 0, 1);
		}	
	}

	void OnAnimatorIK ()
	{
		if (!characterState.OnWall) {
			aimPivot.position = shoulter.position;
			if (characterState.isAiming) {
				aimPivot.LookAt (targetLook);

				anim.SetLookAtWeight (1f, .4f, 1f);
				anim.SetLookAtPosition (targetLook.position);

				anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, lh_Weight);
				anim.SetIKPosition (AvatarIKGoal.LeftHand, l_hand.position);
				anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, lh_Weight);
				anim.SetIKRotation (AvatarIKGoal.LeftHand, lh_rot);

				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, rh_Weight);
				anim.SetIKPosition (AvatarIKGoal.RightHand, r_hand.position);
				anim.SetIKRotationWeight (AvatarIKGoal.RightHand, rh_Weight);
				anim.SetIKRotation (AvatarIKGoal.RightHand, r_hand.rotation);
			} else {
				anim.SetLookAtWeight (.3f, .3f, .8f);
				anim.SetLookAtPosition (targetLook.position);

				anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, lh_Weight);
				anim.SetIKPosition (AvatarIKGoal.LeftHand, l_hand.position);
				anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, lh_Weight);
				anim.SetIKRotation (AvatarIKGoal.LeftHand, lh_rot);

				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, rh_Weight);
				anim.SetIKPosition (AvatarIKGoal.RightHand, r_hand.position);
				anim.SetIKRotationWeight (AvatarIKGoal.RightHand, rh_Weight);
				anim.SetIKRotation (AvatarIKGoal.RightHand, r_hand.rotation);
			}
		}
	}
}
