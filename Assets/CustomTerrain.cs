using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[ExecuteInEditMode]

public class CustomTerrain : MonoBehaviour {

    public Vector2 randomHeightRange = new Vector2(0, 0.1f);
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);

    public bool resetTerrain = true;

    //Perlin Noise
    public float perlinNoiseXScale = 0.01f;
    public float perlinNoiseYScale = 0.01f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8f;
    public float perlinHeightScale = 0.09f;

    //Voronoi 
    //public float 

    //Multiple Perlin
    [System.Serializable]
    public class PerlinParameters
    {
        public float mPerlinNoiseXScale = 0.01f;
        public float mPerlinNoiseYScale = 0.01f;
        public int mPerlinOctaves = 3;
        public float mPerlinPersistance = 8f;
        public float mPerlinHeightScale = 0.09f;
        public int mPerlinOffsetX = 0;
        public int mPerlinOffsetY = 0;
        public bool remove = false;
    }

    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };

    public Terrain terrain;
    public TerrainData terrainData;

    float[,] GetHeightMap()
    {
        if (!resetTerrain)
        {
            return terrainData.GetHeights(0,0,terrainData.heightmapWidth, terrainData.heightmapHeight);
        }
        else
        {
            return new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
        }
    }

    public void LoadHeightMapImage()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for (int z = 0; z < terrainData.heightmapHeight; z++)
            {
                heightMap[x, z] += heightMapImage.GetPixel((int)(x * heightMapScale.x), (int)(z * heightMapScale.z)).grayscale * heightMapScale.y;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void RandomTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for(int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for(int y = 0; y < terrainData.heightmapHeight; y++)
            {
                heightMap[x, y] += UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);

    }

    public void PerlinNoise()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for (int y = 0; y < terrainData.heightmapHeight; y++)
            {
                //heightMap[x, y] = Mathf.PerlinNoise((x + perlinOffsetX) * perlinNoiseXScale, (y + perlinOffsetY) * perlinNoiseYScale);
                heightMap[x, y] += Utils.fBM((x + perlinOffsetX) * perlinNoiseXScale, (y + perlinOffsetY) * perlinNoiseYScale, perlinOctaves, perlinPersistance) * perlinHeightScale;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void MultiPerlin()
    {
        float[,] heightMap = GetHeightMap();
        
        for (int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for(int y = 0; y < terrainData.heightmapHeight; y++)
            {
                foreach(PerlinParameters p in perlinParameters)
                {
                    heightMap[x, y] += Utils.fBM((x + p.mPerlinOffsetX) * p.mPerlinNoiseXScale, (y + p.mPerlinOffsetY) * p.mPerlinNoiseYScale, p.mPerlinOctaves, p.mPerlinPersistance) * p.mPerlinHeightScale;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void AddNewPerlin()
    {
        perlinParameters.Add(new PerlinParameters());
    }

    public void RemovePerlin()
    {
        List<PerlinParameters> keptPerlinParamaters = new List<PerlinParameters>();

        for (int i = 0; i < perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                keptPerlinParamaters.Add(perlinParameters[i]);
            }
        }

        if(keptPerlinParamaters.Count == 0)
        {
            keptPerlinParamaters.Add(perlinParameters[0]);
        }

        perlinParameters = keptPerlinParamaters;
    }

    public void VeronoiLift()
    {
        float[,] heightMap = GetHeightMap();
        float fallOff = 0.5f;

        Vector3 voronoiPeak = new Vector3(UnityEngine.Random.Range(0, terrainData.heightmapWidth), UnityEngine.Random.Range(0.0f, 0.25f), UnityEngine.Random.Range(0, terrainData.heightmapHeight));

        heightMap[(int)voronoiPeak.x, (int)voronoiPeak.z] = voronoiPeak.y;

        Vector2 peakLocation = new Vector2(voronoiPeak.x, voronoiPeak.z);
        float maxDistance = Vector2.Distance(new Vector2(0, 0), new Vector2(terrainData.heightmapWidth, terrainData.heightmapHeight));

        for(int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for(int y = 0; y < terrainData.heightmapHeight; y++)
            {
                if(!(x == voronoiPeak.x && y == voronoiPeak.z))
                {
                    float distanceToPeak = Vector2.Distance(peakLocation, new Vector2(x, y)) * fallOff;
                    heightMap[x, y] = voronoiPeak.y - (distanceToPeak / maxDistance);
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void ResetTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < terrainData.heightmapWidth; x++)
        {
            for (int y = 0; y < terrainData.heightmapHeight; y++)
            {
                heightMap[x, y] += randomHeightRange.x;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    private void OnEnable()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    private void Awake()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        AddTag(tagsProp, "Terrain");
        AddTag(tagsProp, "Cloud");
        AddTag(tagsProp, "Shore");

        tagManager.ApplyModifiedProperties();

        this.gameObject.tag = "Terrain";
        
    }

    void AddTag(SerializedProperty tagsProp, string newTag)
    {
        bool found = false;

        for(int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
