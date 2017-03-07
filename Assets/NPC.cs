using System;
using UnityEngine;

public class NPC : MonoBehaviour,Activable {

	public string conversationKey;
	Action<NPC> talk;

    public void activate()
    {
			talk(this);
    }
	
    // Use this for initialization
    void Start () 
		{
			talk = Game.Event.startConversation;
		}
	
	// Update is called once per frame
	void Update () {
		
	}
}
