using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<MeshRenderer> PanelModel = new List<MeshRenderer>();
    public MeshRenderer DoorModel;

    public Material panelMat;
    public Material doorMat;


    public void ChangeColor() {
        for (int i = 0; i < PanelModel.Count; i++)
        {
            PanelModel[i].material = panelMat;
        }

        DoorModel.material = doorMat;
    }
    
}
