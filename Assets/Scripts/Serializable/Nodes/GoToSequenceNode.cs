using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("GoToSequenceNode")]
public class GoToSequenceNode : EventNode
{
    [XmlAttribute("duration")]
    public string sequenceId = "";
    
    public GoToSequenceNode()
    {
        windowTitle = "Sequence Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);
        
        EditorGUILayout.LabelField("sequence name :");
        sequenceId = EditorGUILayout.TextField(sequenceId);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToSequenceNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToSequenceNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as GoToSequenceNode);
        stream.Close();
    }

    protected void copy(GoToSequenceNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        sequenceId = nodeToCopy.sequenceId;
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
