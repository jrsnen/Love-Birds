using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {

    public string tag;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == tag)
            Debug.Log("win");
    }
}
