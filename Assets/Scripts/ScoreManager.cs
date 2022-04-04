using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public GameObject chance1, chance2, chance3;

    public Text highScoreText;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    
    private void OnEnable() {
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
        
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void RetryGame(){
		gameController.OpenPanel("ScorePanel");
    }

    public void StartGame(bool isTest){
        if(isTest){
            PlayerPrefs.SetInt("IsTry", 1); 
        }else{
            PlayerPrefs.SetInt("IsTry", 0); 
        }
		gameController.StartGame();
    }

    public void BonusLink(){
        Application.ExternalEval("window.open('https://www.yoodo.com.my', '_blank');");
        // Application.OpenURL("https://www.yoodo.com.my/");
    }
}
