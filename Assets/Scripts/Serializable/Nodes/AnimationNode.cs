using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("AnimationNode")]
public class AnimationNode : EventNode
{
    [XmlAttribute("toAnimName")]
    public string toAnimName = "";

    [XmlIgnore]
    public Sprite imageToAnim;

	[XmlIgnore]
	public GameObject objectToAnim;

    [XmlAttribute("animDuration")]
    public float animDuration;

    [XmlAttribute("endPosX")]
    public float endPosX;
    [XmlAttribute("endPosY")]
    public float endPosY;

    [XmlAttribute("endRot")]
    public float endRot;

    [XmlAttribute("endScaleX")]
    public float endScaleX = 1;
    [XmlAttribute("endScaleY")]
    public float endScaleY = 1;

    [XmlAttribute("ease")]
    public bool ease = true;

    [XmlIgnore]
    public Vector2 endPos;
    [XmlIgnore]
    public Vector2 endScale;

	[XmlAttribute("isImage")]
	public bool isImage=true;

    public AnimationNode()
    {
        windowTitle = "Animation Node";
        endScale = Vector2.one;
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

		isImage = EditorGUILayout.Toggle ("Image", isImage);

		if (isImage) 
		{
			EditorGUILayout.LabelField("image name : " + toAnimName);

			imageToAnim = EditorGUILayout.ObjectField(imageToAnim, typeof(Sprite), false) as Sprite;

			if (imageToAnim != null)
			{
				toAnimName = imageToAnim.name;
			}
		} 
		else 
		{
			EditorGUILayout.LabelField("gameObject name : " + toAnimName);

			objectToAnim = EditorGUILayout.ObjectField(objectToAnim, typeof(GameObject), false) as GameObject;

			if (objectToAnim != null)
			{
				toAnimName = objectToAnim.name;
			}
		}

        ease = EditorGUILayout.ToggleLeft("Ease", ease);
        GUILayout.Label("Duration (seconds)", EditorStyles.boldLabel);
        animDuration = EditorGUILayout.FloatField(animDuration);

        GUILayout.Label("End", EditorStyles.boldLabel);
        endPos = EditorGUILayout.Vector2Field("Position (pixels)", endPos);
        GUILayout.Label("Rotation (degrees)");
        endRot = EditorGUILayout.FloatField(endRot);
        endScale = EditorGUILayout.Vector2Field("Scale (1=100%)", endScale);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(AnimationNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(AnimationNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as AnimationNode);
        stream.Close();
    }

    protected void copy(AnimationNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        toAnimName = nodeToCopy.toAnimName;

        endPosX = nodeToCopy.endPosX;
        endPosY = nodeToCopy.endPosY;

        endRot = nodeToCopy.endRot;

        endScaleX = nodeToCopy.endScaleX;
        endScaleY = nodeToCopy.endScaleY;

        animDuration = nodeToCopy.animDuration;

        ease = nodeToCopy.ease;

		isImage = nodeToCopy.isImage;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();

        endPosX = endPos.x;
        endPosY = endPos.y;

        endScaleX = endScale.x;
        endScaleY = endScale.y;
    }

    public override void RecreateLinks()
    {
        base.RecreateLinks();

        endPos = new Vector2(endPosX, endPosY);
        endScale = new Vector2(endScaleX, endScaleY);
    }
}
