using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject prefab;

    private MeshRenderer meshRend;
    public Material GoodMat;
    public Material BadMat;

    [SerializeField]
    private bool IsSnapped = false;
    [SerializeField]
    private bool IsTrueRotation = false;

    public enum TypeComponent { Floor, Wall, Door, Furniture }
    public TypeComponent typeComponent;

    public List<string> TagSnapId = new List<string>();
    private List<GameObject> collideList = new List<GameObject>();

    [Header("Wall & Option")]
    public bool IsVertical = false;
    public MeshRenderer[] meshRendDoor;
    private GameObject SelectedWall;

    [Header("Furniture Option")]
    public MeshRenderer[] meshRendFurniture;
    public List<GameObject> collideFurniture = new List<GameObject>();

    private void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        ChangeMat();
    }

    private void Update()
    {
        ChangeMat();
        if (typeComponent == TypeComponent.Furniture && collideFurniture.Count > 0)
        {
            CheckCollideFurniture();
        }
    }

    #region Public Method
    public void Place()
    {
        if (typeComponent == TypeComponent.Floor)
        {
            GameObject floor = Instantiate(prefab, transform.position, Quaternion.identity, BuildManager.instance.FloorParent.transform);
            floor.transform.GetChild(0).rotation = transform.rotation;

            floor.GetComponent<Props>().SetColor(BuildManager.instance.ColorId);
            GameManager.instance.SaveObject(transform.position, BuildManager.instance.NameComponent, typeComponent.ToString(), new Vector3(0f, floor.transform.GetChild(0).rotation.eulerAngles.y,0f), BuildManager.instance.ColorId);
        }
        else if (typeComponent == TypeComponent.Wall)
        {
            GameObject wall = Instantiate(prefab, transform.position, transform.rotation, BuildManager.instance.WallParent.transform);

            wall.GetComponent<Props>().SetColor(BuildManager.instance.ColorId);
            GameManager.instance.SaveObject(transform.position, BuildManager.instance.NameComponent, typeComponent.ToString(), new Vector3(0f, wall.transform.rotation.eulerAngles.y, 0f), BuildManager.instance.ColorId);
        }
        else if (typeComponent == TypeComponent.Door)
        {
            GameObject door = Instantiate(prefab, transform.position, transform.rotation, BuildManager.instance.DoorParent.transform);

            door.GetComponent<Props>().SetColor(BuildManager.instance.ColorId);
            GameManager.instance.SaveObject(transform.position, BuildManager.instance.NameComponent, typeComponent.ToString(), new Vector3(0f, door.transform.rotation.eulerAngles.y, 0f), BuildManager.instance.ColorId);

            Destroy(SelectedWall);
            GameManager.instance.DeleteObject(SelectedWall.transform.position, SelectedWall.GetComponent<Props>().typeComponent.ToString());
        }
        else if (typeComponent == TypeComponent.Furniture)
        {
            GameObject furniture = Instantiate(prefab, transform.position, transform.rotation, BuildManager.instance.FurnitureParent.transform);

            furniture.GetComponent<Props>().SetColor(BuildManager.instance.ColorId);
            GameManager.instance.SaveObject(transform.position, BuildManager.instance.NameComponent, typeComponent.ToString(), new Vector3(0f, furniture.transform.rotation.eulerAngles.y, 0f), BuildManager.instance.ColorId);
        }

        Destroy(gameObject);
    }

        
    public void ChangeMat()
    {
        if (typeComponent == TypeComponent.Floor)
        {
            IsSnapped = true;
            IsTrueRotation = true;
        }

        if (typeComponent == TypeComponent.Furniture)
        {
            IsTrueRotation = true;
        }

        if (BuildSystem.instance.CanBuild && IsSnapped && IsTrueRotation)
        {
            if (typeComponent == TypeComponent.Door)
            {
                for (int i = 0; i < meshRendDoor.Length; i++)
                {
                    meshRendDoor[i].material = GoodMat;
                }
            }
            else if (typeComponent == TypeComponent.Furniture)
            {
                for (int i = 0; i < meshRendFurniture.Length; i++)
                {
                    meshRendFurniture[i].material = GoodMat;
                }
            }
            else
            {
                meshRend.material = GoodMat;
            }

        }
        else
        {
            if (typeComponent == TypeComponent.Door)
            {
                for (int i = 0; i < meshRendDoor.Length; i++)
                {
                    meshRendDoor[i].material = BadMat;
                }
            }
            else if (typeComponent == TypeComponent.Furniture)
            {
                for (int i = 0; i < meshRendFurniture.Length; i++)
                {
                    meshRendFurniture[i].material = BadMat;
                }
            }
            else
            {
                meshRend.material = BadMat;
            }
        }
    }

    public bool GetSnapped()
    {
        return IsSnapped;
    }

    public void CheckRotation()
    {
        if (typeComponent == TypeComponent.Wall)
        {
            WallRotChecker();
        }

        if (typeComponent == TypeComponent.Door)
        {
            DoorRotChecker();
        }
    }

    private void DoorRotChecker()
    {
        if (SelectedWall != null)
        {
            bool vertival = false;
            if (SelectedWall.transform.rotation.eulerAngles.y == 0 || SelectedWall.transform.rotation.eulerAngles.y == 180)
            {
                vertival = true;
            }

            if (SelectedWall.transform.rotation.eulerAngles.y == 90 || SelectedWall.transform.rotation.eulerAngles.y == 270)
            {
                vertival = false;
            }

            if (vertival)
            {
                if (transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180)
                {
                    IsTrueRotation = true;
                }
                else
                {
                    IsTrueRotation = false;
                }
            }
            else
            {
                if (transform.rotation.eulerAngles.y == 90 || transform.rotation.eulerAngles.y == 270)
                {
                    IsTrueRotation = true;
                }
                else
                {
                    IsTrueRotation = false;
                }
            }
        }
    }

    private void WallRotChecker()
    {
        if (IsVertical)
        {
            if (transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180)
            {
                IsTrueRotation = true;
            }
            else
            {
                IsTrueRotation = false;
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.y == 90 || transform.rotation.eulerAngles.y == 270)
            {
                IsTrueRotation = true;
            }
            else
            {
                IsTrueRotation = false;
            }
        }
    }

    private void CheckCollideFurniture() {
        int err = 0;
        for (int i = 0; i < collideFurniture.Count; i++)
        {
            if (collideFurniture[i].tag == "Door" || collideFurniture[i].tag == "Wall")
            {
                err++;
            }
        }

        if (err == 0)
        {
            IsSnapped = true;
        }
        else {
            IsSnapped = false;
        }
    }
    #endregion


    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < TagSnapId.Count; i++)
        {
            string currentTag = TagSnapId[i];

            if (other.tag == currentTag)
            {
                //BuildSystem.instance.PauseBuild(true);
                transform.position = other.transform.position;
                IsSnapped = true;

                // Checking Wall Vertical or Horizontal
                if (typeComponent == TypeComponent.Wall)
                {
                    IsVertical = other.GetComponent<SnapWall>().IsVertical;
                    WallRotChecker();
                    if (!IsTrueRotation)
                    {
                        transform.Rotate(0f,90f,0f);
                        WallRotChecker();
                    }
                }

                // Checking Door Vertical or Horizontal
                if (typeComponent == TypeComponent.Door)
                {
                    DoorRotChecker();
                    if (!IsTrueRotation)
                    {
                        transform.Rotate(0f,90f,0f);
                        DoorRotChecker();
                    }
                }
            }
        }

        // Hide Wall when placing Door
        if (typeComponent == TypeComponent.Door && other.tag == "Wall")
        {
            other.GetComponent<MeshRenderer>().enabled = false;
            SelectedWall = other.gameObject;
        }

        // Add colide list for furniture
        if (typeComponent == TypeComponent.Furniture && other.tag != "Ground")
        {
            if (!collideFurniture.Contains(other.gameObject))
                collideFurniture.Add(other.gameObject);
        }

        // Checking Double Component
        if (other.tag == typeComponent.ToString())
        {
            if (!collideList.Contains(other.gameObject))
                collideList.Add(other.gameObject);

            if (collideList.Count > 0)
            {
                BuildSystem.instance.CanBuild = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < TagSnapId.Count; i++)
        {
            string currentTag = TagSnapId[i];

            if (other.tag == currentTag)
            {
                IsSnapped = false;
            }

        }

        // Unhide Wall When Placing Door 
        if (typeComponent == TypeComponent.Door && other.tag == "Wall")
        {
            other.GetComponent<MeshRenderer>().enabled = true;
            SelectedWall = null;
        }

        // Remove colide list for furniture
        if (typeComponent == TypeComponent.Furniture)
        {
            if (collideFurniture.Contains(other.gameObject))
                collideFurniture.Remove(other.gameObject);
        }

        // Check Double Component
        if (other.tag == typeComponent.ToString())
        {
            if (collideList.Contains(other.gameObject))
                collideList.Remove(other.gameObject);

            if (collideList.Count == 0)
            {
                BuildSystem.instance.CanBuild = true;
            }
        }
    }
    #endregion
}
