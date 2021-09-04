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
    public GameObject FloorPrev;
    public GameObject WallPrev;
    public GameObject DoorPrev;
    public GameObject[] FurniturePrev;

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
                BuildSystem.instance.NewBuild(FloorPrev);
                NameComponent = "Floor";
                ColorId = "Floor01";
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                BuildSystem.instance.NewBuild(WallPrev);
                NameComponent = "Wall";
                ColorId = "Wall01";
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                BuildSystem.instance.NewBuild(DoorPrev);
                NameComponent = "Door";
                ColorId = "Door01";
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                BuildSystem.instance.NewBuild(FurniturePrev[0]);
                NameComponent = "Sofa";
                ColorId = "Sofa01";
            }
        }
    }
}
