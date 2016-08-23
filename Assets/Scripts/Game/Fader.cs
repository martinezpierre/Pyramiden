using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Fader : MonoBehaviour {

    [HideInInspector]
	public bool isFadingIn = false;
    [HideInInspector]
	public bool isFadingOut = false;

	public Image image;
    
	public void StartFade(FadeNode node)
	{
		isFadingIn = true;

		Debug.Log (node);

		image.color = node.color;
        
		if (node.duration > 0) {
			image.color = new Color (image.color.r, image.color.g, image.color.b, 0);
		}

		StartCoroutine (Fade (true,node.duration));
	}

	IEnumerator Fade(bool fadeIn, float duration)
	{
		float timer = Time.time;

		if (fadeIn) {
			while (Time.time-timer < duration) {

				float ratio = (Time.time - timer) / duration;

				image.color = new Color (image.color.r, image.color.g, image.color.b, ratio);

				yield return null;
			}
			isFadingIn = false;
		} else {
			while (Time.time-timer < duration) {

				float ratio = 1-((Time.time - timer) / duration);

				image.color = new Color (image.color.r, image.color.g, image.color.b, ratio);

				yield return null;
			}
			isFadingOut = false;
		}

		yield return null;
	}

	public void StopFade(FadeNode node)
	{
		isFadingOut = true;
        
		StartCoroutine (Fade (false, node.duration));
	}
}
