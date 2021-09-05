using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [Header("Building Parent")]
    public GameObject FloorParent;
    public GameObject WallParent;
    public GameObject DoorParent;
    public GameObject FurnitureParent;

    [Header("Object Preview")]
    public List<GameObject> PreviewComponents = new List<GameObject>();

    [Header("Object Prefab")]
    public GameObject Floor;
    public GameObject Wall;
    public GameObject Door;
    public GameObject[] Furniture;

    public string NameComponent;
    public string ColorId;

    private void Update()
    {
        if (!BuildSystem.instance.isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                BuildSystem.instance.NewBuild(PreviewComponents[0]);
                NameComponent = "Floor";
                ColorId = "Floor01";
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                BuildSystem.instance.NewBuild(PreviewComponents[1]);
                NameComponent = "Wall";
                ColorId = "Wall01";
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                BuildSystem.instance.NewBuild(PreviewComponents[2]);
                NameComponent = "Door";
                ColorId = "Door01";
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                BuildSystem.instance.NewBuild(PreviewComponents[3]);
                NameComponent = "Sofa";
                ColorId = "Sofa01";
            }
        }
    }

    public void CreateComponent(string component)
    {
        NameComponent = component;

        for (int i = 0; i < PreviewComponents.Count; i++)
        {
            if (PreviewComponents[i].GetComponent<Preview>().NameComponent == component)
            {
                BuildSystem.instance.NewBuild(PreviewComponents[i]);
            }   
        }
    }

    public void SelectColor(string colorId) {
        ColorId = colorId;
    }
}
