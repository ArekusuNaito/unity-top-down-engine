using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;

public class Game : MonoBehaviour {

	public static Sound Sound;
	public static Player player;
	public static HUD HUD;
	static JSONNode conversations;
	static DialogueBox dialogueBox;

	public static bool onConversation=false;

	public static void startConversation(NPC npc)
	{
		
		if(!onConversation)
		{
			onConversation=true;
			// conversations[conversationKey]["dialogues"];
			JSONNode node = conversations[npc.conversationKey]["dialogues"]; //conv.tanooki.conversation
			string[] currentDialogues = JSONNodeToStringArray(node);
			if(dialogueBox!=null)HUD.destroyDialogueBox(dialogueBox);
			dialogueBox = HUD.createDialogueBox(currentDialogues);
		}

		
	}

	public static void endConversation()
	{
		onConversation=false;
		HUD.destroyDialogueBox(dialogueBox);
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		createSingleton();
		SceneManager.sceneLoaded += OnLevelFinishedLoading; //When the scene loads invoke OnLevelFinishedLoading()
		TextAsset jsonData = Resources.Load("Data/dialogues") as TextAsset;
		Game.Sound = transform.FindChild("SoundMaster").GetComponent<Sound>();
		Game.HUD = transform.FindChild("HUDMaster").GetComponent<HUD>();
		Game.conversations = JSON.Parse(jsonData.text);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		// Debug.Log("Level Loaded");
		// Debug.Log(scene.name);
		// Debug.Log(mode);
		Game.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		
	}

	static string [] JSONNodeToStringArray(JSONNode node)
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
	public static Game instance = null;

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
