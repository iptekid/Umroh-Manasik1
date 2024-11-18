using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

// Video Preloader and Cache Manager
public class VideoManager1 : MonoBehaviour
{
    private static VideoManager instance;
    public static VideoManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("VideoManager");
                instance = go.AddComponent<VideoManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private Dictionary<string, VideoPlayer> videoCache = new Dictionary<string, VideoPlayer>();
    private Queue<string> cacheQueue = new Queue<string>();
    private const int MAX_CACHED_VIDEOS = 5; // Adjust based on your needs

    private void Awake()
    {


        Application.targetFrameRate = 60; // Set target frame rate
    }

    public void PreloadVideo(string videoURL, System.Action<VideoPlayer> onPreloadComplete = null)
    {
        StartCoroutine(PreloadVideoRoutine(videoURL, onPreloadComplete));
    }

    private IEnumerator PreloadVideoRoutine(string videoURL, System.Action<VideoPlayer> onPreloadComplete)
    {
        if (videoCache.ContainsKey(videoURL))
        {
            onPreloadComplete?.Invoke(videoCache[videoURL]);
            yield break;
        }

        // Create new video player
        GameObject videoObject = new GameObject("PreloadedVideo_" + videoURL.GetHashCode());
        videoObject.transform.parent = transform;

        VideoPlayer videoPlayer = videoObject.AddComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;
        videoPlayer.playOnAwake = false;
        videoPlayer.skipOnDrop = true; // Skip frames if needed to catch up
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.renderMode = VideoRenderMode.APIOnly; // Don't render until needed
        videoPlayer.prepareCompleted += (source) => { };

        // Start preparing the video
        videoPlayer.Prepare();

        // Wait for video to be prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Cache management
        if (videoCache.Count >= MAX_CACHED_VIDEOS)
        {
            string oldestURL = cacheQueue.Dequeue();
            if (videoCache.TryGetValue(oldestURL, out VideoPlayer oldPlayer))
            {
                Destroy(oldPlayer.gameObject);
                videoCache.Remove(oldestURL);
            }
        }

        videoCache[videoURL] = videoPlayer;
        cacheQueue.Enqueue(videoURL);

        onPreloadComplete?.Invoke(videoPlayer);
    }

    public VideoPlayer GetCachedVideo(string videoURL)
    {
        return videoCache.ContainsKey(videoURL) ? videoCache[videoURL] : null;
    }

    public void ClearCache()
    {
        foreach (var video in videoCache.Values)
        {
            if (video != null)
            {
                Destroy(video.gameObject);
            }
        }
        videoCache.Clear();
        cacheQueue.Clear();
    }
}

// Optimized Video Player Component
public class OptimizedVideoPlayer : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] private string videoURL;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loopVideo = false;
    [SerializeField] private int targetFrameRate = 30;
    [SerializeField] private bool usePreloading = true;

    [Header("Performance Settings")]
    [SerializeField] private bool skipOnDrop = true;
    [SerializeField] private VideoRenderMode renderMode = VideoRenderMode.RenderTexture;
    [SerializeField] private bool asyncLoading = true;

    private VideoPlayer videoPlayer;
    private bool isPreloaded = false;

    private void Awake()
    {
        InitializeVideoPlayer();
    }

    private void Start()
    {
        if (usePreloading)
        {
            PreloadVideo();
        }
        else if (playOnStart)
        {
            PlayVideo();
        }
    }

    private void InitializeVideoPlayer()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        // Optimize video player settings
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;
        videoPlayer.playOnAwake = false;
        videoPlayer.skipOnDrop = skipOnDrop;
        videoPlayer.renderMode = renderMode;
        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
        //videoPlayer.frameRate = targetFrameRate;
        videoPlayer.waitForFirstFrame = asyncLoading;
        videoPlayer.isLooping = loopVideo;

        // Set error handling
        videoPlayer.errorReceived += HandleVideoError;
    }

    private void PreloadVideo()
    {
       // VideoManager.Instance.PreloadVideo(videoURL, OnPreloadComplete);
    }

    private void OnPreloadComplete(VideoPlayer preloadedPlayer)
    {
        isPreloaded = true;
        if (playOnStart)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        if (usePreloading && !isPreloaded)
        {
            PreloadVideo();
            return;
        }

        if (usePreloading)
        {
            //VideoPlayer cachedPlayer = VideoManager.Instance.GetCachedVideo(videoURL);
            //if (cachedPlayer != null)
            //{
            //    videoPlayer = cachedPlayer;
            //}
        }

        StartCoroutine(PlayVideoWhenReady());
    }

    private IEnumerator PlayVideoWhenReady()
    {
        if (!videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }
        }

        videoPlayer.Play();
    }

    public void PauseVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Pause();
        }
    }

    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

    private void HandleVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"Video Error: {message}");
        // Implement your error handling here
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.errorReceived -= HandleVideoError;
        }
    }

    // Optional: Memory optimization
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
        }
    }
}