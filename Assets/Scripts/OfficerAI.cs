using UnityEngine;
using UnityEngine.AI;

public class OfficerAI : MonoBehaviour
{
    public Transform player;
    public Transform chaseAreaCenter;
    public float chaseRadius = 10f;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRadius)
        {
            // Set the agent's destination to the player's position
            agent.SetDestination(player.position);
        }
        else
        {
            // If the player is outside the chase radius, stop the agent
            agent.ResetPath();
        }
    }
}
