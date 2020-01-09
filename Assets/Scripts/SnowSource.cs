using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSource : MonoBehaviour
{
	public Wind wind;
	public GameObject prefab;
	public int snowFlakesLimit;
	public int snowFlakesCount;
	public int flakesPerFrame;
    public Transform leftBorder, rightBorder;
    public float delay;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(snowFlakesCount < snowFlakesLimit && timer >= delay + Random.value * delay / 5)
        {
        	timer = 0;
        	for(int i = 0; i < flakesPerFrame; i++)
        	{
	        	Instantiate(prefab, (Vector3.Lerp(leftBorder.position, rightBorder.position, Random.value)), transform.rotation).GetComponent<Snowflake>().wind = wind;
        	}
        }
        timer += Time.fixedDeltaTime;
    }
}
