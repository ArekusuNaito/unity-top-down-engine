using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class DialogueBox : MonoBehaviour {

	//Public
	public Font font;
	public AudioClip letterSFX;
	public AudioClip dialogueEndSFX;
	public AudioClip lastDialogueEndSFX;
	public AudioClip conversationEndSFX;
	public float lettersBySecond = 0.3f;
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

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		//Get components
		dialogueUI = GetComponentInChildren<Text>();
		dialogueUI.font = font;
		// dialogue  = dialogues[0];
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
		// StartCoroutine(drawDialogue());
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputCheck();
		checkIfDialogueEnded();
		if(!dialogueEnded)
		{
			timer+=Time.deltaTime;
			if(timer>=lettersBySecond)
			{
				drawNextLetter();
				timer=0;
			}
		}
		
	}

	// IEnumerator drawDialogue()
	// {
		
	// 	// checkIfDialogueEnded();
	// 	// if(!dialogueEnded)
	// 	// {
	// 	// 	timer+=Time.deltaTime;
	// 	// 	if(timer>=lettersBySecond)
	// 	// 	{
	// 	// 		drawNextLetter();
	// 	// 		timer=0;
	// 	// 	}
	// 	// }
	// }

	void drawNextLetter()
	{
		SoundMaster.playSFX(this.letterSFX);
		dialogueUI.text = dialogue.Substring(0,letterIndex);
		letterIndex++;
	}

	bool checkIfDialogueEnded()
	{
		if(dialogueEnded)return true;
		if(letterIndex>dialogue.Length)
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
				cursor.enabled=false;
				SoundMaster.playSFX(dialogueEndSFX);
				letterIndex=0;
				dialogueIndex++;
				dialogue=dialogues[dialogueIndex];
				dialogueEnded=false;
			}
			else if(conversationEnded)
			{

				SoundMaster.playSFX(conversationEndSFX);
				GameMaster.endConversation();
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
			SoundMaster.playSFX(lastDialogueEndSFX); 
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
