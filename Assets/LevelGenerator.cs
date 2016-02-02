using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public GameObject julia;
    public GameObject romeo;
    public GameObject bush;
    public GameObject background;

    public const float lookAhead = 25;

	// Use this for initialization
	void Start () {

        while(yGenerated < lookAhead)
        {
            yGenerated += GenerateSection();
        }
        juliaMove = julia.GetComponent<PlayerMovement>();
        romeoMove = romeo.GetComponent<PlayerMovement>();
	}
	
    uint GenerateSection()
    {
        Debug.Log("Generatin section");


        //generate level
        List<List<bool>> section = new List<List<bool>>();
        for(uint i = 0; i < 25; ++i)
        {
            List<bool> row = new List<bool>(new bool[] {true,false,false,false,false,false,true});
            section.Add(row);
        }

        // check integrity









        // create section
        for (int i = 0; i < section.Count; ++i )
        {
            for(int j = 0; j < section[i].Count; ++j)
            {
                if(section[i][j])
                    Instantiate(bush, new Vector3(j + 0.5f, yGenerated + i, 0), transform.rotation);
            }
        }


        //add background
        Instantiate(background, new Vector3(3.5f, bgGenerated, 0), transform.rotation);
        Instantiate(background, new Vector3(3.5f, bgGenerated + 12.5f, 0), transform.rotation);
        bgGenerated += 25;
        return 25;
    }


	// Update is called once per frame
	void Update () 
    {
        float yPosition = 0;

        if (julia.activeSelf && !juliaMove.dead)
        {
            Debug.Log("Player1(julia) active");
            yPosition = julia.transform.position.y;
        }
        else if(romeo.activeSelf && !romeoMove.dead)
        {
            Debug.Log("Player2(romeo) active");
            yPosition = romeo.transform.position.y;
        }
        else
        {
            Debug.Log("ERROR: No active player");
        }

        while(yGenerated < yPosition + lookAhead)
        {
            yGenerated += GenerateSection();
        }
	}

    private bool[] openings;

    private float yGenerated = 0;
    private float bgGenerated = 0;

    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;
}
