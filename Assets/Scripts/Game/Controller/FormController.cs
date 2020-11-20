/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Controller of Form in Level Play Mode

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DigitalRuby.Tween;

public class FormController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Form FormPlan;
    public Form FormBuild;
    private RectTransform rectTransform;

    public int xpos, ypos;
    public bool Init = false;
    public Sprite LevelSprite;

    public bool isInFormContainer = true;

    private Vector2 StartPosition;
    public bool isDrag = false;
    bool wasInBottom = true;

    public float RealX;
    public float RealY;

    public Sprite[] texturesAndShadows;


    public List<Color32> ColorPalette = new List<Color32>
    {
        new Color32(255,064,064,255), //Color1
        new Color32(078,255,078,255), //Color2
        new Color32(071,071,255,255), //Color3
        new Color32(070,255,255,255), //Color4
        new Color32(255,255,255,255), //Color5       
    };

    private void Start()
    {
        FormBuild = ScriptableObject.CreateInstance<Form>();
        FormPlan.CloneTo(FormBuild);
        rectTransform = GetComponent<RectTransform>();
        Init = true;

    }

    public void UpdateImage()
    {
        Image FormSprite = this.GetComponent<Image>();
        FormSprite.sprite = LevelSprite;
        //FormSprite.overrideSprite = LevelSprite;
        FormSprite.color = MISC.GetColorCode(FormBuild.Color);
        //FormSprite.transform.Find("Texture").GetComponent<Image>().color= MISC.GetColorCode(FormBuild.Color);
        //FormSprite.GetComponentInChildren<GameObject>()//.Find("Gun").gameObject;
        FormSprite.rectTransform.sizeDelta = new Vector2(LevelSprite.texture.width, LevelSprite.texture.height);
        rectTransform.anchoredPosition = new Vector2(xpos, ypos);

        this.GetComponent<Image>().enabled = false;




        for (int i = 0; i < texturesAndShadows.Length; i++)
        {

            if (this.name + "texture" == texturesAndShadows[i].name)
            {
                this.transform.Find("texture").GetComponent<Image>().sprite = texturesAndShadows[i];
            }
            if (this.name + "shadow" == texturesAndShadows[i].name)
            {
                this.transform.Find("shadow").GetComponent<Image>().sprite = texturesAndShadows[i];
            }
        }

        this.transform.Find("texture").transform.Rotate(0.0f, 0.0f, this.GetComponent<FormController>().FormBuild.Rotated * 90f, Space.Self);
        this.transform.Find("texture").GetComponent<Image>().color = ColorPalette[Random.Range(0, ColorPalette.Count)];
        this.transform.Find("texture").GetComponent<Image>().preserveAspect = true;
        //this.transform.Find("texture").GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta;

        float max = Compare(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y);    


        this.transform.Find("shadow").transform.Rotate(0.0f, 0.0f, this.GetComponent<FormController>().FormBuild.Rotated * 90f, Space.Self);       
        this.transform.Find("shadow").GetComponent<Image>().preserveAspect = true;
        this.transform.Find("shadow").GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta;


        this.transform.Find("texture").GetComponent<RectTransform>().sizeDelta = new Vector2(max, max);
        this.transform.Find("shadow").GetComponent<RectTransform>().sizeDelta = new Vector2(max, max);
       // this.transform.Find("shadow").GetComponent<RectTransform>().transform.position = new Vector2(7,-7);

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // README
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Because rotating shifts the position
        // Just rotate the images in Photoshop etc
        // and put the rotated versions also in the list
        // Then load it.
        //
        // Check it with rotationstate again (from FormBuild)
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /* || this.name + "texture" == "Tri5texture"
        {
            this.transform.Find("texture").GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.y, this.GetComponent<RectTransform>().sizeDelta.x);
        }
        if (this.name == "Tri5" && this.FormBuild.Rotated == 3/* || this.name + "texture" == "Tri5texture"*
       /* {
            this.transform.Find("texture").GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.y, this.GetComponent<RectTransform>().sizeDelta.x);
        }
        else*/

    }
    private float Compare(float a, float b)
    {
        return a > b ? a : b;
    }

    public void OnDrag(PointerEventData eventData)
    {
        System.Action<ITween<Vector3>> updateSize = (t) =>
        {
            gameObject.transform.localScale = t.CurrentValue;
        };

        rectTransform.anchoredPosition += eventData.delta;

        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;

        if (isInFormContainer == false)
        {
            //Bounds
            if (rectTransform.anchoredPosition.x < 0) rectTransform.anchoredPosition = new Vector2(000, y);
            if (rectTransform.anchoredPosition.y > 0) rectTransform.anchoredPosition = new Vector2(x, 000);
            if (rectTransform.anchoredPosition.x >  576 - FormBuild.Width  * 32) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32, y);
            if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);

            if (rectTransform.anchoredPosition.y < -680)
            {
                gameObject.transform.parent = GameObject.Find("ScaleThem").transform;
                isInFormContainer = true;
                gameObject.Tween("SizeUp", Vector3.one, Vector3.one * 0.5f, 0.1f, TweenScaleFunctions.CubicEaseIn, updateSize);
            }



        }
        else
        {
            if (rectTransform.anchoredPosition.y > 64)
            {
                gameObject.transform.parent = GameObject.Find("SelectedForm").transform;
                isInFormContainer = false;
                gameObject.Tween("SizeUp", Vector3.one * 0.5f, Vector3.one * 1, 0.1f, TweenScaleFunctions.CubicEaseIn, updateSize);
            }
        }

        this.transform.Find("shadow").GetComponent<Image>().enabled = true;
       // LEM.SelectedForm = this.gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.childCount);
        StartPosition = rectTransform.anchoredPosition;

        isDrag = true;
        wasInBottom = true;
        if (isInFormContainer == false) wasInBottom = false;

        LevelM.RegenerateCurrentLevel();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;


        System.Action<ITween<Vector3>> updateSize = (t) =>
        {
            gameObject.transform.localScale = t.CurrentValue;
        };

        if (isInFormContainer == false)
        {
            if (rectTransform.anchoredPosition.x < 0) rectTransform.anchoredPosition = new Vector2(000, y);
            if (rectTransform.anchoredPosition.y > 0) rectTransform.anchoredPosition = new Vector2(x, 000);
            if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32, y);
            if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);


            //Snap to Grid
            x = rectTransform.anchoredPosition.x;
            y = rectTransform.anchoredPosition.y;

            int xf = (int)Mathf.Round(x / 64);
            int yf = (int)Mathf.Round(y / 64);

            x = xf * 64 - 32;
            y = yf * 64 - 32;
            //x = xf * 32 + LevelM.LevelWUneven * 16;

            //y = yf * 32 + LevelM.LevelHUneven * 16;

            rectTransform.anchoredPosition = new Vector2(x, y);

            xpos = (int)x;
            ypos = (int)y;

            RealX = (xpos - (int)LevelM.LevelX) / 64 - 1;
            RealY = (ypos + (int)LevelM.LevelY + 235) / 64 * -1 - 2;

            if (LevelM.CheckNoCollision(this) == false)
            {
                if (wasInBottom == true)
                {
                    gameObject.transform.parent = GameObject.Find("ScaleThem").transform;
                    gameObject.Tween("SizeUp", Vector3.one, Vector3.one * 0.5f, 0.1f, TweenScaleFunctions.CubicEaseIn, updateSize);
                    isInFormContainer = true;
                    rectTransform.anchoredPosition = StartPosition;
                    xpos = (int)StartPosition.x;
                    ypos = (int)StartPosition.y;
                }
                else
                {
                    rectTransform.anchoredPosition = StartPosition;
                    xpos = (int)StartPosition.x;
                    ypos = (int)StartPosition.y;
                }
            }
        }else{

            if (x < 10) x = 10;
            if (x > 580 - FormBuild.Width * 32) x = 580 - FormBuild.Width * 32;
            if (y < -290 + FormBuild.Height * 32) y = -290 + FormBuild.Height * 32;
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        isDrag = false;
        LevelM.RegenerateCurrentLevel();
        this.transform.Find("shadow").GetComponent<Image>().enabled = false;

    }





}

//=======================================================================