using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomEventTrigger : EventTrigger {

    public string sequenceId;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public override void OnPointerClick(PointerEventData data)
    {
		FindObjectOfType<PyramidenMainScript>().ChangeSequence(sequenceId,true);
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }
}
