
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
	public List<Item> items = new List<Item>();

	public void add(Item itemToAdd)
	{
		// itemToAdd = UnityEngine.Object.Instantiate(itemToAdd) as Item; //clone
		if(!has(itemToAdd.name))
		{
			//Look for it on the database, to get all the data
			try
			{
				var dbItem = Resources.Load<Item>("Items/Database/"+itemToAdd.name);
				dbItem = UnityEngine.Object.Instantiate(dbItem) as Item; //a clone of the resource, because we don't want to modify the data on runtime
				dbItem.name = itemToAdd.name; //remove the (clone) from string from the name. Eg. Shuriken (Clone)
				dbItem.quantity = itemToAdd.quantity; //Add the quantity the itemToAdd specifies, not the one from the database
				items.Add(dbItem);

			}catch(Exception error)
			{

				Debug.LogError("Item name is not on the database:"+error);
			}
			
		}
		else //if we have it just update the quantity
		{
			var foundItemIndex = items.FindIndex(item=>item.name==itemToAdd.name);
			items[foundItemIndex].quantity+=itemToAdd.quantity;
			
		}
		// this.print();
	}

	public void remove(Item item)
	{
		items.Remove(item);
	}

	public void print()
	{
		print("There's "+this.items.Count);
		// items.ForEach(Debug.Log);
		foreach(var item in items)
		{
			Debug.Log(item.name+":"+item.quantity);
		}
		
		print("End inventory Print");
	}


	void Start()
	{

	}

	public bool has(string name)
	{
		return items.Exists(item =>item.name==name);
	}

}
