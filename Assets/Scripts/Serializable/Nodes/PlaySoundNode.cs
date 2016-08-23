using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("PlaySoundNode")]
public class PlaySoundNode : EventNode
{
	[XmlAttribute("soundPath")]
	public string soundPath = "";

	[XmlIgnore]
	public AudioClip sound;

	[XmlAttribute("fadeIn")]
	public bool fadeIn;

	[XmlAttribute("fadeInDuration")]
	public float fadeInDuration;

	[XmlAttribute("loop")]
	public bool loop;

	[XmlAttribute("volume")]
	public float volume = 1f;

	public PlaySoundNode()
	{
		windowTitle = "Sound Node";
	}

	public override void DrawWindow(bool isFirst)
	{
		base.DrawWindow(isFirst);

		string sndName = sound == null ? "" : sound.name;

		GUILayout.BeginHorizontal ();
		fadeIn = EditorGUILayout.ToggleLeft("Fade In",fadeIn,GUILayout.Width(100));
		if (fadeIn) 
		{
			fadeInDuration = EditorGUILayout.FloatField(fadeInDuration);
		}
		GUILayout.EndHorizontal ();

		EditorGUILayout.LabelField("Sound : "+ sndName);
		sound = EditorGUILayout.ObjectField(sound, typeof(AudioClip), true) as AudioClip;

		volume = EditorGUILayout.Slider("Volume",volume,0f,1f);

		loop = EditorGUILayout.Toggle ("Loop", loop);
	}

	public override void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(PlaySoundNode));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public override void Deserialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(PlaySoundNode));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as PlaySoundNode);
		stream.Close();
	}

	protected void copy(PlaySoundNode nodeToCopy)
	{
		base.copy(nodeToCopy);

		soundPath = nodeToCopy.soundPath;
		fadeIn = nodeToCopy.fadeIn;
		fadeInDuration = nodeToCopy.fadeInDuration;
	}

	public override void SaveLinks()
	{
		base.SaveLinks();

		if (sound != null)
		{
			soundPath = AssetDatabase.GetAssetPath(sound);
		}
		else
		{
			soundPath = "";
		}
	}

	public override void RecreateLinks()
	{
		base.RecreateLinks();

		string[] result;

		if (soundPath != "")
		{
			result = soundPath.Split(stringSeparators, System.StringSplitOptions.None);
			result = result[1].Split('.');

			sound = (AudioClip)Resources.Load(result[0], typeof(AudioClip));
		}
	}
}
