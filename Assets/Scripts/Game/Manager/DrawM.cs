/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawM : MonoBehaviour
{
    [Header("Draw Sprites ===========")]
    public Sprite Form_Empty;
    public Sprite Form_Square;
    public Sprite Form_Triangle;

    public static Sprite fEmpty;
    public static Sprite fSquare;
    public static Sprite fTriangle;

    private void Awake()
    {
        fEmpty    = Form_Empty;
        fSquare   = Form_Square;
        fTriangle = Form_Triangle;
    }


    //===================================================================
    public Sprite FromForm(Form Template)
    {
     
        return null;
    }

}

//=======================================================================