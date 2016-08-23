using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("StopSoundNode")]
public class StopSoundNode : EventNode
{
	[XmlAttribute("toDestroyName")]
	public string toDestroyName = "";

	[XmlIgnore]
	public AudioClip sound;

	[XmlAttribute("fadeOut")]
	public bool fadeOut;

	[XmlAttribute("fadeOutDuration")]
	public float fadeOutDuration;

	[XmlAttribute("stopAll")]
	public bool stopAll;

	public StopSoundNode()
	{
		windowTitle = "Stop Node";
	}

	public override void DrawWindow(bool isFirst)
	{
		base.DrawWindow(isFirst);

		stopAll = EditorGUILayout.Toggle ("Stop All", stopAll);

		GUILayout.BeginHorizontal ();
		fadeOut = EditorGUILayout.ToggleLeft("Fade Out",fadeOut,GUILayout.Width(100));
		if (fadeOut) 
		{
			fadeOutDuration = EditorGUILayout.FloatField(fadeOutDuration);
		}
		GUILayout.EndHorizontal ();

		if (!stopAll) 
		{
			EditorGUILayout.LabelField("Sound : "+ toDestroyName);
			sound = EditorGUILayout.ObjectField(sound, typeof(AudioClip), true) as AudioClip;

			if (sound != null)
			{
				toDestroyName = sound.name;
			}
		}
	}

	public override void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(StopSoundNode));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public override void Deserialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(StopSoundNode));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as StopSoundNode);
		stream.Close();
	}

	protected void copy(StopSoundNode nodeToCopy)
	{
		base.copy(nodeToCopy);

		toDestroyName = nodeToCopy.toDestroyName;
		fadeOut = nodeToCopy.fadeOut;
		fadeOutDuration = nodeToCopy.fadeOutDuration;
	}

	public override void SaveLinks()
	{
		base.SaveLinks();
	}

	public override void RecreateLinks()
	{
		base.RecreateLinks();
	}
}
