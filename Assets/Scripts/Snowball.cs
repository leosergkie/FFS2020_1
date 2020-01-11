using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
	public List<FixedJoint2D> joints;
	public float mergeTreshold;
	public float breakforce;
	public Snowball otherSB;
	public Rigidbody2D rb;
	public Transform sb_transf;

	public float snowCollectTimer;
	public float snowCollectDelay;

	public float velToCollect;
	public float angVelToCollect;

	public bool collect = false;
	public bool collectContidiots = false;

	public bool controlable;
	public float torque;
	public float jumpForce;

	public ParticleSystem particles;

	private bool jumpNextFrame = false;

	void Start()
	{
		// rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if(controlable)
		{
			rb.AddTorque(-Input.GetAxis("Horizontal") * torque * rb.mass * rb.mass);
			if(jumpNextFrame)
			{
				jumpNextFrame = false;
				rb.AddForce(Vector2.up * jumpForce * rb.mass);
			}
		}
		collectContidiots = (rb.velocity.magnitude > velToCollect || rb.angularVelocity > angVelToCollect);
		if(snowCollectTimer > snowCollectDelay)
		{
			snowCollectTimer = 0;
			collect = true;
		}
		snowCollectTimer += Time.fixedDeltaTime;
		if(collect && snowCollectTimer > Time.fixedDeltaTime*2)
		{
			collect = false;
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			jumpNextFrame = true;
		}
	}


    public void Grow()
    {
    	sb_transf.localScale+= (new Vector3(0.01f, 0.01f, 0.01f))/sb_transf.localScale.x;
    	ParticleSystem.ShapeModule shape = particles.shape;
    	shape.scale = sb_transf.localScale;
    	// particles.shape = shape;
    }

    public void LoseFlakes(int amount)
    {
    	for(int i = 0; i < amount; i++)
    	{
    		sb_transf.localScale -= (new Vector3(0.015f, 0.015f, 0.015f))/sb_transf.localScale.x;
    	}
		ParticleSystem.ShapeModule shape = particles.shape;
    	shape.scale = sb_transf.localScale;
    	// particles.shape = shape;
    	particles.Emit(amount);
    }

    [ContextMenu("Lose 10")]
    public void Lose10()
    {
    	LoseFlakes(10);
    }
    [ContextMenu("Lose 50")]
    public void Lose100()
    {
    	LoseFlakes(50);
    }

    public Vector2 KineticEnergy()
    {
    	return rb.mass * rb.velocity.magnitude * rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
    	Snowball sb = collisionInfo.gameObject.GetComponent<Snowball>();
    	if(sb != null)
    	{
    		FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
    		joint.connectedBody = sb.rb;
			joint.breakForce = breakforce;
			joints.Add(joint);
    		return;
    		// Doesn't work yet
    		Debug.Log((KineticEnergy() + sb.KineticEnergy()).magnitude);
    		if((KineticEnergy() + sb.KineticEnergy()).magnitude > mergeTreshold)
    		{
				// FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
				joint.connectedBody = sb.rb;
				joint.breakForce = breakforce;
				joints.Add(joint);
    		}
    	}
    	SnowPile sp = collisionInfo.gameObject.GetComponent<SnowPile>();
    	if(sp != null)
    	{
    		foreach(ContactPoint2D contact in collisionInfo.contacts)
			{
				Vector2 hitPoint = contact.point;
				// Instantiate(explosion,new Vector3(, hitPoint.y, 0), Quaternion.identity);

				if(sp.GetSnowFromPoint(hitPoint.x) > 0)
	    		{
	    			Grow();
	    		}
			}
    		
    	}
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
    	SnowPile sp = collisionInfo.gameObject.GetComponent<SnowPile>();
    	if(sp != null && collect && collectContidiots)
    	{
    		foreach(ContactPoint2D contact in collisionInfo.contacts)
			{
				Vector2 hitPoint = contact.point;
				// Instantiate(explosion,new Vector3(, hitPoint.y, 0), Quaternion.identity);

				if(sp.GetSnowFromPoint(hitPoint.x + sp.cellSize * (Random.value*4-2)) > 0)
	    		{
	    			Grow();
	    		}
			}
    		
    	}
    }
}
