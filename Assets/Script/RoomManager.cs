using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject startScreenCanvas;
    public OpacityAnimator startScreenOA;
    public UIOpacityAnimator startScreenCanvasOA;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScreenOA = startScreen.GetComponent<OpacityAnimator>();
        startScreenCanvasOA = startScreenCanvas.GetComponent<UIOpacityAnimator>();
        startScreenOA.FadeIn(.5f);
        startScreenCanvasOA.FadeIn(.5f);
    }

    public void DoHide() {

        startScreenOA.FadeOut(.5f);
        startScreenCanvasOA.FadeOut(.5f);
        Invoke(nameof(DelayOFF), .5f);
    }
    public void DelayOFF() {
        startScreenOA.GetComponent<MeshRenderer>().enabled = false;


    }
}
