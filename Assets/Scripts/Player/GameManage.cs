using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public static GameManage instance;

    [Header("Timer")]
    public float gameTime = 120f;

    public TextMeshProUGUI timerText;

    bool gameEnded = false;

    [Header("Collectibles")]
    public int totalCubes = 3;

    int collectedCubes = 0;

    [Header("Player")]
    public Transform player;

    [Header("Cube UI")]
    public TextMeshProUGUI cubeText;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        UpdateCubeUI();
        if (gameEnded)
        {
            return;
        }

        HandleTimer();

        CheckPlayerFall();
    }

    // timer
    void HandleTimer()
    {
        gameTime -= Time.deltaTime;

        if (gameTime < 0)
        {
            gameTime = 0;
        }

        int minutes = Mathf.FloorToInt(gameTime / 60);

        int seconds = Mathf.FloorToInt(gameTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

       //timer - lose
        if (gameTime <= 0)
        {
            gameEnded = true;

            SceneManager.LoadScene("LoseScene");
        }
    }

    
    public void CollectCube()
    {
        collectedCubes++;
        UpdateCubeUI();
        Debug.Log("Collected Cubes: "+ collectedCubes+ " / "+ totalCubes);

        // WIN CONDITION
        if (collectedCubes >= totalCubes)
        {
            gameEnded = true;

            SceneManager.LoadScene("WinScene");
        }
    }

    // FALL CHECK
    void CheckPlayerFall()
    {
        float maxDistance = 50f;

        // DISTANCE FROM CENTER
        if (player.position.magnitude > maxDistance)
        {
            gameEnded = true;

            SceneManager.LoadScene("LoseScene");
        }
    }

    void UpdateCubeUI()
    {
        cubeText.text = collectedCubes + " / " + totalCubes + " Cubes";
    }

}