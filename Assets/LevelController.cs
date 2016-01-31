using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{

    public GameObject romeo;
    public GameObject julia;
    public GameObject rghost;
    public GameObject jghost;

    //public Animation juliaAnim;
    //public Animation romeoAnim;


    //public Sprite ghostJulia;
    //public Sprite ghostRomeo;

    public CameraFollow cf;

    public Text playerOne;
    public Text playerTwo;
    public AudioSource coinAudio;
    public uint pathLength = 1000;

    public Animator juliaAnim;
    public Animator romeoAnim;

    public Animator rghostAnim;
    public Animator jghostAnim;


    public float pathInterval = 0.01f;
    public float pathNodeDistance = 1.0f;
    public float ghostDistance = 10.0f;

    public float levelLength = 200;
    private float speed = 3.0f;
    private float ghostSpeed = 3.5f;

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
        rghost.SetActive(false);
        jghost.SetActive(false);

        juliaMove = julia.GetComponent<PlayerMovement>();
        romeoMove = romeo.GetComponent<PlayerMovement>();
        //ghostRend = ghost.GetComponent<SpriteRenderer>();
        juliaMove.speed = speed;
        juliaMove.ready = false;
        romeoMove.ready = false;
        juliaAnim.SetBool("fleeing", true);
        rghostAnim.SetBool("fleeing", true);
        jghostAnim.SetBool("fleeing", true);
        paused = true;

        path[0] = startPosition;

        //SEt GUi
        startGUI.InitPosition();
    }

    void pause()
    {
        paused = true;
        if (romeosTurnNext)
            romeoMove.ready = false;
        else
            juliaMove.ready = false;
    }
    void unpause()
    {
        paused = false;
        startTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {

        

        if (roundCounter < 4 )
        {
            roundCounterText.text = "Round: " + (roundCounter);

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


                uint maxpoints = 2;
                if (!firstRound)
                {
                    if (romeosTurnNext)
                    {
                        if (path[scoreIndex].y < julia.transform.position.y && scoreIndex <= maxindexNow)
                        {
                            ++scoreIndex;

                            if (maxpoints - Mathf.Abs(julia.transform.position.x - path[scoreIndex].x) > 0)
                            {
                                if(!coinAudio.isPlaying)
                                    coinAudio.Play();
                                juliascore += (uint)(maxpoints - Mathf.Abs(julia.transform.position.x - path[scoreIndex].x));
                            }
                        }
                    }
                    else
                    {
                        if (path[scoreIndex].y < romeo.transform.position.y && scoreIndex <= maxindexNow)
                        {
                            ++scoreIndex;

                            if (maxpoints - Mathf.Abs(romeo.transform.position.x - path[scoreIndex].x) > 0)
                            {
                                if (!coinAudio.isPlaying)
                                    coinAudio.Play();
                                romeoscore += (uint)(maxpoints - Mathf.Abs(romeo.transform.position.x - path[scoreIndex].x));
                            }
                        }
                    }
                }



                //Debug.Log("julia Y: " + julia.transform.position.y);
                if (romeosTurnNext)
                {
                       if(!firstRound && index <= maxindexNow)
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
                        rghost.SetActive(false);
                        jghost.SetActive(true);
                        rghost.transform.position = path[ghostIndex];
                        jghost.transform.position = path[ghostIndex];
                        cf.target = romeo;
                        pause();
                        scoreIndex = 0;
                        printPath();
                        //if(!firstRound)
                            roundCounter++;
                        //ghostRend.sprite = ghostRomeo;
                        //<juliaAnim.Play("m_fleeing");
                        maxindexNow = maxindex;

                        juliaAnim.SetBool("fleeing", true);
                        romeoAnim.SetBool("fleeing", false);
                        
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
                    if (!firstRound && index <= maxindexNow)
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
                        rghost.SetActive(true);
                        jghost.SetActive(false);
                        cf.target = julia;
                        index = 0;
                        rghost.transform.position = path[ghostIndex];
                        jghost.transform.position = path[ghostIndex];
                        pause();
                        maxindexNow = maxindex;
                        scoreIndex = 0;
                        printPath();
                        juliaAnim.SetBool("fleeing", false);
                        romeoAnim.SetBool("fleeing", true);
                        //ghostRend.sprite = ghostJulia;
                    }
                }
                rghost.transform.position = Vector3.MoveTowards(rghost.transform.position, targetPosition, Time.deltaTime * ghostSpeed);
                jghost.transform.position = Vector3.MoveTowards(jghost.transform.position, targetPosition, Time.deltaTime * ghostSpeed);


            }
        }
        else
        {
            //Show the score Screen
            Application.LoadLevel( 2 );
        }

        playerOne.text = juliascore.ToString();
        playerTwo.text = romeoscore.ToString();
        //scoreField.text = "Julia's Score: " + juliascore + " Romeo's score: " + romeoscore;
        
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
    private uint maxindexNow = 0;
    private Vector3[] path;

    private Vector3 targetPosition;
    private bool paused;
    private bool firstRound = true;

    private bool romeosTurnNext;
    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;
    //private SpriteRenderer ghostRend;
    //private Animator juliaAnim;
    //private Animator romeoAnim;
    //private Animator ghostAnim;

    private float timer = 0f;
    private float startTime = 0f;
    private float timeCap = 0.2f;

    private const uint ghostIndex = 0;


    private uint scoreIndex = 0;
    private uint lastScore = 0;

    private uint juliascore = 0;
    private uint romeoscore = 0;
    
}