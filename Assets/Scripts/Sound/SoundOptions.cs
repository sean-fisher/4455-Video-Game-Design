using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundOptions : MonoBehaviour
{

    [SerializeField] AudioMixer mixer;

	static SoundOptions instance;

	public static SoundOptions Instance() {return instance;}

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}

    public void SetMasterVolume(int percentage) {
        // Convert percentage to 0 - -80 range
        mixer.SetFloat("MasterVolume", (percentage - 100) * 0.8f);
    }

    public void SetMusicVolume(int percentage) {
        // Convert percentage to 0 - -80 range
        mixer.SetFloat("MusicVolume", (percentage - 100) * 0.8f);
    }
    
    public void SetSFXVolume(int percentage) {
        // Convert percentage to 0 - -80 range
        mixer.SetFloat("SFXVolume", (percentage - 100) * 0.8f);
    }
}
