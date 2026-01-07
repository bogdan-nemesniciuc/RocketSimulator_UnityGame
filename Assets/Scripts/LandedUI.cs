using TMPro;
using UnityEngine;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;


    private void Start()
    {
        Lander.Instace.OnLanded += Instace_OnLanded;
        Hide();
    }

    private void Instace_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType == Lander.LandingType.Succes)
        {
            titleTextMesh.text = "Successful landing!";

        } else
        {
            titleTextMesh.text = "<color=#000000>Crash!</color>";
        }
        statsTextMesh.text = Mathf.Round(e.landingSpeed * 2f) + "\n" + Mathf.Round(e.dotVector * 100f) + "\n" + "x" + e.scoreMultiplier + "\n" + e.score;
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
