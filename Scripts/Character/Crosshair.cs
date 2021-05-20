using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{

	public float CurrentSpread;
	public float speedSpread;

	public Parts[] parts;
	public CharacterAnimation characterAnimation;
	public CharacterState characterState;

	float t;
	float curSpread;

	void Update ()
	{
		if (!characterState.isAimingMove) {
			if (characterAnimation.moveAmound > 0) {
				CurrentSpread = 20 * (5 + characterAnimation.moveAmound);
			} else {
				CurrentSpread = 20f;
			}
		} else {
			if (characterAnimation.moveAmound > 0) {
				CurrentSpread = 20 * (3 + characterAnimation.moveAmound);
			} else {
				CurrentSpread = 20f;
			}
		}

		CrosshairUpdate ();
	}

	public void CrosshairUpdate ()
	{
		t = 0.005f * speedSpread;
		curSpread = Mathf.Lerp (curSpread, CurrentSpread, t);

		for (int i = 0; i < parts.Length; i++) {
			Parts p = parts [i];
			p.trans.anchoredPosition = p.pos * curSpread;
		}
	}

	[System.Serializable]
	public class Parts
	{
		public RectTransform trans;
		public Vector2 pos;
	}
}
