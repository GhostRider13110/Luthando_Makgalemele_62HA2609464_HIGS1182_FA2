using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private float updateInterval;

    private NavMeshAgent agent;
    private float timer;
    private bool gameEnded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || gameEnded) return;

        timer += Time.deltaTime; 
        if(timer > updateInterval)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }
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
}
