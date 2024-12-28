using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;


public class Agent : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject agentpos;
    public GameObject player, Destination, Destination2;
    public float DetectRange = 10;
    public float DetectAngle = 45;

    public TMP_Text RangeText;
    public TMP_Text HiddenText;
    public TMP_Text AngleText;
    public TMP_Text DetectedText;
    public int caseControl = 0;
    private int caseMemory;

    private bool isInAngle = false;
    private bool isInRange = false;
    private bool isNotHidden = false;

   

    void Start()
    {
        agent.SetDestination(Destination.transform.position);;



    }



    void Update()    {
        


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
        //How far away the AI can see the Player
        if (angle < DetectAngle && angle > -3 * DetectAngle)
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


        //Basically the whole system for chasing. Need to implement the time system here as well, so it tracks how much time the player has left until they are "caught".

        switch (caseControl)
        {
            case 0:
                //caseControl = 1; <-- first iteration tried to put it here, RUINED EVERYTHING! BAD IDEA!
                if (Vector3.Distance(agent.transform.position, Destination.transform.position) < 1f)
                {

                    agent.SetDestination(Destination2.transform.position);;
                    caseControl = 1;
                    caseMemory = 1;


                }
                else if (isInAngle && isInRange && isNotHidden == true)
                {
                    caseControl = 2;

                }
                break;
            case 1:

                if (Vector3.Distance(agent.transform.position, Destination2.transform.position) < 1f)
                {

                    agent.SetDestination(Destination.transform.position);;
                    caseControl = 0;
                    caseMemory = 0;

                }
                else if (isInAngle && isInRange && isNotHidden)
                {
                    caseControl = 2;

                }
                break;
            case 2:

                if (isInAngle && isInRange && isNotHidden)
                {

                    agent.SetDestination(player.transform.position);


                }
                else
                {
                    caseControl = caseMemory;
                    if (caseMemory == 0)
                    {
                        agent.SetDestination(Destination.transform.position);;
                    }
                    else if (caseMemory == 1)
                    {
                        agent.SetDestination(Destination2.transform.position);;
                    }
                    
                }
                break;
            case 3:
                Debug.Log("It FUCKING WORKS!");
                break;


        }



    }
}


