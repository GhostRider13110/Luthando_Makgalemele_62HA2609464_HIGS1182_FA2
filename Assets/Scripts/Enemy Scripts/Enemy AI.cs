using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private enum State { Patrol, Chase }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 12f;
    [SerializeField] private float patrolWaittime = 2f;
    [SerializeField] private float patrolSpeedMultiplier = 0.5f;

    [Header("Chase")]
    [SerializeField] private float updateInterval = 0.2f; 

    private NavMeshAgent agent;
    private State state = State.Patrol;
    private Vector3 spawnPosition; 
    private float chaseTimer;
    private float waitTimer;
    private float chaseSpeed;
    private bool waiting; 
    private bool gameEnded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;

        if (chaseSpeed <= 0f) chaseSpeed = agent.speed;
        ApplySpeed();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        PickNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || gameEnded) return; 

        switch (state)
        {
            case State.Patrol:
                Patrol();
                CheckForPlayer();
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaittime)
            {
                waiting = false;
                PickNewPatrolPoint();
            }
            return; 
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            waiting = true; 
            waitTimer = 0;
        }
    }

    void PickNewPatrolPoint()
    {
        Vector3 randomPoint = spawnPosition + Random.insideUnitSphere * patrolRadius;
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void CheckForPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            state = State.Chase;
            waiting = false;
            ApplySpeed();
            Debug.Log(name + " detected the player! Distance: " + distance.ToString("F1"));
        }
    }

    void Chase()
    {
        chaseTimer += Time.deltaTime; 
        if (chaseTimer >= updateInterval)
        {
            agent.SetDestination(player.position);
            chaseTimer = 0f;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            
        }

        chaseSpeed = newSpeed;
        ApplySpeed();
    }

    void ApplySpeed()
    {
        if (agent == null) return;
        agent.speed = (state == State.Chase) ? chaseSpeed : chaseSpeed * patrolSpeedMultiplier;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!gameEnded && collision.gameObject.CompareTag("Player"))
        {
            GameOver(); 
        }
    }

    void GameOver()
    {
        gameEnded = true;
        agent.isStopped = true;

        if (gameOverPanel !=null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? spawnPosition : transform.position, patrolRadius);
    }
}
