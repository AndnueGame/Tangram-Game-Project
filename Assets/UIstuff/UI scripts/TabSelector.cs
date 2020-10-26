using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabSelector : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    GameObject tmp;

    public bool winCondition = false;
    public bool giftCondition = false;
    public bool coinsCondition = false;
    public bool goldCondition = false;


    //Progress variables       progress, gift, golden piece, collections
    public int progress = 0;
    public int giftProgress = 0;
    public int goldenPieceProgress = 0;
    public int collection = 0;

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


    GameObject[] levels;

    List<GameObject> ff;

    public GameObject levela;
    public GameObject locked;

    public Sprite l;
    public Sprite o;

    public bool lockedd = false;
    public int[] levelss;

    public bool sound;
    public bool music;

    int n = 125;
    int m = 1;
    int selectedWorld = 0;

    int currentLevel = 0;



    private void Start()
    {
        ff = new List<GameObject>();

        PlayerPrefs.GetInt("progress", progress);
     /*   PlayerPrefs.GetInt("progress", progress);
        PlayerPrefs.GetInt("progress", progress);
        PlayerPrefs.GetInt("progress", progress);
        PlayerPrefs.GetInt("progress", progress);
        PlayerPrefs.GetInt("progress", progress);
        PlayerPrefs.GetInt("progress", progress);*/

        levels = new GameObject[n];
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
        /* if (winCondition)
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

        if (LevelM.correctForms == LevelM.countForms)
        {
            if (currentLevel > progress && winCondition)
            {
                progress++;
                progressCheck();
                winScreen.SetActive(true);
                //gift and other stuff will be progressing
                SaveGame();
            }
        }
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
            settings.SetActive(false);

        if (nameOfButton == "collections")
            collections.SetActive(true);

        if (nameOfButton == "stop")
        {
            pauseScreen.SetActive(true);
            musicSoundStuff(nameOfButton, 0);

           
        }

        if (nameOfButton == "closePause")
            pauseScreen.SetActive(false);

        if (nameOfButton == "home")
        {
            pauseScreen.SetActive(false);
            level.SetActive(false);
            mainMenu.SetActive(true);
        }

        if (nameOfButton == "goBack")
            LevelM.RegenerateCurrentLevel();


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

        if (nameOfButton == "claimGoldFigure")
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
        }

        if (nameOfButton == "music" || nameOfButton == "sound")
            musicSoundStuff(nameOfButton,1);


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
                    //   GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 0;
                }
                else
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = on;
                    //   GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 1;
                }
                music = !music;
            }
            if (button == "sound")
            {
                if (sound)
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = off;
                    //   GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 0;
                }
                else
                {
                    GameObject.Find(button).GetComponent<Image>().sprite = on;
                    //    GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 1;
                }
                sound = !sound;
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
        string[] name = button.Split(' ');
        level.SetActive(true);
        GameObject.Find("LevelCounter").GetComponent<Text>().text = "level " + GameObject.Find(button).GetComponentInChildren<Text>().text;
        foreach (Transform child in GameObject.Find("ScaleThem").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("SelectedForm").transform)
        {
            Destroy(child.gameObject);
        }
        LevelM.LoadLevel(int.Parse("0" + GameObject.Find(button).GetComponentInChildren<Text>().text));
        levelSelection.SetActive(false);
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


    //After any move we need to save game progress!!!!
    void SaveGame()
    {
        PlayerPrefs.SetInt("progress", progress);

    }
}