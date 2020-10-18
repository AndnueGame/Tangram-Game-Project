/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MISC : MonoBehaviour
{
    public static List<Color> ColorPalette = new List<Color>
    {
        new Color(255,000,000), //Color1
        new Color(000,255,000), //Color2
        new Color(000,000,255), //Color3
        new Color(000,255,255), //Color4
        new Color(255,255,255), //Color5
    };

    //Functions ========================================================

    public static Color GetColorCode(int Color)
    {
        Color ReturnColor = new Color(255, 0, 0);

        if (Color >= 0 && Color < ColorPalette.Count)
        {
            ReturnColor = ColorPalette[Color];
        }

        if (Color == 100)
        {
            ReturnColor = new Color(40, 40, 40);
        }

        return ReturnColor;
    }


    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }


}

//=======================================================================
