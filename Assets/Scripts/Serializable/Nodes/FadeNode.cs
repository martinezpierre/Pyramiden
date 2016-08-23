using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("FadeNode")]
public class FadeNode : EventNode
{
    [XmlAttribute("duration")]
    public float duration = 0f;

    [XmlIgnore]
    public Color color = Color.black;

    [XmlAttribute("colorR")]
    public float colorR = 0;
    [XmlAttribute("colorG")]
    public float colorG = 0;
    [XmlAttribute("colorB")]
    public float colorB = 0;

    [XmlAttribute("startFade")]
    public bool startFade = true;

    public FadeNode()
    {
        windowTitle = "Fade Node";
    }

    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        startFade = EditorGUILayout.Toggle("Fade Out", startFade);

        if (startFade)
        {
            color = EditorGUILayout.ColorField(color);
        }
        
        EditorGUILayout.LabelField("Duration :");
        duration = EditorGUILayout.FloatField(duration);
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(FadeNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(FadeNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as FadeNode);
        stream.Close();
    }

    protected void copy(FadeNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        colorR = nodeToCopy.colorR;
        colorG = nodeToCopy.colorG;
        colorB = nodeToCopy.colorB;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();

        colorR = color.r;
        colorB = color.b;
        colorG = color.g;
    }

    public override void RecreateLinks()
    {
        base.RecreateLinks();

        color = new Color(colorR, colorG, colorB,1f);
    }
}
