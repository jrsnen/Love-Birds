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

    public GameObject pathObject;

    // Use this for initialization
    void Start()
    {
        path = new float[pathLength];
        index = 0;
        InvokeRepeating("recordPath", 0, pathInterval);
        startPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void recordPath()
    {
        Debug.Log("Path length: " + index + "/" + pathLength);

        path[index] = transform.position.x;
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

    private GameObject active;

    private float[] path;
    private Vector3 startPosition;
}