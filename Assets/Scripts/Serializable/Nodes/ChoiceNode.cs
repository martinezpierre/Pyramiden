using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("ChoiceNode")]
public class ChoiceNode : EventNode
{

    [XmlArray("Choices")]
    [XmlArrayItem("String")]
    public string[] choices;

    [XmlArray("nextNodeIdList")]
    [XmlArrayItem("int")]
    public int[] nextNodeIdList;

    [XmlIgnore]
    public EventNode[] nextNodeList;

    [XmlIgnore]
    public int nbChoice=0;

    public ChoiceNode()
    {
        windowTitle = "Choice Node";

        choices = new string[4];

        nextNodeIdList = new int[4];

        nextNodeList = new EventNode[4];
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        for(int i = 0; i < nbChoice; i++)
        {
            GUILayout.BeginHorizontal();
            choices[i] = GUILayout.TextField(choices[i]);
            GUILayout.Label("->" + nextNodeIdList[i]);
            GUILayout.EndHorizontal();
        }

        if (nbChoice < 4)
        {
            if (GUILayout.Button("New Choice"))
            {
                choices[nbChoice] = "Choice "+(nbChoice+1);
                nextNodeIdList[nbChoice] = -1;
                nbChoice++;
            }
        }
    }

    public override void NodeDeleted(EventNode node)
    {
        for (int i = 0; i < nbChoice; i++)
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
        XmlSerializer serializer = new XmlSerializer(typeof(ChoiceNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ChoiceNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as ChoiceNode);
        stream.Close();
    }

    protected void copy(ChoiceNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        choices = nodeToCopy.choices;
        nextNodeIdList = nodeToCopy.nextNodeIdList;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();
        
    }

    public override void RecreateLinks()
    {
        base.RecreateLinks();

        nbChoice = choices.Length;

        string[] Newchoices = new string[4];
        for(int i = 0; i < nbChoice; i++)
        {
            Newchoices[i] = choices[i];
        }
        choices = Newchoices;

        int[] NewnextNodeIdList = new int[4];
        for (int i = 0; i < nbChoice; i++)
        {
            NewnextNodeIdList[i] = nextNodeIdList[i];
        }
        nextNodeIdList = NewnextNodeIdList;
    }
}
