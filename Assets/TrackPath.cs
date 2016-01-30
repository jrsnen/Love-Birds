using UnityEngine;
using System.Collections;

public class TrackPath : MonoBehaviour {

    public uint pathLength = 0;
    public float pathInterval = 1;

    public GameObject pathObject;
    public GameObject ghost;

    // Use this for initialization
    void Start () {
        Debug.Assert(pathLength != 0);
        Debug.Assert(false);
        path = new float[pathLength];
        index = 0;
        InvokeRepeating("recordPath", 0, pathInterval);
        pathShown = false;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update () 
    {
        // for debugging
        if (index > 10 && !pathShown)
        {
            printPath();
            pathShown = true;
        }

    }

    void recordPath()
    {
        Debug.Log("Path length: " + index + "/" + pathLength);

        path[index] = transform.position.x;
        Debug.Log("Path x: " + path[index]);
        ++index;
    }

    void printPath()
    {
        for (uint i = 0; i < index; ++i)
        {
            Vector3 v = new Vector3(path[i], pathInterval*i, 0);
            Instantiate(pathObject, v - startPosition, transform.rotation);
            Debug.Log("Path x: " + path[i]);
        }
    }

    private bool pathShown;
    private uint index;

    private float[] path;
    private Vector3 startPosition;


    
}
