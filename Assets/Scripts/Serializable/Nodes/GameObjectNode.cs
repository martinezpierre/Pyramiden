using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("GameObjectNode")]
public class GameObjectNode : EventNode
{
	[XmlAttribute("objectPath")]
	public string objectPath = "";

	[XmlIgnore]
	public GameObject prefab;

	[XmlAttribute("beginPosX")]
	public float beginPosX;
	[XmlAttribute("beginPosY")]
	public float beginPosY;

	[XmlAttribute("beginRot")]
	public float beginRot;

	[XmlAttribute("beginScaleX")]
	public float beginScaleX;
	[XmlAttribute("beginScaleY")]
	public float beginScaleY;

	[XmlIgnore]
	public Vector2 beginPos;
	[XmlIgnore]
	public Vector2 beginScale;

	public GameObjectNode()
	{
		windowTitle = "GameObject Node";

		beginScale = Vector2.one;
	}

	public override void DrawWindow(bool isFirst)
	{
		base.DrawWindow(isFirst);

		string imgName = prefab == null ? "" : prefab.name;

		EditorGUILayout.LabelField("Prefab : "+ imgName);
		prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false) as GameObject;

		beginPos = EditorGUILayout.Vector2Field("Position (pixels)", beginPos);
		GUILayout.Label("Rotation (degrees)");
		beginRot = EditorGUILayout.FloatField(beginRot);

		beginScale = EditorGUILayout.Vector2Field("Scale (1=100%)", beginScale);
	}

	public override void Serialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(GameObjectNode));
		FileStream stream = new FileStream(path, FileMode.Append);
		serializer.Serialize(stream, this);
		stream.Close();
	}
	public override void Deserialize(string path)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(GameObjectNode));
		FileStream stream = new FileStream(path, FileMode.Open);
		copy(serializer.Deserialize(stream) as GameObjectNode);
		stream.Close();
	}

	protected void copy(GameObjectNode nodeToCopy)
	{
		base.copy(nodeToCopy);

		objectPath = nodeToCopy.objectPath;

		beginPosX = nodeToCopy.beginPosX;
		beginPosY = nodeToCopy.beginPosY;

		beginRot = nodeToCopy.beginRot;

		beginScaleX = nodeToCopy.beginScaleX;
		beginScaleY = nodeToCopy.beginScaleY;
	}

	public override void SaveLinks()
	{
		base.SaveLinks();

		if (prefab != null)
		{
			objectPath = AssetDatabase.GetAssetPath(prefab);
		}
		else
		{
			objectPath = "";
		}

		beginPosX = beginPos.x;
		beginPosY = beginPos.y;

		beginScaleX = beginScale.x;
		beginScaleY = beginScale.y;
	}

	public override void RecreateLinks()
	{
		base.RecreateLinks();

		string[] result;

		if (objectPath != "")
		{
			result = objectPath.Split(stringSeparators, System.StringSplitOptions.None);
			result = result[1].Split('.');

			prefab = (GameObject)Resources.Load(result[0], typeof(GameObject));
		}

		beginPos = new Vector2(beginPosX, beginPosY);
		beginScale = new Vector2(beginScaleX, beginScaleY);
	}
}
