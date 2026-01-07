using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    //[SerializeField] private Lander lander;


    public static GameManager Instance { get; private set; }
    private int score;
    private float time;


    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        time += Time.deltaTime;
    }

    private void Start()
    {
        Lander.Instace.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instace.OnLanded += Lander_OnLanded;
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
}
