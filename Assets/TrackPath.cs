using UnityEngine;
using System.Collections;

public class TrackPath : MonoBehaviour {

    public uint pathLength = 0;


	// Use this for initialization
	void Start () {
        Debug.Assert(pathLength != 0);
        path = new float[pathLength];
        index = 0;
        InvokeRepeating("recordPath", 1, 1.0F);
	}
	
	// Update is called once per frame
	void Update () 
    {

	}


    void recordPath()
    {
        Debug.Log("Path length: " + index + "/" + pathLength);

        path[index] = transform.position.x;

        ++index;
    }

    private uint index;
    public float[] path;
}
