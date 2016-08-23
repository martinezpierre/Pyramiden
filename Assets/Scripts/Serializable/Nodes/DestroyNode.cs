using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("DestroyNode")]
public class DestroyNode : EventNode
{
    [XmlAttribute("toDestroyName")]
    public string toDestroyName = "";

    [XmlIgnore]
    public Sprite imageToDestroy;

	[XmlIgnore]
	public GameObject objectToDestroy;

	[XmlAttribute("isImage")]
	public bool isImage=true;

    public DestroyNode()
    {
        windowTitle = "Destroy Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

		isImage = EditorGUILayout.Toggle ("Image", isImage);

		if (isImage) {
			EditorGUILayout.LabelField("image name : " + toDestroyName);
			imageToDestroy = EditorGUILayout.ObjectField(imageToDestroy, typeof(Sprite),false) as Sprite;
			if (imageToDestroy != null)
			{
				toDestroyName = imageToDestroy.name;
			}
		} else {
			EditorGUILayout.LabelField("gameObject name : " + toDestroyName);
			objectToDestroy = EditorGUILayout.ObjectField(objectToDestroy, typeof(GameObject),false) as GameObject;
			if (objectToDestroy != null)
			{
				toDestroyName = objectToDestroy.name;
			}
		}
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DestroyNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DestroyNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as DestroyNode);
        stream.Close();
    }

    protected void copy(DestroyNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        toDestroyName = nodeToCopy.toDestroyName;
		isImage = nodeToCopy.isImage;
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
