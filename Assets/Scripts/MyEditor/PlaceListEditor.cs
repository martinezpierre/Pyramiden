using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PlaceListEditor : EditorWindow
{
    static List<PlaceObject> places = new List<PlaceObject>();

    Vector2 scrollPos;

    [MenuItem("My Tools/Places Editor")]
    static void ShowEditor()
    {
        Load();

        PlaceListEditor editor = EditorWindow.GetWindow<PlaceListEditor>();
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

        foreach (PlaceObject place in places)
        {
            GUILayout.BeginHorizontal();

            place.name = EditorGUILayout.TextField(place.name);

            if (GUILayout.Button("Edit"))
            {
                PlaceEditor.ShowEditor(places.IndexOf(place));
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (GUILayout.Button("New"))
        {
            PlaceObject pO = new PlaceObject();
            pO.name = "Place " + (places.Count + 1);
            places.Add(pO);
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

        places = GameInfoManager.gameInfo.places;
    }

    void Save()
    {
        GameInfoManager.gameInfo.places = new List<PlaceObject>();

        foreach (PlaceObject place in places)
        {
            if (place.name != "")
            {
                GameInfoManager.gameInfo.places.Add(place);
            }
        }

        GameInfoManager.SaveGameInfo();

        Load();
    }
}
