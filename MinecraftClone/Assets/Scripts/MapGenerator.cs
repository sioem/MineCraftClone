using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public float xOrigin;
    public float zOrigin;

    public float scale = 1.0f;

    private Texture2D noiseTexture;

    private Color[] pix;
    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private Material dirtMaterial;
    [SerializeField]
    private Material grassMaterial;

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private GameObject trees;

    [SerializeField]
    private GameObject treePrefab;
    [SerializeField]
    private int numberOfTrees = 20;
    [SerializeField]
    private float minDistance = 5f;

    private List<Vector3> treePositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        this.xOrigin = Random.Range(0f, 100f);
        this.zOrigin = Random.Range(0f, 100f);
        this.scale = Random.Range(1f, 5f);
        this.CalcNoise();
        this.CreateBlocks();
        this.CreateTrees();
    }

    void CalcNoise()
    {
        this.noiseTexture = new Texture2D(this.width, this.height);
        this.pix = new Color[this.noiseTexture.width * this.noiseTexture.height];
        this.rend.material.mainTexture = this.noiseTexture;

        float z = 0.0f;
        while (z < this.noiseTexture.height)
        {
            float x = 0.0f;
            while (x < this.noiseTexture.width)
            {
                float xCoord = this.xOrigin + x / this.noiseTexture.width * scale;
                float zCoord = this.zOrigin + z / this.noiseTexture.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, zCoord);

                pix[(int)z * this.noiseTexture.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            z++;
        }
        this.noiseTexture.SetPixels(pix);
        this.noiseTexture.Apply();
    }

    private void CreateBlocks()
    {
        float z = 0.0f;
        while (z < this.noiseTexture.height)
        {
            float x = 0.0f;
            while (x < this.noiseTexture.width)
            {
                float y = this.GetY(pix[(int)z * this.noiseTexture.width + (int)x].r);
                float perlin = y;
                while (y > -2)
                {                 
                    GameObject cube = Instantiate(this.blockPrefab, this.stage.transform);

                    if (y == perlin)
                    {
                        cube.GetComponent<MeshRenderer>().material = grassMaterial;
                    }
                    else
                    {
                        cube.GetComponent<MeshRenderer>().material = dirtMaterial;
                    }
                    cube.transform.position = new Vector3(x, y, z);
                    y--;
                }
                x++;
            }
            z++;
        }
    }

    private void CreateTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(treePrefab, randomPosition, Quaternion.identity, this.trees.transform);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition;
        Vector3 treePosition;
        bool validPosition = false;

        do
        {
            randomPosition = new Vector3(Random.Range(0, this.width), 0, Random.Range(0, this.height));
            float y = this.GetY(pix[(int)randomPosition.z * this.noiseTexture.width + (int)randomPosition.x].r) + 1;
            treePosition = new Vector3(randomPosition.x, y, randomPosition.z);

            validPosition = true;

            foreach (Vector3 position in treePositions)
            {
                if (Vector3.Distance(treePosition, position) < minDistance)
                {
                    validPosition = false;
                    break;
                }
            }

        } while (!validPosition);

        treePositions.Add(treePosition);
        return treePosition;
    }

    private float GetY(float sample)
    {
        if (sample >= 0f && sample < 0.1f)
        {
            return 0f;
        }
        else if (sample >= 0.1f && sample < 0.2f)
        {
            return 1f;
        }
        else if (sample >= 0.2f && sample < 0.3f)
        {
            return 2f;
        }
        else if (sample >= 0.3f && sample < 0.4f)
        {
            return 3f;
        }
        else if (sample >= 0.4f && sample < 0.5f)
        {
            return 4f;
        }
        else if (sample >= 0.5f && sample < 0.6f)
        {
            return 5f;
        }
        else if (sample >= 0.6f && sample < 0.7f)
        {
            return 6f;
        }
        else if (sample >= 0.7f && sample < 0.8f)
        {
            return 7f;
        }
        else if (sample >= 0.8f && sample < 0.9f)
        {
            return 8f;
        }
        else if (sample >= 0.9f && sample < 1.0f)
        {
            return 9f;
        }
        else
        {
            return -1f;
        }
    }
}
