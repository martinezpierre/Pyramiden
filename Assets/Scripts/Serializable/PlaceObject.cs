using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;

[XmlRoot("PlaceObject")]
public class PlaceObject
{

    [XmlAttribute("name")]
    public string name = "";
    
    [XmlIgnore]
    public Sprite mapPicture;

    [XmlAttribute("mapPicturePath")]
    public string mapPicturePath = "";

    [XmlAttribute("lucidityLevel")]
    public int lucidityLevel = 0;

    [XmlAttribute("firstSequenceId")]
    public string firstSequenceId = "";

    [XmlIgnore]
    public static string[] stringSeparators = new string[] { "Resources/" };

    public void Serialize(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(PlaceObject));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public void Deserialize(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(PlaceObject));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as PlaceObject);
        stream.Close();
    }

    void copy(PlaceObject characterToCopy)
    {
        name = characterToCopy.name;

        mapPicturePath = characterToCopy.mapPicturePath;

        lucidityLevel = characterToCopy.lucidityLevel;

        firstSequenceId = characterToCopy.firstSequenceId;
    }

    public void SaveLinks()
    {
        if (mapPicture != null)
        {
            mapPicturePath = AssetDatabase.GetAssetPath(mapPicture);
        }
        else
        {
            mapPicturePath = "";
        }
    }

    public void RecreateLinks()
    {
        string[] result;

        if (mapPicturePath != "")
        {
            result = mapPicturePath.Split(stringSeparators, System.StringSplitOptions.None);
            result = result[1].Split('.');

            mapPicture = (Sprite)Resources.Load(result[0], typeof(Sprite));
        }
    }
}

