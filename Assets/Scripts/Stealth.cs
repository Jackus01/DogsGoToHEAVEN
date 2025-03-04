using System.Threading;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Stealth : MonoBehaviour
{
    public Agent CaseAccess;
    public Slider StealthTime;
    public float TimeLeft = 10.0f;
    public float Cooldown = 2.5f;


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
            Cooldown = 2.5f;

        }
        else if (TimeLeft <= 10.0f)
        {
            //FIX IT SO COOLDOWN CANT GO INTO NEGATIVES!
            // <= WORKS! I CHANGED IT TO DOUBLE EQUALS BUT IT WORKS RIGHT NOW
            Cooldown -= Time.deltaTime;
            if (Cooldown == 0f)
            {
                TimeLeft += Time.deltaTime;
                StealthTime.value = TimeLeft;

            }
            
            

        } 
        

        if (TimeLeft <= 0.0f)
        {
            Debug.Log("Time Up.");
            CaseAccess.caseControl = 3;
        }


    }
}
