/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataM : MonoBehaviour
{

    //Basic Load/Save Stuff =======================================

    public static string GetString(string Key)
    {
        return PlayerPrefs.GetString(Key, "");
    }

    public static void SetString(string Key, string value)
    {
        PlayerPrefs.SetString(Key, value);
        PlayerPrefs.Save();
    }

    public static int GetInt(string Key)
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    public static void SetInt(string Key, int value)
    {
        PlayerPrefs.SetInt(Key, value);
        PlayerPrefs.Save();
    }

    public static float GetFloat(string Key)
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    public static void SetFloat(string Key, float value)
    {
        PlayerPrefs.SetFloat(Key, value);
        PlayerPrefs.Save();
    }

    //More Complex Save/Load Stuff ======================================

    public static void SetListOfStrings(string key, List<string> strings)
    {
        SetString(key, string.Join("§", strings));
    }

    public static void GetListOfStrings(string key, List<string> strings)
    {
        strings.Clear();
        strings = GetString(key).Split('§').ToList();
    }

}

//=======================================================================
