using MiniJSON;
using System.Collections.Generic;
using UnityEngine;

public class Switches : Dictionary<string,object>
{	

	public Switches()
	{
		addFalse("boss1Defeated");
		addFalse("redGemGet");
	}

	void addFalse(string switchName)
	{
		Add(switchName,false);	
	}

}
