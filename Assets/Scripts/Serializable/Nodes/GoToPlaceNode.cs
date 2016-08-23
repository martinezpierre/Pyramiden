using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("GoToPlaceNode")]
public class GoToPlaceNode : EventNode
{
    [XmlAttribute("placeId")]
    public string placeId = "";

    public GoToPlaceNode()
    {
        windowTitle = "Sequence Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        EditorGUILayout.LabelField("place name :");
        placeId = EditorGUILayout.TextField(placeId);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToPlaceNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToPlaceNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as GoToPlaceNode);
        stream.Close();
    }

    protected void copy(GoToPlaceNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        placeId = nodeToCopy.placeId;
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
