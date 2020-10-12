/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */

// This is the basic class for the game
// Each form consists out of triangles

//     /\
//    ¯¯¯¯


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleTriforce 
{
    public enum TriforceType{
        EMPTY,     // EMPTY
        TOPLEFT,   // |¯
        TOPRIGHT,  // ¯|
        BOTLEFT,   // |_
        BOTRIGHT,  // _|
        FILLED     // SQUARE
    }

    public TriforceType Type;

    //Constructors ======================================================
    public SimpleTriforce() {}
    public SimpleTriforce(TriforceType TriType)
    {
        Type = TriType;
    }

    //=================================================================== Rotate Functions
    public SimpleTriforce RotateRight()
    {
        if (Type == TriforceType.TOPLEFT)  { Type = TriforceType.TOPRIGHT;  return this; }
        if (Type == TriforceType.TOPRIGHT) { Type = TriforceType.BOTRIGHT;  return this; }
        if (Type == TriforceType.BOTLEFT)  { Type = TriforceType.TOPLEFT;   return this; }
        if (Type == TriforceType.BOTRIGHT) { Type = TriforceType.BOTLEFT;   return this; }
        return this;
    }


    public SimpleTriforce RotateLeft()
    {
        if (Type == TriforceType.TOPLEFT)  { Type = TriforceType.BOTLEFT;  return this; }
        if (Type == TriforceType.TOPRIGHT) { Type = TriforceType.TOPLEFT;  return this; }
        if (Type == TriforceType.BOTLEFT)  { Type = TriforceType.BOTRIGHT; return this; }
        if (Type == TriforceType.BOTRIGHT) { Type = TriforceType.TOPRIGHT; return this; }
        return this;
    }
}

//=======================================================================