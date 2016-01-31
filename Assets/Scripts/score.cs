using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class score : MonoBehaviour {

    int romeoscore = 0;
    int juliascore = 0;

    public Text r;
      public  Text j;
	// Use this for initialization
	void Start () {
         romeoscore = PlayerPrefs.GetInt("romeoScore");
         juliascore = PlayerPrefs.GetInt("juliaScore");
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    j.text =""+ juliascore;
         r.text =""+ romeoscore;
	}
}
