/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundM : MonoBehaviour
{
    //Generate AudioSources?
    public bool GenerateObjects = false;

    //Static Stuff
    public static bool Init = false;
    private static AudioSource Effects;
    private static AudioSource Music;

    //List of Sounds + Music
    public AudioClip BackgroundMusic;
    public List<AudioClip> SoundList;

    public static SoundM instance;

    public static bool SoundEnabled = true;
    public static bool MusicEnabled = true;

    void Start()
    {
        if (GenerateObjects == true)
        {
            GameObject createCache = new GameObject();
            createCache.name = "Audio_Effects";
            createCache.AddComponent<AudioSource>();
            createCache = new GameObject();
            createCache.name = "Audio_Music";
            createCache.AddComponent<AudioSource>();
        }

        //Find Objects
        GameObject cache = GameObject.Find("Audio_Effects");
        if (cache != null) Effects = cache.GetComponent<AudioSource>();

        cache = GameObject.Find("Audio_Music");
        if (cache != null) Music = cache.GetComponent<AudioSource>();

        //Set Instance
        instance = this;

        //Start Music and end Initphase
        if (Effects != null && Music != null) {

            if(MusicEnabled == true){

                //Play Background Muisc
                Music.clip = BackgroundMusic;
                Music.loop = true;
                Music.Play();
            }

            Init = true;

        }
    }

    //As Simple as it is...
    public static void PlaySound(int Number)
    {
        if (!Init || Number<0 || Number >= instance.SoundList.Count) return;
        Effects.PlayOneShot(instance.SoundList[Number], 1);
    }

    public static void EnableSound(bool State=true){
        SoundEnabled = State;
    }

    public static void EnableMusic(bool State=true){
        MusicEnabled = State;
        
        if (State == true)  Music.Play();
        if (State == false) Music.Stop();
    }


}

//=======================================================================