﻿/* =====================================================================
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

        //this.GetComponent<Image>().enabled = false;
       


        
        for (int i=0;i<texturesAndShadows.Length;i++)
        {
            if (this.name + "texture" == texturesAndShadows[i].name)
            {
       
                FormSprite.sprite = texturesAndShadows[i];

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


            }
        }  
        
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

            if (x < 0) x = 0;
            if (x > 600 - FormBuild.Width * 32) x = 600 - FormBuild.Width * 32;
            if (y < -350 + FormBuild.Height * 32) y = -350 + FormBuild.Width * 32;
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        isDrag = false;
        LevelM.RegenerateCurrentLevel();
        //this.transform.Find("shadow").GetComponent<Image>().enabled = false;

    }





}

//=======================================================================