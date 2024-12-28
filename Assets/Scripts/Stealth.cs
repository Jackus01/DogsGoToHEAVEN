using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Stealth : MonoBehaviour
{
    public Agent CaseAccess;
    public Slider StealthTime;
    public float TimeLeft = 10.0f;
    public bool Countdown = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if (CaseAccess.caseControl == 2)
        {
            TimeLeft -= Time.deltaTime;
            StealthTime.value = TimeLeft;

        }
        if (TimeLeft <= 0.0f)
        {
            Debug.Log("Time Up.");
            CaseAccess.caseControl = 3;
        }


    }
}
