using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextWriter : MonoBehaviour {

	float letterPause;
    
	public Text characterName;
	public Text text;
    
	public Image imageSentence;
	public Image imageScene;

	[HideInInspector] public bool isWriting;

	string currentMessage;
	CharacterObject currentCharacter;
    
	int policeSize;

    public GameObject textBlock;

	// Use this for initialization
	void Awake () {
        
		imageSentence.enabled = false;
		imageScene.enabled = false;

        letterPause = 0.025f;// Options.Instance.textSpeed;

		currentMessage = "";

		characterName.text = "";

        text.text = "";

		isWriting = false;
        
	}
    
	public void WriteText(TextNode message)
	{
		StopAllCoroutines ();

		currentMessage = message.text;
        
		currentCharacter = GameInfoManager.getCharacter (message.characterName);

		characterName.color = currentCharacter.color;
		characterName.text = currentCharacter.name;
        characterName.fontSize = 40;//Options.Instance.nameSize;

		if (currentCharacter.isLeft) {
			characterName.alignment = TextAnchor.UpperLeft;
		} else {
			characterName.alignment = TextAnchor.UpperRight;
		}
        
		text.color = currentCharacter.color;

		imageSentence.color = currentCharacter.color;
		imageScene.color = currentCharacter.color;

		isWriting = true;

        text.fontSize = 30;// Options.Instance.textSize;

		SetTextFront();

		StartCoroutine(TypeText (currentMessage));
	}

	public void SetTextFront(){
		text.text = "";
		imageSentence.enabled = false;
		imageScene.enabled = false;
	}

	public void FinishText()
	{
		StopAllCoroutines ();
		isWriting = false;
		text.text = currentMessage;
        
		imageSentence.enabled = true;
	}

	IEnumerator TypeText (string message) 
	{
		foreach (char letter in message.ToCharArray()) 
		{
			text.text += letter;
            
			if (letter > 'A' && letter < 'z') 
			{
				yield return new WaitForSeconds (letterPause);
			}
		}   

		yield return 0;

		isWriting = false;
        
		imageSentence.enabled = true;
	}
}
