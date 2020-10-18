/* =====================================================================
 * Tristan Herpich - 2020 - Tangram Project
======================================================================== */
// Editor Window for Forms (1.0)

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Form))]
public class FormEditor : Editor
{
    private int  oldW = -1;
    private int  oldH = -1;
    private bool change = false;



    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("When you used this form in a level, NEVER ever change it anymore - no new name, no new Triangles - nothing! Just make a new one!", MessageType.Warning);
        EditorGUILayout.HelpBox("Also take care to not use the same names TWICE!", MessageType.Warning);

        //Selection for the DropDown
        string[] fields = new[] { "_", "◤", "◥", "◣", "◢", "■" };
        change = false;

        EditorGUILayout.LabelField("================== Custom Form Editor 1.0");
        EditorGUILayout.Space();
        Form sf = target as Form;
        if (sf == null)
        {
            EditorGUILayout.LabelField("> Target Error!");
            return;
        }else {
            if (oldW == -1 && oldH == -1) { 
                oldW = sf.Width;
                oldH = sf.Height;
            }
        }

        //Loading Textures for Preview
        Texture Empty      = Resources.Load<Texture2D>("FormEditor/Shape_Empty");
        Texture TriangleDL = Resources.Load<Texture2D>("FormEditor/Shape_TriangleDL");
        Texture TriangleDR = Resources.Load<Texture2D>("FormEditor/Shape_TriangleDR");
        Texture TriangleTL = Resources.Load<Texture2D>("FormEditor/Shape_TriangleTL");
        Texture TriangleTR = Resources.Load<Texture2D>("FormEditor/Shape_TriangleTR");
        Texture Square     = Resources.Load<Texture2D>("FormEditor/Shape_Square");


        serializedObject.Update();

        sf.Name = EditorGUILayout.TextField("Name:", sf.Name);
        if(sf.Name == "")
        {
            EditorGUILayout.HelpBox("Please enter a UNIQUE name for this form!", MessageType.Error);
        }

        serializedObject.targetObject.name = sf.name;
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dimensions of Form ::::::::::::::::::::::");
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Warning: Changing these values, will empty the Triangles!", MessageType.Warning);
        EditorGUILayout.Space();

        oldW = sf.Width;
        sf.Width  = EditorGUILayout.IntField("Width: ", sf.Width);
        if (sf.Width  < 1)  sf.Width  = 1;
        if (sf.Width  > 10) sf.Width  = 10;

        oldH = sf.Height;
        sf.Height = EditorGUILayout.IntField("Height: ", sf.Height);
        if (sf.Height < 1)  sf.Height = 1;
        if (sf.Height > 10) sf.Height = 10;

        

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Triangles of Form :::::::::::::::::::::::");
        EditorGUILayout.Space();

        if (oldW != sf.Width || oldH != sf.Height) change = true;
        if (change) sf.Resize(sf.Width, sf.Height);

        for (int y = 0; y < sf.Height; y++) {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < sf.Width; x++)
            {
                int oldType = (int)sf.Triforces[x + y * sf.Width].Type;
                sf.Triforces[x+y*sf.Width].Type = (SimpleTriforce.TriforceType)  EditorGUILayout.Popup((int) sf.Triforces[x + y * sf.Width].Type, fields);
                if (oldType != (int)sf.Triforces[x + y * sf.Width].Type) change = true;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Preview ::::::::::::::::::::::: [Beta]");

        for (int y = 0; y < sf.Height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < sf.Width; x++)
            {
                if (Square == null) break;
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.FILLED)    GUILayout.Box(Square);
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.EMPTY)     GUILayout.Box(Empty);
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.TOPLEFT)   GUILayout.Box(TriangleTL);
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.TOPRIGHT)  GUILayout.Box(TriangleTR);
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.BOTLEFT)   GUILayout.Box(TriangleDL);
                if (sf.Triforces[x + y * sf.Width].Type == SimpleTriforce.TriforceType.BOTRIGHT)  GUILayout.Box(TriangleDR);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Utilities ::::::::::::::::::::::: [Beta]");
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("RotationState:" + sf.Rotated.ToString());
        EditorGUILayout.Space();

        if (GUILayout.Button("Rotate Left"))
        {
            sf.RotateLeft();
            oldW = sf.Width;
            oldH = sf.Height;
        }

        if (GUILayout.Button("Rotate Right"))
        {
            sf.RotateRight();
            oldW = sf.Width;
            oldH = sf.Height;
        }


        //Save the Form
        serializedObject.ApplyModifiedProperties();
    }
}

//=======================================================================