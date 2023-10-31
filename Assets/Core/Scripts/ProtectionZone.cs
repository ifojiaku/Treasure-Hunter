using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProtectionZone : MonoBehaviour
{
    //setup container for player to be chased
    //layer mask is created to only chase gameObject located in specified layer
    public LayerMask targetLayer;
    public Transform player;
    public GameObject treasure;
    //container is created for the center of detection zone, as well as the radius
    //container also created for any specified patrol points
    public Transform zoneCenter;
    public float zoneRadius;
    public Transform[] points;
    //initialize/define pointers for the array of patrol points
    private int destPoint = 0;
    private int newDestpoint;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //if the player is in the state of invulnerability, enemy will flee.
        //Otherwise, enemies will either chase or patrol
        if (!player.CompareTag("Invulnerable"))
        {
            //checks to see if guarded chest is still safe. if not, guard is unbounded by zone
            if (treasure.activeSelf)
            {
                //if a gameObject (located in 'target layer') is inside the 'radius' of 'zone center'
                if (Physics.CheckSphere(zoneCenter.position, zoneRadius, targetLayer))
                    //chase the object
                    chase();
                else
                {
                    //if statement prevents npc from prematurely choosing points
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        //will visit specified patrol points at random
                        patrol();
                }
            }
            //if chest is stolen, NPC will chase player no matter what
            else
            {
                chase();
            }
        }
        else
        {
            flee();
        }
        
    }

    void patrol()
    {
        if (points.Length == 0)
            return;
        //makes enemies move to their destination
        agent.destination = points[destPoint].position;
        //will look through list of given destination points at random
        do
            newDestpoint = Random.Range(0, points.Length);
        while (newDestpoint == points.Length);
        //sets next destination point for next iteration
        destPoint = newDestpoint;
    }

    void chase()
    {
        //enemy will follow the player
        agent.destination = player.position;
    }

    void flee()
    {
        //vector calculates the player's direction from the current enemy
        Vector3 playerDirection = (player.position - transform.position).normalized;
        //agent will follow a path opposite of the player direction
        agent.destination = transform.position - (playerDirection * 5);
    }
}
