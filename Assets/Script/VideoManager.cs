using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{

    public DataManager dataManager;
    //public GameObject buttonPrefabs;
    //public Transform mainMenu, sub2Menu, sub3Menu;
    public List<Button> mainMenu, sub2MenuPengantar, sub2MenuMiqat, sub3Menu;

    void Start()
    {
        dataManager = GetComponent<DataManager>();
        Invoke(nameof(WaitSort), .5f);

    }
    public void WaitSort() {
        if (dataManager.isSort)
        {
            Attaching();
        }
        else {
            Invoke(nameof(WaitSort), .5f);
        }
    
    }
    public void Attaching() {
        for (int index = 0; index < dataManager.sortingData.Count; index++)
        {
            int i;
            i = index;
            dataManager.sortingData[i].button = mainMenu[i];
            dataManager.sortingData[i].button.name = dataManager.sortingData[i].nama;
            dataManager.sortingData[i].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].nama;
            

            for (int index2 = 0; index2 < dataManager.sortingData[i].videoData.Count; index2++)
            {
                int j;
                j = index2;
                List<Button> sub = new List<Button>();
                if (i == 0)
                {
                    sub = sub2MenuPengantar;
                }
                else if (i == 1) { 
                    sub = sub2MenuMiqat;
                }
                dataManager.sortingData[i].videoData[j].button = sub[j];
                dataManager.sortingData[i].videoData[j].button.name = dataManager.sortingData[i].videoData[j].nama;
                dataManager.sortingData[i].videoData[j].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].videoData[j].nama;

                for (int index3 = 0; index3 < dataManager.sortingData[i].videoData[j].videoData.Count; index3++)
                {
                    int k;
                    k = index3;

                    dataManager.sortingData[i].videoData[j].videoData[k].button = sub3Menu[k];
                    dataManager.sortingData[i].videoData[j].videoData[k].button.name = dataManager.sortingData[i].videoData[j].videoData[k].nama;
                    dataManager.sortingData[i].videoData[j].videoData[k].button.transform.GetChild(0).GetComponent<Text>().text = dataManager.sortingData[i].videoData[j].videoData[k].nama;
                }

            }
        }
    }
}