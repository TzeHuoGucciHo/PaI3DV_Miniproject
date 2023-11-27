using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
//Components:
    private Transform player;
    private NavMeshAgent agent;
    private void Start()
    {
//Get the player transform and the NavMeshAgent component from the game object.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
//Set the destination of the NavMeshAgent to the player position.
        agent.SetDestination(player.position);
    }
}
