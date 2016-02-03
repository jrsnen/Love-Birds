using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public GameObject julia;
    public GameObject romeo;
    public GameObject bush;
    public GameObject background;

    public const float lookAhead = 25;
    public const float maxLookAhead = 75;


    struct Candidate
    {
        Vector2 position;
    }

	// Use this for initialization
	void Start () {

        bool succeed = false;
        while (!succeed)
        {
            if (!initialized)
                initializeGeneration();
            succeed = onePass();
        }
        createLevel();

        juliaMove = julia.GetComponent<PlayerMovement>();
        romeoMove = romeo.GetComponent<PlayerMovement>();
	}


    bool randomBool()
    {
        return Random.value >= objectDensity;
    }

    struct Vector2i
    {
        public int x;
        public int y;

        public Vector2i(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
       
    }

    void initializeGeneration()
    {
        Debug.Log("Generating section");
        endings = new bool[] { false, false, false, false, false, false, false };

        if (yGenerated > 49.9f)
            objectDensity = 0.75f;

        //generate a random level
        section = new List<List<bool>>();
        for (uint i = 0; i < 25; ++i)
        {
            bool[] content;
            if (i == 0)
                content = new bool[] { true, 
                    randomBool() && !openings[1], 
                    randomBool() && !openings[2],
                    randomBool() && !openings[3],
                    randomBool() && !openings[4],
                    randomBool() && !openings[5], 
                    true};
            else
                content = new bool[] { true, randomBool(), randomBool(), randomBool(), randomBool(), randomBool(), true };
            List<bool> row = new List<bool>( content );
            section.Add(row);
        }

        // add start points
        candidates = new List<Vector2i>();
        for (int i = 1; i < 6; ++i)
        {
            if (openings[i])
            {
                candidates.Add(new Vector2i(i, 0));
            }
        }

        maxTime = 100;

        initialized = true;
    }






    // returns whether section is finished
    bool onePass()
    {

        Vector2i current = candidates[candidates.Count - 1];
        candidates.RemoveAt(candidates.Count - 1);
        Debug.Log("Candidate x,y: " + current.x + "," + current.y);

        if (current.y == 24)
        {
            endings[current.x] = true;
            initialized = false;
            return true;
        }


        if (!section[current.y + 1][current.x])
        {

            if (!section[current.y + 1][current.x - 1])
            {
                candidates.Add(new Vector2i(current.x - 1, current.y + 1));
            }

            if (!section[current.y + 1][current.x + 1])
            {
                candidates.Add(new Vector2i(current.x + 1, current.y + 1));
            }

            candidates.Add(new Vector2i(current.x, current.y + 1));

        }
        --maxTime;

        if (candidates.Count == 0 || maxTime == 0)
        {
            initialized = false;
            Debug.Log("Section rejected!");
        }

        return false;
}
   


    // creates level from generation
    void createLevel()
    {

        openings = endings;


        // create section
        for (int i = 0; i < section.Count; ++i)
        {
            for (int j = 0; j < section[i].Count; ++j)
            {
                if (section[i][j])
                    Instantiate(bush, new Vector3(j + 0.5f, yGenerated + i, 0), transform.rotation);
            }
        }


        //add background
        Instantiate(background, new Vector3(3.5f, bgGenerated, 0), transform.rotation);
        Instantiate(background, new Vector3(3.5f, bgGenerated + 12.5f, 0), transform.rotation);
        bgGenerated += 25;
        yGenerated += 25;
    }




	// Update is called once per frame
	void Update () 
    {
        float yPosition = 0;

        if (julia.activeSelf && !juliaMove.dead)
        {
            yPosition = julia.transform.position.y;
        }
        else if(romeo.activeSelf && !romeoMove.dead)
        {
            yPosition = romeo.transform.position.y;
        }
        else
        {
            Debug.Log("ERROR: No active player");
        }

        //while(yGenerated < yPosition + lookAhead)
        //{
        //    yGenerated += GenerateSection();
        //}

        if (!initialized)
            initializeGeneration();

        // check if enough tiles are stored
        if (yGenerated < yPosition + maxLookAhead)
        {
            // small update
            if (yGenerated >= yPosition + lookAhead)
            {
                if (onePass())
                {
                    createLevel();
                }
            }// generate section, we are in a hurry
            else
            {
                bool succeed = false;
                while (!succeed)
                {
                    if (!initialized)
                        initializeGeneration();
                    succeed = onePass();
                }
                createLevel();
            }
        }

	}

    private float objectDensity = 0.8f;

    private bool[] openings = new bool[] { false, false, false, true,  false, false, false };
    private bool[] endings  = new bool[] { false, false, false, false, false, false, false };

    private float yGenerated = 0;
    private float bgGenerated = 0;

    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;
    
    
    // temp generation stuff
    private bool initialized = false;
    private uint maxTime = 100;
    private List<List<bool>> section = new List<List<bool>>();
    private List<Vector2i> candidates = new List<Vector2i>();
}
