using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    string previousTab = "MainMenu";
    string currentTab = "MainMenu";

    public void ClickOnButton()
    {
        string name = this.name.ToString();
        if (this.name == "play")
        {
            currentTab = "WorldSelection";
            previousTab = "MainMenu";
            OpenTab(currentTab, false);
        }

        if (this.name == "collections")
        {
            currentTab = "Collections";
            previousTab = "MainMenu";
            OpenTab(currentTab, false);
        }

        if (this.name == "settings")
        {
            currentTab = "Collections";
            previousTab = "MainMenu";
            OpenTab(currentTab, true);
        }

        if (this.name == "world")
        {
            currentTab = "LevelSelection";
            previousTab = "WorldSelection";
            OpenTab(currentTab,false);
        }
        if (this.name.StartsWith("level"))
        {
            currentTab = "Level";
            previousTab = "LevelSelection";
            //Need load scene actually Scene.LoadScene("level"+ number);
            OpenTab(currentTab, false);
        }
      
        if (this.name == "back")
        {
            if (currentTab == "WorldSelection")
            {
                currentTab = "MainMenu";
                previousTab = "MainMenu";
                OpenTab(currentTab, false);
            }
            if (currentTab == "LevelSelection")
            {
                currentTab = "WorldSelection";
                OpenTab(currentTab, false);
            }
            if (currentTab == "Collections")
            {
                currentTab = "MainMenu";
                OpenTab(currentTab, false);
            }
        }
    }
    /*
    switch (name)
    {
        case "play":
            {
                previousTab = "MainMenu";
                currentTab = "WorldSelection";
                OpenTab("WorldSelection");
                return 1;
            }
        case "world":
            {
                previousTab = "WorldSelection";
                currentTab = "LevelSelection";
                OpenTab("LevelSelection");
                return 1;
            }
        case "back":
            {
                if (currentTab == "WorldSelection")
                {
                    previousTab = "WorldSelection";
                    currentTab = "MainMenu";
                    OpenTab("MainMenu");
                }
                if (currentTab == "LevelSelection")
                {
                    previousTab = "LevelSelection";
                    currentTab = "WorldSelection";
                    OpenTab("WorldSelection");
                }
                return 1;
            }

        default:
            return 0;
    }

    /*if(this.name =="back")
    {
        OpenTab(currentTab);
    }*/
    
    void OpenTab(string nameOfTab,bool visibility)
    {
        Debug.Log(nameOfTab);
        if (GameObject.Find("Canvas").transform.FindChild(nameOfTab))
        {
            GameObject.Find("Canvas").transform.FindChild(nameOfTab).gameObject.SetActive(true);
           // GameObject.Find(nameOfTab).SetActive(true);
            if (!visibility)
                if (nameOfTab != previousTab)
                    GameObject.Find(previousTab).SetActive(false);
        }
        else
            Debug.Log("not found");
    }
}
