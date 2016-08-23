using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ExplorationObject")]
public class ExplorationObject
{

    [XmlAttribute("name")]
    public string name = "";

    [XmlIgnore]
    public Sprite backGround;

    [XmlAttribute("mapPicturePath")]
    public string backGroundPath = "";

    [XmlArray("interactables")]
    [XmlArrayItem("InteractableObject")]
    public List<InteractableObject> interactables;

    [XmlIgnore]
    public static string[] stringSeparators = new string[] { "Resources/" };

    [XmlAttribute("scenePath")]
    public string scenePath = "";

	[XmlAttribute("beginingSequence")]
	public string beginingSequence = "";

    public ExplorationObject()
    {
        interactables = new List<InteractableObject>();
    }

    public void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ExplorationObject));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public void Deserialize(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(ExplorationObject));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as ExplorationObject);
        stream.Close();
    }

    void copy(ExplorationObject characterToCopy)
    {
        name = characterToCopy.name;

        backGroundPath = characterToCopy.backGroundPath;

        interactables = characterToCopy.interactables;

        scenePath = characterToCopy.scenePath;

		beginingSequence = characterToCopy.beginingSequence;
    }

    public void SaveLinks()
    {
        if (backGround != null)
        {
            backGroundPath = AssetDatabase.GetAssetPath(backGround);
        }
        else
        {
            backGroundPath = "";
        }

        foreach(InteractableObject iO in interactables)
        {
            iO.SaveLinks();
        }
    }

    public void RecreateLinks()
    {
        string[] result;

        if (backGroundPath != "")
        {
            result = backGroundPath.Split(stringSeparators, System.StringSplitOptions.None);
            result = result[1].Split('.');

            backGround = (Sprite)Resources.Load(result[0], typeof(Sprite));
        }

        foreach (InteractableObject iO in interactables)
        {
            iO.RecreateLinks();
        }
    }
}

