using UnityEngine;
using TMPro;
using UnityEditor;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Canisters")]
    [SerializeField] private int totalCanisters = 5;
    private int collectedCanisters = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text canisterText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject loseScreen;

    private bool gameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startScreen.SetActive(true);

        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        Time.timeScale = 0f; 

        UpdateCanisterUI();
    }

    public void CanisterCollected()
    {
        if (gameOver)
            return;

        collectedCanisters++;

        UpdateCanisterUI();

        if (collectedCanisters >= totalCanisters)
        {
            WinGame();
        }
    }

    private void UpdateCanisterUI()
    {
        canisterText.text = "Canisters: " + collectedCanisters + " / " + totalCanisters;
    }

    public void StartGame()
    {
        startScreen.SetActive (false);

        Time.timeScale = 1f;

        Debug.Log("Game Started");
    }

    private void WinGame()
    {
        gameOver = true;

        Debug.Log("You Win!"); 

        winScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void PlayerDied()
    {
        if (gameOver)
            return ;

        gameOver = true;

        Debug.Log("You Lose!");

        loseScreen.SetActive(true);

        Time .timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
}
