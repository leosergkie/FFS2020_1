using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPile : MonoBehaviour
{

	public Texture2D texture;
	public Sprite sprite;
	public SpriteRenderer spriteRenderer;
    public int sizeX, sizeY;

    public int[] points;

    public Transform leftBorder, rightBorder;

    // public float baseX;
    public float baseY;
    public int startSnowHeight;
    	
    public float cellSize = 0.01f;

    public float pixelDens;

    public CollisionDetector pcd;
    
    public float colliderUpdateDelay = 0.5f;
    public float colliderUpdateTimer;

    public PolygonCollider2D selfCollider, partDetColl;
    public int currentHeight = 1;

    // Start is called before the first frame update
    [ContextMenu("Start")]
    void Start()
    {
        pixelDens = 1f / cellSize;
    	baseY = Mathf.RoundToInt(leftBorder.position.y);
    	sizeX = Mathf.RoundToInt((rightBorder.position.x - leftBorder.position.x) / cellSize);
    	selfCollider = GetComponent<PolygonCollider2D>();
    	partDetColl = pcd.GetComponent<PolygonCollider2D>();
    	// bcoll.size = new Vector2(rightBorder.position.x - leftBorder.position.x, .5f);
    	// bcoll.offset = new Vector2((rightBorder.position.x - leftBorder.position.x)/2, -.2f);
        points = new int[sizeX];
        Create(sizeX, sizeY);
        // Destroy(gameObject, 20f);
    	// points = new List<int>();
    	// points.Add(0);
    	// points.Add(sizeX);
    	// part = GetComponent<ParticleSystem>();
    }



    void Update()
    {
    	if(colliderUpdateTimer >= colliderUpdateDelay)
    	{
    		colliderUpdateTimer = 0;
		    MakeCollider();
    	}
    	colliderUpdateTimer += Time.deltaTime;
    }

    public float GetHeightAtPoint(float coordX)
    {
    	return baseY + cellSize * points[Mathf.RoundToInt(coordX - leftBorder.position.x)];
    }

    public SnowPile FindAnotherPile(float pos)
    {
    	return null;
    }

    public int GetSnowFromPoint(float x)
    {
    	int i = Mathf.RoundToInt((x + cellSize/2 - leftBorder.position.x)/cellSize);
    	// Debug.Log(i);
    	if(points[i] < 3)
    	{
    		return 0;
    	}
    	points[i]--;
    	InspectPile(i);
		texture.SetPixel(i, points[i], Color.clear);
		texture.Apply();
		MakeCollider();
		Redraw();
    	return 1;
    }

	public void InspectPile(int i)
	{
		if(i > 0)
		{
			if(points[i-1] > points[i] + 1)
			{
				points[i-1]--;
				points[i]++;
				return;
			}
		}
		if(i < sizeX-1)
		{
			if(points[i+1] > points[i] + 1)
			{
				points[i+1]--;
				points[i]++;
				return;
			}
		}
	}

    public void AddSnowAtPoint(float x)
    {
    	int i = Mathf.RoundToInt((x - leftBorder.position.x)/cellSize);
    	// Debug.Log(i);
    	if(i < 0)
    	{
    		i = 0;
    	}
    	if(i >= sizeX)
    	{
    		i = sizeX - 1;
    	}
    	points[i]++;
		bool leftFree = false;
		bool rightFree = false;
		SnowPile pile = null;

		if(i > 0)
		{
			if(points[i] > points[i-1] + 1)
			{
				leftFree = true;
			}
		}
		else
		{
			points[i]--;
			return;
			// pile = FindAnotherPile(leftBorder.position.x - cellSize);
			// if(pile != null)
			// {
			// 	if( baseY + cellSize * points[0] - pile.GetHeightAtPoint(leftBorder.position.x - cellSize) > cellSize)
			// 	{
			// 		leftFree = false;
			// 	}
			// 	// else
			// 	// {

			// 	// }
			// }
		}

		if(i < sizeX - 1)
		{
			if(points[i] > points[i+1] + 1)
			{
				rightFree = true;
			}
		}
		else
		{
			points[i]--;
			return;
			// pile = FindAnotherPile(rightBorder.position.x + cellSize);
			// if(pile != null)
			// {
			// 	if( baseY + cellSize * points[0] - pile.GetHeightAtPoint(rightBorder.position.x + cellSize) > cellSize)
			// 	{
			// 		rightFree = false;
			// 	}
			// 	// else
			// 	// {

			// 	// }
			// }
		}

		if(leftFree && rightFree)
		{
			if(Random.value > 0.5f)
    		{
	    		leftFree = false;
    		}
    		else
    		{
    			rightFree = false;
    		}
		}

		if(leftFree)
		{
			if(i > 0)
			{
				AddSnowAtPoint(x - cellSize);
				points[i]--;
			} 
			else
			{
				if(pile != null){
					pile.AddSnowAtPoint(x - cellSize);
					points[i]--;
				}
			}
			return;
		}
		
		if(rightFree)
		{
			if(i > 0)
			{
				AddSnowAtPoint(x + cellSize);
				points[i]--;
			} 
			else
			{
				if(pile != null){
					pile.AddSnowAtPoint(x + cellSize);
					points[i]--;
				}
			}
			return;
		}
		if(points[i] > currentHeight)
		{
			texture.Resize(sizeX, points[i] + 1);
			sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0), pixelDens);
		    spriteRenderer.sprite = sprite;
			for(int j = 0; j < sizeX; j++)
			{
				for(int k = 0; k < points[i] + 1; k++)
				{
					if(k <= points[j]-1)
					{
						texture.SetPixel(j, k, Color.white);
					}
					else
					{
						texture.SetPixel(j, k, Color.clear);
					}
				}
			}
			currentHeight = points[i] + 1;
		}
		texture.SetPixel(i, points[i]-1, Color.white);
		texture.Apply();


    }

    public void Create(int x, int y)
    {
    	texture = new Texture2D(x, 1, TextureFormat.ARGB32, false);
    	texture.filterMode = FilterMode.Point;
    	for(int i = 0; i < sizeX; i++)
    	{
    		for(int j = 0; j < sizeY; j++)
    		{
    			texture.SetPixel(i, j, Color.clear);
    		}
    	}
		for(int i = 0; i < startSnowHeight; i++)
		{
			for(int j = 0; j < sizeX; j++)
	    	{
	    		if(Random.value > 0.5f)
	    		{
	    			AddSnowAtPoint(leftBorder.position.x + j * cellSize);
	    		}
	    	}
	    }
	    sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0), pixelDens);
	    spriteRenderer.sprite = sprite;
	    // polygonCollider.Reset();
	    MakeCollider();
	    // AddSomeSnow(500, (leftBorder.position.x + rightBorder.position.x)/2f);
    }

    public void AddSomeSnow(int amount, float point)
    {
    	for(int i = 0; i < amount; i++)
    	{
    		AddSnowAtPoint(point);
    	}
    	// sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0, 0), pixelDens);
	    // spriteRenderer.sprite = sprite;
	    // polygonCollider.Reset();
	    MakeCollider();
    }

    public void MakeCollider()
    {
    	// Debug.Log("wtf");
    	// PolygonCollider2D polcol = GetComponent<PolygonCollider2D>();
	    // Destroy(polcol);
		// polcol = gameObject.AddComponent<PolygonCollider2D>();
	  	// polcol.enabled = false;
	  	// polcol.points = new Vector2[sizeX+2];
	  	// // polcol.pathCount = sizeX+2;
	  	Vector2[] vertices = new Vector2[sizeX+2];
	  	vertices[0] = Vector2.zero;
	  	for(int i = 0; i < sizeX; i++)
	  	{
		  	vertices[i+1] = new Vector2(i*cellSize, points[i] * cellSize);
	  	}
	  	vertices[sizeX+1] = new Vector2(sizeX * cellSize, 0);
	  	selfCollider.points = vertices;
	  	partDetColl.points = vertices;
	  	// polcol.enabled = true;
    	// polcol.offset = new Vector2(0, -cellSize*2.5f);
    }
    public void Redraw()
    {
    	for(int i = 0; i < sizeX; i++)
		{
			for(int j= 0; j< currentHeight; j++)
			{
				if(j <= points[i]-1)
				{
					texture.SetPixel(i, j, Color.white);
				}
				else
				{
					texture.SetPixel(i, j, Color.clear);
				}
			}
		}
    }
}
