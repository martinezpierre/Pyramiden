using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("ConditionNode")]
public class ConditionNode : EventNode
{
    [XmlArray("numbers")]
    [XmlArrayItem("string")]
    public string[] numbers;

    [XmlArray("nextNodeIdList")]
    [XmlArrayItem("int")]
    public int[] nextNodeIdList;

    [XmlIgnore]
    public EventNode[] nextNodeList;
    
    [XmlIgnore]
    private ComparisonType comparisonType;

    [XmlAttribute("comparisonId")]
    public int comparisonId = 0;

    public enum ComparisonType
    {
        Greater,
        Less,
        Equal
    }

    public ConditionNode()
    {
        windowTitle = "Condition Node";

        numbers = new string[2];

        nextNodeIdList = new int[2];
        nextNodeIdList[0] = -1;
        nextNodeIdList[1] = -1;

        nextNodeList = new EventNode[2];
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);
        EditorGUILayout.LabelField("Calculation Type");
        comparisonType = (ComparisonType)EditorGUILayout.EnumPopup(comparisonType);
        
        numbers[0] = EditorGUILayout.TextField(numbers[0]);

        string myOperator = comparisonType == ComparisonType.Equal ? "=" : comparisonType == ComparisonType.Greater ? ">" : "<";

        EditorGUILayout.LabelField(myOperator);

        numbers[1] = EditorGUILayout.TextField(numbers[1]);

        EditorGUILayout.LabelField("true -> " + nextNodeIdList[0]);
        EditorGUILayout.LabelField("false -> " + nextNodeIdList[1]);
    }

    public override void NodeDeleted(EventNode node)
    {
        for (int i = 0; i < 2; i++)
        {
            if (node.id == nextNodeIdList[i])
            {
                nextNodeIdList[i] = -1;
                nextNodeList[i] = null;
            }
        }

        base.NodeDeleted(node);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ConditionNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ConditionNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as ConditionNode);
        stream.Close();
    }

    protected void copy(ConditionNode nodeToCopy)
    {
        base.copy(nodeToCopy);
        
        nextNodeIdList = nodeToCopy.nextNodeIdList;
        nextNodeList = nodeToCopy.nextNodeList;
        numbers = nodeToCopy.numbers;
        comparisonId = nodeToCopy.comparisonId;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();

        if(comparisonType == ComparisonType.Equal)
        {
            comparisonId = 0;
        }
        else if (comparisonType == ComparisonType.Greater)
        {
            comparisonId = 1;
        }
        else if (comparisonType == ComparisonType.Less)
        {
            comparisonId = 2;
        }
    }

    public override void RecreateLinks()
    {
        base.RecreateLinks();

        comparisonType = comparisonId == 0 ? ComparisonType.Equal : comparisonId == 1 ? ComparisonType.Greater : ComparisonType.Less;

    }
}
