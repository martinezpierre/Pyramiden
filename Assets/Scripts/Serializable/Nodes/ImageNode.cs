using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("TextNode")]
public class ImageNode : EventNode
{
    [XmlAttribute("imagePath")]
    public string imagePath = "";

    [XmlIgnore]
    public Sprite image;
    
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
    
	[XmlAttribute("crossfade")]
	public bool crossfade;

	[XmlAttribute("crossfadeDuration")]
	public float crossfadeDuration = 0;

    public ImageNode()
    {
        windowTitle = "Image Node";

        beginScale = Vector2.one;
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        string imgName = image == null ? "" : image.name;

		GUILayout.BeginHorizontal ();
		crossfade = EditorGUILayout.ToggleLeft("Crossfade",crossfade,GUILayout.Width(100));
		if (crossfade) 
		{
			crossfadeDuration = EditorGUILayout.FloatField(crossfadeDuration);
		}
		GUILayout.EndHorizontal ();

        EditorGUILayout.LabelField("Image : "+ imgName);
        image = EditorGUILayout.ObjectField(image, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100)) as Sprite;
        
        beginPos = EditorGUILayout.Vector2Field("Position (pixels)", beginPos);
        GUILayout.Label("Rotation (degrees)");
        beginRot = EditorGUILayout.FloatField(beginRot);
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("Force height fill"))
		{
			float ratio = 16f / 9f;

			float imgWidth = image.rect.width;
			float imgHeight = image.rect.height;

			float imgNewHeight = imgHeight * ratio;

			float newRatio = imgWidth / imgNewHeight;

			beginScale = Vector2.one * newRatio;
		}
		if (GUILayout.Button("Force width fill"))
		{
			float ratio = 9f / 16f;

			float imgWidth = image.rect.width;
			float imgHeight = image.rect.height;

			float imgNewWidth = imgWidth * ratio;

			float newRatio = imgHeight / imgNewWidth;

			beginScale = Vector2.one * newRatio;
		}
		GUILayout.EndHorizontal ();
        beginScale = EditorGUILayout.Vector2Field("Scale (1=100%)", beginScale);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ImageNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ImageNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as ImageNode);
        stream.Close();
    }

    protected void copy(ImageNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        imagePath = nodeToCopy.imagePath;

        beginPosX = nodeToCopy.beginPosX;
        beginPosY = nodeToCopy.beginPosY;

        beginRot = nodeToCopy.beginRot;

        beginScaleX = nodeToCopy.beginScaleX;
        beginScaleY = nodeToCopy.beginScaleY;

		crossfade = nodeToCopy.crossfade;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();

        if (image != null)
        {
            imagePath = AssetDatabase.GetAssetPath(image);
        }
        else
        {
            imagePath = "";
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

        if (imagePath != "")
        {
            result = imagePath.Split(stringSeparators, System.StringSplitOptions.None);
            result = result[1].Split('.');

            image = (Sprite)Resources.Load(result[0], typeof(Sprite));
        }

        beginPos = new Vector2(beginPosX, beginPosY);
        beginScale = new Vector2(beginScaleX, beginScaleY);
    }
}
