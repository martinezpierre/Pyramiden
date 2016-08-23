using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ExplorationListEditor : EditorWindow
{
    static List<ExplorationObject> explorations = new List<ExplorationObject>();

    Vector2 scrollPos;

    public GameObject CameraPrefab;
    public GameObject DisplayPrefab;

    static string currentScenePath = "";

    [MenuItem("My Tools/Exploration Editor")]
    static void ShowEditor()
    {
        Load();

        currentScenePath = EditorSceneManager.GetActiveScene().path;

        ExplorationListEditor editor = EditorWindow.GetWindow<ExplorationListEditor>();
        editor.Init();
    }

    void Init()
    {

    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
        GUILayout.BeginVertical();

        foreach (ExplorationObject exploration in explorations)
        {
            GUILayout.BeginHorizontal();

            exploration.name = EditorGUILayout.TextField(exploration.name);

            if (GUILayout.Button("Edit"))
            {
                ExplorationEditor.ShowEditor(explorations.IndexOf(exploration));
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (GUILayout.Button("New"))
        {
            NewExplo();
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save"))
        {
            Save();
        }
    }

    void NewExplo()
    {
        string currentScene = EditorSceneManager.GetActiveScene().path;

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        
        Instantiate(CameraPrefab);
        Instantiate(DisplayPrefab);

        DestroyImmediate(GameObject.FindObjectOfType<PyramidenMainScript>());

        EditorSceneManager.SaveScene(newScene, "Assets/Resources/Scenes/Exploration" + (explorations.Count + 1)+".unity");

        EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;
        EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + 1];
        System.Array.Copy(original, newSettings, original.Length);

        EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(newScene.path, true);
        newSettings[newSettings.Length - 1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;

        ExplorationObject eO = new ExplorationObject();
        eO.scenePath = newScene.path;

        EditorSceneManager.OpenScene(currentScene, OpenSceneMode.Single);

        eO.name = "Exploration " + (explorations.Count + 1);
        explorations.Add(eO);
        Save();
    }
    
    static void Load()
    {
        GameInfoManager.LoadGameInfo();

        explorations = GameInfoManager.gameInfo.explorations;
    }

    void Save()
    {
        GameInfoManager.gameInfo.explorations = new List<ExplorationObject>();

        foreach (ExplorationObject exploration in explorations)
        {
            if (exploration.name != "")
            {
                GameInfoManager.gameInfo.explorations.Add(exploration);
            }
        }

        GameInfoManager.SaveGameInfo();

        Load();
    }

    void OnDestroy()
    {
        EditorSceneManager.OpenScene(currentScenePath);
    }
}
