using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearGradientController : MonoBehaviour
{
    public SpriteRenderer sprite;
    [ContextMenu("Redraw")]
    void FixedUpdate()
    {
    	sprite.material.SetFloat("_TopCoord", transform.position.y + sprite.bounds.extents.y);
    	sprite.material.SetFloat("_BotCoord", transform.position.y -sprite.bounds.extents.y);
    }
}
