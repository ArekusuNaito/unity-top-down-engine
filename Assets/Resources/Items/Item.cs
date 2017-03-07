using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)] 
public class Item : ScriptableObject 
{
	// public string name="New Item";
	public string description="Go ahead and customize your item";
	public int quantity=1;
	public Sprite sprite;

}
