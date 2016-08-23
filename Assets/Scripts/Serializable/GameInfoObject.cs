using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

[XmlRoot("GameInfoObject")]
public class GameInfoObject{

    [XmlArray("Sequences")]
    [XmlArrayItem("SequenceObject")]
    public List<SequenceObject> sequences;

    [XmlArray("Explorations")]
    [XmlArrayItem("ExplorationObject")]
    public List<ExplorationObject> explorations;

    [XmlArray("Characters")]
    [XmlArrayItem("CharacterObject")]
    public List<CharacterObject> characters;

    [XmlArray("Places")]
    [XmlArrayItem("PlaceObject")]
    public List<PlaceObject> places;

    public GameInfoObject()
	{
        sequences = new List<SequenceObject>();
        explorations = new List<ExplorationObject>();
        characters = new List<CharacterObject>();
        places = new List<PlaceObject>();
	}

	public void Save(string name)
	{
		SaveLinks ();

		string path = Application.dataPath + "/Resources/Save/" + name + ".txt";

		XmlSerializer serializer = new XmlSerializer(typeof(GameInfoObject));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {

            XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);

            serializer.Serialize(xmlWriter, this);
            stream.Close();
        }

		Debug.Log("saved at "+path);
	}

	public void Load(string name)
	{
		string path = Application.dataPath + "/Resources/Save/" + name + ".txt";

        DirectoryInfo info = new DirectoryInfo("Assets/Resources/Save");
		FileInfo[] fileInfo = info.GetFiles();

        bool fileExist = false;

		foreach (FileInfo file in fileInfo) {
			if (file.Name.Contains (name)) 
			{
                fileExist = true;
            }
		}
        
        if (fileExist)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameInfoObject));
            FileStream stream = new FileStream(path, FileMode.Open);
            copy(serializer.Deserialize(stream) as GameInfoObject);
            stream.Close();

            RecreateLinks();
        }
	}

	void copy(GameInfoObject gameInfoToCopy)
	{
        sequences = gameInfoToCopy.sequences;
        explorations = gameInfoToCopy.explorations;
        characters = gameInfoToCopy.characters;
        places = gameInfoToCopy.places;
	}

    public void SaveLinks()
    {
        foreach(SequenceObject sequence in sequences)
        {
            if (sequence != null)
            {
                sequence.SaveLinks();
            }
        }
        foreach (CharacterObject character in characters)
        {
            if (character != null)
            {
                character.SaveLinks();
            }
        }
        foreach (PlaceObject place in places)
        {
            if (place != null)
            {
                place.SaveLinks();
            }
        }
        foreach (ExplorationObject exploration in explorations)
        {
            if (exploration != null)
            {
                exploration.SaveLinks();
            }
        }
    }

    public void RecreateLinks()
    {
        foreach (CharacterObject character in characters)
        {
            character.RecreateLinks();
        }
        foreach (SequenceObject sequence in sequences)
        {
            sequence.RecreateLinks();
        }
        foreach (PlaceObject place in places)
        {
            place.RecreateLinks();
        }
        foreach (ExplorationObject exploration in explorations)
        {
            exploration.RecreateLinks();
        }
    }
}
