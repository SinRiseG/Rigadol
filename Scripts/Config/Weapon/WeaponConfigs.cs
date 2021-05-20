using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Weapon/Config")]
public class WeaponConfigs : ScriptableObject
{

	[Header ("Позиция правой руки и поворот.")]
	public Vector3 rHandPos;
	public Vector3 rHandRot;
	[Space (3)]
	[Header ("Прифаб оружия.")]
	public GameObject WeaponPrefabs;
}
