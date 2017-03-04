using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueBox : MonoBehaviour {

	//Public
	public Font font;
	public AudioClip letterSFX;
	public AudioClip dialogueEndSFX;
	public AudioClip lastDialogueEndSFX;
	public AudioClip conversationEndSFX;
	public float lettersBySecond = 0.3f;
	public int letterSFXInterval=1;//How many letters does we skip to play a sound
	int skipLetterCount=0;
	public string [] dialogues;
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
	Action endConversation;


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
		loadExternalMethods();
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
		dialogue  = dialogues[0]; //take the first dialogue
		// print("Dialogue Length:"+dialogue.Length);
		StartCoroutine(drawDialogue());
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputCheck();
		checkIfDialogueEnded();		
	}

	IEnumerator drawDialogue()
	{
		
		for(int drawTimes=0;drawTimes<dialogue.Length;drawTimes++)
		{
			drawNextLetter();
			yield return new WaitForSecondsRealtime(lettersBySecond); //Realtime allows UI to be drawed normally if we change time.scale
		}
		// print(dialogue.Length+" = "+lettersBySecond*dialogue.Length+" : "+Time.time);
	}

	void drawNextLetter()
	{
		
		if(!isSilentCharacter())
		{
			//You can go ahead and make some noise, playing the letterSFX
			skipLetterCount++;
			if(skipLetterCount>letterSFXInterval)
			{
				playSFX(this.letterSFX);
				skipLetterCount=0;
			}
		}
		letterIndex++;
		dialogueUI.text = dialogue.Substring(0,letterIndex);
		
	}

	bool isSilentCharacter()
	{
		char character = dialogue[letterIndex];
		if(character==' ' || character=='\n')
		{
			return true;
		}
		return false;
	}

	bool checkIfDialogueEnded()
	{
		if(dialogueEnded)return true;
		if(letterIndex>=dialogue.Length)
		{
			onDialogueEnd();
			return true;
		}
		return false;
	}

	bool checkIfConversationEnded()
	{
		if(dialogueIndex>= dialogues.Length-1)
		{
			
			onConversationEnd();
			return true;
		}
		else
		{
			return false;
		}
	}

	void inputCheck()
	{
		//Confirm Button
		if(Input.GetKeyDown(KeyCode.Z))
		{
			
			if(dialogueEnded && !conversationEnded)
			{
				print("next dialogue");
				cursor.enabled=false;
				playSFX(dialogueEndSFX);
				letterIndex=0;
				dialogueIndex++;
				dialogue=dialogues[dialogueIndex];
				dialogueEnded=false;
				StartCoroutine(drawDialogue());
			}
			else if(conversationEnded)
			{

				playSFX(conversationEndSFX);
				endConversation();
			}
		}
		//Cancel Button
	}

	void onDialogueEnd()
	{
		checkIfConversationEnded();
		if(conversationEnded)
		{
			playCursorAnimation("Next");
			playSFX(lastDialogueEndSFX); 
		}
		else //then we still have dialogues
		{
			playCursorAnimation("Done");
			// SoundMaster.playSFX(dialogueEndSFX); 
		}
		dialogueEnded = true;
	}

	void onConversationEnd()
	{	

		conversationEnded=true;
	}


	void playCursorAnimation(string animationName)
	{
		cursor.enabled=true;
		Animator cursorAnimator = cursor.GetComponent<Animator>();
		if(cursorAnimator!=null)cursorAnimator.Play(animationName);
	}
}
