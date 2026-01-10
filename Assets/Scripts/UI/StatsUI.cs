using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;

    [SerializeField] private Image fuelImage;

    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {

        speedUpArrowGameObject.SetActive(Lander.Instace.GetSpeedY() >= 0);
        speedDownArrowGameObject.SetActive(Lander.Instace.GetSpeedY() < 0);

        speedLeftArrowGameObject.SetActive(Lander.Instace.GetSpeedX() < 0);
        speedRightArrowGameObject.SetActive(Lander.Instace.GetSpeedX() >= 0);

        fuelImage.fillAmount = Lander.Instace.GetFuelAmountNormalized(); 


        statsTextMesh.text = GameManager.Instance.GetLevelNumber() + "\n" +
            GameManager.Instance.GetScore() + "\n" + 
           Mathf.Round( GameManager.Instance.GetTime() ) + "\n" + 
            Mathf.Round(Lander.Instace.GetSpeedX() *10f) + "\n" +
            Mathf.Round(Lander.Instace.GetSpeedY()*10f) 
            
            ;
    }
}
