using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool isFire;
    public enum ShootStyle//
    {
        mono,
        laser
    }

    // Start is called before the first frame update
    public float shootSpeed = 0.5f;
    public GameObject shootObj;
    public int Delay = 7;
    public ShootStyle ss;//

    private int time;
    private GameObject inst;
    private Transform tr;
    private Transform ptr;
    private Vector3 shootPos;
    private SpriteRenderer sr;
    private GameObject laserObj;

    void Start()
    {
        tr = GetComponent<Transform>();
        ptr = GameObject.Find("Player").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        ss = ShootStyle.laser;
        laserObj = tr.GetChild(5).gameObject;
        isFire = false;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (time == 0)
        {
            if (Input.GetAxis("X") == 1)
            {
                if(ss == ShootStyle.mono)
                {
                    Vector3 t;
                    t.x = ptr.gameObject.GetComponent<Player>().dirX;
                    t.y = ptr.gameObject.GetComponent<Player>().dirY;
                    Vector2 pv;
                    pv.x = ptr.gameObject.GetComponent<Player>().Vx;
                    pv.y = ptr.gameObject.GetComponent<Player>().Vy;
                    float pV = Mathf.Sqrt(Mathf.Pow(pv.x, 2) + Mathf.Pow(pv.y, 2));

                    shootPos = ptr.position;
                    shootPos.x += (t.x) * (sr.bounds.size.x / 2);
                    shootPos.y += (t.y) * (sr.bounds.size.x / 2);
                    shootPos.z = 0;

                    Quaternion n = new Quaternion();
                    inst = Instantiate(shootObj, shootPos, n);
                    inst.GetComponent<bulletMove>().Setbullet(t.x, t.y, pV + shootSpeed, 0);
                }
                else if(ss == ShootStyle.laser)
                {
                    isFire = true;
                }
            }
            time = Delay;

            laserObj.SetActive(isFire);
            isFire = false;
        }
        else
        {
            time--;
        }
    }
}
