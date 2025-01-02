using System.Collections;
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

    private void RefillTimer()
    {
        if (TimeLeft < 10.0f)
        {
            TimeLeft += Time.deltaTime;
            StealthTime.value = TimeLeft;
        }
        else if (TimeLeft > 10.0f)
        {
            TimeLeft = 10.0f;
        }

    }

   

    // Update is called once per frame
    void Update()
    {
        if (TimeLeft <= 0f)
        {

            CaseAccess.caseControl = 3;

        }

        //FIX THIS NEXT TIME

        if (CaseAccess.caseControl == 2 && isInAngle && isInRange && isNotHidden == true)
        {

            TimeLeft -= Time.deltaTime;
            StealthTime.value = TimeLeft;

        }
        else if (CaseAccess.caseControl >= 1)
        {

            Invoke("RefillTimer", 3);

        }
        


    }

}

