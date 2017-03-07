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
		Debug.Log(conversations);
		
		Game.Switches = new Switches();
		
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
