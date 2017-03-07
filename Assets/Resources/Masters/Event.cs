using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Event : MonoBehaviour 
{
	Player player;
	Switches switches;
	//Dialogue Related
	DialogueBox dialogueBox;
	bool onConversation=false;
	Dictionary<string,object> conversations;
	// JSONNode conversations;

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
			// JSONNode node = conversations[npc.conversationKey]["dialogues"]; //conv.tanooki.conversation
			// JSONNode requiredSwitches = conversations[npc.conversationKey]["required"];
			
			// if(requiredSwitches.Count>0)
			// {
			
			// }
			string[] npcDialogues = getDialoguesOf(npc.conversationKey);
			if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
			else dialogueBox = createDialogueBox(npcDialogues,null);
		}
	}

	public IEnumerator openChest(Chest chest)
	{
		disablePlayer();
		chest.isClosed=false;
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


	string[] getDialoguesOf(string conversationKey)
	{
		Debug.Log("erre");
		var conversation = getConversation(conversationKey);
		var dialoguesListObject = (List<object>)conversation["dialogues"];
		var dialoguesListString = dialoguesListObject.ConvertAll(dialogue =>dialogue.ToString());
		return dialoguesListString.ToArray();
	}

	Dictionary<string,object> getConversation(string conversationKey)
	{
		return (Dictionary<string,object>)(Game.conversations[conversationKey]);
	}

	void loadExternalMethods()
	{
		createDialogueBox = Game.HUD.createDialogueBox;
		destroyDialogueBox = Game.HUD.destroyDialogueBox;
		addToInventory = Game.Inventory.add;
		playSFX = Game.Sound.playSFX;
		player = Game.player;
		this.switches = Game.Switches;
	}

	void Start()
	{
		loadExternalMethods();
	}


	

}
