using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System;



public class Agent : MonoBehaviour
{
    public NavMeshAgent[] agents;
    public GameObject agentModel;
    public GameObject player, ghost, meat;
    public GameObject[] cats, destinations;
    public GameObject testinggroup, gameplaygroup, menugroup;
    public Animator CurrentAnimation;

    private int State = 0;
    private int Animations = 0;
    private int WinLose = 0;
    private float DetectRange = 10;
    private float DetectAngle = 45;
    

    public TMP_Text RangeText, HiddenText, AngleText, DetectedText, TouchingText, AlarmText, WinLoseText;
    public GameObject Continue, Retry, Exit;
    private bool isInAngle = false;
    private bool isInRange = false;
    private bool isNotHidden = false;
    private bool isTouched = false;
    private bool testing = false;
    private bool isAlarmed;
    private bool menu;


    public Slider StealthTime;
    private float TimeLeft = 10.0f;

    private void RefillTimer()
    {
        if (TimeLeft < 10.0f)
        {
            TimeLeft += Time.deltaTime;
            StealthTime.value = TimeLeft;
            return;
        }

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

    public void ProgressCheck()
    {
        switch (WinLose)
        {
            case 0:
                {
                    menu = true;
                    WinLoseText.text = ("You Lost!");
                    Continue.SetActive(false);
                    Retry.SetActive(true);
                    Exit.SetActive(true);


                }
                break;
            case 1:
                {
                    menu = true;
                    WinLoseText.text = ("You Won!");
                    Continue.SetActive(true);
                    Retry.SetActive(true);
                    Exit.SetActive(true);


                }
                break;
        }

    }
    
    public void AssignRandomDestination(NavMeshAgent agent)
    {
        
        GameObject randomDestination = destinations[UnityEngine.Random.Range(0, destinations.Length)];
        agent.SetDestination(randomDestination.transform.position);
        




    }

    public void ChaseMode()
    {

        foreach (NavMeshAgent agent in agents)
        {
            agent.SetDestination(ghost.transform.position);

            if (isInAngle && isInRange && isNotHidden == true)
            {
                //A ghost should constantly follow the player, and it will freeze once the player can no longer be spotted. This allows the agents to act more human-like, going to where it was last spotted.
                //Once the agent clears that place, it returns to patrol, since the player has disappeared.
                
                ghost.transform.position = player.transform.position;

            }

            if (isTouched == true)
            {

            }
            else if (Vector3.Distance(agent.transform.position, ghost.transform.position) < 1f)
            {
                //MAKE THIS WORK FOR EVERY AGENT, SO THEY DONT ALL TRY GO THERE
                Debug.Log($"{agent.name} has lost the dog!");
                State = 0;
            }

        }

    }

    public void PatrolMode() 
    {
        foreach (NavMeshAgent agent in agents)
            if (Vector3.Distance(agent.transform.position, agent.destination) < 2f)
            {
                Debug.Log($"{agent.name} has met their destination!");
                AssignRandomDestination(agent);


            }
            else if (isInAngle && isInRange && isNotHidden == true)
            {
                State = 1;
               
            }

    }


    void Start()
    {
        menu = false;
        CurrentAnimation = GetComponent<Animator>();
        foreach (NavMeshAgent agent in agents)
        {
            AssignRandomDestination(agent);
            Debug.Log($"{agent.name} has been assigned a new destination!");

        }

       


    }

    void Update()    
    {
        if (testing == false)
        {

            testinggroup.SetActive(false);

        }
        else
        {

            testinggroup.SetActive(true);

        }
        if (menu == false)
        {

            menugroup.SetActive(false);

        }
        else
        {

            menugroup.SetActive(true);

        }

        if (Vector3.Distance(player.transform.position, meat.transform.position) < 2f)
        {
            WinLose = 1;
            State = 2;
           
        }


        //TEXT IS NOW REDUNDANT, I SHOULDVE BEEN USING DEBUG LOGS INSTEAD
        //A LOT OF INSTANTCES EXISTED, I LEFT ONE AS AN EXAMPLE

        foreach (NavMeshAgent agent in agents)
        {

            if (Vector3.Distance(agent.transform.position, player.transform.position) < DetectRange)
            {
                isInRange = true;
                //Debug.Log($"{agent.name} is in detection Range!"); <-- VERSION TWO, STILL ANNOYING

                //RangeText.text = "In Range"; <-- VERSION ONE, ONLY NECESSARY FOR TESTING IN THE VERY START
                //RangeText.color = Color.green;


            }
            else
            {
                isInRange = true;
               



            }

            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, (player.transform.position - agent.transform.position), out hit, Mathf.Infinity))
            {

                if (hit.transform == player.transform)
                {
                    isNotHidden = true;
                    //Debug.Log($"{agent.name} can see the player!");

                }
                else
                {
                    isNotHidden = false;
                    


                }
            }

            Vector3 side1 = player.transform.position - agent.transform.position;
            Vector3 side2 = agent.transform.forward;
            float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
            //Angle the player can see.
            if (angle < DetectAngle && angle > -1 * DetectAngle)
            {

                isInAngle = true;
                //Debug.Log($"{agent.name} has LOS on the player.");


            }
            else
            {
                isInAngle = false;    
            }

            if (isInAngle && isInRange && isNotHidden)
            {
                Debug.Log($"{agent.name} is now hunting!");
            }
            else
            {

            }

            if (Vector3.Distance(agent.transform.position, player.transform.position) < 1f)
            {
                isTouched = true;
                //Debug.Log($"{agent.name} is inside the player? Are they afk or something?");

            }
            else
            {
                isTouched = false;
            }




        }

        

       
        //CAT BEHAVIOUR!

        foreach (GameObject cat in cats)
        {
            RaycastHit AlarmHit;
            Physics.Raycast(cat.transform.position, cat.transform.forward, out AlarmHit, Mathf.Infinity);
           //HAD SOME PROBLEMS HERE, FIGURED OUT IT WAS A HEIGHT ISSUE OF THE VECTOR!


            if (AlarmHit.transform == player.transform)
            {

                isAlarmed = true;
                AlarmText.text = $"{cat.name} is Alarmed!";
                AlarmText.color = Color.green;
                Debug.Log($"{cat.name} has been triggered!");
                ghost.transform.position = AlarmHit.transform.position;
                State = 1;
                
            }
            else
            {
                isAlarmed = false;
                AlarmText.text = $"{cat.name} is not Alarmed!";
                AlarmText.color = Color.red;
                

            }

            

        }
        

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            State = 2;
            WinLose = 0;

        }

        //TESTING MOVING IT INTO THE SAME SCRIPT

        if (State == 1 && isInAngle && isInRange && isNotHidden == true || isTouched == true)
        {

            TimeLeft -= Time.deltaTime;
            StealthTime.value = TimeLeft;

        }
        else if (State == 0)
        {
            //FIX SO EVERYTIME IT ALWAYS TAKES 3 SECONDS
            Invoke("RefillTimer", 3);

        }

        //Basically the whole system for chasing. Need to implement the time system here as well, so it tracks how much time the player has left until they are "caught".

        switch (State)
        {

            case 0:

                //caseControl = 1; <-- first iteration tried to put it here, RUINED EVERYTHING! BAD IDEA!

                ghost.transform.position = player.transform.position;
                PatrolMode();

                break;
            case 1:

                //CHASE BEHAVIOUR!

                ChaseMode();

                break;
            case 2:
                //END OF GAME SHIT HERE.
                Debug.Log("Game might freeze, depends on win or lose.");
                ProgressCheck();
                break;


        }

        //LinkedList<string> Stars = new LinkedList<string>();
        ////Stars.AddFirst("Star1");
        ////Stars.AddFirst("Star3");

        //if (Stars.Contains("Star1"))
        //{
        //    Console.WriteLine("Star 1 Gained!");
        //    StarOne.sprite = StarFull;
        //}
        //else
        //{
        //    Console.WriteLine("No star 1, don't light up.");

        //}

        //if (Stars.Contains("Star2"))
        //{
        //    Console.WriteLine("Star 2 Gained!");
        //    StarTwo.sprite = StarFull;

        //}
        //else
        //{
        //    Console.WriteLine("No star 2, don't light up.");

        //}

        //if (Stars.Contains("Star3"))
        //{
        //    Console.WriteLine("Star 3 Gained!");
        //    StarThree.sprite = StarFull;

        //}
        //else
        //{
        //    Console.WriteLine("No star 3, don't light up.");

        //}

        //public Image StarOne, StarTwo, StarThree;
        //public Sprite StarFull;





}
}


