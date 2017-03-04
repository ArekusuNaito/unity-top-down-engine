using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	public void add(Item item)
	{
		items.Add(item);
	}

	public void remove(Item item)
	{
		items.Remove(item);
	}

}
