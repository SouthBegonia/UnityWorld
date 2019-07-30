using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The PT_Scoreboard class manages showing the score to the player
public class PT_Scoreboard : MonoBehaviour {
	public static PT_Scoreboard S; // The singleton for PT_Scoreboard
	
	public GameObject    prefabPT_FloatingScore;
	
	public bool ________________;
	
	public int            _score = 0;
	public string        _scoreString;
	
	// The score property also sets the scoreString
	public int score {
		get {
			return(_score);
		}
		set {
			_score = value;
			scoreString = Utils.AddCommasToNumber(_score);
		}
	}
	
	// The scoreString property also sets the GUIText.text
	public string scoreString {
		get {
			return(_scoreString);
		}
		set {
			_scoreString = value;
			GetComponent<GUIText>().text = _scoreString;
		}
	}
	
	void Awake() {
		S = this;
	}
	
	// When called by SendMessage, this adds the fs.score to this.score
	public void FSCallback(PT_FloatingScore fs) {
		score += fs.score;
	}
	
	// This will Instantiate a new PT_FloatingScore GameObject and initialize it.
	// It also returns a pointer to the PT_FloatingScore created so that the
	//  calling function can do more with it (like set fontSizes, etc.)
	public PT_FloatingScore CreatePT_FloatingScore(int amt, List<Vector3> pts) {
		GameObject go = Instantiate(prefabPT_FloatingScore) as GameObject;
		PT_FloatingScore fs = go.GetComponent<PT_FloatingScore>();
		fs.score = amt;
		fs.reportFinishTo = this.gameObject; // Set fs to call back to this
		fs.Init(pts);
		return(fs);
	}
	
	
}
