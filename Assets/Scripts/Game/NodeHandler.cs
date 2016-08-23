using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NodeHandler : MonoBehaviour {
    
    private TextWriter textWriter;
    private ImageHandler imageHandler;
    private Fader fader;
    private ChoiceHandler choiceHandler;
	private SoundHandler soundHandler;

    public EventNode currentNode;

    private bool isWaiting;

    int ConditionId = -1;

	// Use this for initialization
	void Start () {
		InitVariables ();
    }

	void InitVariables()
	{
		PlayerPrefs.DeleteAll ();

        textWriter = GameObject.FindObjectOfType<TextWriter>();

        imageHandler = GameObject.FindObjectOfType<ImageHandler>();

        fader = GameObject.FindObjectOfType<Fader>();

        choiceHandler = GameObject.FindObjectOfType<ChoiceHandler>();

		soundHandler = GameObject.FindObjectOfType<SoundHandler> ();

    }

	public void HandleNode(EventNode node)
	{
        currentNode = node;

		if (node.GetType () == typeof(ImageNode)) 
		{
            imageHandler.DisplayImage((ImageNode)node);

        }
		else if (node.GetType () == typeof(TextNode)) 
		{
            textWriter.WriteText((TextNode)node);
			soundHandler.PlayVoice ((TextNode)node);
		}
		else if (node.GetType () == typeof(LucidityAddNode)) 
		{
			int currentLucidity = PlayerPrefs.GetInt ("Lucidity");
			PlayerPrefs.SetInt ("Lucidity", currentLucidity + ((LucidityAddNode)node).amount);
		}
		else if (node.GetType () == typeof(SetVariableNode)) 
		{
			int currentLucidity = 0;

			string variable = ((SetVariableNode)node).variableName;

			if(((SetVariableNode)node).incrementation)
			{
				currentLucidity = PlayerPrefs.GetInt (variable);
			}

			PlayerPrefs.SetInt (variable, currentLucidity + ((SetVariableNode)node).amount);
		}
        else if (node.GetType() == typeof(WaitNode))
        {
            StartCoroutine(StartWait(((WaitNode)node).delay));
        }
        else if (node.GetType() == typeof(FadeNode))
        {
            if (((FadeNode)node).startFade)
            {
                fader.StartFade((FadeNode)node);
            }
            else
            {
                fader.StopFade((FadeNode)node);
            }
        }
        else if (node.GetType() == typeof(ChoiceNode))
        {
            choiceHandler.DisplayChoices((ChoiceNode)node);
        }
        else if (node.GetType() == typeof(DestroyNode))
        {
			if (((DestroyNode)node).isImage) {
				imageHandler.DestroyImage(((DestroyNode)node).toDestroyName);
			} else {
				imageHandler.DestroyObject(((DestroyNode)node).toDestroyName);
			}
            
        }
        else if (node.GetType() == typeof(ConditionNode))
        {
            EvaluateCondition((ConditionNode)node);
        }
        else if (node.GetType() == typeof(AnimationNode))
        {
            imageHandler.AnimateImage((AnimationNode)node);
        }
		else if (node.GetType() == typeof(PlaySoundNode))
		{
			soundHandler.PlaySound ((PlaySoundNode)node);
		}
		else if (node.GetType() == typeof(StopSoundNode))
		{
			soundHandler.StopSound ((StopSoundNode)node);
		}
		else if (node.GetType() == typeof(GameObjectNode))
		{
			imageHandler.InstantiateObject ((GameObjectNode)node);
		}
    }

    void EvaluateCondition(ConditionNode node)
    {
        bool res = false;

        int n1 = -1;

        if(!int.TryParse(node.numbers[0], out n1))
        {
            n1 = PlayerPrefs.GetInt(node.numbers[0]);
        }

        int n2 = -1;

        if (!int.TryParse(node.numbers[1], out n2))
        {
            n2 = PlayerPrefs.GetInt(node.numbers[1]);
        }

        if (node.comparisonId == 0)
        {
            res = n1 == n2;
            Debug.Log(n1 + "=" + n2 + "->" + res);
        }
        else if (node.comparisonId == 1)
        {
            res = n1 > n2;
            Debug.Log(n1 + ">" + n2 + "->" + res);
        }
        else
        {
            res = n1 < n2;
            Debug.Log(n1 + "<" + n2 + "->" + res);
        }


        ConditionId = res ? node.nextNodeIdList[0] : node.nextNodeIdList[1];
    }

    IEnumerator StartWait(float duration)
    {
        isWaiting = true;

        float timer = Time.time;

        while(Time.time - timer < duration)
        {
            yield return null;
        }

        isWaiting = false;
    }

    public bool CanGoNextNode(bool onClick, out int newId)
    {
        bool b = true;

        newId = choiceHandler.GetNextId();

        if(newId == -1)
        {
            newId = ConditionId;
            ConditionId = -1;
        }

        if (isWaiting)
        {
            b = false;
        }
        else if(textWriter.isWriting && onClick)
        {
            textWriter.FinishText();
            b = false;
        }
        else if (textWriter.isWriting)
        {
            b = false;
        }
        else if (choiceHandler.choicesShown)
        {
            b = false;
        }

        return b;
    }

    public int GetNextNodeId(EventNode currentNode)
    {
        int res = currentNode.nextNodeId;
        
        return res;
    }

    public void ClearNodes()
    {
        imageHandler.DestroyAllImages();

		textWriter.characterName.text = "";
		textWriter.text.text = "";

        textWriter.textBlock.SetActive(false);

		soundHandler.StopAllSounds (0);
    }

    public void InitNodes()
    {
        isWaiting = false;

        textWriter.textBlock.SetActive(true);
    }
}
