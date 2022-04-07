using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;

public class Keypad : MonoBehaviour
{
    string word = null;
    int wordIndex = 1;
    public int maxIndex = 12;
    string alpha;
    public Text phoneNoText,loadingGameText;
    public Text toastText;
    private IEnumerator login,displayLoad;
    public GameObject loadingBar,loadingGame;
    public Button playBtn;
    
    GameController gameController;

    private void Awake() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    public void KeypadPressed(string alphabet){
        toastText.text = "";
        if(alphabet == "-1"){
            word = word.Substring(0, word.Length - 1);
            wordIndex --;
            phoneNoText.text = word; 
        }
        if(wordIndex <= maxIndex && alphabet != "-1"){
            wordIndex++;
            word = word + alphabet;
            phoneNoText.text = word;
        }
    }

    public void Play(){
        toastText.text = "";
        if(phoneNoText.text != ""){
            playBtn.interactable = false;
            if(login != null){
                StopCoroutine(login);
            }
            login = CheckPhone();
            StartCoroutine(login);
        }
    }

    IEnumerator CheckPhone(){        
        loadingBar.SetActive(true);
        PlayerPrefs.DeleteAll();

        var phoneNo = phoneNoText.text;
        var url = NetworkManager._url +phoneNo;

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + NetworkManager._token);

        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.ConnectionError){
            toastText.text = "Network Error";
            loadingBar.SetActive(false);
        }else{
            if(www.isDone){
                if(www.result == UnityWebRequest.Result.Success){
                    loadingBar.SetActive(false);
                    JSONNode data = JSON.Parse(www.downloadHandler.text);
                    PlayerPrefs.SetString("PhoneNo", data["phone"]["phone_no"]);
                    PlayerPrefs.SetInt("HighScore", int.Parse(data["phone"]["score"]));
                    PlayerPrefs.SetInt("Tries", int.Parse(data["phone"]["tries"]));
                    if(displayLoad != null){
                        StopCoroutine(displayLoad);
                    }
                    displayLoad = DisplayLoad();
                    StartCoroutine(displayLoad);
                }else{
                    loadingBar.SetActive(false);
                    word = null;
                    wordIndex = 1;
                    phoneNoText.text = "";
                    JSONNode error = JSON.Parse(www.downloadHandler.text);
                    toastText.text = error["error"];
                    playBtn.interactable = true;
                }
            }
        }
    }
    IEnumerator DisplayLoad()
    {
        loadingGame.SetActive(true);

        yield return new WaitForSeconds(3);

        loadingGameText.text = "Jangan sampai terlanggar hantu.";

        yield return new WaitForSeconds(3);
        

        loadingBar.SetActive(false);
        loadingGame.SetActive(false);

        playBtn.interactable = true;

        if(PlayerPrefs.GetInt("Tries") < 3){
            gameController.OpenPanel("ScorePanel");
        }else{
            gameController.OpenPanel("ZeroChancePanel");
        }
    }
}