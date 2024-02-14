using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private Camera playerCam;

    [SerializeField]
    private GameObject playerGo;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float minDistance;
    
    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private GameObject hotbarSelection;
    [SerializeField]
    private List<Material> listBlockMaterial = new List<Material>();
    [SerializeField]
    private List<GameObject> listHotbarItems = new List<GameObject>();

    private bool isGround = true;
    private Material currentMaterial;
    private Rigidbody rBody;
    private float xRotate, yRotate;
    private GameObject prevBlock = null;
    
    // Start is called before the first frame update
    void Start()
    {
        this.rBody = GetComponent<Rigidbody>();
        this.currentMaterial = this.listBlockMaterial[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.ChangeCam();
        }
        
        this.Rotate();
        this.Move();       
        this.Jump();
        this.FindBlock();
        this.ChangeHotbar();
    }

    private void ChangeHotbar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[0].transform.position;
            this.currentMaterial = this.listBlockMaterial[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[1].transform.position;
            this.currentMaterial = this.listBlockMaterial[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[2].transform.position;
            this.currentMaterial = this.listBlockMaterial[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[3].transform.position;
            this.currentMaterial = this.listBlockMaterial[3];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[4].transform.position;
            this.currentMaterial = this.listBlockMaterial[4];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[5].transform.position;
            this.currentMaterial = this.listBlockMaterial[5];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[6].transform.position;
            this.currentMaterial = this.listBlockMaterial[6];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[7].transform.position;
            this.currentMaterial = this.listBlockMaterial[7];

        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            this.hotbarSelection.transform.position = this.listHotbarItems[8].transform.position;
            this.currentMaterial = this.listBlockMaterial[8];
        }
    }

    private void FindBlock()
    {        
        Ray ray = new Ray(this.playerCam.transform.position, this.playerCam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Block"))
            {
                if (prevBlock != null && prevBlock != hit.collider.gameObject)
                {
                    prevBlock.GetComponent<TestBlock>().UnselectBlock();
                }
                hit.collider.GetComponent<TestBlock>().SelectBlock();
                prevBlock = hit.collider.gameObject;

                if (Input.GetMouseButton(0))
                {
                    hit.collider.GetComponent<TestBlock>().HitBlock();
                }
                else 
                { 
                    hit.collider.GetComponent<TestBlock>().InitBlock();
                }

                if (Input.GetMouseButtonDown(1) && Vector3.Distance(this.transform.position,hit.point) > 0.3f)
                {
                    this.PutBlock(hit.collider.transform.position, hit.normal);
                }
            }
        }
        else
        {
            if (prevBlock != null)
            {
                prevBlock.GetComponent<TestBlock>().UnselectBlock();
            }
        }
    }

    private void PutBlock(Vector3 pos, Vector3 normal)
    {
        Vector3 blockPos = pos + normal;
        GameObject blockGo = Instantiate(this.blockPrefab);
        blockGo.transform.position = blockPos;
        blockGo.GetComponent<MeshRenderer>().material = this.currentMaterial;
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        Vector3 offset = new Vector3(0, 0.1f, 0);

        Quaternion rotation = transform.rotation;
        Vector3 rayDir = rotation * dir;

        Ray ray = new Ray(transform.position + offset, rayDir.normalized + offset);
        RaycastHit hit;

        if (dir != Vector3.zero && !Physics.Raycast(ray, out hit, 1f))
        {        
            this.transform.Translate(dir.normalized * Time.deltaTime * this.moveSpeed);
        }
    }

    private void Rotate()
    {
        float r = Input.GetAxisRaw("Mouse X");
        float u = Input.GetAxisRaw("Mouse Y");
        
        this.xRotate += turnSpeed * r;             
        this.yRotate += turnSpeed * -u;
        
        this.yRotate = Mathf.Clamp(this.yRotate, -90, 90);     
        this.transform.eulerAngles = new Vector3(0, xRotate, 0);
        this.playerCam.transform.eulerAngles = new Vector3(yRotate, xRotate, 0);
    }

    private void Jump()
    {
        if (this.isGround && Input.GetKeyDown(KeyCode.Space))
        {
            this.isGround = false;
            this.rBody.AddForce(Vector3.up * this.jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.isGround = true;
    }


    private void ChangeCam()
    {
        if (this.mainCam.enabled)
        {
            this.mainCam.enabled = false;
            this.playerCam.enabled = true;
        }
        else
        {
            this.playerCam.enabled = false;
            this.mainCam.enabled = true;
        }
    }
}
