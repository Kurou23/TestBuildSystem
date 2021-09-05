using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHander : MonoBehaviour
{
    public static UIHander Instance;

    public GameObject BuildPanel;
    public GameObject InstrucionBuild;
    public GameObject InstrucionBuild2;
    public GameObject[] CreateComponentPanel;
    public GameObject EditPanel;
    public GameObject[] EditComponentPanel;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        ShowBuildPanel();
        SelectCreateLayout(0);
    }

    public void HideBuildPanel()
    {
        BuildPanel.SetActive(false);
        InstrucionBuild.SetActive(true);
    }

    public void ShowBuildPanel()
    {
        BuildPanel.SetActive(true);
        InstrucionBuild.SetActive(false);
        InstrucionBuild2.SetActive(false);
        EditPanel.SetActive(false);
    }

    public void SelectCreateLayout(int index) {
        for (int i = 0; i < CreateComponentPanel.Length; i++)
        {
            if (i == index)
            {
                CreateComponentPanel[i].SetActive(true);
            }
            else
            {
                CreateComponentPanel[i].SetActive(false);
            }
        }
    }

    public void ShowEditPanel() {
        EditPanel.SetActive(true);
        InstrucionBuild2.SetActive(true);
        BuildPanel.SetActive(false);
    }

    public void HideEditPanel() {
        EditPanel.SetActive(false);
        InstrucionBuild2.SetActive(false);
        BuildPanel.SetActive(true);
    }

    public void SelectEditLayout(string name) {
        int a = 0;

        switch (name)
        {
            case "Floor":
                a = 0;
                break;
            case "Wall":
                a = 1;
                break;
            case "Door":
                a = 2;
                break;
            case "Sofa":
                a = 3;
                break;
            case "Table":
                a = 4;
                break;
        }

        for (int i = 0; i < EditComponentPanel.Length; i++)
        {
            if (i == a)
            {
                EditComponentPanel[i].SetActive(true);
            }
            else {
                EditComponentPanel[i].SetActive(false);
            }
        }
    }
}
