using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputGetter : EventTrigger {

	public bool click;

	bool hasReceivedInput;

	public override void OnPointerClick(PointerEventData data)
	{
		click = true;
		hasReceivedInput = true;
	}

	void Update(){

		if (hasReceivedInput) 
		{
			hasReceivedInput = false;
		}
		else
		{
			click = false;
		}
	}
}
