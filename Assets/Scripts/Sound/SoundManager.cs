using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] AudioSource bgm;
	[SerializeField] Transform sfxSourceHolder;
	AudioSource[] generalUseAudioSources;
	[SerializeField] NameClipPair[] sfxClips;
	[SerializeField] NameClipPair[] musicTracks;
	Dictionary<string, AudioClip> audioClipLookup;

	public GroupNamePair[] soundGroups;
	Dictionary<string, SoundGroup> audioGroupLookup;

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

			// create sfx dictionary for faster access
			audioClipLookup = new Dictionary<string, AudioClip>();
			foreach (var pair  in sfxClips) {
				audioClipLookup[pair.clipName] = pair.clip;
			}
			foreach (var pair  in musicTracks) {
				audioClipLookup[pair.clipName] = pair.clip;
			}

			audioGroupLookup = new Dictionary<string, SoundGroup>();
			foreach (var pair  in soundGroups) {
				audioGroupLookup[pair.groupname] = pair.group;
			}

			// get references to audio source
			int i = 0;
			generalUseAudioSources = new AudioSource[sfxSourceHolder.childCount];
			foreach (Transform child in sfxSourceHolder) {
				generalUseAudioSources[i++] = child.GetComponent<AudioSource>();
			}
		} else {
			Destroy(this.gameObject);
		}
	}

	public void PlayBGM(string bgmName) {
		bgm.clip = audioClipLookup[bgmName];
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
		
		AudioClip clip = audioClipLookup[sfxName];

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

	public void PlaySoundFromGroupAtRandom(string groupName) {
		SoundGroup group = audioGroupLookup[groupName];
		PlayAnySFX(group.soundNames[Random.Range(0, group.soundNames.Length)]);
	}

	public void StopAllsfx() {
		for (int i = 0; i < generalUseAudioSources.Length; i++) {
			generalUseAudioSources[i].Stop();
		}
	}
	[System.Serializable]
	public class NameClipPair {
		public string clipName;
		public AudioClip clip;
	}

	// groups similar sounds so they can be picked from randomly
	[System.Serializable]
	public class SoundGroup {
		public string[] soundNames;
	}
	[System.Serializable]
	public class GroupNamePair {
		public string groupname;
		public SoundGroup group;
	}
}