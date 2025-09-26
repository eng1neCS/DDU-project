using UnityEngine;
using UnityEngine.AI;

public class EnemyNavAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Waypoints til patrulje
    public float detectionRadius = 10f; // Radius for at opdage spilleren
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;
    public float stopDistance = 1.5f;

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private Transform player;
    private bool chasingPlayer = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void FixedUpdate()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Spilleren er indenfor radius, s� start jagten.
            chasingPlayer = true;
            agent.speed = chaseSpeed;
            Vector3 directionToPlayer = player.position - transform.position;
            if (directionToPlayer.magnitude > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath(); // Stop bev�gelsen, n�r vi er t�t nok p�
            }
        }
        else
        {
            if (chasingPlayer)
            {
                // Stop jagten og vend tilbage til patrulje.
                chasingPlayer = false;
                agent.speed = patrolSpeed;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }

            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Tegner detection radius i editoren.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}