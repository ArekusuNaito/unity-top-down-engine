using UnityEngine;
using SimpleJSON;
using System;

public class Event : MonoBehaviour 
{
	//Dialogue Related
	DialogueBox dialogueBox;
	bool onConversation=false;
	JSONNode conversations;

//##############################################################################
//#### External Methods
//##############################################################################
	Func<string[],DialogueBox> createDialogueBox;
	Action<DialogueBox> destroyDialogueBox;
//##############################################################################
//#### Public Events
//##############################################################################

	public void startConversation(NPC npc)
	{
		
		if(!onConversation)
		{
			onConversation=true;
			// conversations[conversationKey]["dialogues"];
			JSONNode node = conversations[npc.conversationKey]["dialogues"]; //conv.tanooki.conversation
			string[] currentDialogues = this.JSONNodeToStringArray(node);

			if(dialogueBox!=null)destroyDialogueBox(dialogueBox);
			else dialogueBox = createDialogueBox(currentDialogues);
		}
	}

	public void endConversation()
	{
		onConversation=false;
		destroyDialogueBox(dialogueBox);
	}
	
//##############################################################################
//#### Private
//##############################################################################

	

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
	}

	void Start()
	{
		
		loadExternalMethods();
	}


	

}
