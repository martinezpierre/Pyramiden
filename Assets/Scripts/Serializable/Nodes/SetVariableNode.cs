using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("SetVariableNode")]
public class SetVariableNode : EventNode {

    [XmlAttribute("incrementation")]
    public bool incrementation = false;

    [XmlAttribute("amount")]
    public int amount = 0;

    [XmlAttribute("variableName")]
    public string variableName = "newVariable";

    public SetVariableNode()
    {
        windowTitle = "Calcul Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        incrementation = EditorGUILayout.Toggle("Incrementation",incrementation);

        variableName = EditorGUILayout.TextField(variableName);
        EditorGUILayout.LabelField("=");
        if (incrementation)
        {
            EditorGUILayout.LabelField(variableName + " +");
        }

        amount = EditorGUILayout.IntField(amount);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SetVariableNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SetVariableNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as SetVariableNode);
        stream.Close();
    }

    protected void copy(SetVariableNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        amount = nodeToCopy.amount;
        incrementation = nodeToCopy.incrementation;
        variableName = nodeToCopy.variableName;
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
