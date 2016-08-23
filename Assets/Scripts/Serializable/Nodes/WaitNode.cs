using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("WaitNode")]
public class WaitNode : EventNode
{

    [XmlAttribute("delay")]
    public float delay = 0f;

    public WaitNode()
    {
        windowTitle = "Wait Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        EditorGUILayout.LabelField("Delay (seconds) :");
        delay = EditorGUILayout.FloatField(delay);

    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WaitNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WaitNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as WaitNode);
        stream.Close();
    }

    protected void copy(WaitNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        delay = nodeToCopy.delay;
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
