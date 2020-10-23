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

    public bool lockedd=false;
    public int[] levelss;

    public bool sound;
    public bool music;

    int n=125;
    int m = 1;
    int selectedWorld=0;

    int currentLevel=0;

    public int progress = 0;

    private void Start()
    {
        ff = new List<GameObject>();

        PlayerPrefs.GetInt("progress",progress);
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
        //mainMenu.SetActive(false);

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
        }*
        else
        {
            goldLevelDone.SetActive(false);
            winScreen.SetActive(false);
        }*/
        if (giftCondition)
            giftDone.SetActive(true);
        else giftDone.SetActive(false);
        if (coinsCondition)
            coinsDone.SetActive(true);
        else coinsDone.SetActive(false);
        if (goldCondition)
            goldDone.SetActive(true);
        else goldDone.SetActive(false);

        

        if(LevelM.correctForms == LevelM.countForms)
        {
            if (currentLevel > progress && winCondition)
            {
                progress++;
                winScreen.SetActive(true);
                PlayerPrefs.SetInt("progress", progress);
                //  PlayerPrefs("save", progress);
            }
        }
        
        /* if(music)
             while(GameObject.Find("music"))
                 G

                 if(sound)*/
    }

    void progressCheck()
    {
        for(int i=1;i<6;i++)
        {
            if((progress/25)>=i)
                {
                GameObject.Find("progress" + i).GetComponent<Text>().text ="25/25";
            }
            else
                GameObject.Find("progress" + i).GetComponent<Text>().text = (progress-(25 * (i-1))) + "/25";
            if ((progress - (25 * (i - 1)) < 1))
            {
                GameObject.Find("progress" + i).GetComponent<Text>().text = "0/25";
                if(GameObject.Find("world " + i).GetComponent<Button>().interactable)
                GameObject.Find("world " + i).GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("world " + i).GetComponent<Button>().interactable = true;
            }

        }
    }
    public void OpenTab(string nameOfButton)
    {
        if (nameOfButton == "play")
        {
            worldSelection.SetActive(true);
            mainMenu.SetActive(false);
            progressCheck();
        }

        if (nameOfButton.StartsWith("world"))
        {
            levelSelection.SetActive(true);
            worldSelection.SetActive(false);
            if (nameOfButton == "world 1")
            {
                m = 0;
                selectedWorld = 1;
            }
            if (nameOfButton == "world 2")
            {
                m = 25;
                selectedWorld = 2;
            }
            if (nameOfButton == "world 3")
            {
                m = 50;
                selectedWorld = 3;
            }
            if (nameOfButton == "world 4")
            {
                m = 75;
                selectedWorld = 4;
            }
            if (nameOfButton == "world 5")
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
                GameObject.Find("level " + i).GetComponentInChildren<Text>().text = (i  + m).ToString();
                if (levelss[i-1] == 1 && i+m-1<progress/*&& ((i - 1)*selectedWorld > progress)*/)
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
        if (nameOfButton.StartsWith("level"))
        {
            string[] name = nameOfButton.Split(' ');

            level.SetActive(true);
            GameObject.Find("LevelCounter").GetComponent<Text>().text = "level " + GameObject.Find(nameOfButton).GetComponentInChildren<Text>().text;
            foreach (Transform child in GameObject.Find("ScaleThem").transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in GameObject.Find("SelectedForm").transform)
            {
                Destroy(child.gameObject);
            }
            LevelM.LoadLevel(int.Parse("0" + GameObject.Find(nameOfButton).GetComponentInChildren<Text>().text));          
            levelSelection.SetActive(false);
        }

        if (nameOfButton == "settings")
            settings.SetActive(true);
        if (nameOfButton == "closeSettings")
            settings.SetActive(false);
        if (nameOfButton == "collections")
            collections.SetActive(true);

        if (nameOfButton == "stop")
            pauseScreen.SetActive(true);

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
            progressCheck();
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

        if (nameOfButton == "music")
        {
            tmp = GameObject.Find(nameOfButton);
            music = !music;
            if (music)
                GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 1;
            else
                GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 0;
        }
        if (nameOfButton == "sound")
        {
            tmp = GameObject.Find(nameOfButton);
            sound = !sound;
            if (sound)
                GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 1;
            else
                GameObject.Find("MainCamera").GetComponent<AudioSource>().volume = 0;
        }

    }


    void LoadLevels()
    {
        Debug.Log("sad");
        int n = GameObject.Find("Levels").transform.childCount;
        GameObject[] levels = new GameObject[n];
        for (int i = 0; i < n; i++)
        {
            levels[i] = GameObject.Find("level " + n);
            levels[i] = levela;
            levels[i].GetComponentInChildren<Text>().text = n.ToString();
        }
    }
}