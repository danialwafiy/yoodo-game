using UnityEngine;

// This controls the action after cube or circle touches the car. 
public class PlayerController : MonoBehaviour {

	GameController gameController;
	private void Start() {
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Point"){
			other.gameObject.SetActive (false);
			gameController.AddScore();
		}
		if(other.gameObject.tag == "Obstacle"){
			gameController.GameOver();
		}
	}
}