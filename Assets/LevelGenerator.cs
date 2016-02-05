using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public GameObject julia;
    public GameObject romeo;
    public GameObject bush;
    public GameObject background;

    public const float lookAhead = 25;

    struct Candidate
    {
        Vector2 position;
    }

	// Use this for initialization
	void Start () {

        int loopMax = 500;
        List<bool> firstRow = new List<bool>(new bool[] { true, false, false, false, false, false, true });
        createRow(firstRow);
        ++yGenerated;
        while (lookAhead > yGenerated && loopMax > 0)
        {
            generateRow();
            --loopMax;
        }
        if (loopMax == 0)
            Debug.Log("Startloop failed");

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

    void generateRow()
    {
        bool legal = false;
        List<bool> row = new List<bool>();
        // create row
        while (!legal)
        {

            bool[] content;
            content = new bool[] { true, 
                randomBool(), 
                randomBool(),
                randomBool(),
                randomBool(),
                randomBool(), 
                true};
            row = new List<bool>(content);

            legal = checkRow(row);

        }

        createRow(row);
        ++yGenerated;
    }

    // returns whether section is finished
    bool checkRow(List<bool> row)
    {
        bool[] thisRowPath = new bool[] { false, false, false, false, false, false, false };
        // go through last rows possiblilities
        for (int i = 1; i < 6; ++i)
        {
            if (lastRowPath[i])
            {
                if(!row[i])
                {
                    thisRowPath[i] = true;
                    if(!row[i-1])
                    {
                        thisRowPath[i - 1] = true;
                    }
                    if(!row[i + 1])
                    {
                        thisRowPath[i + 1] = true;
                    }

                }
            }
        }
        if (thisRowPath[1] || thisRowPath[2] || thisRowPath[3] || thisRowPath[4] || thisRowPath[5])
        {
            lastRowPath = thisRowPath;
            return true;
        }
        return false;
    }

    void createRow(List<bool> row)
    {
        for (int i = 0; i < 7; ++i)
        {
            if (row[i])
                Instantiate(bush, new Vector3(i + 0.5f, yGenerated, 0), transform.rotation);
        }
    }

    void generateBackground()
    {
        //add background
        Instantiate(background, new Vector3(3.5f, bgGenerated, 0), transform.rotation);
        Instantiate(background, new Vector3(3.5f, bgGenerated + 12.5f, 0), transform.rotation);
        bgGenerated += 25;
    }

	// Update is called once per frame
	void Update () 
    {
        Debug.Log("Generation update");
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


        int loopMax = 100;
        // check if enough tiles are stored
        while (yGenerated < yPosition + lookAhead && loopMax > 0)
        {
            generateRow();

            if (bgGenerated < yPosition + lookAhead)
                generateBackground();
        }
        if (loopMax == 0)
            Debug.Log("Update loop failed");

	}

    private float objectDensity = 0.8f;

    private float yGenerated = 0;
    private float bgGenerated = 0;

    private bool[] lastRowPath = new bool[] { false, false, false, true, false, false, false };

    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;

}
