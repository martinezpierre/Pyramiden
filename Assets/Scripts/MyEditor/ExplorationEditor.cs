using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationEditor : EditorWindow
{
    static private ExplorationObject exploration;

    static int exploIndex;

    public GameObject imagePrefab;
    public GameObject interactablePrefab;

    Image backgroundImage;

    Transform imgCanvas;
    Transform interactableCanvas;

    Vector2 scrollPos;
    
    public static void ShowEditor(int explorationIndex)
    {
        exploIndex = explorationIndex;

        Load();

        EditorSceneManager.OpenScene(exploration.scenePath);

        ExplorationEditor editor = EditorWindow.GetWindow<ExplorationEditor>(false, exploration.name);
        editor.Init();
    }

    void Init()
    {
        if (exploration == null)
        {
            exploration = new ExplorationObject();
        }
        
        imgCanvas = FindObjectOfType<exploInfo>().bgCanvas;
        interactableCanvas = FindObjectOfType<exploInfo>().interactCanvas;

        if (imgCanvas.childCount > 0)
        {
            backgroundImage = imgCanvas.GetChild(0).GetComponent<Image>();
        }
        else
        {
            backgroundImage = null;
        }

        foreach(InteractableObject iO in exploration.interactables)
        {
            foreach(Image img in interactableCanvas.GetComponentsInChildren<Image>())
            {
                if(img.gameObject.name == iO.id)
                {
                    iO.detectionRect = img;
                }
            }
        }
    }

    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

        EditorGUILayout.LabelField("Background :");
        exploration.backGround = EditorGUILayout.ObjectField(exploration.backGround, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100)) as Sprite;

        CheckBaground();

		if (GUILayout.Button("Edit",GUILayout.Width(100)))
		{
			Selection.activeGameObject = backgroundImage.gameObject;
		}

		EditorGUILayout.LabelField("Begining sequence :");
		exploration.beginingSequence = EditorGUILayout.TextField(exploration.beginingSequence);

        foreach (InteractableObject iO in exploration.interactables)
        {
            GUILayout.BeginHorizontal();

            iO.id = EditorGUILayout.TextField(iO.id);

            iO.detectionRect.gameObject.name = iO.id;

            if (iO.detectionRect.GetComponent<CustomEventTrigger>())
            {
                iO.detectionRect.GetComponent<CustomEventTrigger>().sequenceId = iO.sequenceId;
            }
            else
            {
                iO.detectionRect.gameObject.AddComponent<CustomEventTrigger>();
            }


            EditorGUILayout.LabelField("start sequence :",GUILayout.Width(100));

            iO.sequenceId = EditorGUILayout.TextField(iO.sequenceId);

            if (GUILayout.Button("Edit"))
            {
                Selection.activeGameObject = iO.detectionRect.gameObject;
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("New"))
        {
            NewInteractable();
        }

        GUILayout.EndScrollView();

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save"))
        {
            Save();
        }
    }

    void CheckBaground()
    {
        if (exploration.backGround != null && (!backgroundImage || backgroundImage.sprite != exploration.backGround))
        {
            if (imgCanvas.childCount > 0)
            {
                DestroyImmediate(imgCanvas.GetChild(0).gameObject);
            }

            backgroundImage = Instantiate(imagePrefab).GetComponent<Image>();
            backgroundImage.sprite = exploration.backGround;
            backgroundImage.name = "background";
            backgroundImage.transform.SetParent(imgCanvas, false);
        }
    }


    void NewInteractable()
    {
        InteractableObject iO = new InteractableObject();
        iO.id = "Interactable " + (exploration.interactables.Count + 1);

        GameObject gO = Instantiate(interactablePrefab);
        gO.transform.SetParent(interactableCanvas, false);

        Image img = gO.GetComponent<Image>();
        img.name = iO.id;
        
        iO.detectionRect = img;

        exploration.interactables.Add(iO);
    }
    static void Load()
    {
        GameInfoManager.LoadGameInfo();

        exploration = GameInfoManager.gameInfo.explorations[exploIndex];
    }

    void Save()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        
        GameInfoManager.gameInfo.explorations[exploIndex] = exploration;

        GameInfoManager.SaveGameInfo();
    }
}
