using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageHandler : MonoBehaviour {

	public GameObject imagePrefab;
    
    public Transform imageCanvas;
    public Transform interactableCanvas;

    [HideInInspector]public bool canInterupt = true;

    List<Image> currentImages = new List<Image>();

	List<GameObject> currentObjects = new List<GameObject>();

	public void LoadPrevious()
	{
       //TODO
    }
    	
    public void DisplayImage(ImageNode node)
    {
        GameObject gO;

        gO = Instantiate(imagePrefab);
        gO.transform.SetParent(imageCanvas, false);
        gO.GetComponent<Image>().sprite = node.image;

        currentImages.Add(gO.GetComponent<Image>());

        InitImagePos(gO, node);

		if (node.crossfade) {
			StartCoroutine (CrossFade (node, gO.GetComponent<Image> ()));
		}
    }

	public void InstantiateObject(GameObjectNode node)
	{
		GameObject gO;

		gO = Instantiate(node.prefab);

		gO.name = node.prefab.name;

		currentObjects.Add(gO);

		gO.transform.position = new Vector3 (node.beginPos.x, node.beginPos.y, gO.transform.position.z);
		gO.transform.localEulerAngles = new Vector3 (gO.transform.localEulerAngles.x, gO.transform.localEulerAngles.y, node.beginRot);
		gO.transform.localScale = new Vector3 (node.beginScale.x, node.beginScale.y, gO.transform.localScale.z);
	}

    public void AnimateImage(AnimationNode node)
    {
		int index = -1;

		if (node.isImage) {
			foreach (Image img in currentImages)
			{
				if (img.sprite.name == node.toAnimName)
				{
					index = currentImages.IndexOf(img);
				}
			}
		} else {
			foreach (GameObject gO in currentObjects)
			{
				if (gO.name == node.toAnimName)
				{
					index = currentObjects.IndexOf(gO);
				}
			}
		}

		if (index != -1) {
			StartCoroutine(StartAnimation(index, node, true));
		}
    }

    public void DestroyImage(string name)
    {
        int index = -1;

        foreach(Image img in currentImages)
        {
            if(img.sprite.name == name)
            {
                index = currentImages.IndexOf(img);
            }
        }

        if(index != -1)
        {
            Destroy(currentImages[index].gameObject);
            currentImages.RemoveAt(index);
        }
    }

	public void DestroyObject(string name)
	{
		int index = -1;

		foreach(GameObject gO in currentObjects)
		{
			if(gO.name == name)
			{
				index = currentObjects.IndexOf(gO);
			}
		}

		if(index != -1)
		{
			Destroy(currentObjects[index]);
			currentObjects.RemoveAt(index);
		}
	}

    public void DestroyAllImages()
    {
        StopAllCoroutines();

        for(int i = 0; i < currentImages.Count; i++)
        {
            Destroy(currentImages[i].gameObject);
        }
		for(int i = 0; i < currentObjects.Count; i++)
		{
			Destroy(currentObjects[i]);
		}

        currentImages.Clear ();
		currentObjects.Clear ();
    }

    public void InitImagePos(GameObject image, ImageNode node)
    {
        Transform rT = image.GetComponent<Transform>();

        rT.localPosition = new Vector3(node.beginPos.x, node.beginPos.y, rT.localPosition.z);
        rT.localEulerAngles = new Vector3(0, 0, node.beginRot);

        rT.localScale = new Vector3(node.beginScale.x, node.beginScale.y, rT.localScale.z); ;
    }

	IEnumerator CrossFade(ImageNode node, Image image)
	{
		image.color = new Color (image.color.r, image.color.g, image.color.b, 0f);

		float timer = Time.time;

		while (Time.time - timer < node.crossfadeDuration) 
		{
			float ratio = (Time.time - timer) / node.crossfadeDuration;
			image.color = new Color (image.color.r, image.color.g, image.color.b, ratio);
			yield return null;
		}
	}

	//for UI images
	IEnumerator StartAnimation(int index, AnimationNode node, bool canInterup)
	{
		Transform rT;

		if (node.isImage) 
		{
			rT = currentImages[index].GetComponent<Transform>();
		} 
		else 
		{
			rT = currentObjects[index].GetComponent<Transform>();
		}

		AnimationCurve curvePosX;
		AnimationCurve curvePosY;
		AnimationCurve curveRot;
		AnimationCurve curveScaleX;
		AnimationCurve curveScaleY;

        if (node.ease)
		{
			curvePosX = AnimationCurve.EaseInOut (0, rT.localPosition.x, node.animDuration, node.endPosX);
			curvePosY = AnimationCurve.EaseInOut (0, rT.localPosition.y, node.animDuration, node.endPosY);
			curveRot = AnimationCurve.EaseInOut (0, rT.localEulerAngles.z, node.animDuration, node.endRot);
			curveScaleX = AnimationCurve.EaseInOut (0, rT.localScale.x, node.animDuration, node.endScaleX);
			curveScaleY = AnimationCurve.EaseInOut (0, rT.localScale.y, node.animDuration, node.endScaleY);
		} 
		else 
		{
			curvePosX = AnimationCurve.Linear (0, rT.localPosition.x, node.animDuration, node.endPosX);
			curvePosY = AnimationCurve.Linear (0, rT.localPosition.y, node.animDuration, node.endPosY);
			curveRot = AnimationCurve.Linear (0, rT.localEulerAngles.z, node.animDuration, node.endRot);
			curveScaleX = AnimationCurve.Linear (0, rT.localScale.x, node.animDuration, node.endScaleX);
			curveScaleY = AnimationCurve.Linear (0, rT.localScale.y, node.animDuration, node.endScaleY);
		}

		canInterupt = canInterup;
        
        float timer = Time.time;

		while (Time.time-timer < node.animDuration) {

            if (rT == null) break;

			rT.localPosition = new Vector3(curvePosX.Evaluate(Time.time-timer),curvePosY.Evaluate(Time.time-timer),0);

			rT.localEulerAngles = new Vector3(0, 0, curveRot.Evaluate(Time.time-timer));

			rT.localScale = new Vector3(curveScaleX.Evaluate(Time.time-timer),curveScaleY.Evaluate(Time.time-timer),1f);

			yield return null;
		}

        if (rT != null)
        {
            rT.localPosition = new Vector3(node.endPos.x, node.endPos.y, rT.localPosition.z);
            rT.localEulerAngles = new Vector3(0, 0, node.endRot);
            rT.localScale = new Vector3(node.endScale.x, node.endScale.y, rT.localScale.z);
        }
		
		yield return null;

		canInterupt = true;

	}

}
