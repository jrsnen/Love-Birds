using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flashing : MonoBehaviour {

    public Color bright;
    public Color normal;
    private Color target;
    private Color currentColor;

    float speed = 10.0f;

     Text ButtonText;

	// Use this for initialization
	void Start ()
    {
        ButtonText = this.GetComponent<Text>();
        currentColor = normal;
        target = bright;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentColor != target)
        {
            currentColor = Color.Lerp(currentColor, target, Time.deltaTime * speed);
        }
        else
        {
            if (target == bright)
            {
                target = normal;
            }
            else
            {
                target = bright;
            }
        }
        ButtonText.color = currentColor;

    }
}
