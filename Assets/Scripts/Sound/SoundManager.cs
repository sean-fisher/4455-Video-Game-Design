using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] AudioSource bgm;
	[SerializeField] Transform sfxSourceHolder;
	AudioSource[] generalUseAudioSources;
	[SerializeField] EnumClipPair[] audioClips;

	static SoundManager instance;

	public static SoundManager Instance { 
		get{
			return instance;
		}
	}

	void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this.gameObject);

			int i = 0;
			generalUseAudioSources = new AudioSource[sfxSourceHolder.childCount];
			foreach (Transform child in sfxSourceHolder) {
				generalUseAudioSources[i++] = child.GetComponent<AudioSource>();
			}
		} else {
			Destroy(this.gameObject);
		}
	}

	void Start() {
		// PlayBGM();
	}

	public void PlayBGM() {
		bgm.Stop();
		bgm.Play();
	}

	public void PauseBGM() {
		bgm.Pause();
	}

	public void StopBGM() {
		bgm.Stop();
	}

	public void UnPauseBGM() {
		bgm.UnPause();
	}

	public void PlayAnySFX(string sfxName) {
		
		AudioClip clip = null;
		foreach (var pair  in audioClips) {
			if (pair.clipName == sfxName) {

				clip = pair.clip;
				break;
			}
		}
		bool sfxPlayed = false;
		foreach (AudioSource source in generalUseAudioSources) {
			if (!source.isPlaying) {
				source.clip = clip;
				source.Play();
				sfxPlayed = true;
				break;
			}
		}
		if (!sfxPlayed) {
			generalUseAudioSources[0].clip = clip;
			generalUseAudioSources[0].Play();
		}
	}

	public void StopAllsfx() {
		for (int i = 0; i < generalUseAudioSources.Length; i++) {
			generalUseAudioSources[i].Stop();
		}
	}
	[System.Serializable]
	public class EnumClipPair {
		public string clipName;
		public AudioClip clip;
	}

}