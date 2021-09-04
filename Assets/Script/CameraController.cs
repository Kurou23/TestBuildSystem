using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public bool IsCameraMove;

    private Vector3 mouseFirstPos;
    private Vector3 cameratPos;
    public Vector3 mouseDiff;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!BuildSystem.instance.isBuilding && !IsMouseOverUI())
        {
            if (!IsCameraMove && Input.GetMouseButtonDown(0))
            {
                mouseFirstPos = Input.mousePosition;
                cameratPos = transform.position;
                mouseDiff = Vector3.zero;
            }

            if (IsCameraMove)
            {
                mouseDiff = (mouseFirstPos - Input.mousePosition) / 50;
                transform.position = new Vector3(cameratPos.x + mouseDiff.x, transform.position.y, cameratPos.z + mouseDiff.y);
            }

            if (IsCameraMove && Input.GetMouseButtonUp(0))
            {
                IsCameraMove = false;
            }
        }
    }

    public bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
