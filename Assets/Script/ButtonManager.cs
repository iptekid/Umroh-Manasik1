using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public List<GameObject> menuParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> mainMenu, subMenu1Pengantar, subMenu1Miqat, subMenu2Pengantar;
    [Space(10)]
    public List<Material> mainMenuMat;
    public List<Material> subMenu1PengantarMat, subMenu1MiqatMat, subMenu2PengantarMat;
    public List<Vector3> mainMenuPos, subMenu1PengantarPos, subMenu1MiqatPos, subMenu2PengantarPos;
    Vector3 defScale = new Vector3(156.5381f, 9.084319f, 156.5381f);
    public state state;
    void Start()
    {
        for (int i = 0; i < mainMenu.Count; i++) {
            int index = i;
            Vector3 vector3 = new Vector3();
            vector3 = mainMenu[index].transform.localPosition;
            Material mat;
            mat = mainMenu[index].GetComponent<MeshRenderer>().material;
            mainMenuMat.Add(mat);
            mainMenuPos.Add(vector3);
        }
        for (int i = 0; i < subMenu1Pengantar.Count; i++)
        {
            int index = i;
            Vector3 vector3 = new Vector3();
            vector3 = subMenu1Pengantar[index].transform.localPosition;
            Material mat;
            mat = subMenu1Pengantar[index].GetComponent<MeshRenderer>().material;
            subMenu1PengantarMat.Add(mat);
            subMenu1PengantarPos.Add(vector3);
        }
        for (int i = 0; i < subMenu1Miqat.Count; i++)
        {
            int index = i;
            Vector3 vector3 = new Vector3();
            vector3 = subMenu1Miqat[index].transform.localPosition;
            Material mat;
            mat = subMenu1Miqat[index].GetComponent<MeshRenderer>().material;
            subMenu1MiqatMat.Add(mat);
            subMenu1MiqatPos.Add(vector3);
        }
        for (int i = 0; i < subMenu2Pengantar.Count; i++)
        {
            int index = i;
            Vector3 vector3 = new Vector3();
            vector3 = subMenu2Pengantar[index].transform.localPosition;
            Material mat;
            mat = subMenu2Pengantar[index].GetComponent<MeshRenderer>().material;
            subMenu2PengantarMat.Add(mat);
            subMenu2PengantarPos.Add(vector3);
        }

        menuParent.Add(mainMenu[0].transform.parent.gameObject);
        menuParent.Add(subMenu1Pengantar[0].transform.parent.gameObject);
        menuParent.Add(subMenu1Miqat[0].transform.parent.gameObject);
        menuParent.Add(subMenu2Pengantar[0].transform.parent.gameObject);

        ResetingAll(true);
        // Invoke(nameof(PlayMainMenu), 2);
    }
    public void ResetingAll1() { 
        ResetingAll(true);

    }
    public void ResetingAll() {
        if (state == state.mainMenu) {
            menuParent[0].SetActive(true);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub1Pengantar)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(true);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub1Miqat)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(true);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub2Pengantar)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(true);
        }


        for (int i = 0; i < mainMenu.Count; i++) {
            int index = i;
            mainMenu[index].transform.DOScale(defScale, .5f);
            mainMenuMat[index].DOFade(1, .4f);
            mainMenu[index].transform.DOLocalMove(Vector3.zero, .5f).OnComplete(delegate {
                //mainMenu[index].transform.DOLocalMove(mainMenuPos[index], .5f);
                mainMenu[index].transform.DOScale(Vector3.zero, .5f);
            });
        }

        for (int i = 0; i < subMenu1Pengantar.Count; i++)
        {
            int index = i;
            subMenu1Pengantar[index].transform.DOScale(defScale, .5f);
            subMenu1PengantarMat[index].DOFade(1, .4f);
            subMenu1Pengantar[index].transform.DOLocalMove(Vector3.zero, .5f).OnComplete(delegate {
                subMenu1Pengantar[index].transform.DOScale(Vector3.zero, .5f);

            });
        }

        for (int i = 0; i < subMenu1Miqat.Count; i++)
        {
            int index = i;
            subMenu1Miqat[index].transform.DOScale(defScale, .5f);
            subMenu1MiqatMat[index].DOFade(1, .4f);

            subMenu1Miqat[index].transform.DOLocalMove(Vector3.zero, .5f).OnComplete(delegate {
                subMenu1Miqat[index].transform.DOScale(Vector3.zero, .5f);

            });
        }

        for (int i = 0; i < subMenu2Pengantar.Count; i++)
        {
            int index = i;
            subMenu2Pengantar[index].transform.DOScale(defScale, .5f);
            subMenu2PengantarMat[index].DOFade(1, .4f);

            subMenu2Pengantar[index].transform.DOLocalMove(Vector3.zero, .5f).OnComplete(delegate {
                subMenu2Pengantar[index].transform.DOScale(Vector3.zero, .5f);

            });
        }
    }
    public void ResetingAll(bool force)
    {
        if (state == state.mainMenu)
        {
            menuParent[0].SetActive(true);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub1Pengantar)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(true);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub1Miqat)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(true);
            menuParent[3].SetActive(false);
        }
        else if (state == state.sub2Pengantar)
        {
            menuParent[0].SetActive(false);
            menuParent[1].SetActive(false);
            menuParent[2].SetActive(false);
            menuParent[3].SetActive(true);
        }
        float durs;
        if (force)
        {
            durs = 0;
        }
        else { 
            durs = .5f;

        }
        for (int i = 0; i < mainMenu.Count; i++)
        {
            int index = i;
            mainMenu[index].transform.DOScale(defScale, durs);
            mainMenuMat[index].DOFade(1, durs);
            mainMenu[index].transform.DOLocalMove(Vector3.zero, durs).OnComplete(delegate {
                //mainMenu[index].transform.DOLocalMove(mainMenuPos[index], .5f);
                mainMenu[index].transform.DOScale(Vector3.zero, durs);
            });
        }

        for (int i = 0; i < subMenu1Pengantar.Count; i++)
        {
            int index = i;
            subMenu1Pengantar[index].transform.DOScale(defScale, durs);
            subMenu1PengantarMat[index].DOFade(1, durs);
            subMenu1Pengantar[index].transform.DOLocalMove(Vector3.zero, durs).OnComplete(delegate {
                subMenu1Pengantar[index].transform.DOScale(Vector3.zero, durs);

            });
        }

        for (int i = 0; i < subMenu1Miqat.Count; i++)
        {
            int index = i;
            subMenu1Miqat[index].transform.DOScale(defScale, durs);
            subMenu1MiqatMat[index].DOFade(1, durs);

            subMenu1Miqat[index].transform.DOLocalMove(Vector3.zero, durs).OnComplete(delegate {
                subMenu1Miqat[index].transform.DOScale(Vector3.zero, durs);

            });
        }

        for (int i = 0; i < subMenu2Pengantar.Count; i++)
        {
            int index = i;
            subMenu2Pengantar[index].transform.DOScale(defScale, durs);
            subMenu2PengantarMat[index].DOFade(1, durs);

            subMenu2Pengantar[index].transform.DOLocalMove(Vector3.zero, durs).OnComplete(delegate {
                subMenu2Pengantar[index].transform.DOScale(Vector3.zero, durs);

            });
        }
    }
    public void PlayMainMenu()
    {
        state = state.mainMenu;
        menuParent[0].SetActive(true);
        menuParent[1].SetActive(true);
        menuParent[2].SetActive(true);
        menuParent[3].SetActive(true);

        for (int i = 0; i < mainMenu.Count; i++)
        {
            int index = i;
            mainMenu[index].transform.DOScale(defScale, .5f);
            //mainMenu[index].transform.GetChild(0).DOScale(Vector3.zero, 0);
            mainMenu[index].transform.DOLocalMove(Vector3.zero, .5f).OnComplete(delegate {
                mainMenu[index].transform.DOLocalMove(mainMenuPos[index], .5f);
                mainMenuMat[index].DOFade(1, .4f);
                //mainMenu[index].transform.GetChild(0).DOScale(new Vector3(0.005305198f, 0.005305198f, 0.005305198f), .5f).OnComplete(delegate {
                //});
                //mainMenu[index].transform.DOScale(Vector3.zero, .5f);
            });
        }
    }
    public void PlaySubMenu1Pengantar()
    {
        for (int i = 1; i < mainMenu.Count; i++)
        {
            int index = i;
            mainMenu[index].transform.DOScale(Vector3.zero, .5f);
        }
       // mainMenu[0].transform.GetChild(0).DOScale(Vector3.zero, .5f).OnComplete(delegate {
            mainMenu[0].transform.DOLocalMove(Vector3.zero, 2);
            mainMenu[0].transform.DOLocalRotate(new Vector3(mainMenu[0].transform.localEulerAngles.x, mainMenu[0].transform.localEulerAngles.y, 1800), 3f,RotateMode.FastBeyond360).
                OnComplete(delegate {

                    mainMenu[0].transform.DOLocalMove(subMenu1PengantarPos[0], 1f);
                    mainMenuMat[0].DOFade(0, .8f);
                    mainMenu[0].transform.DOScale(Vector3.zero, .8f);

                    for (int i = 0; i < subMenu1Pengantar.Count; i++)
                {
                    int index = i;
                    subMenu1Pengantar[index].transform.DOLocalMove(Vector3.zero, 0);
                    //subMenu1Pengantar[index].transform.GetChild(0).DOScale(Vector3.zero, 0);
                    subMenu1Pengantar[index].transform.DOScale(defScale, 0).OnComplete(delegate {
                        subMenu1Pengantar[index].transform.DOLocalMove(subMenu1PengantarPos[index], 1f).OnComplete(delegate { 
                        //subMenu1Pengantar[index].transform.GetChild(0).DOScale(new Vector3(0.005305198f, 0.005305198f, 0.005305198f), .5f).OnComplete(delegate {
                        //});


                        });

                    });
                }


            });
        //});

    }
    public void PlaySubMenu1Miqat()
    {
        state = state.sub1Miqat;

        for (int i = 0; i < mainMenu.Count; i++)
        {
            int index = i;
            if (i != 1) {
                mainMenu[index].transform.DOScale(Vector3.zero, .5f);
            }
            
        }
        //mainMenu[1].transform.GetChild(0).DOScale(Vector3.zero, .5f).OnComplete(delegate {
            mainMenu[1].transform.DOLocalMove(Vector3.zero, 2);
            mainMenu[1].transform.DOLocalRotate(new Vector3(mainMenu[1].transform.localEulerAngles.x, mainMenu[1].transform.localEulerAngles.y, 1800), 3f, RotateMode.FastBeyond360).
                OnComplete(delegate {

                    mainMenu[1].transform.DOLocalMove(subMenu1MiqatPos[0], 1f);
                    mainMenuMat[1].DOFade(0, .8f);
                    mainMenu[1].transform.DOScale(Vector3.zero, .8f);

                    for (int i = 0; i < subMenu1Miqat.Count; i++)
                    {
                        int index = i;
                        subMenu1Miqat[index].transform.DOLocalMove(Vector3.zero, 0);
                        //subMenu1Miqat[index].transform.GetChild(0).DOScale(Vector3.zero, 0);
                        subMenu1Miqat[index].transform.DOScale(defScale, 0).OnComplete(delegate {
                            subMenu1Miqat[index].transform.DOLocalMove(subMenu1MiqatPos[index], 1f).OnComplete(delegate {
                                //subMenu1Miqat[index].transform.GetChild(0).DOScale(new Vector3(0.005305198f, 0.005305198f, 0.005305198f), .5f).OnComplete(delegate {
                               // });


                            });

                        });
                    }




                });
        //});

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu() { 
        
    
    }
}
