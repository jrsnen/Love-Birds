using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    public float speed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPosition = transform.position;

        Vector3 target = currentPosition;
        target.y += speed;

        if(Input.GetKey("left") || Input.GetKey("a"))
        {
            target.x -= speed;
        }

        if(Input.GetKey("right") || Input.GetKey("d"))
        {
            target.x += speed;
        }
        transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
	}
}
