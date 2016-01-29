using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    public float speed;
	// Use this for initialization
	void Start () {
        path = new float[20];

	}
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPosition = transform.position;

        Vector3 target = currentPosition;
        target.y += speed;

        transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
	}

    public float[] path;
}
