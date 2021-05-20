using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHondler : MonoBehaviour
{
	[Header ("Положение самой камеры.")]
	public Transform camTrans;
	[Space (5)]
	[Header ("Положения пивота.")]
	public Transform pivot;
	[Space (5)]
	[Header ("Положения персонажа.")]
	public Transform Character;
	[Space (5)]
	[Header ("Положения камеры холдер.")]
	public Transform mTransform;
	[Space (5)]
	[Header ("Положения камеры холдер.")]
	public Transform targetLook;
	[Space (10)]
	[Header ("Компоненты с персонажа.")]
	public CharacterState characterStatus;
	public CharacterInput characterInput;
	[Header ("Настройки камеры.")]
	public CameraConfig cameraConfig;
	[Space (5)]
	[Header ("Смена позиции камеры от левого плеча")]
	public bool leftPivot;


	[HideInInspector]
	public float delta;
	[HideInInspector]
	public float mouseX;
	[HideInInspector]
	public float mouseY;
	[HideInInspector]
	public float smothX;
	[HideInInspector]
	public float smothY;
	[HideInInspector]
	public float smoothXVelocity;
	[HideInInspector]
	public float smoothYVelocity;
	[HideInInspector]
	public float lookAngle;
	[HideInInspector]
	public float titlAngle;

	public float t;


	void Update ()
	{
		Tick ();
	}

	void Tick ()
	{
		delta = Time.deltaTime;

		HandlePosition ();
		HandleRotation ();

		Vector3 targetPosition = Vector3.Lerp (mTransform.position, Character.position, 1);
		mTransform.position = targetPosition;

		mouseX = characterInput.MouseX;
		mouseY = characterInput.MouseY;

		TargetLook ();
	}

	void HandlePosition ()
	{
		float targetX = cameraConfig.normalX;
		float targetY = cameraConfig.normalY;
		float targetZ = cameraConfig.normalZ;

		if (characterStatus.isAimingMove) {
			targetX = cameraConfig.aimX;
			targetZ = cameraConfig.aimZ;
		}

		if (characterStatus.isCrouch) {
			targetX = cameraConfig.crouchX;
			targetZ = cameraConfig.crouchZ;
			targetY = cameraConfig.crouchY;
		}
		if (characterStatus.OnWall) {
			targetX = cameraConfig.OnWallX;
			targetZ = cameraConfig.OnWallY;
			targetY = cameraConfig.OnWallZ;
		}

		if (leftPivot) {
			targetX = -targetX;
		}

		Vector3 newPivotPosition = pivot.localPosition;
		newPivotPosition.x = targetX;
		newPivotPosition.y = targetY;

		Vector3 newCameraPosition = camTrans.localPosition;
		newCameraPosition.z = targetZ;

		float t = delta * cameraConfig.pivotSpeed;
		pivot.localPosition = Vector3.Lerp (pivot.localPosition, newPivotPosition, t);
		camTrans.localPosition = Vector3.Lerp (camTrans.localPosition, newCameraPosition, t);
	}

	void HandleRotation ()
	{
		
		if (cameraConfig.turnSmooth > 0) {
			smothX = Mathf.SmoothDamp (smothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
			smothY = Mathf.SmoothDamp (smothY, mouseY, ref smoothYVelocity, cameraConfig.turnSmooth);
		} else {
			smothX = mouseX;
			smothY = mouseY;
		}

		lookAngle += smothX * cameraConfig.Y_rot_speed;
		Quaternion targetRot = Quaternion.Euler (0, lookAngle, 0);
		mTransform.rotation = targetRot;

		titlAngle -= smothY * cameraConfig.X_rot_speed;
		titlAngle = Mathf.Clamp (titlAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
		pivot.localRotation = Quaternion.Euler (titlAngle, 0, 0);
	}

	void TargetLook ()
	{

		if (characterStatus.isAimingMove) {
			t += Time.deltaTime;
			if (t > 0.3f) {
				Ray ray = new  Ray (camTrans.position, camTrans.forward * 2000);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					targetLook.position = Vector3.Lerp (targetLook.position, hit.point, Time.deltaTime * 40);
				} else {
					targetLook.position = Vector3.Lerp (targetLook.position, targetLook.transform.forward * 200, Time.deltaTime * 5);
				}
			}
		} else {
			targetLook.localPosition = new Vector3 (0f, 0f, 10f);
			t = 0;
		}
	}
	
}
