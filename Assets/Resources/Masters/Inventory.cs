using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	public void add(Item item)
	{
		items.Add(item);
		this.print();
	}

	public void remove(Item item)
	{
		items.Remove(item);
	}

	public void print()
	{
		print("There's "+this.items.Count);
		items.ForEach(Debug.Log);
		print("End inventory Print");
	}

	public bool has(string name)
	{
		return items.Exists(item =>item.name==name);
	}

}
