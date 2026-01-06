using UnityEngine;

public class GameManager : MonoBehaviour
{

    //[SerializeField] private Lander lander;

    private int score;

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
}
