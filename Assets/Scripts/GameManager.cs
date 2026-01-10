using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    //[SerializeField] private Lander lander;

     private static int levelNumber = 1;
    private static int totalScore = 0 ;


    public static void ResetStaticData ()
    {
        levelNumber = 1;
        totalScore = 0 ;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;


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
       GameLevel gameLevel =  GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instace.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gamelevel in gameLevelList)
        {
            if (gamelevel.GetLevelNumber() == levelNumber)
            {
                return gamelevel;
               
            }
        }
        return null;
    }

    private void Start()
    {
        Lander.Instace.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instace.OnLanded += Lander_OnLanded;
        Lander.Instace.OnStateChanged += Lander_OnStateChanged;
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
        if(e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instace.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();

        }
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

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if(GetGameLevel() == null)
        {
            //No more levels
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        } else
        {

            //We still have more levels
                SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }

            
    }
    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }

    public void PauseUnpauseGame()
    {
        if(Time.timeScale == 1f)
        {
            PauseGame();

        } else
        {
            UnpauseGame();
        }
    }
}
