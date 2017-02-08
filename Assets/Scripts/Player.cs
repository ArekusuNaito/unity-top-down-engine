using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Rigidbody2D body;
	Animator animator;
	public float speed = 1;
	Vector2 destination;
	Vector2 axisInput;
	Collider2D boxCollider;
	bool canGetInput = true;
	int facing=1;
	// float skinWidth = 0.016f;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
		destination = transform.position;
	}

	// Use this for initialization
	void Start () 
	{

	}
	

	void FixedUpdate()
	{
		inputCheck();

	} 

	void inputCheck()
	{
		axisInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		// if((Vector2)transform.position == (Vector2)(transform.position)+destination)
		if(canGetInput)
		{
			if(axisInput.x>0 && !collides(Vector2.right))
			{
				facing=3;
				canGetInput=false;
				animator.SetInteger("facing",3);
				animator.SetFloat("speed",1);
				destination = (Vector2)transform.position+Vector2.right;
			}
			if(axisInput.x<0 && !collides(Vector2.left))
			{
				facing=2;
				canGetInput=false;
				animator.SetInteger("facing",2);
				animator.SetFloat("speed",1);
				destination = (Vector2)transform.position+Vector2.left;
			}
			if(axisInput.y>0 && !collides(Vector2.up))
			{
				facing=0;
				canGetInput=false;
				animator.SetInteger("facing",0);
				animator.SetFloat("speed",1);
				destination = (Vector2)transform.position+Vector2.up;
			}
			if(axisInput.y<0 && !collides(Vector2.down))
			{
				facing=1;
				canGetInput=false;
				animator.SetInteger("facing",1);
				animator.SetFloat("speed",1);
				destination = (Vector2)transform.position+Vector2.down;
			}
		}
		move();
	}

	void move()
	{
		
		Vector2 p = Vector2.MoveTowards(transform.position,destination, speed);
		if((Vector2)transform.position == destination)
		{
			if(axisInput.x==0 && (facing==2 || facing == 3))animator.SetFloat("speed",0);
			if(axisInput.y==0 && (facing==0 || facing == 1))animator.SetFloat("speed",0);
			
			// if(axisInput.x == 0 || axisInput.y == 0)animator.SetFloat("speed",0);
			canGetInput=true;
		}
    	body.MovePosition(p);
	}

	bool collides(Vector2 direction)
	{
		
		Vector2 targetPosition = (Vector2)this.transform.position + direction;
		RaycastHit2D hit = Physics2D.Linecast(targetPosition,this.transform.position);
		if(hit && hit.transform.tag != "Player")
		{
			return true;
		}
		else 
		{
			return false;
		}
		
	}

}
