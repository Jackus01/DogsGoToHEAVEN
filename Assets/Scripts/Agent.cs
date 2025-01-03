using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class Agent : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject agentModel;
    public GameObject player, ghost, Destination, Destination2;
    public Animator CurrentAnimation;

    private int caseControl = 0;
    private int Animations = 0;
    private int caseMemory;  
    private float DetectRange = 10;
    private float DetectAngle = 45;

    public TMP_Text RangeText, HiddenText, AngleText, DetectedText, TouchingText;
    private bool isInAngle = false;
    private bool isInRange = false;
    private bool isNotHidden = false;
    private bool isTouched = false;

    public Slider StealthTime;
    private float TimeLeft = 10.0f;

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
    private void DestinationOne()
    {
        
        agent.SetDestination(Destination.transform.position);

    }
    private void DestinationTwo()
    {
        
        agent.SetDestination(Destination2.transform.position);

    }
    public void WalkCycle()
    {
        switch (Animations)
        {
            case 0:
                {



                }





                break;
        }


    }

    void Start()
    {
        DestinationOne();
        CurrentAnimation = GetComponent<Animator>();

    }



    void Update()    
    {

        if (Vector3.Distance(agent.transform.position, player.transform.position) < DetectRange)
        {
            isInRange = true;
            RangeText.text = "In Range";
            RangeText.color = Color.green;


        }
        else
        {
            isInRange = true;
            RangeText.text = "Not In Range";
            RangeText.color = Color.red;



        }

        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, (player.transform.position - agent.transform.position), out hit, Mathf.Infinity))
        {

            if (hit.transform == player.transform)
            {
                isNotHidden = true;
                HiddenText.text = "Not Hidden";
                HiddenText.color = Color.green;
            }
            else
            {
                isNotHidden = false;
                HiddenText.text = "Hidden";
                HiddenText.color = Color.red;


            }
        }


        
        Vector3 side1 = player.transform.position - agent.transform.position;
        Vector3 side2 = agent.transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        //Angle the player can see.
        if (angle < DetectAngle && angle > -1 * DetectAngle)
        {

            isInAngle = true;
            AngleText.text = "In Vision Angle";
            AngleText.color = Color.green;


        }
        else
        {

            isInAngle = false;
            AngleText.text = "Not In Vision Angle";
            AngleText.color = Color.red;


        }

        if (isInAngle && isInRange && isNotHidden)
        {

            DetectedText.text = "Player Detected";
            DetectedText.color = Color.green;

        }
        else
        {

            DetectedText.text = "Player Undetected";
            DetectedText.color = Color.red;

        }

        if (Vector3.Distance(agent.transform.position, player.transform.position) < 1f)
        {
            isTouched = true;
            TouchingText.text = "Inside Player";

        }
        else
        {
            isTouched = false;
            TouchingText.text = "Not Inside Player";

        }



        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            caseControl = 3;

        }

        //TESTING MOVING IT INTO THE SAME SCRIPT

        if (caseControl == 2 && isInAngle && isInRange && isNotHidden == true || isTouched == true)
        {

            TimeLeft -= Time.deltaTime;
            StealthTime.value = TimeLeft;

        }
        else if (caseControl >= 1)
        {
            //FIX SO EVERYTIME IT ALWAYS TAKES 3 SECONDS
            Invoke("RefillTimer", 3);

        }

        //Basically the whole system for chasing. Need to implement the time system here as well, so it tracks how much time the player has left until they are "caught".

        switch (caseControl)
        {
            case 0:

                ghost.transform.position = player.transform.position;

                //caseControl = 1; <-- first iteration tried to put it here, RUINED EVERYTHING! BAD IDEA!
                if (Vector3.Distance(agent.transform.position, Destination.transform.position) < 1f)
                {

                    DestinationTwo();
                    caseControl = 1;
                    caseMemory = 1;


                }
                else if (isInAngle && isInRange && isNotHidden == true)
                {
                    caseControl = 2;

                }
                break;
            case 1:

                ghost.transform.position = player.transform.position;

                if (Vector3.Distance(agent.transform.position, Destination2.transform.position) < 1f)
                {

                    DestinationOne();
                    caseControl = 0;
                    caseMemory = 0;

                }
                else if (isInAngle && isInRange && isNotHidden == true)
                {
                    caseControl = 2;

                }
                break;
            case 2:

                agent.SetDestination(ghost.transform.position);

                if (isInAngle && isInRange && isNotHidden == true)
                {
                    //A ghost should constantly follow the player, but only when in range! It acts as a much more fair chasing mechanic. If the player can be seen, it will be chased!
                    //If not, the agent knows where it last was, and can chase after it!
                    ghost.transform.position = player.transform.position;

                }
                

                if (isTouched == true)
                {




                }
                else if (Vector3.Distance(agent.transform.position, ghost.transform.position) < 1f)
                {
                    //add idle animation here, invoke that shizzle
                    Debug.Log("Shit. Lost him!");
                    if (caseMemory == 0)
                    {
                        DestinationOne();
                        caseControl = caseMemory;

                    }
                    else if (caseMemory == 1)
                    {

                        DestinationTwo();
                        caseControl = caseMemory;

                    }




                }


                break;
            case 3:
                //END OF GAME SHIT HERE.
                Debug.Log("Game OVer Screen should be booted up, everything else frozen.");
                break;


        }



    }
}


