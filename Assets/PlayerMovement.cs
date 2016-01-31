using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public bool facingRight = false;
	public float speed;
    public bool dead = false;
    public bool ready = true;

    public ParticleSystem leafParticles;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

        if (!dead && ready)
        {
            Vector3 currentPosition = transform.position;

            Vector3 target = currentPosition;
            target.y += speed;

            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                target.x -= speed;
                if (facingRight)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    facingRight = false;
                }
            }

            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                target.x += speed;
                if (!facingRight)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    facingRight = true;
                }
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
         
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        dead = true;
        leafParticles.Play();
    }


    
}