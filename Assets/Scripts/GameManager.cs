using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    //[SerializeField] private Lander lander;

     private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelList;



    public static GameManager Instance { get; private set; }
    private int score;
    private float time;
    private bool isTimerActive;


    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if(isTimerActive)
        {
            time += Time.deltaTime;
        }
        
    }
    private void LoadCurrentLevel()
    {
        foreach (GameLevel gamelevel in gameLevelList)
        {
            if (gamelevel.GetLevelNumber() == levelNumber)
            {
              GameLevel spawnedGameLevel =   Instantiate(gamelevel, Vector3.zero, Quaternion.identity);
               Lander.Instace.transform.position =  spawnedGameLevel.GetLanderStartPosition();
            }
        }
    }

    private void Start()
    {
        Lander.Instace.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instace.OnLanded += Lander_OnLanded;
        Lander.Instace.OnStateChanged += Lander_OnStateChanged;

        LoadCurrentLevel();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log("Total score " + score);
    }


    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(0);
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
}
