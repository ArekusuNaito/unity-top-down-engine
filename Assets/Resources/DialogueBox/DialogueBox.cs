using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueBox : MonoBehaviour {

	//Public
	public Font font;
	public AudioClip letterSFX;
	public AudioClip nextDialogueSFX;
	public AudioClip lastDialogueEndSFX;
	public AudioClip conversationEndSFX;
	public float lettersBySecond = 0.3f;
	public int letterSFXInterval=1;//How many letters does we skip to play a sound
	public string [] dialogues;
	public Action endConversationCallback;
	// public JSONNode dialogues;

	//Private
	Text dialogueUI;
	Image boxSkin;
	Image cursor;

	string dialogue;
	int letterIndex = 0;
	int dialogueIndex = 0;
	float timer;

	bool dialogueEnded = false;
	bool conversationEnded = false;

//##############################################################################
//#### External Methods
//##############################################################################
	Action<AudioClip> playSFX;
	Action<Action> endConversation;


	void loadExternalMethods()
	{
		playSFX = Game.Sound.playSFX;
		endConversation = Game.Event.endConversation;
	}
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		//Get components
		
		dialogueUI = GetComponentInChildren<Text>();
		dialogueUI.font = font;
		lookForImageComponents();
		
	}

	void lookForImageComponents()
	{
		Image [] images = GetComponentsInChildren<Image>();
		foreach(Image image in images)
		{
			if(image.name == "BoxSkin")
			{
				boxSkin = image;
			}
			if(image.name == "Cursor")
			{
				cursor = image;
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		loadExternalMethods();
		dialogue  = dialogues[0]; //take the first dialogue
		StartCoroutine(drawDialogue());
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputCheck();
	}

	IEnumerator drawDialogue()
	{
		// yield return new WaitForEndOfFrame(); //This way it skips the first OnKeyDown.Z and it won't skip the text
		
		while(letterIndex<dialogue.Length)
		{
			drawNextLetter();
			yield return new WaitForSecondsRealtime(lettersBySecond); //Realtime allows UI to be drawed normally if we change time.scale
		}
		dialogueEnded=true; //Even if the conversation ended , a dialogue will always end here.
		dialogueIndex++; //Go to the next dialogue. If it's the last one then the conversation ended.
		//Check if conversation ended
		if(dialogueIndex>=dialogues.Length)
		{
			onConversationDrawEnd();

		}
		else //Just the dialogue ended
		{
			onDialogueDrawEnd();
			
		}
	}

	void onDialogueDrawEnd()
	{
		dialogue=dialogues[dialogueIndex];
		playCursorAnimation("Next");
	}

	void onConversationDrawEnd()
	{
		playCursorAnimation("Done");
		playSFX(this.lastDialogueEndSFX);
		conversationEnded=true;
	}

	void drawNextLetter()
	{
		
		dialogueUI.text = dialogue.Substring(0,letterIndex+1);
		if(!isSilentCharacter() && letterIndex%letterSFXInterval==0)playSFX(this.letterSFX);
		letterIndex++;
		
	}

	bool isSilentCharacter()
	{
		print(letterIndex);
		char character = dialogue[letterIndex];
		if(character==' ' || character=='\n')
		{
			return true;
		}
		return false;
	}


	void inputCheck()
	{
		//Confirm Button
		if(Input.GetButtonDown("Submit"))
		{
			if(conversationEnded)
			{
				playSFX(this.conversationEndSFX);
				endConversation(endConversationCallback);
			}
			else if(dialogueEnded)
			{
				displayNextDialogue();
			}
		}
		if(Input.GetButtonDown("Cancel"))
		{
			if(!dialogueEnded)
			{
				letterIndex=dialogue.Length-1;
			}
		}
		
	}

	void displayNextDialogue()
	{
			//Reset this stuff
			letterIndex=0;
			cursor.enabled=false;
			dialogueEnded=false;
			//Play SFX and restart
			playSFX(this.nextDialogueSFX);
			StartCoroutine(drawDialogue());
	}


	void playCursorAnimation(string animationName)
	{
		cursor.enabled=true;
		Animator cursorAnimator = cursor.GetComponent<Animator>();
		if(cursorAnimator!=null)cursorAnimator.Play(animationName);
	}
}
