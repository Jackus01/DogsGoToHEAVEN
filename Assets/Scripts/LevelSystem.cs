using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public LevelSystem instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame  
    void Update()
    {
     if (Input.GetKey(KeyCode.U))
        {
            SceneManager.LoadScene("Level Template");
        }
    }


}
