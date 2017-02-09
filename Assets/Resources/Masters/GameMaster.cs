using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameMaster : MonoBehaviour {

	public static bool onConversation=false;
	static JSONNode conversations;
	static DialogueBox dialogueBox;

	public static void startConversation(string conversationKey)
	{
		if(!onConversation)
		{
			onConversation=true;
			// conversations[conversationKey]["dialogues"];
			JSONNode node = conversations[conversationKey]["dialogues"]; //conv.tanooki.conversation
			string[] currentDialogues = JSONNodeToStringArray(node);
			if(dialogueBox!=null)HUDMaster.destroyDialogueBox(dialogueBox);
			dialogueBox = HUDMaster.createDialogueBox(currentDialogues);
		}

		
	}

	public static void endConversation()
	{
		onConversation=false;
		HUDMaster.destroyDialogueBox(dialogueBox);
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		createSingleton();
		TextAsset jsonData = Resources.Load("Data/dialogues") as TextAsset;
		conversations = JSON.Parse(jsonData.text);
	}

	public static string [] JSONNodeToStringArray(JSONNode node)
	{
		
		
		string [] array = new string[node.Count];
		
		for(int index=0;index<node.Count ; index++)
		{
			string temp = node[index];
			array[index] = temp;
		}
		return array;
	}

	//Singleton code
	public static GameMaster instance = null;

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
}
