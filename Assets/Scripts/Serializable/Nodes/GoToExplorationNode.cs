using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("GoToExplorationNode")]
public class GoToExplorationNode : EventNode
{
    [XmlAttribute("placeId")]
    public string explorationId = "";

    public GoToExplorationNode()
    {
        windowTitle = "Exploration Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        EditorGUILayout.LabelField("exploration name :");
        explorationId = EditorGUILayout.TextField(explorationId);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToExplorationNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GoToExplorationNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as GoToExplorationNode);
        stream.Close();
    }

    protected void copy(GoToExplorationNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        explorationId = nodeToCopy.explorationId;
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
