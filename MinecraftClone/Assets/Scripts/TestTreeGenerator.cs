using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTreeGenerator : MonoBehaviour
{
    [SerializeField]
    private Button btnCreate;
    [SerializeField]
    private GameObject cubePrefab;

    public int width;
    public int height;
    public int distance;

    // Start is called before the first frame update
    void Start()
    {
        this.btnCreate.onClick.AddListener(() =>
        {
            this.CreateTree();
        });
    }

    private void CreateTree()
    {
        for(int i = 0; i < 20; i++)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, height);

            if (!this.IsTreeNearby(new Vector3(x,0,z)))
            {
                GameObject cubeGo = Instantiate(cubePrefab);
                cubeGo.transform.position = new Vector3(x, 0, z);
            }
        }
    }

    private bool IsTreeNearby(Vector3 pos)
    {
        Collider[] col = Physics.OverlapSphere(pos, this.distance);
        foreach(Collider c in col)
        {
            if (c.CompareTag("Tree"))
            {
                return true;
            }
        }
        return false;
    }
}
