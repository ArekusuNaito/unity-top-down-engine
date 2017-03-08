using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour,Activable {

	public List<string> conversations;
	Action<List<string>,int> talk;

    public void activate()
    {
			// talk(conversationKey);
			talk(conversations,0);
    }
	
    // Use this for initialization
    void Start () 
	{
		talk = Game.Event.startConversation;
	}
}
