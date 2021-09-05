using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    public Camera cam;
    public LayerMask blueLayer;
    public LayerMask buildingLayer;
    private GameObject previewGameObject = null;
    [SerializeField]
    private GameObject selectGameObject = null;

    public List<Material> tempMats = new List<Material>();
    public Material SelectMat;

    [SerializeField]
    private Preview previewScript;

    public float stickTolerance = 1.5f;
    public bool CanBuild = true;
    public bool isBuilding = false;
    private bool isPause = false;
    private bool IsEditMode;

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

    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            // Rotate Component
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("rotate");
                previewGameObject.transform.Rotate(0f, 90f, 0f);
                previewScript.CheckRotation();
            }

            // Cancel Buid
            if (Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(1))
            {
                CancelBuild();
            }

            // Build Component
            if (Input.GetMouseButton(0) && CanBuild)
            {
                if (previewScript.GetSnapped())
                {
                    StopBuild();
                    return;
                }
                else
                {
                    Debug.Log("Not Snapped !!!");
                }
            }

            if (isPause)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (Mathf.Abs(mouseX) >= stickTolerance || Mathf.Abs(mouseY) >= stickTolerance)
                {
                    isPause = false;
                }
            }
            else
            {
                DoBuildRay();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoCastRay();
            }

            if (selectGameObject != null)
            {
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    DestroyBuild();
                    UIHander.Instance.HideEditPanel();
                    UIHander.Instance.ShowBuildPanel();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    IsEditMode = true;
                    selectGameObject.SetActive(false);
                    Props prop = selectGameObject.GetComponent<Props>();

                    BuildManager.instance.CreateComponent(prop.NameComponent);
                    BuildManager.instance.SelectColor(prop.ColorId);

                    //DestroyBuild();
                    UIHander.Instance.HideEditPanel();
                    UIHander.Instance.HideBuildPanel();

                }

            }
        }
    }

    public void NewBuild(GameObject prefab)
    {
        if (selectGameObject != null)
        {
            DeSelectColor();
        }
        isBuilding = true;
        previewGameObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        previewScript = previewGameObject.GetComponent<Preview>();

        UIHander.Instance.HideBuildPanel();
    }

    public void CancelBuild()
    {
        if (IsEditMode)
        {
            selectGameObject.SetActive(true);
            IsEditMode = false;
        }

        isBuilding = false;
        Destroy(previewGameObject);
        previewGameObject = null;
        previewScript = null;

        UIHander.Instance.ShowBuildPanel();
    }

    public void StopBuild()
    {
        if (IsEditMode)
        {
            DestroyBuild();
            IsEditMode = false;
        }

        previewScript.Place();
        previewGameObject = null;
        previewScript = null;
        isBuilding = false;

        UIHander.Instance.ShowBuildPanel();
    }

    public void PauseBuild(bool value)
    {
        isPause = value;
    }

    public void SelectColor()
    {
        Props prop = selectGameObject.GetComponent<Props>();
        tempMats.Clear();

        for (int i = 0; i < prop.RenderModels.Length; i++)
        {
            tempMats.Add(prop.RenderModels[i].material);
            prop.RenderModels[i].material = SelectMat;
        }

        UIHander.Instance.ShowEditPanel();
        UIHander.Instance.SelectEditLayout(prop.NameComponent);

        Debug.Log("hit building");
    }

    public void DeSelectColor()
    {
        Props prop = selectGameObject.GetComponent<Props>();

        for (int i = 0; i < prop.RenderModels.Length; i++)
        {
            prop.RenderModels[i].material = tempMats[i];
        }
    }

    public void ChangeColor(string colorId)
    {
        Props prop = selectGameObject.GetComponent<Props>();
        prop.ColorId = colorId;
        prop.SetColor();
        if (prop.typeComponent.ToString() == "Floor")
        {
            GameManager.instance.UpdateObjet(selectGameObject.transform.position, prop.NameComponent, prop.typeComponent.ToString(), prop.transform.GetChild(0).rotation.eulerAngles, prop.ColorId);
        }
        else
        {
            GameManager.instance.UpdateObjet(selectGameObject.transform.position, prop.NameComponent, prop.typeComponent.ToString(), prop.transform.rotation.eulerAngles, prop.ColorId);
        }

        selectGameObject = null;
        UIHander.Instance.ShowBuildPanel();
    }

    public void DestroyBuild()
    {
        GameManager.instance.DeleteObject(selectGameObject.transform.position, selectGameObject.GetComponent<Props>().typeComponent.ToString());
        Destroy(selectGameObject);
    }

    private void DoBuildRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, blueLayer))
        {
            if (previewGameObject != null)
            {
                float y = hit.point.y + (previewGameObject.transform.localScale.y / 2);
                Vector3 pos = new Vector3(Mathf.FloorToInt(hit.point.x), y, Mathf.FloorToInt(hit.point.z));
                previewGameObject.transform.position = pos;
            }
        }
    }

    private void DoCastRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, buildingLayer))
        {
            if (!CameraController.Instance.IsMouseOverUI())
            {
                if (hit.collider.tag == "Floor" || hit.collider.tag == "Wall" || hit.collider.tag == "Door" || hit.collider.tag == "Furniture")
                {
                    if (selectGameObject != null)
                    {
                        DeSelectColor();
                    }
                    selectGameObject = hit.collider.gameObject;
                    SelectColor();
                }
                else if (hit.collider.tag == "Ground")
                {
                    if (selectGameObject != null)
                    {
                        DeSelectColor();
                        UIHander.Instance.ShowBuildPanel();
                    }
                }
            }

        }
    }
}
