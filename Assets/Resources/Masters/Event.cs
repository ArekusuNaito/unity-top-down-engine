using UnityEngine;
using SimpleJSON;
using System;
using System.Collections;

public class Event : MonoBehaviour 
{
	Player player;
	//Dialogue Related
	DialogueBox dialogueBox;
	bool onConversation=false;
	JSONNode conversations;

//##############################################################################
//#### External Methods
//##############################################################################
	Func<string[],Action,DialogueBox> createDialogueBox;
	Action<DialogueBox> destroyDialogueBox;
	Action<Item> addToInventory;
	Action<AudioClip> playSFX;
//##############################################################################
//#### Public Events
//##############################################################################

	public void startConversation(NPC npc)
	{
		
		if(!onConversation)
		{
			disablePlayer();
			onConversation=true;
			// conversations[conversationKey]["dialogues"];
			JSONNode node = conversations[npc.conversationKey]["dialogues"]; //conv.tanooki.conversation
			string[] currentDialogues = this.JSONNodeToStringArray(node);

			if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
			else dialogueBox = createDialogueBox(currentDialogues,null);
		}
	}

	public IEnumerator openChest(Chest chest)
	{
		disablePlayer();
		chest.changeToOpenSprite();
		addToInventory(chest.item);
		yield return playSFXAndWait(chest.openSFX);
		playSFX(chest.jingleSFX);
		GameObject popupItem = chest.spawnPopupItem();
		float popupItemFloatTime = getCurrentAnimationLength(popupItem);
		yield return new WaitForSeconds(popupItemFloatTime);
		displayMessage("You found the Silver key!",()=> Destroy(popupItem));
	}

	public void displayMessage(string message,Action callback)
	{
		disablePlayer();
		onConversation=true;
		if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
		else dialogueBox = createDialogueBox(new string[]{message},callback);

	}

	public void endConversation(Action callback)
	{
		if(callback!=null)callback();
		onConversation=false;
		enablePlayer();
		destroyDialogueBox(this.dialogueBox);
	}
	
//##############################################################################
//#### Private
//##############################################################################

	IEnumerator playSFXAndWait(AudioClip audioClip)
	{
		playSFX(audioClip);
		// StartCoroutine(waitSeconds(audioClip.length));
		yield return new WaitForSeconds(audioClip.length);
	}

	void disablePlayer()
	{
		player.enabled=false;
	}
	
	void enablePlayer()
	{
		player.enabled=true;
	}

	float getCurrentAnimationLength(GameObject gameObject,int layerIndex=0)
	{
		return gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(layerIndex).length;
	}

	string [] JSONNodeToStringArray(JSONNode node)
	{	
		string [] array = new string[node.Count];
		
		for(int index=0;index<node.Count ; index++)
		{
			string temp = node[index];
			array[index] = temp;
		}
		return array;
	}

	void loadExternalMethods()
	{
		conversations = Game.conversations;
		createDialogueBox = Game.HUD.createDialogueBox;
		destroyDialogueBox = Game.HUD.destroyDialogueBox;
		addToInventory = Game.Inventory.add;
		playSFX = Game.Sound.playSFX;
		player = Game.player;
	}

	void Start()
	{
		
		loadExternalMethods();
	}


	

}
