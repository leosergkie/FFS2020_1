using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public SnowPile sp;
    public Snowball sb;
    public int objectType;

	public void HandleCollision(float point)
	{
		switch(objectType)
		{
			case 0:
				sp.AddSnowAtPoint(point);
				break;
			case 1:
				sb.Grow(point);
				break;
		}
	}
}
