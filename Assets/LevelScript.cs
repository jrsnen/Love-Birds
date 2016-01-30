using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {

    public float gamespeed = 1;
    public float levelLength = 100;

    public GameObject player;

	// Use this for initialization
	void Start () {
        romeosTurn = false;
        pathRun();
	}
	
    void pathRun()
    {
        Vector3 start = new Vector3(0, 0, 0);
        julia = (GameObject)Instantiate(player, start, transform.rotation);
    }

    void pointRun()
    {

        //level end
        Vector3 start = new Vector3(0, 0, 0);

        if(romeosTurn)
        {
            romeo = (GameObject)Instantiate(player, start, transform.rotation);
            julia = (GameObject)Instantiate(player, start, transform.rotation);
            //julia.setGhost();

        }
        else
        {
            romeo = (GameObject)Instantiate(player, start, transform.rotation);
            //romeo.setGhost();
            julia = (GameObject)Instantiate(player, start, transform.rotation);
        }
    }

	// Update is called once per frame
	void Update () {

        if(romeosTurn)
        {
            if(romeo.transform.position.y >= levelLength)
            {
                romeosTurn = false;
                pointRun();
            }
        }
        else
        {
            if(julia.transform.position.y >= levelLength)
            {
                romeosTurn = true;
                pointRun();
            }
        }
	
	}

    private bool romeosTurn;
    private GameObject romeo;
    private GameObject julia;
}
