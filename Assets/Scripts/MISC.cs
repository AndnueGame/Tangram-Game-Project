/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MISC : MonoBehaviour
{
    public static List<Color32> ColorPalette = new List<Color32>
    {
        new Color32(255,136,255,255), //Color1
        new Color32(000,255,000,255), //Color2
        new Color32(000,000,255,255), //Color3
        new Color32(000,255,255,255), //Color4
        new Color32(255,255,255,255), //Color5
        /*
        new Color(255,064,064,255), //Color1
        new Color(078,255,078,255), //Color2
        new Color(071,071,255,255), //Color3
        new Color(070,255,255,255), //Color4
        new Color(255,255,255,255), //Color5
        */
    };

    //Functions ========================================================

    public static Color GetColorCode(int Color)
    {
        Color32 ReturnColor = new Color32(255, 0, 0, 255);

        if (Color >= 0 && Color < ColorPalette.Count)
        {
            ReturnColor = ColorPalette[Color];
        }

        if (Color == 100)
        {
            ReturnColor = new Color32(40, 40, 40, 255);
        }

        return ReturnColor;
    }


    public static List<Form> LoadForms()
    {
        Object[] forms;
        forms = Resources.LoadAll("Forms", typeof(Form));

        List<Form> assets = new List<Form>();
        for(int x=0; x < forms.Length; x++)
        {
            assets.Add((Form)forms[x]);
        }

        //string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        //for (int i = 0; i < guids.Length; i++)
        //{
        //    string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        //    T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        //    if (asset != null)
        //    {
        //        assets.Add(asset);
        //    }
        // }

        return assets;
    }


}

//=======================================================================
