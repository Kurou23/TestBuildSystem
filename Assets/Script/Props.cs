using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    public enum TypeComponent { Floor, Wall, Door, Furniture}
    public TypeComponent typeComponent;

    public MeshRenderer[] RenderModels;

    public ColorSet[] colorSet;

    public void SetColor(string colorId)
    {
        int id = 0;
        for (int i = 0; i < colorSet.Length; i++)
        {
            if (colorSet[i].ColorId == colorId)
            {
                id = i;
            }
        }

        for (int i = 0; i < RenderModels.Length; i++)
        {
            RenderModels[i].material = colorSet[id].MatsColor[i];
        }


    }
}
