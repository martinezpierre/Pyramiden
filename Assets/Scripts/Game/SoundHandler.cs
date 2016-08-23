using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundHandler : MonoBehaviour {

	List<AudioSource> currentSound = new List<AudioSource>();

	AudioSource currentVoice;

	void Start(){

		GameObject gO = new GameObject ("Voice");

		currentVoice = gO.AddComponent<AudioSource> ();
		currentVoice.transform.SetParent (transform);
		currentVoice.loop = false;
	}

	public void PlayVoice(TextNode node)
	{
		if (node.voice) {
			currentVoice.Stop ();
			currentVoice.clip = node.voice;
			currentVoice.Play ();
		}
	}

	public void PlaySound(PlaySoundNode node)
	{
		GameObject gO = new GameObject ("Sound");
		AudioSource aS = gO.AddComponent<AudioSource> ();
		aS.clip = node.sound;
		aS.loop = node.loop;
		aS.volume = node.volume;

		currentSound.Add (aS);

		aS.Play ();


		if (!aS.loop) 
		{
			StartCoroutine (DestroySound (aS));
		}

		if (node.fadeIn) 
		{
			StartCoroutine (FadeIn (node, aS));
		}
	}

	IEnumerator DestroySound(AudioSource aS)
	{
		float timer = Time.time;

		while (Time.time - timer < aS.clip.length+1f) {
			yield return null;
		}

		currentSound.Remove (aS);
		Destroy (aS.gameObject);
	}

	public void StopSound(StopSoundNode node)
	{
		if (!node.stopAll) 
		{
			int index = -1;

			foreach (AudioSource aS in currentSound) {
				if (aS.clip.name == node.toDestroyName) {
					index = currentSound.IndexOf (aS);
				}
			}

			if (node.fadeOut && index != -1) {
				StartCoroutine (FadeOut (node.fadeOutDuration, currentSound [index]));
				currentSound.RemoveAt (index);
			} else if (index != -1) {
				Destroy (currentSound [index].gameObject);
				currentSound.RemoveAt (index);
			}
		} 
		else 
		{
			if (node.fadeOut) {
				StopAllSounds (node.fadeOutDuration);
			} else {
				StopAllSounds (0);
			}

		}

	}

	public void StopAllSounds(float fadeOut)
	{
		StopAllCoroutines();

		if (fadeOut > 0) {
			for(int i = 0; i < currentSound.Count; i++)
			{
				StartCoroutine (FadeOut (fadeOut, currentSound [i]));
			}
		} else {
			for(int i = 0; i < currentSound.Count; i++)
			{
				Destroy(currentSound[i].gameObject);
			}
		}

		currentSound.Clear();

	}

	IEnumerator FadeIn(PlaySoundNode node, AudioSource sound)
	{
		sound.volume = 0f;

		float timer = Time.time;

		while (Time.time - timer < node.fadeInDuration) 
		{
			float ratio = (Time.time - timer) / node.fadeInDuration;
			sound.volume = ratio * node.volume;
			yield return null;
		}

		sound.volume = node.volume;

	}

	IEnumerator FadeOut(float fadeOutDuration, AudioSource sound)
	{
		float timer = Time.time;

		float beginVolume = sound.volume;

		while (Time.time - timer < fadeOutDuration) 
		{
			float ratio = 1- ((Time.time - timer) / fadeOutDuration);
			sound.volume = ratio * beginVolume;
			yield return null;
		}

		sound.volume = 0f;

		Destroy(sound.gameObject);
	}
}
