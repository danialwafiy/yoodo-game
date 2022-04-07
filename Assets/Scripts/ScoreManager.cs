using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public GameObject chance1, chance2, chance3;

    public Text highScoreText, scoreText;

    public bool IsIgnoreChance = false;
    GameController gameController;

    // Start is called before the first frame update
    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    
    private void OnEnable() {
        if(!IsIgnoreChance){
            if(PlayerPrefs.GetInt("Tries") == 1){
                chance3.SetActive(false);
            }
            if(PlayerPrefs.GetInt("Tries") == 2){
                chance2.SetActive(false);
                chance3.SetActive(false);
            }
            if(PlayerPrefs.GetInt("Tries") == 3){
                chance1.SetActive(false);
                chance2.SetActive(false);
                chance3.SetActive(false);
            }
        }
        
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

        if(scoreText != null){
            scoreText.text = gameController.score.ToString();
        }
    }
    public void StartGame(bool isTest){
		gameController.StartGame(isTest);
    }

    public void MainMenu(){
        gameController.OpenPanel("ScorePanel");
    }

    public void BonusLink(){
        Application.ExternalEval("window.open('https://www.yoodo.com.my/rayapuaka', '_blank');");
        // Application.OpenURL("https://www.yoodo.com.my/");
    }
}
