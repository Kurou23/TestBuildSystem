using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorSet")]
public class ColorSet : ScriptableObject
{
    public string ColorId;
    public Material[] MatsColor;
}
