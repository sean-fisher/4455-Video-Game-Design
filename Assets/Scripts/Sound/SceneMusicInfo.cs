using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicInfo : MonoBehaviour
{

    public string bgmSongName;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(bgmSongName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
