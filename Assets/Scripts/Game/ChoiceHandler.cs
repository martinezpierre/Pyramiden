using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChoiceHandler : MonoBehaviour {

	public GameObject choicesObject;

	public Button[] choicesButtons;

	Text[] choicesTexts;
    
	public Text characName;
	public Text sentence;
    
    [HideInInspector]
    public int idChoice = -1;

	[HideInInspector]public bool choicesShown;

    int[] nextNodeIds;

    // Use this for initialization
    void Start () 
	{
		choicesTexts = new Text[choicesButtons.Length];

        nextNodeIds = new int[choicesButtons.Length];

        for (int i = 0; i < choicesButtons.Length; i++)
		{
			choicesTexts [i] = choicesButtons [i].GetComponentInChildren<Text> ();
		}
	}

	public void DisplayChoices(ChoiceNode node)
	{
		choicesShown = true;

        idChoice = -1;

        characName.text = "";
		sentence.text = "";
        
		choicesObject.SetActive (true);

		for (int i = 0; i < node.nbChoice; i++) 
		{
			choicesButtons [i].gameObject.SetActive (true);
			choicesTexts [i].text = node.choices [i];
            
            nextNodeIds[i] = node.nextNodeIdList[i];
        }
		for (int i = node.nbChoice; i < choicesButtons.Length; i++) 
		{
			choicesButtons [i].gameObject.SetActive (false);
		}
	}

	public void SelectChoice(int choiceIndex)
    {
        idChoice = choiceIndex;

        choicesShown = false;

		choicesObject.SetActive (false);
    }

    public int GetNextId()
    {
        int res = -1;

        if (idChoice != -1)
        {
            res = nextNodeIds[idChoice];
            idChoice = -1;
        }
        
        return res;
    }
}
