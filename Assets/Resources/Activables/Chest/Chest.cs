using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Chest : MonoBehaviour,Activable 
{
	public Sprite openSprite;
	public Sprite closedSprite;
	public AudioClip openSFX;
	public AudioClip jingleSFX;
	public Item item;
	public string spritesPath = "Activables/Chest/Sprites/";
	SpriteRenderer spriteRenderer;
	GameObject itemGet;

	//External Methods
	Func<Chest,IEnumerator> open;

	public void activate()
    {

		StartCoroutine(open(this));
    }

	

	public void changeToOpenSprite()
	{
		spriteRenderer.sprite = openSprite;
	}

	public GameObject spawnPopupItem()
	{
		return Instantiate(itemGet,this.transform);
	}

	void Awake()
	{
		openSprite = Resources.Load<Sprite>(spritesPath+"OpenChestA");
		closedSprite = Resources.Load<Sprite>(spritesPath+"ClosedChestA");
		openSFX = Resources.Load<AudioClip>("Audio/openChest");
		jingleSFX = Resources.Load<AudioClip>("Audio/jingle");
		spriteRenderer = GetComponent<SpriteRenderer>();
		itemGet = Resources.Load<GameObject>("Items/SilverKey");
		changeSprite(closedSprite);
	}

	void changeSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

    // Use this for initialization
    void Start () 
	{
		loadExternalMethods();
	}
	
	void loadExternalMethods()
	{
		open = Game.Event.openChest;
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
