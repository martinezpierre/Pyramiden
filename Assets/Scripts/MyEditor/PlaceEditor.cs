using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class PlaceEditor : EditorWindow
{
    static private PlaceObject place;

    static int placeIndex;

    public static void ShowEditor(int plIndex)
    {
        placeIndex = plIndex;

        Load();

        PlaceEditor editor = EditorWindow.GetWindow<PlaceEditor>(false, place.name);
        editor.Init();
    }

    void Init()
    {
        if (place == null)
        {
            place = new PlaceObject();
        }
    }

    void OnGUI()
    {
        place.mapPicture = EditorGUILayout.ObjectField(place.mapPicture, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100)) as Sprite;

        place.lucidityLevel = EditorGUILayout.IntField("Lucidity level min", place.lucidityLevel);

        place.firstSequenceId = EditorGUILayout.TextField("First Sequence", place.firstSequenceId);

        if (GUILayout.Button("Save"))
        {
            Save();
        }
    }

    static void Load()
    {
        GameInfoManager.LoadGameInfo();

        place = GameInfoManager.gameInfo.places[placeIndex];
    }

    void Save()
    {
        GameInfoManager.gameInfo.places[placeIndex] = place;

        GameInfoManager.SaveGameInfo();
    }
}
