using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMaster : MonoBehaviour {

	public static DialogueBox createDialogueBox(string [] dialogues)
	{

		GameObject dialogueBoxPrefab = Resources.Load<GameObject>("DialogueBox/DialogueBox");
		//DialogueBox's Start method hasn't been called yet, but awake has
		DialogueBox dialogueBox = Instantiate(dialogueBoxPrefab,dialogueBoxPrefab.transform.position,Quaternion.identity).GetComponent<DialogueBox>();
		dialogueBox.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform,false);
		dialogueBox.dialogues=dialogues;
		return dialogueBox;
	}
	

	public static void destroyDialogueBox(DialogueBox dialogueBox)
	{
		Destroy(dialogueBox.gameObject);
	}

	//Singleton code
	public static HUDMaster instance = null;

	void createSingleton()
	{
		if(instance == null)
		{
			instance = this;
		}
		else if(instance != this)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Awake () 
	{
		createSingleton();
		
	}
}
