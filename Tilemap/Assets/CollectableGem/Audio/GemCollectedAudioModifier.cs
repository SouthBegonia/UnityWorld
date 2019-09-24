using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollectedAudioModifier : MonoBehaviour {

	public AudioSource collectedAudioSource;

	void Start(){
		float randomPitch = Random.Range (0.8f, 1.2f);
		collectedAudioSource.pitch = randomPitch;
	}
}
