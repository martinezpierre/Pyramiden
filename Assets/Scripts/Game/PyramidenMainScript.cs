using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PyramidenMainScript : MonoBehaviour {

	SequenceObject currentSequence;

    PlaceObject currentPlace;

	NodeHandler nodeHandler;

	EventNode currentNode;

    bool canRead = true;

    public static string[] stringSceneSeparators = new string[] { "Scenes/" };

	public GameObject movers;

	InputGetter inputGetter;

	Vector3 cameraInitPos;
	Vector3 mainInitPos;

	static PyramidenMainScript instance = null;

    void Awake()
    {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			Destroy (gameObject);
		}
    }

    // Use this for initialization
    void Start () {
	
		mainInitPos = transform.position;
		cameraInitPos = Camera.main.transform.position;

		inputGetter = FindObjectOfType<InputGetter> ();

		nodeHandler = GetComponent<NodeHandler> ();

		GameInfoManager.LoadGameInfo ();

		movers.SetActive (false);

        ChangePlace(GameInfoManager.gameInfo.places[0].name);
	}

    IEnumerator ReadSequence(SequenceObject sequence, int beginingNodeId = -1)
    {
		if (beginingNodeId != -1) {
			currentNode = sequence.GetNode (sequence.GetNode (beginingNodeId).nextNodeId);
		} else {
			currentNode = sequence.GetNode(sequence.firstNodeId);
		}

        int nextNodeId = -1;

        int newId;

        canRead = true;

        nodeHandler.InitNodes();

        while (currentNode != null && canRead)
        {
            if (currentNode.GetType() != typeof(WaitForInputNode) 
                && currentNode.GetType() != typeof(GoToSequenceNode) 
                && currentNode.GetType() != typeof(GoToPlaceNode)
                && currentNode.GetType() != typeof(GoToExplorationNode)
                && nodeHandler.CanGoNextNode(false, out newId))
            {
                if (newId != -1)
                {
                    currentNode = sequence.GetNode(newId);
                }
                else
                {
                    nodeHandler.HandleNode(currentNode);

                    nextNodeId = nodeHandler.GetNextNodeId(currentNode);

                    currentNode = sequence.GetNode(nextNodeId);
                }
            }
            else if(currentNode.GetType() == typeof(WaitForInputNode))
            {
				if (inputGetter.click/*Input.GetMouseButtonUp(0)*/ && nodeHandler.CanGoNextNode(true, out newId))
                {
                    nextNodeId = nodeHandler.GetNextNodeId(currentNode);

                    currentNode = sequence.GetNode(nextNodeId);
                }
            }
            else if (currentNode.GetType() == typeof(GoToSequenceNode))
            {
                if(nodeHandler.CanGoNextNode(false, out newId))
                {
					ChangeSequence(((GoToSequenceNode)currentNode).sequenceId,false);
                }
            }
            else if (currentNode.GetType() == typeof(GoToPlaceNode))
            {
                if (nodeHandler.CanGoNextNode(false, out newId))
                {
                    ChangePlace(((GoToPlaceNode)currentNode).placeId);
                }
            }
            else if (currentNode.GetType() == typeof(GoToExplorationNode))
            {
                if (nodeHandler.CanGoNextNode(false, out newId))
                {
                    ChangeExploration(((GoToExplorationNode)currentNode).explorationId);
                }
            }

            yield return null;
        }

        Debug.Log("End Sequence");

		if (canRead) 
		{
			if (currentSequence.invokedByExploration) 
			{
				nodeHandler.ClearNodes ();
			} 
			else if(currentSequence.invokedByNode != -1 && currentSequence.invokedBySequence != "")
			{
				ChangeSequence (currentSequence.invokedBySequence, false, currentSequence.invokedByNode);
			}
		}

        yield return null;
    }

	public void ChangeSequence(string name, bool changedByExplo, int beginingNodeId = -1, bool newPlace = false)
    {
		string sequenceId = "";
		int nodeId = -1;

		if (!newPlace && !changedByExplo && beginingNodeId==-1)
		{
			sequenceId = currentSequence.id;
			nodeId = currentNode.id;
		}

        StopAllCoroutines();

        SequenceObject newSequence = GameInfoManager.getSequence(name);

		if (changedByExplo) {
			newSequence.invokedByExploration = true;
		} else {
			newSequence.invokedByExploration = false;
			newSequence.invokedBySequence = sequenceId;
			newSequence.invokedByNode = nodeId;
		}

		currentSequence = newSequence;

		StartCoroutine(ReadSequence(newSequence,beginingNodeId));
    }

    public bool ChangePlace(string name)
    {
        bool canAccess = true;
        
        PlaceObject newPlace = GameInfoManager.getPlace(name);

        if(PlayerPrefs.GetInt("Lucidity") < newPlace.lucidityLevel)
        {
            canAccess = false;
        }
        else
        {
            currentPlace = newPlace;

			nodeHandler.ClearNodes ();

			transform.position = mainInitPos;

			Camera.main.transform.position = cameraInitPos;

			if (SceneManager.GetActiveScene ().name != "Main") 
			{
				SceneManager.LoadScene ("Main");
			}

            ChangeSequence(newPlace.firstSequenceId,false,-1,true);
        }

        return canAccess;
    }

    public void ChangeExploration(string name)
    {
        ExplorationObject newExplo = GameInfoManager.getExploration(name);
        
        string sceneName = newExplo.scenePath.Split(stringSceneSeparators, System.StringSplitOptions.None)[1];
        sceneName = sceneName.Split('.')[0];

        SceneManager.LoadScene(sceneName);

        nodeHandler.ClearNodes();

		transform.position = mainInitPos;

		Camera.main.transform.position = cameraInitPos;

        canRead = false;

		if (newExplo.beginingSequence != "") 
		{
			ChangeSequence (newExplo.beginingSequence,true);
		}

		StartCoroutine (FindBG ());
    }

	IEnumerator FindBG()
	{
		yield return null;

		Image background = GameObject.Find ("background").GetComponent<Image>();

		Rect canvasRect = GameObject.Find ("Images").GetComponent<RectTransform>().rect;

		if (background.rectTransform.rect.width > canvasRect.width) {
			movers.SetActive (true);
			foreach (Mover m in FindObjectsOfType<Mover>()) {
				m.background = background.rectTransform;
			}
		} else {
			movers.SetActive (false);
		}
	}
}
