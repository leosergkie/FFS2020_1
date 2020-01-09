using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    [ContextMenu("Start")]
    void Awake()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        // Debug.Log(other);
        CollisionDetector pile = other.GetComponent<CollisionDetector>();
        if(pile == null)
        {
        	return;
        }
        int i = 0;

        while (i < numCollisionEvents)
        {
            pile.HandleCollision(collisionEvents[i].intersection.x);
            i++;
        }
    }
}
