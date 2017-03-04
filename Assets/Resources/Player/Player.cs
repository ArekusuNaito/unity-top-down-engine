using System;
using UnityEngine;

public class Player : MonoBehaviour {

	struct Facing
	{
		public const int UP = 0;
		public const int DOWN = 1;
		public const int LEFT = 2;
		public const int RIGHT = 3;
	}
	Rigidbody2D body;
	Animator animator;
	public float speed = 1;
	Vector2 destination;
	Vector2 axisInput;
	Collider2D boxCollider;
	bool canGetInput = true;
	int facing=Facing.DOWN;
	

//##############################################################################
//#### External Methods
//##############################################################################
	


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

	void loadExternalMethods()
	{
		
		
	}

	// Use this for initialization
	void Start () 
	{
		loadExternalMethods();
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
			if(axisInput.y>0)
			{
				facing=Facing.UP;
				if(!collides(Vector2.up))
				{
					canGetInput=false;
					animator.SetFloat("speed",1);
					destination = (Vector2)transform.position+Vector2.up;
				}
			}
			if(axisInput.y<0)
			{
				facing=Facing.DOWN;
				if(!collides(Vector2.down))
				{
					canGetInput=false;
					animator.SetFloat("speed",1);
					destination = (Vector2)transform.position+Vector2.down;
				}

			}
			if(axisInput.x<0)
			{
				facing=Facing.LEFT;
				if(!collides(Vector2.left))
				{
					canGetInput=false;
					animator.SetFloat("speed",1);
					destination = (Vector2)transform.position+Vector2.left;
				}

			}
			if(axisInput.x>0)
			{
				facing=Facing.RIGHT;
				if(!collides(Vector2.right))
				{
					canGetInput=false;
					animator.SetFloat("speed",1);
					destination = (Vector2)transform.position+Vector2.right;
				}	
			}
		}
		checkActionInput();
		animator.SetInteger("facing",facing);
		move();
	}

	void checkActionInput()
	{
		if(Input.GetKeyDown(KeyCode.Z))
		{
			Vector2 direction = Vector2.zero;
			switch(facing)
			{
				case Facing.UP: direction=Vector2.up; 
				break;
				case Facing.DOWN: direction=Vector2.down; 
				break;
				case Facing.LEFT: direction=Vector2.left; 
				break;
				case Facing.RIGHT: direction=Vector2.right; 
				break;

			}
			Vector2 targetPosition = (Vector2)transform.position+direction;
			RaycastHit2D hit = Physics2D.Linecast(targetPosition,transform.position);
			// if(hit)
			// {
			// 	print(hit.transform.name);
			// 	print(hit.distance);
			// 	print(hit.point);
			// }
			activateIfActivable(hit);
			

			
			
		}
	}

	void activateIfActivable(RaycastHit2D hit)
	{
		if(hit.transform.GetComponent<Activable>() is Activable)
		{
			hit.transform.GetComponent<Activable>().activate();
		}
	}

	void move()
	{
		
		Vector2 p = Vector2.MoveTowards(transform.position,destination, speed);
		if((Vector2)transform.position == destination)
		{
			if(axisInput.x==0 && (facing==Facing.LEFT || facing == Facing.RIGHT))animator.SetFloat("speed",0);
			if(axisInput.y==0 && (facing==Facing.UP || facing == Facing.DOWN))animator.SetFloat("speed",0);
			
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
