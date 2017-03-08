using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

public class Game : MonoBehaviour {

	public static Event Event;
	public static Sound Sound;
	public static Player player;
	public static HUD HUD;
	public static Inventory Inventory;
	public static Dictionary<string,object> conversations;
	public static Switches Switches;


	

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		createSingleton();
		SceneManager.sceneLoaded += OnLevelFinishedLoading; //When the scene loads invoke OnLevelFinishedLoading()
		TextAsset jsonData = Resources.Load("Data/dialogues") as TextAsset;
		Game.Sound = transform.FindChild("SoundMaster").GetComponent<Sound>();
		Game.Inventory = transform.FindChild("Inventory").GetComponent<Inventory>();
		Game.HUD = transform.FindChild("HUDMaster").GetComponent<HUD>();
		Game.Event = transform.FindChild("EventMaster").GetComponent<Event>();
		Game.conversations = Json.Deserialize(jsonData.text) as Dictionary<string,object>;
		Game.Switches = new Switches();
		//Tests
		//Add some items to the inventory
		var item1 = new Item();
		item1.name = "Red Gem";
		var item2 = new Item();
		item2.name = "Blue Gem";
		Game.Inventory.add(item1);
		// Game.Inventory.add(item2);
		//Turn on some switches
		Game.Switches.turnOn("boss1Defeated");
	}



	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		// Debug.Log("Level Loaded");
		// Debug.Log(scene.name);
		// Debug.Log(mode);
		Game.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		
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
