using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("TextNode")]
public class WaitForInputNode : EventNode
{
    public WaitForInputNode()
    {
        windowTitle = "Click Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WaitForInputNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(WaitForInputNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as WaitForInputNode);
        stream.Close();
    }

    protected void copy(WaitForInputNode nodeToCopy)
    {
        base.copy(nodeToCopy);
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
