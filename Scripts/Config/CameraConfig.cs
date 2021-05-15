using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{

	public float turnSmooth;
	public float pivotSpeed;
	public float Y_rot_speed;
	public float X_rot_speed;
	public float minAngle;
	public float maxAngle;
	public float normalZ;
	public float normalX;
	public float aimZ;
	public float aimX;
	public float normalY;

	[Space (5)]
	[Header ("Расположение камеры в присяди")]
	public float crouchX;
	public float crouchY;
	public float crouchZ;

	[Space (5)]
	[Header ("Расположение камеры на стене")]
	public float OnWallX;
	public float OnWallY;
	public float OnWallZ;
}
