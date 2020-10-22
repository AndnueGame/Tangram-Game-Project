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
        FormSprite.color = MISC.GetColorCode(FormBuild.Color);
        FormSprite.rectTransform.sizeDelta = new Vector2(LevelSprite.texture.width, LevelSprite.texture.height);
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
            if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32 - 64) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32 - 64, y);
            if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);

            if (rectTransform.anchoredPosition.y < -12 * 32)
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


        LEM.SelectedForm = this.gameObject;
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
            if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32 - 64) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32 - 64, y);
            if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);


            //Snap to Grid
            x = rectTransform.anchoredPosition.x;
            y = rectTransform.anchoredPosition.y;

            int xf = (int)Mathf.Round(x / 32);
            int yf = (int)Mathf.Round(y / 32);

            x = xf * 32;
            y = yf * 32;
            //x = xf * 32 + LevelM.LevelWUneven * 16;

            //y = yf * 32 + LevelM.LevelHUneven * 16;

            rectTransform.anchoredPosition = new Vector2(x, y);

            xpos = (int)x;
            ypos = (int)y;

            if (LevelM.CheckNoCollision(this) == false)
            {
                if (wasInBottom == true)
                {
                    gameObject.transform.parent = GameObject.Find("ScaleThem").transform;
                    gameObject.Tween("SizeUp", Vector3.one, Vector3.one * 0.5f, 0.1f, TweenScaleFunctions.CubicEaseIn, updateSize);
                    isInFormContainer = true;
                    rectTransform.anchoredPosition = StartPosition;
                }
                else
                {
                    rectTransform.anchoredPosition = StartPosition;
                }
            }
        }
        isDrag = false;
        LevelM.RegenerateCurrentLevel();
        

    }





}

//=======================================================================