using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIMove : MonoBehaviour {


    public Text gui;
    public Text gui2;

    private bool isDone = false;

    public float length = 656f;
    public float duration = 0.5f;
    private bool goScreen = true;

    private bool onScreen = false;

    private Vector3 scale = Vector3.zero;


    public void InitPosition()
    {
        RectTransform rectTrans = this.GetComponent<RectTransform>();
        rectTrans.position = new Vector3(rectTrans.position.x, rectTrans.position.y);

    }

    public void SetText( string text )
    {
        gui.text = text;
        gui2.text = text;
    }

    public void MoveWindow(int easeType)
    {
        switch (easeType)
        {
            case 0:
                Go.defaultEaseType = GoEaseType.Linear;
                break;
            case 1:
                Go.defaultEaseType = GoEaseType.BackOut;
                break;
            case 2:
                Go.defaultEaseType = GoEaseType.BounceOut;
                break;
            case 3:
                Go.defaultEaseType = GoEaseType.CircInOut;
                break;
            case 4:
                Go.defaultEaseType = GoEaseType.SineOut;
                break;
            default:
                Go.defaultEaseType = GoEaseType.Linear;
                break;
        }

        float move = 0;

        if (goScreen)
        {
            move = -length;
        }
        else
        {
            move = length;
        }


        var moveUpConfig = new GoTweenConfig().position(new Vector3(move, 0), true);
        moveUpConfig.onComplete(orginalTween => Done());

        var tweenOne = new GoTween(this.transform, duration, moveUpConfig);

        var chain = new GoTweenChain();
        chain.append(tweenOne);

        chain.play();
    }

    void Done()
    {
        if (goScreen)
        {
            onScreen = true;
            goScreen = false;

            isDone = true;

        }
        else
        {
            onScreen = false;
            goScreen = true;
        }

       

        //Debug.Log ("Settings tween Done!");
    }


    public bool IsDone()
    {
        return isDone;
    }

    public bool IsOnScreen()
    {
        return onScreen;
    }

    public void ScaleWindow()
    {

        if (goScreen)
        {
            Go.defaultEaseType = GoEaseType.BounceOut;
            scale = Vector3.one;
        }
        else
        {
            Go.defaultEaseType = GoEaseType.BounceIn;
            scale = Vector3.zero;
        }
        var scaleConfig = new GoTweenConfig().scale(scale, false);
        scaleConfig.onComplete(orginalTween => ScaleDone());

        var tweenOne = new GoTween(this.transform, duration, scaleConfig);

        var chain = new GoTweenChain();
        chain.append(tweenOne);

        chain.play();
    }

    void ScaleDone()
    {
        if (goScreen)
        {
            onScreen = true;
            goScreen = false;

        }
        else
        {
            onScreen = false;
            goScreen = true;
            isDone = true;
        }

        Debug.Log("Scale tween Done!");
    }

    public void SetScale(Vector3 newScale)
    {
        this.transform.localScale = newScale;
        goScreen = true;
        onScreen = false;
    }
}

