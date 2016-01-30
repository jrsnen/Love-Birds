using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{

    public GameObject romeo;
    public GameObject julia;
    public GameObject ghost;

    public uint pathLength = 100;
    public float pathInterval = 1;
    public float levelLength = 2;
    public GameObject pathObject;

    // Use this for initialization
    void Start()
    {
        path = new float[pathLength];
        index = 0;
        InvokeRepeating("recordPath", 0, pathInterval);
        startPosition = new Vector3(0, 0, 0);
        romeosTurnNext = true;
        romeo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("julia Y: " + julia.transform.position.y);
        if (romeosTurnNext)
        {
            if (julia.transform.position.y >= levelLength)
            {
                romeo.SetActive(true);
                romeo.transform.position = startPosition;
                julia.SetActive(false);
                romeosTurnNext = false;
            }
        }
        else
        {
            if (romeo.transform.position.y >= levelLength)
            {
                julia.SetActive(true);
                julia.transform.position = startPosition;
                romeo.SetActive(false);
                romeosTurnNext = true;
            }
        }
        
    }

    void recordPath()
    {
        Debug.Log("Path length: " + index + "/" + pathLength);

        path[index] = julia.transform.position.x;
        Debug.Log("Path x: " + path[index]);
        ++index;
    }

    //void printPath()
    //{
    //    for (uint i = 0; i < index; ++i)
    //    {
    //        Vector3 v = new Vector3(path[i], pathInterval * i, 0);
    //        Instantiate(pathObject, v - startPosition, transform.rotation);
    //        Debug.Log("Path x: " + path[i]);
    //    }
    //}

    private uint index;

    private bool romeosTurnNext;
    private PlayerMovement romeoMove;
    private PlayerMovement juliaMove;

    private float[] path;
    private Vector3 startPosition;
}