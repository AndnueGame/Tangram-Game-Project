using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    private void Start()
    {
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

        winScreen = GameObject.Find("WinScreen");
        winScreen.SetActive(false);

        goldLevelDone = GameObject.Find("GoldLevelDone");
        goldLevelDone.SetActive(false);

        giftDone = GameObject.Find("GiftDone");
        giftDone.SetActive(false);

        coinsDone = GameObject.Find("CoinsDone");
        coinsDone.SetActive(false);

        goldDone = GameObject.Find("GoldDone");
        goldDone.SetActive(false);
    }

    private void Update()
    {
        if (winCondition)
            if (goldLevel.active)
                goldLevelDone.SetActive(true);
            else
                winScreen.SetActive(true);
        if (giftCondition)
            giftDone.SetActive(true);
        else giftDone.SetActive(false);
        if (coinsCondition)
            coinsDone.SetActive(true);
        else coinsDone.SetActive(false);
        if (goldCondition)
            goldDone.SetActive(true);
        else goldDone.SetActive(false);
    }

    public void OpenTab(string nameOfButton)
    {
        if (nameOfButton == "play")
        {
            worldSelection.SetActive(true);
            mainMenu.SetActive(false);
        }

        if (nameOfButton.StartsWith("world"))
        {
            levelSelection.SetActive(true);
            worldSelection.SetActive(false);
        }
        if (nameOfButton.StartsWith("level"))
        {
            if (nameOfButton == "level 2")
            {
                goldLevel.SetActive(true);
                levelSelection.SetActive(false);
            }
            else
            {
                //load scene=nameOfButton
                level.SetActive(true);
                levelSelection.SetActive(false);
            }
        }
        if (nameOfButton =="settings")
        {
            settings.SetActive(true);
        }
        if (nameOfButton == "closeSettings")
        {
            settings.SetActive(false);
        }
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
        {
            tmp = GameObject.Find(nameOfButton);
            if (tmp.GetComponent<Image>().sprite.name == "on")
                tmp.GetComponent<Image>().sprite = off;
            else if (tmp.GetComponent<Image>().sprite.name == "off")
            {
                tmp.GetComponent<Image>().sprite = on;
            }

        }       
    }
}