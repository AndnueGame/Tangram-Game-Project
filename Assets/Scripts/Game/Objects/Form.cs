/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Tangram/Form", order = 1)]
public class Form : ScriptableObject
{
    //Each form has a lot of Triforces or Squares
    [SerializeField] public List<SimpleTriforce> Triforces;

    //Name
    public string Name;

    //Dimension of the Form
    public int Width;
    public int Height;

    public int X;
    public int Y;

    public int Color;

    //Rotated State
    public int Rotated = 0;

    //=============================================================== Resize
    public void Resize(int w, int h)
    {
        Triforces = new List<SimpleTriforce>();
        Width = w;
        Height = h;
        for(int x=0; x < w * h; x++)
        {
            Triforces.Add(new SimpleTriforce());
        }
    }

    //=============================================================== GetXY

    public SimpleTriforce Get(int x, int y)
    {
        SimpleTriforce ret = new SimpleTriforce();

        if (x>=0 && y>=0 && x<Width && y < Height)
        {
            ret = Triforces[y * Width + x];
        }
        return ret;
    }

    //=============================================================== Rotate Functions
    public void RotateRight()
    {
        List<SimpleTriforce> Cache = new List<SimpleTriforce>();
        int oldWidth  = Width;
        int oldHeight = Height;
        Width  = Height;
        Height = oldWidth;

        for (int x = 0; x <= oldWidth - 1; x++)
        {
            for (int y = oldHeight-1; y >= 0; y--)
            {
                Cache.Add(new SimpleTriforce(Triforces[y * oldWidth + x].Type).RotateRight());
            }
        }

        Rotated--;
        if (Rotated < 0) Rotated = 3;

        Triforces = Cache;
    }


    public void RotateLeft()
    {
        List<SimpleTriforce> Cache = new List<SimpleTriforce>();
        int oldWidth = Width;
        int oldHeight = Height;
        Width = Height;
        Height = oldWidth;

        for(int x = oldWidth -1; x >=0; x--)
        {
            for (int y = 0; y <= oldHeight - 1; y++)
            {
                Cache.Add(new SimpleTriforce(Triforces[y * oldWidth + x].Type).RotateLeft());
            }
        }

        Rotated++;
        if (Rotated > 3) Rotated = 0;

        Triforces = Cache;
    }


    public void CloneTo(Form cache)
    {
        cache.Width     = this.Width;
        cache.Height    = this.Height;
        cache.Color     = this.Color;
        cache.Name      = this.Name;
        cache.Triforces = new List<SimpleTriforce>();
        this.Triforces.ForEach((item) =>
        {
            cache.Triforces.Add(new SimpleTriforce(item.Type));
        });
        cache.Rotated   = this.Rotated;
        cache.X         = this.X;
        cache.Y         = this.Y;
    }
}

//=======================================================================