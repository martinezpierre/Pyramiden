using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[XmlRoot("TextNode")]
public class TextNode : EventNode {

    [XmlAttribute("text")]
    public string text = "";

    [XmlAttribute("characterName")]
    public string characterName = "";

    [XmlIgnore]
    public static string[] options;
    [XmlIgnore]
    public int index;

    [XmlIgnore]
    public AudioClip voice;
    [XmlAttribute("voicePath")]
    public string voicePath="";

    public TextNode()
    {
        windowTitle = "Text Node";

        options = new string[100];

        RecreateLinks();
    }
    
    public override void DrawWindow(bool isFirst)
    {
        base.DrawWindow(isFirst);

        index = EditorGUILayout.Popup(index, options);

        EditorGUILayout.LabelField("Text :");
        text = EditorGUILayout.TextField(text);

        EditorGUILayout.LabelField("Voice :");
        voice = EditorGUILayout.ObjectField(voice, typeof(AudioClip), true) as AudioClip;
    }

    public override void Serialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TextNode));
        FileStream stream = new FileStream(path, FileMode.Append);
        serializer.Serialize(stream, this);
        stream.Close();
    }
    public override void Deserialize(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TextNode));
        FileStream stream = new FileStream(path, FileMode.Open);
        copy(serializer.Deserialize(stream) as TextNode);
        stream.Close();
    }

    protected void copy(TextNode nodeToCopy)
    {
        base.copy(nodeToCopy);

        text = nodeToCopy.text;
        characterName = nodeToCopy.characterName;
    }

    public override void SaveLinks()
    {
        base.SaveLinks();

        characterName = options[index];

        if (voice != null)
        {
            voicePath = AssetDatabase.GetAssetPath(voice);
        }
        else
        {
            voicePath = "";
        }
    }

    public override void RecreateLinks()
    {
        base.RecreateLinks();

        List<CharacterObject>  characters = GameInfoManager.gameInfo.characters;

        int i = 0;

        options = new string[characters.Count];

        foreach (CharacterObject cO in characters)
        {
            options[i] = cO.name;
            i++;
        }

        for (i = 0; i < options.Length; i++)
        {
            if (options[i] == characterName && characterName != "")
            {
                index = i;
            }
        }

        string[] result;

        if (voicePath != "")
        {
            result = voicePath.Split(stringSeparators, System.StringSplitOptions.None);
            result = result[1].Split('.');

            voice = (AudioClip)Resources.Load(result[0], typeof(AudioClip));
        }

    }
}
