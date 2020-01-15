using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    // Start is called before the first frame update
    public float depth;
    private float de;
    private float movingSpeed = 0;
    private float xscale = 50;
    private float yscale = 50;
    public Vector2 realLocation;
    public bool visible = false; // 처음 생성했을 때 false지만, CloudManage에서 SetCloud로 true로 바꾸고 나서 구름이 생긴다.

    private Transform tr;
    private Transform ctr;
    Camera c;
    private SpriteRenderer sr;
    private Animator a;

    void Start()
    {
        c = GameObject.Find("Main Camera").GetComponent<Camera>();
        tr = GetComponent<Transform>();
        ctr = GameObject.Find("Main Camera").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        CloudManage cm = GameObject.Find("BackGround").GetComponent<CloudManage>();
        int n = Random.Range(0, 17);
        a = GetComponent<Animator>();
        a.SetInteger("CloudNum", n);
    }

    // Update is called once per frame
    void Update()
    {
        depth = de;
        if (visible)
        {
            Vector2 CameraCenter = ctr.position;
            float depthnum = Mathf.Pow(2, depth);
            tr.localScale = new Vector3(xscale * depthnum,yscale * depthnum, 0);
            realLocation.x += movingSpeed;
            tr.SetPositionAndRotation(new Vector3((realLocation.x - CameraCenter.x) * depthnum, (realLocation.y - CameraCenter.y) * depthnum, -depth),
                tr.rotation);
        }
    }

    public void SetCloud(float d, float movSpeed, float sizx, float sizy, Vector2 RealPos)
    {
        de = d;
        depth = d;
        movingSpeed = movSpeed;
        xscale = sizx;
        yscale = sizy;
        realLocation = RealPos;
        visible = true;
    }
}
