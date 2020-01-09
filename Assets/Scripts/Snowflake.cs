using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowflake : MonoBehaviour
{
	public Wind wind;

	public Rigidbody2D rb;
	public bool localDeviationRight;
	public float localDeviationCounter;
	public float localWind;

	public float windMultiplier;

	public float gravity = .1f;

	public Vector2 appliedForce;
    // Start is called before the first frame update
    void Start()
    {
        localDeviationRight = Random.value > 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	if((Random.value + localDeviationCounter < 0.99f))
    	{
    		localDeviationCounter += 0.0001f;
    	}
    	else
    	{
    		localDeviationCounter = 0;
    		localDeviationRight = !localDeviationRight;
    	}
    	localWind = wind.windIntesity + (localDeviationRight?1:-1);
    	appliedForce = Vector2.right * localWind * windMultiplier;
    	rb.AddForce((Vector2.right * localWind * windMultiplier + Vector2.down * gravity) * rb.mass / Time.fixedDeltaTime);

    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
    	// Debug.Log("Wtf");
    	if(collisionInfo.gameObject.layer == 8)
    	{
    		collisionInfo.gameObject.GetComponent<SnowPile>().AddSnowAtPoint(transform.position.x);
    		Destroy(gameObject);
    	}
    	
    }
}
