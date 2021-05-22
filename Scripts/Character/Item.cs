using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[Header ("Тип Вещи.")]
	public string typeItem;
	[Space (5)]
	[Header ("Имя Вещи.")]
	public string nameItem;
	[Space (5)]
	[Header ("Индекс объекта.")]
	public int number;
	[Space (5)]
	[Header ("Путь к префабу.")]
	public string spritaPath;
	[Space (5)]
	[Header ("Путь к иконке.")]
	public string prefabPath;
	[Space (5)]
	[Header ("Если оружие то настройки оружия.")]
	public WeaponConfigs weaponConfigsItem;
}
