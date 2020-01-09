using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{

    public void Grow(float point)
    {
    	transform.localScale+= (new Vector3(0.01f, 0.01f, 0.01f))/transform.localScale.x;
    }
}
