using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;
using DG.Tweening;

//This Script controls the complete game play.
public class GameController : MonoBehaviour {
	public GameObject Car;
	private bool isAddPocong = false;
	private bool isAddHantuRaya = false;
	float[] spawnWait = new float[3] {1f, 1.2f, 0.6f};
	public float startWait;
	private float speed = 6;
	float[] XPosition = new float[3] {-1.36f,0f, 1.36f};
	float[] XPositionSpecial = new float[2] {-0.87f,0.53f};
	bool IsGameOver;
	public Text GameScoreText;
	public int score=0;
	int highscore;
	public Text HighScoreText, scoreText;
	int CurrentPosition;
	private IEnumerator updateStatus,spawnWaves, updateScore;
	// Use this for initialization
    private Vector2 startTouchpPosition, endTouchPosition;
	public ParticleSystem leftDustParticle;
	public ParticleSystem rightDustParticle;
	public bool isLeftButton, isRightButton, CanSwipe, IsTest;

	public List<GameObject> panelList = new List<GameObject>();

	public GameObject spawnPoint, hintText;
	private float posX;
    public List<string> prefabList = new List<string>();

    public Renderer bgRenderer;

	public AudioSource scoreSource, musicSource, screamSource;

    private float rendererLength;

	string IsPhone;

	ObjectPoolManager objectPoolManager;	
	private void Awake() {
		objectPoolManager = GameObject.Find("ObjectPool").GetComponent<ObjectPoolManager>();
		IsGameOver = true;
		rendererLength = bgRenderer.bounds.size.y;
		posX = 0;
		CanSwipe = false;
	}

	public void SetIsPhone(string value){
		IsPhone = value;
	}

	void StartSwipe(){
		CanSwipe = true;
	}

	public void StartGame(bool value) {
		IsTest = value;

		score = 0;

		GameScoreText.text= score.ToString ();
		
		Invoke("StartSwipe", .5f);

		screamSource.Stop();

		musicSource.Play();

		IsGameOver = false;

		foreach(var panel in panelList){
			panel.gameObject.SetActive(false);
		}

		InvokeRepeating("IncreaseSpeed", 30, 30);

		if(IsPhone == "true"){
			hintText.GetComponent<Text>().text = "SWIPE TO COLLECT DUIT RAYA";
		}else{
			hintText.GetComponent<Text>().text = "USE ARROW KEYS TO COLLECT DUIT RAYA";
		}

		if(IsTest){
			hintText.SetActive(true);
		}else{
			hintText.SetActive(false);
		}

		if(!PlayerPrefs.HasKey("HighScore")){
			PlayerPrefs.SetInt ("HighScore", 0);
		}

		HighScoreText.text = "Skor Tertinggi " + PlayerPrefs.GetInt("HighScore").ToString ();

		if(spawnWaves != null){
			StopCoroutine(spawnWaves);
		}
		spawnWaves = SpawnWaves();
		StartCoroutine(spawnWaves);
	}

	void IncreaseSpeed(){
		speed += 1;
	}

	//this starts the spawn wave of the cubes and circles.
	IEnumerator SpawnWaves ()
	{
		//wait of startWait(int) seconds
		yield return new WaitForSeconds (startWait);
		while (true) {
			string type = prefabList[Random.Range (0, prefabList.Count)];
			float XPos = XPosition[Random.Range(0, XPosition.Length)];
			if(type == "HantuRaya"){
				XPos = XPositionSpecial[Random.Range(0, XPositionSpecial.Length)];
			}
			GameObject prefab = objectPoolManager.RequestPrefab(type);
            if(prefab != null){
                prefab.transform.position = new Vector2 (XPos, spawnPoint.transform.position.y);
                prefab.SetActive(true);
            }
			yield return new WaitForSeconds (spawnWait[Random.Range(0, spawnWait.Length)]);
		}
	}
	// Update is called once per frame

	void Update () {	
        transform.position += new Vector3(transform.position.x, speed * Time.deltaTime, transform.position.z);

		bgRenderer.material.mainTextureOffset += new Vector2(bgRenderer.transform.position.x, (speed / rendererLength) * Time.deltaTime);

		if(!IsGameOver){
			if (Input.GetKeyDown ("left")){
				MoveLeft();
			}//Action on right key pressed 
			else if (Input.GetKeyDown("right"))
			{
				MoveRight();
			}
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
				startTouchpPosition = Input.GetTouch(0).position;

			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
				endTouchPosition = Input.GetTouch(0).position;

				if(endTouchPosition.x < startTouchpPosition.x && CanSwipe){
					MoveLeft();
				}

				if(endTouchPosition.x > startTouchpPosition.x && CanSwipe){
					MoveRight();
				}
			}
			// If score more than 10 Spawn pocong
			if(score > 5 && !isAddPocong){
				prefabList.Add("Pocong");
				isAddPocong = true;
			}

			// // If score more than 20 Spawn hantu raya
			if(score > 10 && !isAddHantuRaya){
				prefabList.Add("HantuRaya");
				isAddHantuRaya = true;
			}
			Car.transform.DOMoveX (posX, 0.3f);
		}
	}

	private void MoveLeft(){
		leftDustParticle.Play();
		//If at center move to left
		if (CurrentPosition == 0) {
			posX = -1.321f;
			CurrentPosition = -1;
		} else if (CurrentPosition == 1) {
			posX = 0;
			CurrentPosition = 0;
		}
	}

	private void MoveRight(){
		rightDustParticle.Play();
		//If at center move to left
		if (CurrentPosition == 0) {
			posX = 1.321f;
			CurrentPosition = 1;
		} else if (CurrentPosition == -1) {
			posX = 0;
			CurrentPosition = 0;
		}	
	}

	public void AddScore(){
		//Add score by 1 and showing that score to GameScoreText.
		score+=1;
		GameScoreText.text= score.ToString ();
		scoreSource.Play();
	}

    IEnumerator UpdateStatus(){

        var url = NetworkManager._url +PlayerPrefs.GetString("PhoneNo")+"?score="+score;

		byte[] rawData = null;

        UnityWebRequest www = UnityWebRequest.Put(url, rawData);
        www.SetRequestHeader("Authorization", "Bearer " + NetworkManager._token);

        yield return www.SendWebRequest();
        if(www.result == UnityWebRequest.Result.ConnectionError){
        }
        if(www.isDone){
            if(www.result == UnityWebRequest.Result.Success){
                JSONNode data = JSON.Parse(www.downloadHandler.text);
                PlayerPrefs.SetInt("Tries", int.Parse(data["phone"]["tries"]));
				if(PlayerPrefs.GetInt("Tries") >= 3){
					OpenPanel("ZeroChancePanel");
				}else{
					OpenPanel("RealOverPanel");
				}
				ResetGame();
            }else{
                JSONNode error = JSON.Parse(www.downloadHandler.text);
            }
        }
    }
	
	public void GameOver(){	
		speed = 0;
		
		IsGameOver = true;

		musicSource.Stop();

		//sets is isGameOver to true
		CancelInvoke();
		
		//Stop creating cubes and circles.
		if(spawnWaves != null){
			StopCoroutine(spawnWaves);
		}

		screamSource.Play();

		if(updateScore != null){
			StopCoroutine(updateScore);
		}
		updateScore = UpdateScore();
		StartCoroutine(updateScore);
	}

	IEnumerator UpdateScore(){
		yield return new WaitForSeconds(.6f);		
				
		if (!IsTest) {
			if(score > PlayerPrefs.GetInt ("HighScore")){
				PlayerPrefs.SetInt ("HighScore", score);
			}
			if(updateStatus != null){
				StopCoroutine(updateStatus);
			}
			updateStatus = UpdateStatus();
			StartCoroutine(updateStatus);
		}else{
			OpenPanel("TestOverPanel");
			ResetGame();
		}
	}

	void ResetGame(){
		foreach (Transform child in  GameObject.Find("ObjectPool").gameObject.transform)
    		child.gameObject.SetActive(false);

		isAddPocong = false;

		isAddHantuRaya = false;

		posX = 0;

		Car.transform.position = new Vector2(0, Car.transform.position.y);

		CurrentPosition = 0;

		speed = 6;

		prefabList.Clear();

		prefabList.Add("Duit");

		prefabList.Add("Toyol");

		CanSwipe = false;
	}

	public void OpenPanel(string name){
		foreach(var panel in panelList){
			if(panel.name != name){
				panel.gameObject.SetActive(false);
			}else{
				panel.gameObject.SetActive(true);
			}
		}
	}
}
