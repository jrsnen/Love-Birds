using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

  

    public void LoadLevel( int scene )
    {
       Application.LoadLevel( scene );
        Debug.Log("Load Level"+scene);
    }
}
