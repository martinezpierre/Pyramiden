using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	public GameObject mapParent;
	public GameObject mapButton;

	public GameObject buttonPrefab;

	void Start(){
		mapParent.SetActive (false);
	}

	public void ShowMap()
	{
		mapButton.SetActive (false);
		mapParent.SetActive (true);

		ClearMap ();

		int lucidityLevel = PlayerPrefs.GetInt ("Lucidity");

		foreach (PlaceObject place in GameInfoManager.gameInfo.places) 
		{
			if (lucidityLevel >= place.lucidityLevel) 
			{
				GameObject gO = Instantiate (buttonPrefab);
				gO.transform.SetParent (mapParent.transform);
				gO.GetComponentInChildren<Text> ().text = place.name;

				Button b = gO.GetComponent<Button> ();

				b.onClick.AddListener(() => GetMapChoice(gO.GetComponentInChildren<Text> ().text));
			}
		}
	}

	public void ClearMap()
	{
		for (int i = 0; i < mapParent.transform.childCount; i++) 
		{
			Destroy (mapParent.transform.GetChild (i).gameObject);
		}
	}

	public void GetMapChoice(string text)
	{
		mapParent.SetActive (false);
		mapButton.SetActive (true);

		GetComponent<PyramidenMainScript> ().ChangePlace (text);
	}

}
