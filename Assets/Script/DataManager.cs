using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<VideoData> mainData;
    public List<VideoData> mainData2;
    public TextAsset textAsset;
    void Start()
    {
        //VideoData data = LoadFromJson();
        //mainData2.Add(data);
        LoadCourses();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public List<VideoData> courseCollections;


    public void LoadCourses()
    {
        try
        {
            if (textAsset != null)
            {
                // Since Unity's JsonUtility doesn't directly deserialize JSON arrays,
                // we need to wrap the array in a temporary object
                //string wrappedJson = "{ \"videoDatas\": " + textAsset.text + "}";
                //Debug.Log(wrappedJson);
                string wrappedJson = textAsset.text ;
                VideoDataWrapper wrapper = JsonUtility.FromJson<VideoDataWrapper>(wrappedJson);
                mainData2 = wrapper.videoDatas;

                // Log successful loading and course information
                Debug.Log($"Successfully loaded {mainData2.Count} videoDatas collections");
                PrintCourseInfo();
            }
            else
            {
                Debug.LogError("JSON TextAsset is not assigned!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading videoDatas: {e.Message}");
        }
    }

    // Helper method to print course information
    private void PrintCourseInfo()
    {
        for (int i = 0; i < mainData2.Count; i++)
        {
            VideoData vd = mainData2[i];
            Debug.Log($"videoDatas {i + 1}: {vd.nama}");
            Debug.Log($"Description: {vd.description}");
            Debug.Log($"Number of videos: {vd.videoData.Count}");
            Debug.Log("-------------------");
        }
    }

    // Method to get all course collections
    public List<VideoData> GetAllCourses()
    {
        return mainData2;
    }

    // Method to get a specific course by index
    public VideoData GetVideoDatas(int index)
    {
        if (mainData2 != null && index >= 0 && index < mainData2.Count)
        {
            return mainData2[index];
        }
        Debug.LogWarning($"videoDatas index {index} not found");
        return null;
    }

    // Method to get a specific course by name
    public VideoData GetVideoDatasByName(string courseName)
    {
        if (mainData2 != null)
        {
            return mainData2.Find(course => course.nama.Equals(courseName, StringComparison.OrdinalIgnoreCase));
        }
        return null;
    }

    // Method to search for videos across all courses
    public List<VideoData> SearchVideos(string searchTerm)
    {
        List<VideoData> results = new List<VideoData>();
        if (mainData2 == null) return results;

        foreach (var vd in mainData2)
        {
            SearchInVideoData(vd, searchTerm, results);
        }
        return results;
    }

    // Recursive helper method to search through nested video data
    private void SearchInVideoData(VideoData videoData, string searchTerm, List<VideoData> results)
    {
        if (videoData.nama.ToLower().Contains(searchTerm.ToLower()) ||
            videoData.description.ToLower().Contains(searchTerm.ToLower()))
        {
            results.Add(videoData);
        }

        if (videoData.videoData != null)
        {
            foreach (var subVideo in videoData.videoData)
            {
                SearchInVideoData(subVideo, searchTerm, results);
            }
        }
    }
}
