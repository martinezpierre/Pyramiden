using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

[XmlRoot("SequenceObject")]
public class SequenceObject{

    [XmlAttribute("id")]
    public string id = "";

    [XmlAttribute("firstNodeId")]
    public int firstNodeId = -1;

    [XmlAttribute("nbNodes")]
    public int nbNodes = 0;

    [XmlIgnore]
    public Color color = Color.white;

    [XmlAttribute("colorR")]
    public float colorR = 0;
    [XmlAttribute("colorG")]
    public float colorG = 0;
    [XmlAttribute("colorB")]
    public float colorB = 0;
    
    [XmlArray("Nodes")]
    [XmlArrayItem("EventNode")]
    public List<EventNode> nodes;

    [XmlIgnore]
    public Dictionary<int,EventNode> nodesList = new Dictionary<int, EventNode>();

	[XmlIgnore]
	public bool invokedByExploration = false;

	[XmlIgnore]
	public string invokedBySequence = "";

	[XmlIgnore]
	public int invokedByNode = -1;

    public SequenceObject()
    {
        nodes = new List<EventNode>();
    }

    public virtual void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SequenceObject));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public virtual void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SequenceObject));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as SequenceObject);
        stream.Close();
    }

    protected void copy(SequenceObject sequenceToCopy)
    {
        id = sequenceToCopy.id;
        firstNodeId = sequenceToCopy.firstNodeId;

        colorR = sequenceToCopy.colorR;
        colorG = sequenceToCopy.colorG;
        colorB = sequenceToCopy.colorB;

        nodes = sequenceToCopy.nodes;
    }

    public virtual void SaveLinks()
    {
        foreach (EventNode node in nodes)
        {
            if (node != null)
            {
                node.SaveLinks();
            }
        }

        colorR = color.r;
        colorB = color.b;
        colorG = color.g;
    }

    public virtual void RecreateLinks()
    {
        foreach (EventNode node in nodes)
        {
            node.RecreateLinks();

            nodesList.Add(node.id, node);
        }

        color = new Color(colorR, colorG, colorB);
    }

	public EventNode GetNode(int id)
	{
		EventNode res = null;

		if (nodesList.ContainsKey (id)) 
		{
			res = nodesList [id];
		}

		return res;
	}

}
