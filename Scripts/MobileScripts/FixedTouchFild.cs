using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchFild : MonoBehaviour , IPointerDownHandler,IPointerUpHandler
{

	[HideInInspector]
	public Vector2 touchDist;
	Vector2 PointerOld;

	int PointerId;

	public bool Pressed;

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		Pressed = false;
	}

	#endregion

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		Pressed = true;
		PointerId = eventData.pointerId;
		PointerOld = eventData.position;
	}

	#endregion

	private void Update ()
	{
		if (Pressed) {
			if (PointerId >= 0 && PointerId < Input.touches.Length) {
				touchDist = Input.touches [PointerId].position - PointerOld;
				PointerOld = Input.touches [PointerId].position;

			} else {
				touchDist = new Vector2 (Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
				PointerOld = Input.mousePosition;
			}
		} else {
			touchDist = new Vector2 ();
		}
	}


	
}
