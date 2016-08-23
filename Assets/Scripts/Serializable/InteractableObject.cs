using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("InteractableObject")]
public class InteractableObject
{
    [XmlAttribute("id")]
    public string id = "";
    
    [XmlAttribute("rectWidth")]
    public float rectWidth = 0f;
    [XmlAttribute("rectHeight")]
    public float rectHeight = 0f;
    [XmlAttribute("rectX")]
    public float rectX = 0f;
    [XmlAttribute("rectY")]
    public float rectY = 0f;

    [XmlAttribute("sequenceId")]
    public string sequenceId = "";

    [XmlIgnore]
    public Image detectionRect;
    
    public void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InteractableObject));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public void Deserialize(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(InteractableObject));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as InteractableObject);
        stream.Close();
    }

    void copy(InteractableObject characterToCopy)
    {
        id = characterToCopy.id;

        sequenceId = characterToCopy.sequenceId;

        rectWidth = characterToCopy.rectWidth;
        rectHeight = characterToCopy.rectHeight;
        rectX = characterToCopy.rectX;
        rectY = characterToCopy.rectY;
    }

    public void SaveLinks()
    {
        if (detectionRect)
        {
            rectWidth = detectionRect.rectTransform.rect.width;
            rectHeight = detectionRect.rectTransform.rect.height;
            rectX = detectionRect.rectTransform.rect.x;
            rectY = detectionRect.rectTransform.rect.y;
        }
    }

    public void RecreateLinks()
    {
        //detectionRect = new Rect(rectX, rectY, rectWidth, rectHeight);
    }
}

