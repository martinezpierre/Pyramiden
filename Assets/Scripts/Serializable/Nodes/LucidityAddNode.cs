using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("LucidityAddNode")]
public class LucidityAddNode : EventNode {

    [XmlAttribute("amount")]
    public int amount = 0;

    public LucidityAddNode()
    {
        windowTitle = "Lucidity Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);
        
        EditorGUILayout.LabelField("Lucidity +");
        amount = EditorGUILayout.IntField(amount);

    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LucidityAddNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LucidityAddNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as LucidityAddNode);
        stream.Close();
    }

    protected void copy(LucidityAddNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        amount = nodeToCopy.amount;
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
