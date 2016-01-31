using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public bool facingRight = false;
	public float speed;
    public bool dead = false;
    public bool ready = true;

    public ParticleSystem leafParticles;

    AudioSource audioSource;

    bool animationReady = true;


	// Use this for initialization
	void Start () {

        audioSource = GetComponent<AudioSource>();

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
        audioSource.Play();
        
        Vector3 start = this.transform.position;
        Vector3 add = new Vector3( 0f, 0.3f, 0f );
        if (animationReady)
            Move(other.transform, add, add, 0.2f);
    }

    private void Move(Transform target, Vector3 to, Vector3 back, float duration)
    {
       
        animationReady = false;

        Go.defaultEaseType = GoEaseType.Linear;
        var moveUpConfig = new GoTweenConfig().position( to, true );
        var moveBackConfig = new GoTweenConfig().position( -back, true );
        moveBackConfig.onComplete(orginalTween => Done());

        var tweenOne = new GoTween( target, duration, moveUpConfig);
        var tweenTwo = new GoTween( target, duration, moveBackConfig);


        var chain = new GoTweenChain();
        chain.append(tweenOne).append(tweenTwo);

        chain.play();

    }

    void Done()
    {
        //Done
        animationReady = true;
    }
    
}