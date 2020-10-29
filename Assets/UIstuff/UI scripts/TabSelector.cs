using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabSelector : MonoBehaviour
{
    //Sprites for turn music and sounds on/off 
    public Sprite on;
    public Sprite off;

    //Sprites for set levels to opem/locked 
    public Sprite l;
    public Sprite o;

    GameObject tmp;

    public bool winCondition = false;
    public bool giftCondition = false;
    public bool coinsCondition = false;
    public bool goldCondition = false;

    //Progress variables       progress, gift, golden piece, collections
    public int progress = 1;
    public int giftProgress = 0;
    public int goldenPieceProgress = 0;
    public int collection = 0;
    int currentLevelCount = 0;

    //For muliply difficulty
    float giftScore;
    float goldenPieceScore;

    /////////////////////////////////////////////////////////////////////////////

    GameObject level;
    GameObject levelSelection;
    GameObject worldSelection;
    GameObject pauseScreen;
    GameObject settingsScreen;
    GameObject giftDone;
    GameObject coinsDone;
    GameObject goldDone;
    GameObject goldLevel;
    GameObject goldLevelDone;
    GameObject collections;
    GameObject mainMenu;
    GameObject winScreen;
    GameObject settings;
    GameObject next;

    GameObject[] levels;

    List<GameObject> ff;


    public List<ParticleSystem> winParticles;
    public GameObject levela;
    public GameObject locked;

    public bool lockedd = false;
    public int[] levelss;

    public bool sound;
    public bool music;

    int n = 125;
    int m = 1;
    int selectedWorld = 0;
    int currentLevel = 0;

    bool alreadyFinished=false;

    private void Start()
    {
        ff = new List<GameObject>();

        Debug.Log("Loading");
        Debug.Log("==================== Start");
        int.TryParse(DataM.GetString("progress"), out progress);

        if (progress == 0) progress = 1;

        if (DataM.GetString("music") == "True")
        {
            music = true;
            SoundM.MusicEnabled = true;

        }
        else
        {
            music = false;
            GameObject.Find("Audio_Music").GetComponent<AudioSource>().Stop();
            SoundM.MusicEnabled = false;
        }

        if (DataM.GetString("sound") == "True")
        {
            sound = true;
            SoundM.SoundEnabled = true;
        }
        else
        {
            sound = false;
            GameObject.Find("Audio_Effects").GetComponent<AudioSource>().Stop();
            SoundM.SoundEnabled = false;
        }

        Debug.Log("music " + music + " / " + "sound " + sound);

        PlayerPrefs.GetInt("progress", progress);
        /*   PlayerPrefs.GetInt("progress", progress);
           PlayerPrefs.GetInt("progress", progress);
           PlayerPrefs.GetInt("progress", progress);
           PlayerPrefs.GetInt("progress", progress);
           PlayerPrefs.GetInt("progress", progress);
           PlayerPrefs.GetInt("progress", progress);*/

        levels = new GameObject[n];


        next = GameObject.Find("next");

        level = GameObject.Find("Level");
        level.SetActive(false);


        settings = GameObject.Find("SettingsScreen");
        settings.SetActive(false);

        worldSelection = GameObject.Find("WorldSelection");
        worldSelection.SetActive(false);

        levelSelection = GameObject.Find("LevelSelection");
        levelSelection.SetActive(false);

        collections = GameObject.Find("Collections");
        collections.SetActive(false);

        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);

        mainMenu = GameObject.Find("MainMenu");

        goldLevel = GameObject.Find("GoldLevel");
        goldLevel.SetActive(false);

        goldLevelDone = GameObject.Find("GoldLevelDone");
        goldLevelDone.SetActive(false);

        winScreen = GameObject.Find("WinScreen");
        winScreen.SetActive(false);

        giftDone = GameObject.Find("GiftDone");
        giftDone.SetActive(false);

        coinsDone = GameObject.Find("CoinsDone");
        coinsDone.SetActive(false);

        goldDone = GameObject.Find("GoldDone");
        goldDone.SetActive(false);



    }

    private void Update()
    {
        /*if (winCondition)
        {
            if (goldLevel.active)
                goldLevelDone.SetActive(true);
            else
                winScreen.SetActive(true);
        }
        else
        {
            goldLevelDone.SetActive(false);
            winScreen.SetActive(false);
        }
       if (giftCondition)
           giftDone.SetActive(true);
       else giftDone.SetActive(false);
       if (coinsCondition)
           coinsDone.SetActive(true);
       else coinsDone.SetActive(false);
       if (goldCondition)
           goldDone.SetActive(true);
       else goldDone.SetActive(false);
       */
        if (LevelM.correctForms >= LevelM.countForms && LevelM.LevelLoad == true && LevelM.correctForms != 0 && LevelM.countForms != 0 && !winCondition&& level.active)
        {
            string[] name = GameObject.Find("LevelCounter").GetComponent<Text>().text.Split(' ');
            winCondition = true;
            winScreen.SetActive(true);
            if (winScreen.active&& !mainMenu.active)
            {
                for (int i = 0; i < winParticles.Count; i++)
                {
                    winParticles[i].Play();
                }
            }
            GameObject.Find("levelNumber").GetComponent<Text>().text = "Level " + name[1];
            int Cache = 0;
            Debug.Log("==================== Update");
            int.TryParse(name[1], out Cache);
            if (progress <= Cache) 
            progress++;
            saveGame(0);
        }
        //Debug.Log(LevelM.correctForms + " / " + LevelM.countForms);
    }
  

    public void OpenTab(string nameOfButton)
    {
        if (nameOfButton == "play")
        {
            worldSelection.SetActive(true);
            mainMenu.SetActive(false);
            WorldProgressCheck();
        }
        if (nameOfButton.StartsWith("world"))
            LevelSelection(nameOfButton);

        if (nameOfButton.StartsWith("level"))
            LoadLevel(nameOfButton);

        if (nameOfButton == "settings")
        {
            settings.SetActive(true);
            musicSoundStuff(nameOfButton, 0);
        }

        if (nameOfButton == "closeSettings")
        {
            saveGame(1);
            settings.SetActive(false);
        }

        if(nameOfButton == "next")
        {
            LevelM.LevelLoad = false;
            LevelM.FormsInLevel.Clear();
            LevelM.countForms = 0;
            LevelM.correctForms = 0;

            currentLevelCount++;
           
            level.SetActive(false);

            //string[] name = GameObject.Find("LevelCounter").GetComponent<Text>().text.Split(' ');



            winCondition = false;
            winScreen.SetActive(false);
            level.SetActive(true);
            
            //GameObject.Find("LevelCounter").GetComponent<Text>().text = "level " + currentLevelCount.ToString();

            foreach (Transform child in GameObject.Find("ScaleThem").transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in GameObject.Find("SelectedForm").transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(waitForFormLoad(currentLevelCount.ToString(), 1));
        }
        if (nameOfButton == "collections")
            collections.SetActive(true);

        if (nameOfButton == "stop")
        {
            pauseScreen.SetActive(true);
            musicSoundStuff(nameOfButton, 0);    
        }

        if (nameOfButton == "closePause")
        {
            saveGame(1);
            pauseScreen.SetActive(false);

        }


        if (nameOfButton == "home")
        {
            mainMenu.SetActive(true);
            winCondition = false;
            pauseScreen.SetActive(false);
            level.SetActive(false);
            winScreen.SetActive(false);
            LevelM.LevelLoad = false;
            LevelM.FormsInLevel.Clear();
            LevelM.countForms=0;
            LevelM.correctForms=0;
        }

        if (nameOfButton == "goBack")
        {

            LevelM.LevelLoad = false;
            LevelM.FormsInLevel.Clear();
            LevelM.countForms = 0;
            LevelM.correctForms = 0;

            level.SetActive(false);
            string[] name = GameObject.Find("LevelCounter").GetComponent<Text>().text.Split(' ');
            foreach (Transform child in GameObject.Find("ScaleThem").transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in GameObject.Find("SelectedForm").transform)
            {
                Destroy(child.gameObject);
            }

            GameObject.Find("LevelCounter").GetComponent<Text>().text = "level " +name[1];
            StartCoroutine(waitForFormLoad(name[1], 1));
            level.SetActive(true);
            winCondition = false;
            winScreen.SetActive(false);
        }


        if (nameOfButton == "backCollections")
        {
            collections.SetActive(false);
            mainMenu.SetActive(true);
        }

        if (nameOfButton == "backWorldSelection")
        {
            worldSelection.SetActive(false);
            mainMenu.SetActive(true);
        }

        if (nameOfButton == "backLevelSelection")
        {
            levelSelection.SetActive(false);
            worldSelection.SetActive(true);
            WorldProgressCheck();
        }

      /*  if (nameOfButton == "claimGoldFigure")
        {
            //DO SOMETHING
            goldLevel.SetActive(false);
            goldLevelDone.SetActive(false);
            winCondition = false;
            mainMenu.SetActive(true);
        }
        if (nameOfButton == "claimGift")
        {
            //DO SOMETHING
            level.SetActive(false);
            giftDone.SetActive(false);
            giftCondition = false;
            mainMenu.SetActive(true);
        }
        if (nameOfButton == "claimCoins")
        {
            //DO SOMETHING
            level.SetActive(false);
            coinsDone.SetActive(false);
            coinsCondition = false;
            mainMenu.SetActive(true);
        }
        if (nameOfButton == "claimGoldPiece")
        {
            //DO SOMETHING
            level.SetActive(false);
            goldDone.SetActive(false);
            goldCondition = false;
            mainMenu.SetActive(true);
        }*/

        if (nameOfButton == "music" || nameOfButton == "sound")
            musicSoundStuff(nameOfButton, 1);


    }

    void progressCheck()
    {

        if (giftProgress >= 100)
        {

        }
        if (goldenPieceProgress >= 100)
        {

        }

    }

    void saveGame(int type)
    {
        if (type == 0)
        {
            Debug.Log("Save");
            DataM.SetString("progress", progress.ToString());
            //Other progress in game
        }

        //Sound system
        if (type == 1)
        {
            DataM.SetString("music", music.ToString());
            Debug.Log(music);
            DataM.SetString("sound", sound.ToString());
        }
    }

    void musicSoundStuff(string button, int mode) // 0 - just check    1 - change
    {
        if (mode == 1)
        {
            if (button == "music")
            {
                if (music)
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = off;
                }
                else
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = on;
                }
                music = !music;

                SoundM.EnableMusic(music);
            }
            if (button == "sound")
            {
                if (sound)
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = off;                    
                }
                else
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = on;
                }
                sound = !sound;

                SoundM.EnableSound(sound);
            }
        }
        else
        {
            if (music)
                GameObject.Find("music").GetComponent<Image>().sprite = on;
            else
                GameObject.Find("music").GetComponent<Image>().sprite = off;

            if (sound)
                GameObject.Find("sound").GetComponent<Image>().sprite = on;
            else
                GameObject.Find("sound").GetComponent<Image>().sprite = off;
        }
    }

    void LoadLevels()
    {
        int n = GameObject.Find("Levels").transform.childCount;
        GameObject[] levels = new GameObject[n];
        for (int i = 0; i < n; i++)
        {
            levels[i] = GameObject.Find("level " + n);
            levels[i] = levela;
            levels[i].GetComponentInChildren<Text>().text = n.ToString();
        }
    }

    void LoadLevel(string button)
    {
        LevelM.LevelLoad = false;
        LevelM.FormsInLevel.Clear();
        LevelM.countForms = 0;
        LevelM.correctForms = 0;
        winCondition = false;
        level.SetActive(false);
        level.SetActive(true);
        levelSelection.SetActive(false);


        
        int.TryParse(button.Split(' ')[1], out currentLevelCount);

        //GameObject.Find("LevelCounter").GetComponent<Text>().text = "Level " + GameObject.Find("tl " + button.Split(' ')[1]).GetComponent<Text>().text;

        foreach (Transform child in GameObject.Find("ScaleThem").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("SelectedForm").transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(waitForFormLoad(button, 0));

    }


    public IEnumerator waitForFormLoad(string button,int state)
    {

        while (LevelM.Init == false)
        {
            yield return new WaitForSeconds(0.5f);        
        }
        int c1 = 0;
        int c2 = 0;
        if (state == 0) int.TryParse("0" + button.Split(' ')[1], out c1);
        int.TryParse("0" + button, out c2);


        if(state==0)
        LevelM.LoadLevel(c1);
        if( state == 1)
            LevelM.LoadLevel(c2);

       
    }

    void LevelSelection(string button)
    {
        levelSelection.SetActive(true);
        worldSelection.SetActive(false);
        if (button == "world 1")
        {
            m = 0;
            selectedWorld = 1;
        }
        if (button == "world 2")
        {
            m = 25;
            selectedWorld = 2;
        }
        if (button == "world 3")
        {
            m = 50;
            selectedWorld = 3;
        }
        if (button == "world 4")
        {
            m = 75;
            selectedWorld = 4;
        }
        if (button == "world 5")
        {
            m = 100;
            selectedWorld = 5;
        }

        for (int j = 0; j < levelss.Length; j++)
            if (progress > j)
                levelss[j] = 1;
            else
                levelss[j] = 0;
        for (int i = 1; i < 26; i++)
        {
            GameObject.Find("level " + i).GetComponentInChildren<Text>().text = (i + m).ToString();
            if (levelss[i - 1] == 1 && i + m - 1 < progress/*&& ((i - 1)*selectedWorld > progress)*/)
            {
                GameObject.Find("level " + i).GetComponent<Image>().sprite = o;
                GameObject.Find("level " + i).GetComponentInChildren<Text>().text = (i + m).ToString();
                GameObject.Find("level " + i).GetComponentInChildren<Button>().interactable = true;
            }
            else
            {
                GameObject.Find("level " + i).GetComponent<Image>().sprite = l;
                GameObject.Find("level " + i).GetComponentInChildren<Text>().text = "";
                GameObject.Find("level " + i).GetComponentInChildren<Button>().interactable = false;

            }

        }
    }

    void WorldProgressCheck()
    {
        for (int i = 1; i < 6; i++)
        {
            if ((progress / 25) >= i)
            {
                GameObject.Find("progress" + i).GetComponent<Text>().text = "25/25";
            }
            else
                GameObject.Find("progress" + i).GetComponent<Text>().text = (progress - (25 * (i - 1))) + "/25";
            if ((progress - (25 * (i - 1)) < 1))
            {
                GameObject.Find("progress" + i).GetComponent<Text>().text = "0/25";
                if (GameObject.Find("world " + i).GetComponent<Button>().interactable)
                    GameObject.Find("world " + i).GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("world " + i).GetComponent<Button>().interactable = true;
            }

        }
    }
}