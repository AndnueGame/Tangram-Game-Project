/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Serializeable Level-File

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Tangram/Level", order = 1)]
public class Level : ScriptableObject
{
    public string Name;
    public int Width, Height;
    public List<SimpleTriforce> Shape = new List<SimpleTriforce>();
    public List<LevelData> Data = new List<LevelData>();

    //::::::::::::::::::::::::::::::::::::::::::::::::




    //::::::::::::::::::::::::::::::::::::::::::::::::
    public void GenerateTriforceShape() { }
}

//=======================================================================