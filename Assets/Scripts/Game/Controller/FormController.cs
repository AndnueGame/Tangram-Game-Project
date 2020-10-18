/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Controller of Form in Level Play Mode

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FormController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Form FormPlan;
    public Form FormBuild;
    private RectTransform rectTransform;

    int xpos, ypos;

    

    private void Start()
    {
        FormBuild = ScriptableObject.CreateInstance<Form>();
        FormPlan.CloneTo(FormBuild);

        rectTransform = GetComponent<RectTransform>();




        Sprite Cache = LevelM.GenerateFormSprite(FormPlan);

        Image TEST = this.GetComponent<Image>();
        TEST.sprite = Cache;
        TEST.rectTransform.sizeDelta = new Vector2(Cache.texture.width, Cache.texture.height);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;

        float x = rectTransform.anchoredPosition.x;
        float y = rectTransform.anchoredPosition.y;

        //Bounds
        if (rectTransform.anchoredPosition.x < 0) rectTransform.anchoredPosition = new Vector2(000, y);
        if (rectTransform.anchoredPosition.y > 0) rectTransform.anchoredPosition = new Vector2(x, 000);
        if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32 -64) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32 -64, y);
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
        if (rectTransform.anchoredPosition.x > 576 - FormBuild.Width * 32 -64) rectTransform.anchoredPosition = new Vector2(576 - FormBuild.Width * 32 -64, y);
        if (rectTransform.anchoredPosition.y < -832 + FormBuild.Height * 32) rectTransform.anchoredPosition = new Vector2(x, -832 + FormBuild.Height * 32);

        //Snap to Grid
        x = rectTransform.anchoredPosition.x;
        y = rectTransform.anchoredPosition.y;

        int xf = (int)Mathf.Round(x / 64);
        int yf = (int)Mathf.Round(y / 64);

        x = xf * 64;
        y = yf * 64;

        rectTransform.anchoredPosition = new Vector2(x, y);
        xpos = (int)x;
        ypos = (int)y;
    }


   


}

//=======================================================================