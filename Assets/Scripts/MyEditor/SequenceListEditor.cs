using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SequenceListEditor : EditorWindow
{

    static List<SequenceObject> sequences = new List<SequenceObject>();

    Vector2 scrollPos;

    [MenuItem("My Tools/Sequence Editor")]
    static void ShowEditor()
    {
        Load();

        SequenceListEditor editor = EditorWindow.GetWindow<SequenceListEditor>();
        editor.Init();
    }

    void Init()
    {
        
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();

        foreach(SequenceObject sequence in sequences)
        {
            GUILayout.BeginHorizontal();

            sequence.id = EditorGUILayout.TextField(sequence.id);
            sequence.color = EditorGUILayout.ColorField(sequence.color);

            if (GUILayout.Button("Edit"))
            {
                SequenceEditor.ShowEditor(sequences.IndexOf(sequence));
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (GUILayout.Button("New"))
        {
            SequenceObject sO = new SequenceObject();
            sO.id = "Sequence " + (sequences.Count + 1);
            sequences.Add(sO);
            Save();
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save"))
        {
            Save();
        }

        GUILayout.EndVertical();
    }

    static void Load()
    {
        GameInfoManager.LoadGameInfo();

        sequences = GameInfoManager.gameInfo.sequences;
    }

    void Save()
    {
        GameInfoManager.gameInfo.sequences = new List<SequenceObject>();

        foreach (SequenceObject sequence in sequences)
        {
            if (sequence.id != "")
            {
                GameInfoManager.gameInfo.sequences.Add(sequence);
            }
        }

        GameInfoManager.SaveGameInfo();

        Load();
    }
}
