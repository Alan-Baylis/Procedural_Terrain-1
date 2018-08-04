using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor {

    SerializedProperty randomHeightRange;
    SerializedProperty heightMapScale;
    SerializedProperty heightMapImage;
    SerializedProperty perlinNoiseXScale;
    SerializedProperty PerlinNoiseYScale;
    SerializedProperty perlinOffsetX;
    SerializedProperty PerlinOffsetY;
    
    bool showRandom = false;
    bool showLoadHeights = false;
    bool showPerlin = false;

    private void OnEnable()
    {
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        perlinNoiseXScale = serializedObject.FindProperty("perlinNoiseXScale");
        PerlinNoiseYScale = serializedObject.FindProperty("perlinNoiseYScale");
        perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
        PerlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CustomTerrain terrain = (CustomTerrain)target;

        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set heights randomly between values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if(GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }

        }
        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Height Map Texture");
        
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Height Map Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapImage);
            EditorGUILayout.PropertyField(heightMapScale);
            if(GUILayout.Button("Load Texture"))
            {
                terrain.LoadHeightMapImage();
            }
        }

        showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin Noise Generator");

        if (showPerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Generator", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinNoiseXScale, 0, 0.05f, new GUIContent("X Scale"));
            EditorGUILayout.Slider(PerlinNoiseYScale, 0, 0.05f, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(PerlinOffsetY, 0, 10000, new GUIContent("Y Offset"));
            if (GUILayout.Button("Run Generator"))
            {
                terrain.PerlinNoise();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
