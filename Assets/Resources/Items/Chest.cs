using System;
using UnityEngine;

public class Chest : MonoBehaviour,Activable 
{
	
	Action<Item> addToInventory;

	public void activate()
    {
		open();
    }

	void open()
	{
		Debug.Log("Opening Chest");
		Item item = new Item();
		item.name = "Big Sword";
		item.description = "It's just a Big Sword you can't carry it so easily";
		addToInventory(item);
	}

    // Use this for initialization
    void Start () {
		addToInventory = Game.Inventory.add;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
