using MiniJSON;
using System.Collections.Generic;
using UnityEngine;

public class Switches : Dictionary<string,object>
{	

	public Switches()
	{
		//This is the list of our game switches. Initialized as false.
		addFalse("boss1Defeated");
		addFalse("redGemGet");
		addFalse("AlphaNeedsBeta");
		addFalse("AlphaNeedsGamma");
		addFalse("AlphaNeedsDelta");
		addFalse("AlphaQuestComplete");
	}

	void addFalse(string switchName)
	{
		Add(switchName,false);	
	}

	public void turnOn(string switchName)
	{
		if(ContainsKey(switchName))this[switchName]=true;
		else Debug.LogError("The Switch you trying to turn on doesn't exist: "+switchName);
	}

	public void turnOff(string switchName)
	{
		if(ContainsKey(switchName))this[switchName]=false;
		else Debug.LogError("The Switch you trying to turn off doesn't exist: "+switchName);
	}
	public bool isOn(string switchName)
	{
		if(ContainsKey(switchName))
		{
			if((bool)this[switchName])return true;
			else return false;
		}
		else Debug.LogError("The Switch you trying to check doesn't exist: "+switchName);
		return false;
	}

	public bool isOff(string switchName)
	{
		if(ContainsKey(switchName))
		{
			if(!(bool)this[switchName])return true;
			else return false;
		}
		else Debug.LogError("The Switch you trying to check doesn't exist: "+switchName);
		return false;
	}

}
