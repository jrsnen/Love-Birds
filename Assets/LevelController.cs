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

    public CameraFollow cf;

    public Text playerOne;
    public Text playerTwo;
    public Text playerOnefinal;
    public Text playerTwofinal;
    public AudioSource coinAudio;

    public Animator juliaAnim;
    public Animator romeoAnim;

    public Animator rghostAnim;
    public Animator jghostAnim;


    public float pathInterval = 0.01f;
    public float pathNodeDistance = 1.0f;
    public float ghostDistance = 10.0f;

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
        //path = new Vector3[pathLength];
        ghostIndex = 0;
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
        paused = true;

        Coin c = new Coin { picked = false, 
            coin = (GameObject)Instantiate(pathObject, startPosition, transform.rotation),
            position = startPosition };
        path.Add(c);

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



                // check for score addition
                uint maxpoints = 2;
                if (!firstRound)
                {
                    if (romeosTurnNext)
                    {
                        if (scoreIndex <= maxindexNow && path[scoreIndex].position.y < julia.transform.position.y)
                        {
                            

                            if (Mathf.Abs(julia.transform.position.x - path[scoreIndex].position.x) < 0.5f)
                            {
                                if(!path[scoreIndex].picked)
                                {
                                    Coin c = path[scoreIndex];
                                    c.picked = true;
                                    Destroy(c.coin);
                                    path[scoreIndex] = c;
                                    //path[scoreIndex].picked = true;
                                    if (!coinAudio.isPlaying)
                                        coinAudio.Play();
                                    ++juliascore;
                                }

                            }
                            ++scoreIndex;
                        }
                    }
                    else
                    {
                        if (scoreIndex <= maxindexNow && path[scoreIndex].position.y < romeo.transform.position.y)
                        {
                            

                            if (Mathf.Abs(romeo.transform.position.x - path[scoreIndex].position.x) < 0.5f)
                            {
                                if (!path[scoreIndex].picked)
                                {
                                    Coin c = path[scoreIndex];
                                    c.picked = true;
                                    Destroy(c.coin);
                                    path[scoreIndex] = c;
                                    if (!coinAudio.isPlaying)
                                        coinAudio.Play();
                                    ++romeoscore;
                                }
                            }
                            ++scoreIndex;
                        }
                    }
                }

                // check for new coins
                if (romeosTurnNext)
                {
                       if(!firstRound && ghostIndex <= maxindexNow)
                       {
                           if (path[ghostIndex].position.y < julia.transform.position.y + ghostDistance)
                            {
                                targetPosition = path[ghostIndex].position;
                                ++ghostIndex;
                            }
                       }
                    if(path[path.Count - 1].position.y + pathNodeDistance < julia.transform.position.y)
                    {
                        // eka node asetetaan startissa
                        Coin c = new Coin { picked = false,
                            coin = (GameObject)Instantiate(pathObject, julia.transform.position, transform.rotation), 
                            position = julia.transform.position };
                        path.Add(c);
                        ++juliascore;
                    }

                    // died or finished
                    if (juliaMove.dead)
                    {
                        firstRound = false;
                        romeoMove.speed = speed;
                        romeo.SetActive(true);
                        romeo.transform.position = startPosition;
                        //julia.SetActive(false);
                        romeosTurnNext = false;
                        romeoMove.dead = false;
                        ghostIndex = 0;
                        rghost.SetActive(false);
                        jghost.SetActive(true);
                        jghostAnim.SetBool("fleeing", true);
                        rghost.transform.position = path[0].position;
                        jghost.transform.position = path[0].position;
                        cf.target = romeo;
                        pause();
                        scoreIndex = 0;
                        //if(!firstRound)
                            roundCounter++;
                        //ghostRend.sprite = ghostRomeo;
                        //<juliaAnim.Play("m_fleeing");
                        maxindexNow = path.Count - 1;

                        juliaAnim.SetBool("fleeing", true);
                        romeoAnim.SetBool("fleeing", false);
                        
                    }
                }
                else
                {
                    if (path[path.Count - 1].position.y + pathNodeDistance < romeo.transform.position.y)
                    {
                        // eka node asetetaan startissa
                        // add a coin and a point for romeo
                        Coin c = new Coin { picked = false,
                            coin = (GameObject)Instantiate(pathObject, romeo.transform.position, transform.rotation),
                            position = romeo.transform.position };
                        path.Add(c);
                        ++romeoscore;
                    }
                    if (!firstRound && ghostIndex <= maxindexNow)
                    {
                        if (path[ghostIndex].position.y - ghostDistance < romeo.transform.position.y)
                        {
                            targetPosition = path[ghostIndex].position;
                            ++ghostIndex;
                        }
                    }

                    if (romeoMove.dead)
                    {
                        juliaMove.speed = speed;
                        julia.SetActive(true);
                        julia.transform.position = startPosition;
                        //romeo.SetActive(false);
                        romeosTurnNext = true;
                        juliaMove.dead = false;
                        rghost.SetActive(true);
                        rghostAnim.SetBool("fleeing", true);
                        jghost.SetActive(false);
                        cf.target = julia;
                        ghostIndex = 0;
                        rghost.transform.position = path[0].position;
                        jghost.transform.position = path[0].position;
                        pause();
                        maxindexNow = path.Count - 1;
                        scoreIndex = 0;
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
            PlayerPrefs.SetInt("romeoScore", (int)romeoscore);
            PlayerPrefs.SetInt("juliaScore", (int)juliascore);
        }

        playerOne.text = juliascore.ToString();
        playerOnefinal.text = juliascore.ToString();
        playerTwo.text = romeoscore.ToString();
        playerTwofinal.text = romeoscore.ToString();
        //scoreField.text = "Julia's Score: " + juliascore + " Romeo's score: " + romeoscore;
        
    }

    private int ghostIndex;
    private int maxindexNow = 0;
    //private Vector3[] path;
    private List<Coin> path = new List<Coin>();

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

    private int scoreIndex = 0;
    private uint lastScore = 0;

    private uint juliascore = 0;
    private uint romeoscore = 0;

}


struct Coin
{
    public bool picked;
    public GameObject coin;
    public Vector3 position;
}