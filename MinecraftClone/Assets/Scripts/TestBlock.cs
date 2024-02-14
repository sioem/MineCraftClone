using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlock : MonoBehaviour
{
    [SerializeField]
    private Material outline;
    [SerializeField]
    private Material destroy;
    [SerializeField]
    private List<Texture2D> listDestroyStage = new List<Texture2D>();

    private MeshRenderer renderers;
    private List<Material> materialList = new List<Material>();
    private bool isSelected = false;

    private float destroyTime = 1f;
    private float time;

    public void DestroyBlock()
    {
        this.InitBlock();
        Destroy(this.gameObject);
    }

    public void HitBlock()
    {
        this.time += Time.deltaTime;
        float progress = this.time * 100 / this.destroyTime;

        Texture2D blockStage = this.GetBlockStage(progress);
        if (blockStage != null)
        {
            this.destroy.SetTexture("_MainTex", blockStage);
        }

        if (time >= this.destroyTime)
        {
            this.DestroyBlock();
        }
    }

    private Texture2D GetBlockStage(float progress)
    {
        int index = (int)progress / 10;
        if (index <= 10)
        {
            return this.listDestroyStage[index];
        }
        else
        {
            return null;
        }
    }

    public void SelectBlock()
    {
        if (!isSelected)
        {
            renderers = this.GetComponent<MeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderers.sharedMaterials);
            materialList.Add(outline);
            materialList.Add(destroy);

            renderers.materials = materialList.ToArray();
            this.isSelected = true;
        }
    }

    public void UnselectBlock()
    {
        if (isSelected)
        {
            this.InitBlock();
            MeshRenderer renderer = this.GetComponent<MeshRenderer>();

            materialList.Clear();
            materialList.AddRange(renderer.sharedMaterials);
            materialList.Remove(outline);
            materialList.Remove(destroy);

            renderer.materials = materialList.ToArray();
            this.isSelected = false;
        }
    }

    public void InitBlock()
    {
        this.time = 0f;
        this.destroy.SetTexture("_MainTex", this.listDestroyStage[0]);
    }
}
