using System;
using UnityEngine;

public class HUD : MonoBehaviour {

	public DialogueBox createDialogueBox(string [] dialogues, Action endConversationCallback = null)
	{

		GameObject dialogueBoxPrefab = Resources.Load<GameObject>("DialogueBox/DialogueBox");
		//DialogueBox's Start method hasn't been called yet, but awake has
		DialogueBox dialogueBox = Instantiate(dialogueBoxPrefab,dialogueBoxPrefab.transform.position,Quaternion.identity).GetComponent<DialogueBox>();
		dialogueBox.transform.SetParent(GameObject.FindGameObjectWithTag("HUD").transform,false);
		dialogueBox.dialogues=dialogues;
		dialogueBox.endConversationCallback = endConversationCallback;
		return dialogueBox;
	}
	

	public void destroyDialogueBox(DialogueBox dialogueBox)
	{
		Destroy(dialogueBox.gameObject);
	}

}
