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
    SerializedProperty perlinOffsetY;
    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlinHeightScale;
    
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
        perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        
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
            EditorGUILayout.Slider(perlinNoiseXScale, 0f, 0.05f, new GUIContent("X Scale"));
            EditorGUILayout.Slider(PerlinNoiseYScale, 0f, 0.05f, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.IntSlider(perlinOctaves, 0, 50, new GUIContent("Brownian Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 20f, new GUIContent("Brownian Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0f, 1f, new GUIContent("Height Scale"));
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
