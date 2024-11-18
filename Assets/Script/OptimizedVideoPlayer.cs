using UnityEngine;
using UnityEngine.Video;
using System;

public class OptimizedVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private RenderTexture renderTexture;

    // Configuration options
    [Header("Performance Settings")]
    [SerializeField] private bool preloadVideo = true;
    [SerializeField] private uint targetBufferSize = 6; // Frames to buffer
    [SerializeField] private bool skipOnDrop = true;

    [Header("Debug Settings")]
    [SerializeField] private bool logTimestampWarnings = true;

    private bool timestampWarningLogged = false;
    private DateTime lastTimestampWarning = DateTime.MinValue;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }

        ConfigureVideoPlayer();
    }

    private void ConfigureVideoPlayer()
    {
        // Configure render mode
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        // Optimize buffering
        videoPlayer.waitForFirstFrame = preloadVideo;
        videoPlayer.frameDropped += OnFrameDropped;
        videoPlayer.skipOnDrop = skipOnDrop;

        // Set playback properties
        videoPlayer.playbackSpeed = 1.0f;
        videoPlayer.targetCameraAlpha = 1.0f;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

        // Configure buffering
        videoPlayer.controlledAudioTrackCount = 1;
       // videoPlayer.audioTrackCount = 1;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.timeUpdateMode = VideoTimeUpdateMode.GameTime;

        // Set buffer size
        Application.targetFrameRate = 60;
       // videoPlayer.frameReadyTimeout = 1000; // 1 second timeout

        // Add error handling
        videoPlayer.errorReceived += OnVideoError;
        videoPlayer.prepareCompleted += OnPrepareCompleted;
    }

    private void OnPrepareCompleted(VideoPlayer source)
    {
        // Log video information for debugging
        Debug.Log($"Video prepared: {source.url}\n" +
                 $"Duration: {source.length:F2} seconds\n" +
                 $"Frame rate: {source.frameRate:F2} fps\n" +
                 $"Frame count: {source.frameCount}\n" +
                 $"Width x Height: {source.width}x{source.height}");
    }

    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"Video Player Error: {message}");

        // Check for timestamp-related warnings
        if (message.Contains("timestamp") && logTimestampWarnings)
        {
            HandleTimestampWarning();
        }
    }

    private void HandleTimestampWarning()
    {
        // Only log the timestamp warning once per minute to avoid spam
        if (!timestampWarningLogged || (DateTime.Now - lastTimestampWarning).TotalMinutes >= 1)
        {
            Debug.LogWarning(
                "H.264 Profile Warning: Video may not be using Baseline profile.\n" +
                "To resolve this:\n" +
                "1. Re-encode video using H.264 Baseline profile\n" +
                "2. Command line example using FFmpeg:\n" +
                "   ffmpeg -i input.mp4 -vcodec libx264 -profile:v baseline output.mp4"
            );

            timestampWarningLogged = true;
            lastTimestampWarning = DateTime.Now;
        }
    }

    private void OnFrameDropped(VideoPlayer source)
    {
        Debug.LogWarning($"Frame dropped at time: {source.time}");

        // Implement frame drop handling logic here
        if (skipOnDrop)
        {
            // Skip to next keyframe if we're falling behind
            videoPlayer.time += 0.1f;
        }
    }

    // Optional: Monitor performance
    private void Update()
    {
        if (videoPlayer.isPlaying && Time.frameCount % 60 == 0) // Check every 60 frames
        {
            float currentFps = 1.0f / Time.deltaTime;
            if (currentFps < 30)
            {
                Debug.LogWarning($"Low FPS detected: {currentFps}");
            }
        }
    }

    public void PrepareVideo(string videoPath)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
    }

    // Clean up
    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.frameDropped -= OnFrameDropped;
            videoPlayer.errorReceived -= OnVideoError;
            videoPlayer.prepareCompleted -= OnPrepareCompleted;
        }
    }
}