using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject transition;
    public GameObject sphere;
    public OpacityAnimator animator;
    public VideoPlayer videoPlayer;

    void Start()
    {
        animator = sphere.GetComponent<OpacityAnimator>();
        OpacityAnimator transitionOA = transition.GetComponent<OpacityAnimator>();
        transitionOA.FadeIn(0);
        transitionOA.FadeOut(.5f);
        Invoke(nameof(ShowingSphere), 2);

    }

    public void ShowingSphere() {
        animator.FadeTo(0, 0);
        videoPlayer.Play();
        animator.FadeIn(3.0f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void samples() {
        // Get reference to the component
        //OpacityAnimator animator = sphere.GetComponent<OpacityAnimator>();

        // Fade to specific opacity
        animator.FadeTo(0.5f, 1.0f);  // Fade to 50% opacity over 1 second

        // Fade in/out
        animator.FadeIn(2.0f);        // Fade to fully opaque over 2 seconds
        animator.FadeOut(1.0f);       // Fade to fully transparent over 1 second

        // Instant opacity change
        animator.SetOpacity(0.75f);   // Instantly set to 75% opacity

        // Start ping-pong animation
        animator.StartPingPong(0.2f, 0.8f, 2.0f);  // Fade between 20% and 80% opacity every 2 seconds

        // Stop any ongoing animation
        animator.StopAnimation();

    }
}
