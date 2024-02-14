using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class TestPerlinNoise : MonoBehaviour
{
    public int width;
    public int height;

    public float xOrigin;
    public float zOrigin;

    public float scale = 1.0f;

    private Texture2D noiseTexture;

    private RawImage image;
    private Color[] pix;
    [SerializeField]
    private Renderer rend;

    [SerializeField]
    private Material dirtMaterial;
    [SerializeField]
    private Material grassMaterial;

    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private GameObject stage;
    [SerializeField]
    private Button btnRandom;
    [SerializeField]
    private Button btnSave;
    [SerializeField]
    private Button btnCreate;
    [SerializeField]
    private Button btnDestroy;

    private List<GameObject> cubePool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        this.btnRandom.onClick.AddListener(() =>
        {
            this.xOrigin = Random.Range(0f, 100f);
            this.zOrigin = Random.Range(0f, 100f);
            this.scale = Random.Range(1f, 5f);
            this.CalcNoise();
        });

        this.btnSave.onClick.AddListener(() =>
        {
            //파일로 저장 
            byte[] bytes = noiseTexture.EncodeToPNG();

            Debug.LogFormat("bytes.Length: {0}", bytes.Length);

            var dirPath = Application.dataPath + "/SaveImages/";
            Debug.LogFormat("dirPath: {0}", dirPath);

            int cnt = 0;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var files = Directory.GetFiles(dirPath);
            cnt = files.Length / 2;

            File.WriteAllBytes(dirPath + "Image_" + cnt + ".png", bytes);
        });

        this.btnCreate.onClick.AddListener(() =>
        {
            this.MoveBlocks();
            //await InstantiateCubesAsync();
            //this.CreateBlocks();
        });

        this.btnDestroy.onClick.AddListener(() =>
        {
            this.DestroyBlocks();
        });
        this.CreateBlocks();
    }

    void CalcNoise()
    {
        this.noiseTexture = new Texture2D(this.width, this.height);
        this.pix = new Color[this.noiseTexture.width * this.noiseTexture.height];
        this.rend.material.mainTexture = this.noiseTexture;

        float z = 0.0f;       
        while(z < this.noiseTexture.height)
        {
            float x = 0.0f;
            while(x < this.noiseTexture.width)
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

       for(int i = 0; i< 25000; i++)
        {
            GameObject cube = Instantiate(this.cubePrefab, this.stage.transform);
            cubePool.Add(cube);
            cube.SetActive(false);
        }
        //float z = 0.0f;
        //while (z < this.noiseTexture.height)
        //{
        //    float x = 0.0f;
        //    while (x < this.noiseTexture.width)
        //    {
        //        float y = this.GetY(pix[(int)z * this.noiseTexture.width + (int)x].r);
        //        while (y > 0)
        //        {
        //            GameObject cube = Instantiate(this.cubePrefab, this.stage.transform);
        //            cube.transform.position = new Vector3(x, y, z);
        //            cubePool.Add(cube);
        //            cube.SetActive(false);
        //            y--;
        //        }
        //        x++;
        //    }
        //    z++;
        //}
    }

    private void MoveBlocks()
    {
        float z = 0.0f;
        int cnt = 0;
        int objcnt = 0;
        while (z < this.noiseTexture.height)
        {
            float x = 0.0f;
            while (x < this.noiseTexture.width)
            {
                float y = this.GetY(pix[(int)z * this.noiseTexture.width + (int)x].r);
                float perlin = y;
                while (y > -2)
                {
                    if(y == perlin)
                    {
                        this.cubePool[cnt].GetComponent<MeshRenderer>().material = grassMaterial;
                    }
                    else
                    {
                        this.cubePool[cnt].GetComponent<MeshRenderer>().material = dirtMaterial;
                    }
                    this.cubePool[cnt].SetActive(true);
                    this.cubePool[cnt].transform.position = new Vector3(x, y, z);
                    cnt++;
                    objcnt++;
                    y--;
                }
                x++;
            }
            z++;
        }
        Debug.Log(objcnt);
    }

    private async Task InstantiateCubesAsync()
    {
        float z = 0.0f;
        while (z < this.noiseTexture.height)
        {
            float x = 0.0f;
            while (x < this.noiseTexture.width)
            {
                float y = this.GetY(pix[(int)z * this.noiseTexture.width + (int)x].r);
                while (y > 0)
                {
                    GameObject cube = Instantiate(this.cubePrefab, this.stage.transform);
                    cube.transform.position = new Vector3(x, y, z); 
                    await Task.Delay(0);
                    y--;
                }
                x++;
            }
            z++;
        } 
    }

    private IEnumerator CoCreateBlocks(Vector3 createPos)
    {
        GameObject cube = Instantiate(this.cubePrefab, this.stage.transform);
        cube.transform.position = createPos;

        yield return null;
    }

    private void DestroyBlocks()
    {
        Transform[] cubes = this.stage.GetComponentsInChildren<Transform>();
        for(int i = 1;i<cubes.Length;i++)
        {
            cubes[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        else if(sample >= 0.9f && sample < 1.0f)
        {
            return 9f;
        }
        else
        {
            return -1f;
        }
    }

}
