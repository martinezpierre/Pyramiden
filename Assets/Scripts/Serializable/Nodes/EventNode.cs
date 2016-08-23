using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("EventNode")]
[XmlInclude(typeof(TextNode))]
[XmlInclude(typeof(LucidityAddNode))]
[XmlInclude(typeof(SetVariableNode))]
[XmlInclude(typeof(ImageNode))]
[XmlInclude(typeof(WaitForInputNode))]
[XmlInclude(typeof(WaitNode))]
[XmlInclude(typeof(FadeNode))]
[XmlInclude(typeof(ChoiceNode))]
[XmlInclude(typeof(GoToSequenceNode))]
[XmlInclude(typeof(DestroyNode))]
[XmlInclude(typeof(ConditionNode))]
[XmlInclude(typeof(GoToPlaceNode))]
[XmlInclude(typeof(AnimationNode))]
[XmlInclude(typeof(GoToExplorationNode))]
[XmlInclude(typeof(PlaySoundNode))]
[XmlInclude(typeof(StopSoundNode))]
[XmlInclude(typeof(GameObjectNode))]
public abstract class EventNode
{
    [XmlIgnore]
    public Rect windowRect;
    
    [XmlAttribute("rectX")]
    public float rectX = 0;
    [XmlAttribute("rectY")]
    public float rectY = 0;
    [XmlAttribute("rectWidth")]
    public float rectWidth = 0;
    [XmlAttribute("rectHeight")]
    public float rectHeight = 0;

    [XmlAttribute("id")]
    public int id = -1;
    [XmlAttribute("nextNodeId")]
    public int nextNodeId = -1;

    [XmlArray("previousNodesId")]
    public List<int> previousNodesId;

    [XmlIgnore]
    public List<EventNode> previousNodes;
    [XmlIgnore]
    public EventNode nextNode;

    [XmlIgnore]
    public string windowTitle = "";

    [XmlIgnore]
    public static string[] stringSeparators = new string[] { "Resources/" };

    public EventNode()
    {
        previousNodes = new List<EventNode>();
        previousNodesId = new List<int>();
    }

    public virtual void DrawWindow(bool isFirst)
    {
        if (isFirst)
        {
            EditorGUILayout.LabelField("[isFirst]");
        }
        EditorGUILayout.LabelField("[id: "+id+"|next: "+nextNodeId+"]");
    }

    public virtual void DrawCurves()
    {
        foreach (EventNode previousNode in previousNodes)
        {
            SequenceEditor.DrawNodeCurve(previousNode.windowRect, windowRect);
        }
    }

    public virtual void SetPrevious(EventNode previous, Vector2 clickPos)
    {
        previousNodes.Add(previous);
        previousNodesId.Add(previous.id);
        
        previous.nextNodeId = id;
        previous.nextNode = this;
    }

    public virtual void SetPrevious(ChoiceNode previous, Vector2 clickPos, int idChoice)
    {
        SetPrevious(previous, clickPos);

        previous.nextNodeIdList[idChoice] = id;
        previous.nextNodeList[idChoice] = this;
    }

    public virtual void SetPrevious(ConditionNode previous, Vector2 clickPos, int idChoice)
    {
        SetPrevious(previous, clickPos);

        previous.nextNodeIdList[idChoice] = id;
        previous.nextNodeList[idChoice] = this;
    }

    public virtual void NodeDeleted(EventNode node)
    {
        int idPrev = previousNodes.IndexOf(node);

        if (idPrev > -1)
        {
            previousNodes.RemoveAt(idPrev);
            previousNodesId.Remove(node.id);
        }
        
        if (node.Equals(nextNode))
        {
            nextNode = null;
            nextNodeId = -1;
        }
    }
    
    public virtual void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(EventNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public virtual void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(EventNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as EventNode);
        stream.Close();
    }

    protected void copy(EventNode nodeToCopy)
    {
        rectX = nodeToCopy.rectX;
        rectY = nodeToCopy.rectY;
        rectWidth = nodeToCopy.rectWidth;
        rectHeight = nodeToCopy.rectHeight;

        id = nodeToCopy.id;
        previousNodesId = nodeToCopy.previousNodesId;
        nextNodeId = nodeToCopy.nextNodeId;
    }

    public virtual void SaveLinks()
    {
        rectX = windowRect.x;
        rectY = windowRect.y;
        rectWidth = windowRect.width;
        rectHeight = windowRect.height;
    }

    public virtual void RecreateLinks()
    {
        windowRect = new Rect(rectX, rectY, rectWidth, rectHeight);
    }
}
