using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Mover : EventTrigger {

	public bool isLeftMover = false;

	bool canMove = false;

	GameObject main;

	public RectTransform background;

	void Start(){
		
		if (name.Equals("Left")) {
			isLeftMover = true;
		}

		main = GameObject.Find ("Main");
	}

	void Update()
	{
		if (!canMove)
			return;

		float xLeftBg = background.position.x - background.rect.width / 2f;
		float xLeftMov = GetComponent<RectTransform> ().position.x - GetComponent<RectTransform> ().rect.width / 2f;

		float xRightBg = background.position.x + background.rect.width / 2f;
		float xRightMov = GetComponent<RectTransform> ().position.x + GetComponent<RectTransform> ().rect.width / 2f;

		float moveAmout = 0f;

		if (isLeftMover && xLeftBg < xLeftMov) {
			moveAmout = -10f;
		}
		else if(!isLeftMover && xRightBg > xRightMov){
			moveAmout = 10f;
		}

		main.transform.position = main.transform.position + Vector3.right * moveAmout;
		Camera.main.transform.parent.position = Camera.main.transform.parent.position + Vector3.right * moveAmout;
	}

	public override void OnPointerEnter(PointerEventData data)
	{
		canMove = true;
	}

	public override void OnPointerExit(PointerEventData data)
	{
		canMove = false;
	}
}
