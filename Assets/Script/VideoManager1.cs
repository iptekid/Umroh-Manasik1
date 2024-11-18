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
