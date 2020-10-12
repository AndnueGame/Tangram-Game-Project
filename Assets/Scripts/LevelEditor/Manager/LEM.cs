/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Level Editor Manager 1.0
// This handles the functions of the LE


using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LEM : MonoBehaviour
{
    [HideInInspector]
    public string LevelName;


    public Dropdown FormList;
    public Button FormRefresh;
    public Button Spawn;

    public Button mButton_New;
    public Button mButton_Load;
    public Button mButton_Save;
    public Dropdown mDrop;
    public GameObject mPanel;

    public Button Delete;
    public Button Save;
    public GameObject Panel;
    public Dropdown ColorDD;

    public GameObject FormFrame;
    private GameObject FormContainer;

    public static LEM instance;

    [HideInInspector]
    public static GameObject SelectedForm;

    [HideInInspector]
    public static LEM_FormController LEMFC;

    [HideInInspector]
    public static Level LevelToEdit;

    public static List<LEM_FormController> lemfcs = new List<LEM_FormController>();
    public static bool New = false;

    public LevelData _dCache = new LevelData();
    public LEM_FormController _dLem = new LEM_FormController();

    public string Levelpath;

    //=================================================================================================


    private void Start()
    {
        //Link Buttons with functions
        FormRefresh.onClick.AddListener(RefreshForms);
        Spawn.onClick.AddListener(BttnSpawn);
        Delete.onClick.AddListener(Bttn_Delete);
        Save.onClick.AddListener(Bttn_Save);
        mButton_New.onClick.AddListener(Bttn_New);
        mButton_Load.onClick.AddListener(Bttn_Load);
        mButton_Save.onClick.AddListener(Bttn_LevelSave);

        //Once Refresh
        RefreshForms();

        //Link GameObjects
        FormContainer = GameObject.Find("Forms");

        instance = this;

        if (FormContainer == null)
        {
            Debug.LogError("[LEM] Form Container not found!");
        }

        List<string> opts = new List<string>();
        for (int x=0; x<MISC.ColorPalette.Count; x++)
        {
            opts.Add("Color " + (x + 1).ToString());
        }
        ColorDD.AddOptions(opts);


        LevelToEdit = ScriptableObject.CreateInstance<Level>();
    }





    //=================================================================================================

    public void RefreshForms()
    {
        FormList.ClearOptions();
        List<Form>forms = MISC.FindAssetsByType<Form>();
        List<string> options = new List<string>();

        for(int x=0; x<forms.Count; x++)
        {
            options.Add(forms[x].Name);
        }

        FormList.AddOptions(options);
        
    }

    //=================================================================================================

    public void BttnSpawn()
    {
        SpawnForm();
    }

    public LEM_FormController SpawnForm(string Name = "")
    {
        LEM_FormController c = null;
        if (Name == "") Name = FormList.options[FormList.value].text;

        List<Form> forms = MISC.FindAssetsByType<Form>();

        for (int x = 0; x < forms.Count; x++)
        {
            if(forms[x].Name == Name)
            {
                GameObject Cache = Instantiate(FormFrame);
                Cache.transform.SetParent(FormContainer.transform);
                Cache.name = "Form";

                LEM_FormController LEMFC = Cache.GetComponent<LEM_FormController>();
                if (LEMFC != null)
                {
                    LEMFC.FormPlan = forms[x];
                    RectTransform LEMFCRT = Cache.GetComponent<RectTransform>();
                    LEMFCRT.anchoredPosition = Vector2.zero;
                    LEMFCRT.anchorMin = new Vector2(0, 1);
                    LEMFCRT.anchorMax = new Vector2(0, 1);
                    LEMFCRT.pivot = new Vector2(0, 1);
                    return LEMFC;

                }
                else{
                    Debug.LogError("[LEM] No Form Controller found!");
                }

                return c;
            }
        }
        return c;
        Debug.LogWarning("[LEM] Could not find form!");
    }

    //:::::::::::::::::

    public void SwitchSettings(bool state = true)
    {
        Panel.SetActive(state);
        if (state == true) { 
            ColorDD.value = LEMFC.FormPlan.Color;
        }
    }

    public void Bttn_Delete()
    {
        if (EditorUtility.DisplayDialog("Warning","Do you really wanna delete this form?","Sure!","Nah help me I missclicked!")) {
            lemfcs.Remove(LEMFC);
            Destroy(LEMFC.thisGameObject);
        }
        SwitchSettings(false);
    }

    public void Bttn_Save()
    {
        SwitchSettings(false);
        LEMFC.ChangeColor(ColorDD.value);
    }


    public void Bttn_Load()
    {
        string path = EditorUtility.OpenFilePanel("Load Level","Assets/Objects/Levels/","json");

        New = false;
        if (path.Length != 0) {
            string jsonformatted = System.IO.File.ReadAllText(path);
            for(int x=0; x<jsonformatted.Split('\n').Length-1; x++)
            {
                string jsontile = jsonformatted.Split('\n')[x];
                if (jsontile.Trim().Length != 0)
                {
                    LevelData dataCache = JsonUtility.FromJson<LevelData>(jsontile);
                    LEM_FormController Cache = SpawnForm(dataCache.FormName);
                    
                    if (Cache != null && dataCache != null)
                    {
                        StartCoroutine(waitForFormLoad(Cache, dataCache));
                    }
                }
            }

            //HM I guess this won't work... maybe save to JSON?
            Debug.Log("Assets/Objects/Levels/" + LevelName);
            Debug.Log(jsonformatted);

            New = true;

            Levelpath = path;
            mPanel.SetActive(false);
        }
    }

    public IEnumerator waitForFormLoad(LEM_FormController lem, LevelData LED)
    {
        while(lem.Init == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        lem.xpos = LED.x;
        lem.ypos = LED.y;
  
        while (lem.FormBuild.Rotated != LED.RotationState)
        {
            lem.FormBuild.RotateRight();
        }

        lem.GenerateImages();
        lem.ChangeColor(LED.Color);
        lem.UpdatePosition();

    }

    public void Bttn_New()
    {
        New = true;
        string path = EditorUtility.SaveFilePanelInProject("Save Your Level", "Level", "json", "Please select file name to save Level to:", "Assets/Objects/Levels/");
        if (!string.IsNullOrEmpty(path))
        {
            LevelName = path.Split('/')[path.Split('/').Length-1];
            mPanel.SetActive(false);
            Levelpath = path;
        }
        
    }

    public void Bttn_LevelSave()
    {
        string json = "";
        foreach (LEM_FormController c in lemfcs)
        {
            LevelData cache = new LevelData();
            cache.x = c.xpos;
            cache.y = c.ypos;
            cache.RotationState = c.FormBuild.Rotated;
            cache.FormName = c.FormBuild.Name;
            cache.Color = c.FormBuild.Color;
            
            string potion = JsonUtility.ToJson(cache);
            json += potion + "\n";
        }
        System.IO.File.WriteAllText(Levelpath, json);



        if (EditorUtility.DisplayDialog("Information", "Your Tangram has been saved!","Nice!"))
        {
            #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

}

//=======================================================================