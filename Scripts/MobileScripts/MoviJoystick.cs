using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoviJoystick : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
	[Header ("Площадка для джойстика")]
	public Image PlaneForJoystic;
	[Space (5)]
	[Header ("Кортинка джойстика")]
	public Image joistick;
	[Space (5)]
	[Header ("Кортинка стика")]
	public Image stick;
	[Space (5)]
	[Header ("Игровой объект джойстик ")]
	public GameObject joustickGameObject;

	[HideInInspector]
	public Vector2 posJoystic;
	[HideInInspector]
	public Vector2 inputStick;
	[Space (5)]
	[Header ("Чувствительность")]
	public float Sensetive;

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
		joustickGameObject.SetActive (false);
		posJoystic = Vector2.zero;
		inputStick = Vector2.zero;
		joistick.rectTransform.anchoredPosition = Vector2.zero;
		stick.rectTransform.anchoredPosition = Vector2.zero;
	}

	#endregion



	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		joistick.rectTransform.anchoredPosition = new Vector2 (posJoystic.x * (PlaneForJoystic.rectTransform.sizeDelta.x), posJoystic.y * (PlaneForJoystic.rectTransform.sizeDelta.y));

		Vector2 pos;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (joistick.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
			
			pos.x = (pos.x / joistick.rectTransform.sizeDelta.x);
			pos.y = (pos.y / joistick.rectTransform.sizeDelta.y);

			inputStick = new Vector2 (pos.x * Sensetive, pos.y * Sensetive);
			inputStick = (inputStick.magnitude > 1.0f) ? inputStick.normalized : inputStick;

			stick.rectTransform.anchoredPosition = new Vector2 (inputStick.x * (joistick.rectTransform.sizeDelta.x / 2), inputStick.y * (joistick.rectTransform.sizeDelta.y / 2));
		}

	}

	#endregion

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		joustickGameObject.SetActive (true);
		Vector2 safePos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (joistick.rectTransform, eventData.position, eventData.pressEventCamera, out safePos)) {
			safePos.x = (safePos.x / PlaneForJoystic.rectTransform.sizeDelta.x);
			safePos.y = (safePos.y / PlaneForJoystic.rectTransform.sizeDelta.y);
			posJoystic = safePos;
		}
		joistick.rectTransform.anchoredPosition = new Vector2 (posJoystic.x * (PlaneForJoystic.rectTransform.sizeDelta.x), posJoystic.y * (PlaneForJoystic.rectTransform.sizeDelta.y));
	}

	#endregion


}
