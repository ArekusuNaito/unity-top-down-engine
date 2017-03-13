using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Event : MonoBehaviour 
{
	Player player;
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
	Func<string,bool> inventoryHas;
	Action<AudioClip> playSFX;
	Func<string,bool> isSwitchOff;
	Action<string> turnSwitchOn;
	Action<string> turnSwitchOff;
//##############################################################################
//#### Public Events
//##############################################################################

	public void startConversation(List<string> conversations,int conversationIndex=0)
	{
		string conversationKey = conversations[conversationIndex];
		if(!onConversation)
		{	
			if(hasRequirements(conversationKey))
			{
				if(requirementsMet(conversationKey))
				{
					string[] dialogues = getDialoguesOf(conversationKey);
					addRewardsToInventory(conversationKey);
					turnSwitchesOn(conversationKey);
					turnSwitchesOff(conversationKey);
					displayMessage(dialogues);
				}else
				{
					// Debug.LogError(conversationKey+" doesn't fulfill the requirements");
					startConversation(conversations,conversationIndex-1); //-1, we are going from end to start
				} 
			}
			else
			{

				// Debug.LogError(conversationKey+" doesn't have any requirements");
				addRewardsToInventory(conversationKey);
				turnSwitchesOn(conversationKey);
				turnSwitchesOff(conversationKey);
				string[] dialogues = getDialoguesOf(conversationKey);
				displayMessage(dialogues);
			} 
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

	void displayMessage(string[] dialogues,Action callback=null)
	{
		if(!onConversation)
		{
			disablePlayer();
			onConversation=true;
			if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
			else dialogueBox = createDialogueBox(dialogues,callback);
		}else Debug.LogError("You are already on a conversation");
	}

	void displayMessage(string message,Action callback=null)
	{
		if(!onConversation)
		{
			disablePlayer();
			onConversation=true;
			if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
			else dialogueBox = createDialogueBox(new string[]{message},callback);
		}else Debug.LogError("You are already on a conversation");

	}

	IEnumerator playSFXAndWait(AudioClip audioClip)
	{
		playSFX(audioClip);
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

	bool hasRequirements(string conversationKey)
	{
		var conversation = getConversation(conversationKey);
		return conversation.ContainsKey("requirements");
	}

	void addRewardsToInventory(string conversationKey)
	{
		var conversation = getConversation(conversationKey);
		if(conversation.ContainsKey("rewards"))
		{
			var rewards = (Dictionary<string,object>)conversation["rewards"];
			if(rewards.ContainsKey("items"))
			{
				var items = (List<object>)rewards["items"];
				items.ForEach((item)=>
				{
					var newItem = item as Dictionary<string,object>; //Creating an new one here because it must be casted, lambda's parameters doesn't let me cast it 
					var itemName = (string)newItem["name"];
					var itemQuantity = Convert.ToInt32(newItem["quantity"]);
					addToInventory(new Item{name=itemName,quantity=itemQuantity});
				});
			}

		}
	}

	void turnSwitchesOn(string conversationKey)
	{
		var conversation = getConversation(conversationKey);
		if(conversation.ContainsKey("rewards"))
		{
			var rewards = (Dictionary<string,object>)conversation["rewards"];
			if(rewards.ContainsKey("switches"))
			{
				var switches = (List<object>)rewards["switches"];
				switches.ForEach((theSwitch)=>
				{
					var switchName = (string)theSwitch;	
					turnSwitchOn(switchName);
				});
			}
		}
	}
	void turnSwitchesOff(string conversationKey)
	{
		var conversation = getConversation(conversationKey);
		if(conversation.ContainsKey("removes"))
		{
			var removes = (Dictionary<string,object>)conversation["removes"];
			if(removes.ContainsKey("switches"))
			{
				var switches = (List<object>)removes["switches"];
				switches.ForEach((theSwitch)=>
				{
					var switchName = (string)theSwitch;	
					turnSwitchOff(switchName);
				});
			}
		}
	}

	bool requirementsMet(string conversationKey)
	{

		var conversation = getConversation(conversationKey);
		if(requiredSwitchesAreOn(conversation) && itemsRequiredOnInventory(conversation) )return true;
		else return false;
	}

	bool requiredSwitchesAreOn(Dictionary<string,object> conversation)
	{
		var requirements = (Dictionary<string,object>)conversation["requirements"];
		if(requirements.ContainsKey("switches")) //are game switches required?
		{
			var requiredSwitches = (List<object>)requirements["switches"];
			foreach(var gameSwitch in requiredSwitches)
			{
				if(isSwitchOff((string)gameSwitch))
				{
					return false; //no need to check more requirements if one is not met
				}
			}
		}
		return true;
	}

	bool itemsRequiredOnInventory(Dictionary<string,object> conversation)
	{
		var requirements = (Dictionary<string,object>)conversation["requirements"];
		if(requirements.ContainsKey("items")) //are items required?
		{
			var requiredItems = (List<object>)requirements["items"];
			foreach(var item in requiredItems)
			{
				if(!inventoryHas((string)item))
				{
					return false;
				}
			}
		}
		return true;
	}



	string[] getDialoguesOf(string conversationKey)
	{
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
		isSwitchOff = Game.Switches.isOff;
		turnSwitchOn = Game.Switches.turnOn;
		turnSwitchOff = Game.Switches.turnOff;
		inventoryHas = Game.Inventory.has;
		playSFX = Game.Sound.playSFX;
		player = Game.player;
	}

	void Start()
	{
		loadExternalMethods();
	}


	

}
