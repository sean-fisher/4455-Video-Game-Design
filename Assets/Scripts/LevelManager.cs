using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public string[] levels;

    static LevelManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public static LevelManager Instance() {return instance;}

    // Update is called once per frame
    void Update()
    {
        
    }
}
