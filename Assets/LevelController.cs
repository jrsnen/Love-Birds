using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    public GameObject romeo;
    public GameObject julia;
    public GameObject ghost;

    public CameraFollow cf;

    public Text playerOne;
    public Text playerTwo;

    public uint pathLength = 1000;
    
    
    public float pathInterval = 0.01f;
    public float pathNodeDistance = 1.0f;
    public float ghostDistance = 10.0f;

    public float levelLength = 200;
    private float speed = 3.0f;
    private float ghostSpeed = 4.0f;

    public GameObject pathObject;

    public GUIMove startGUI;
    public GUIMove endScreen;
    public Text roundCounterText; 

    public Vector3 startPosition = new Vector3(0, 0, 0);

    private int roundCounter = 0;


    // Use this for initialization
    void Start()
    {
        path = new Vector3[pathLength];
        index = 0;
        romeosTurnNext = true;
        romeo.SetActive(false);
        ghost.SetActive(false);

        juliaMove = julia.GetComponent<PlayerMovement>();
        romeoMove = romeo.GetComponent<PlayerMovement>();

        juliaMove.ready = false;
        romeoMove.ready = false;

        paused = true;

        path[0] = startPosition;

        //SEt GUi
        startGUI.InitPosition();
    }

    void pause()
    {
        paused = true;
       // CancelInvoke();
        if (romeosTurnNext)
            romeoMove.ready = false;
        else
            juliaMove.ready = false;
    }
    void unpause()
    {
        paused = false;
        InvokeRepeating("ghostPath", 0, pathInterval);
       


        startTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {

        roundCounterText.text = "Round: "+roundCounter;

        if (roundCounter < 6 )
        {


            if (paused)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    unpause();

                    if (startGUI.IsDone() == false)
                    {
                        startGUI.MoveWindow(1);
                    }

                }
            }
            else
            {
                timer = Time.time - startTime;

                if (timer > timeCap)
                {

                    if (!romeosTurnNext)
                        romeoMove.ready = true;
                    else
                        juliaMove.ready = true;
                }




                //Debug.Log("julia Y: " + julia.transform.position.y);
                if (romeosTurnNext)
                {
                       if(!firstRound && index <= maxindex)
                       {
                           if (path[index].y < julia.transform.position.y + ghostDistance)
                            {
                                targetPosition = path[index];
                                ++index;
                            }
                       }
                    if(path[maxindex].y + pathNodeDistance < julia.transform.position.y)
                    {
                        // eka node asetetaan startissa
                        ++maxindex;
                        path[maxindex] = julia.transform.position;
                    }

                    // died or finished
                    if (julia.transform.position.y >= levelLength || juliaMove.dead)
                    {
                        firstRound = false;
                        romeoMove.speed = speed;
                        romeo.SetActive(true);
                        romeo.transform.position = startPosition;
                        //julia.SetActive(false);
                        romeosTurnNext = false;
                        romeoMove.dead = false;
                        index = 0;
                        ghost.SetActive(true);
                        ghost.transform.position = path[ghostIndex];
                        cf.target = romeo;
                        pause();
                        scoreIndex = 0;
                        printPath();
                    }
                }
                else
                {
                    if (path[maxindex].y + pathNodeDistance < romeo.transform.position.y)
                    {
                        // eka node asetetaan startissa
                        ++maxindex;
                        path[maxindex] = romeo.transform.position;
                    }
                    if (!firstRound && index <= maxindex)
                    {
                        if (path[index].y - ghostDistance < romeo.transform.position.y)
                        {
                            targetPosition = path[index];
                            ++index;
                        }
                    }


                    if (romeo.transform.position.y >= levelLength || romeoMove.dead)
                    {
                        juliaMove.speed = speed;
                        julia.SetActive(true);
                        julia.transform.position = startPosition;
                        //romeo.SetActive(false);
                        romeosTurnNext = true;
                        juliaMove.dead = false;
                        ghost.SetActive(true);
                        cf.target = julia;
                        index = 0;
                        ghost.transform.position = path[ghostIndex];
                        pause();
                        roundCounter++;
                        scoreIndex = 0;
                        printPath();
                    }
                }
                ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, targetPosition, Time.deltaTime * ghostSpeed);

                if (!firstRound)
                {
                    if (romeosTurnNext)
                    {
                        if (path[scoreIndex].y > julia.transform.position.y)
                        {
                            ++scoreIndex;

                            if (2 - Mathf.Abs(julia.transform.position.x - path[scoreIndex].x) > 0)
                                juliascore += 2 - Mathf.Abs(julia.transform.position.x - path[scoreIndex].x);
                        }
                    }
                    else
                    {
                        if (path[scoreIndex].y > romeo.transform.position.y)
                        {
                            ++scoreIndex;

                            if (2 - Mathf.Abs(julia.transform.position.x - path[scoreIndex].x) > 0)
                                romeoscore += 2 - Mathf.Abs(romeo.transform.position.x - path[scoreIndex].x);
                        }
                    }
                }
            }
        }
        else
        {
            //Show the score Screen
            if( endScreen.IsDone() == false )
                endScreen.MoveWindow( 4  );
        }

        playerOne.text = juliascore.ToString();
        playerTwo.text = romeoscore.ToString();
        //scoreField.text = "Julia's Score: " + juliascore + " Romeo's score: " + romeoscore;
        
    }

    void ghostPath()
    {
        //Debug.Log("Index:" + index + "/" + maxindex);
        ////if (maxindex <= index)
        ////{
        ////    // record path
        ////    Debug.Log("Path length: " + index + "/" + pathLength);
        ////    if(romeosTurnNext)
        ////    {
        ////        path[index] = julia.transform.position;
        ////    }
        ////    else
        ////        path[index] = romeo.transform.position;

            
        ////    //path[index] = julia.transform.position.x;
        ////    Debug.Log("Path x: " + path[index]);
        ////    ++maxindex;
        ////}
        ////else
        //if (maxindex > index + ghostIndex )
        //{
        //    Debug.Log("Ghosting");
        //    //ghost.transform.position = path[index];
        //    targetPosition = path[index + ghostIndex];
        //    // 
        //}
        //++index;
    }

    void printPath()
    {
        for (uint i = 0; i < maxindex; ++i)
        {

            Instantiate(pathObject,path[i], transform.rotation);
            Debug.Log("Path x: " + path[i]);
        }
    }

    private uint index;
    private uint maxindex;
    private Vector3[] path;

    private Vector3 targetPosition;
    private bool paused;
    private bool firstRound = true;


    private bool romeosTurnNext;
    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;

    private float timer = 0f;
    private float startTime = 0f;
    private float timeCap = 0.2f;

    private const uint ghostIndex = 0;


    private uint scoreIndex = 0;

    private float juliascore = 0;
    private float romeoscore = 0;
    
}