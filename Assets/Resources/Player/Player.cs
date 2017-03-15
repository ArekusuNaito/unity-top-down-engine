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
	enum State
	{
		IDLE,
		WALK
	}
	Rigidbody2D body;
	Animator animator;
	State state;
	public float speed = 1;
	Vector2 destination;
	Vector2 axisInput;
	Collider2D boxCollider;
	int facing=Facing.DOWN;
	Action processingState;

//##############################################################################
//#### External Methods
//##############################################################################
	


	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		state = State.IDLE;
		processingState = idleState;
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
	

	void Update() 
	{
		// inputCheck();
		axisInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		checkActionInput();
		processingState();
	}

	void enterIdleState()
	{
		animator.SetFloat("speed",0);
		state=State.IDLE;
		processingState = idleState;
	}

	void enterWalkState(int facing)
	{
		Vector2 facingVector = Vector2.zero;
		switch(facing)
		{
			case Facing.UP: facingVector = Vector2.up; break;
			case Facing.DOWN: facingVector = Vector2.down; break;
			case Facing.LEFT: facingVector = Vector2.left; break;
			case Facing.RIGHT: facingVector = Vector2.right; break;
		}
		this.facing = facing;
		animator.SetInteger("facing",facing);
		destination = (Vector2)(this.transform.position)+facingVector;
		animator.SetFloat("speed",1);
		state = State.WALK;
		processingState = walkState;
	}

	

	void walkState()
	{
			Vector2 p = Vector2.MoveTowards(transform.position,destination, speed);
			body.MovePosition(p);
			if((Vector2)this.transform.position == destination )
			{
				if(axisInput.x>0)
				{
					facing=Facing.RIGHT;
					animator.SetInteger("facing",facing);
					if(!collides(Vector2.right))destination = (Vector2)(this.transform.position)+Vector2.right;
				}
				if(axisInput.x<0 )
				{
					facing=Facing.LEFT;
					animator.SetInteger("facing",facing);
					if(!collides(Vector2.left))destination = (Vector2)(this.transform.position)+Vector2.left;
				}
				if(axisInput.y>0 )
				{
					facing=Facing.UP;
					animator.SetInteger("facing",facing);
					if(!collides(Vector2.up))destination = (Vector2)(this.transform.position)+Vector2.up;
				}
				if(axisInput.y<0 )
				{
					facing=Facing.DOWN;
					animator.SetInteger("facing",facing);
					if(!collides(Vector2.down))destination = (Vector2)(this.transform.position)+Vector2.down;
				}
				if(axisInput.x ==0 && axisInput.y == 0)
				{
					enterIdleState();
				}
			}
	}

	

	void idleState()
	{
		if(axisInput.x>0 && !collides(Vector2.right))
			{
				enterWalkState(Facing.RIGHT);	
			}
			if(axisInput.x<0 && !collides(Vector2.left))
			{
				enterWalkState(Facing.LEFT);
			}
			if(axisInput.y>0 && !collides(Vector2.up))
			{
				enterWalkState(Facing.UP);				
			}
			if(axisInput.y<0 && !collides(Vector2.down))
			{
				enterWalkState(Facing.DOWN);
			}
	}


	void checkActionInput()
	{
		if(Input.GetButtonDown("Submit"))
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
