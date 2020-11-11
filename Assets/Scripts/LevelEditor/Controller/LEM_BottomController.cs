/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Controller of Form in LevelEditor-Mode


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LEM_BottomController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Form FormPlan;
    public GameObject imageContainer;
    public GameObject tilePrefab;

    private Button RotateIcon;
    private RectTransform rectTransform;
    private static Sprite Empty, TriangleDL, TriangleDR, TriangleTL, TriangleTR, Square;
    public Form FormBuild;
    private GameObject Helper;

    public LEM_FormController LEMFC;

    public GameObject thisGameObject;
    public LevelData DataSet;

    public int xpos, ypos;
    public bool Init = false;

    private void Start()
    {
        FormBuild = ScriptableObject.CreateInstance<Form>();
        FormPlan.CloneTo(FormBuild);

        rectTransform = GetComponent<RectTransform>();





        //Loading Textures for Form
        Empty = Resources.Load<Sprite>("FormEditor/Shape_Empty");
        TriangleDL = Resources.Load<Sprite>("FormEditor/Shape_TriangleDL");
        TriangleDR = Resources.Load<Sprite>("FormEditor/Shape_TriangleDR");
        TriangleTL = Resources.Load<Sprite>("FormEditor/Shape_TriangleTL");
        TriangleTR = Resources.Load<Sprite>("FormEditor/Shape_TriangleTR");
        Square = Resources.Load<Sprite>("FormEditor/Shape_Square");


        if (Empty == null || TriangleDL == null || TriangleDR == null || TriangleTL == null || TriangleTR == null || Square == null)
        {
            Debug.LogError("[LEM_FE] Textures could not be loaded!");
            return;
        }


        if (FormBuild == null)
        {
            Debug.LogError("[LEM_FE] No Form specified!");
            return;
        }


        Transform Cache = transform.Find("Images");
        if (Cache == null)
        {
            Debug.LogError("[LEM_FE] Could not find Container!");
            return;

        }
        else
        {
            imageContainer = Cache.gameObject;
            GenerateImages();
        }

        if (LEM.New == true)
        {
            FormBuild.Color = Random.Range(0, MISC.ColorPalette.Count - 1);
        }
        ChangeColor(FormBuild.Color);

        thisGameObject = this.gameObject;
        //LEM.lemfcs.Add(this);

        Init = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;

        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;

        //Bounds
        if (rectTransform.anchoredPosition.x < 0) rectTransform.anchoredPosition = new Vector2(000, y);
        if (rectTransform.anchoredPosition.y > 0) rectTransform.anchoredPosition = new Vector2(x, 000);
        if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32, y);
        if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);


        LEM.SelectedForm = this.gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.childCount);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;

        //Bounds
        if (rectTransform.anchoredPosition.x < 0) rectTransform.anchoredPosition = new Vector2(000, y);
        if (rectTransform.anchoredPosition.y > 0) rectTransform.anchoredPosition = new Vector2(x, 000);
        if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32, y);
        if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);

        //Snap to Grid
        x = rectTransform.anchoredPosition.x;
        y = rectTransform.anchoredPosition.y;

        int xf = (int)Mathf.Round(x / 16);
        int yf = (int)Mathf.Round(y / 16);

        x = xf * 16;
        y = yf * 16;

        rectTransform.anchoredPosition = new Vector2(x, y);
        xpos = (int)x;
        ypos = (int)y;
    }

    public void GenerateImages()
    {
        Transform Cache;
        Cache = transform.Find("Images");
        this.transform.localScale = new Vector3(1, 1, 1);

        for (int i = Cache.childCount - 1; i >= 0; i--)
        {
            Transform child = Cache.GetChild(i);
            Destroy(child.gameObject);
        }


        imageContainer = Cache.gameObject;

        for (int y = 0; y < FormBuild.Height; y++)
        {
            for (int x = 0; x < FormBuild.Width; x++)
            {
                SimpleTriforce.TriforceType typeCache = FormBuild.Get(x, y).Type;

                if (typeCache != SimpleTriforce.TriforceType.EMPTY)
                {
                    GameObject cache = Instantiate(tilePrefab);
                    cache.transform.SetParent(imageContainer.transform);
                    RectTransform rt = cache.GetComponent<RectTransform>();

                    rt.anchoredPosition = Vector2.zero;
                    rt.anchoredPosition += new Vector2(x * 32, -y * 32);

                    if (typeCache != SimpleTriforce.TriforceType.EMPTY)
                    {
                        Image imgCache = cache.GetComponent<Image>();
                        imgCache.color = MISC.ColorPalette[FormBuild.Color];

                        switch (typeCache)
                        {
                            case SimpleTriforce.TriforceType.FILLED:
                                imgCache.sprite = Square;
                                break;
                            case SimpleTriforce.TriforceType.BOTLEFT:
                                imgCache.sprite = TriangleDL;
                                break;
                            case SimpleTriforce.TriforceType.BOTRIGHT:
                                imgCache.sprite = TriangleDR;
                                break;
                            case SimpleTriforce.TriforceType.TOPLEFT:
                                imgCache.sprite = TriangleTL;
                                break;
                            case SimpleTriforce.TriforceType.TOPRIGHT:
                                imgCache.sprite = TriangleTR;
                                break;
                        }
                    }
                }

            }
        }
        this.transform.localScale = new Vector3(0.75f,0.75f,0.75f);

    }



    void Bttn_RotateRight()
    {
        FormBuild.RotateRight();
        GenerateImages();
    }

    void Bttn_Settings()
    {
        //LEM.LEMFC = this;
        LEM.instance.SwitchSettings(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (LEM.SelectedForm != this.gameObject)
        {
            Helper.SetActive(false);
        }
        else
        {
            Helper.SetActive(true);
        }
    }

    public void ChangeColor(int Color)
    {
        FormBuild.Color = Color;
        GameObject Cache = this.transform.Find("Images").gameObject;
        for (int x = 0; x < Cache.transform.childCount; x++)
        {
            Image iTile = Cache.transform.GetChild(x).GetComponent<Image>();
            if (iTile)
            {
                iTile.color = MISC.GetColorCode(Color);
            }
        }
    }

    public void UpdatePosition()
    {
        rectTransform.anchoredPosition = new Vector2(xpos, ypos);
    }
}

//=======================================================================