using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    //rotate , boster, shoot
    const float PI = 3.141592f;

    private float deltaX = 0;
    private float deltaY = 0;

    private Transform tr;
    private Transform mctr;
    private Animator boostAnimator;
    private Transform Flighttr1;
    private Gravity g;
    private SpriteRenderer sr;
    

    public float dirX = 0;
    public float dirY = 0;
    public float Vx = 0;
    public float Vy = 0;

    public float decreaseSpeed = 0.1f;

    private float angleadd = 0;

    public float boostSpeed = 0.25f;
    public float LimitSpeed = 1.2f;

    private bool bBoosted = false;
    bool bBoostLast = false;

    float FlightYScale = 6;

    void Start()
    {
        tr = GetComponent<Transform>();
        mctr = GameObject.Find("Main Camera").GetComponent<Transform>();
        boostAnimator = GameObject.Find("BoostImage").GetComponent<Animator>();
        Flighttr1 = GameObject.Find("Flight1").GetComponent<Transform>();
        g = GetComponent<Gravity>();
        tr.gameObject.layer = 9;
    }

    // Update is called once per frame
    void Update()
    {
        float rotateVariable = -Input.GetAxis("Ho") * 4;
        if (bBoosted)
        {
            rotateVariable = -Input.GetAxis("Ho") * 2;
            bBoosted = false;
        }
        
        
        //tr.Rotate(v, rotateVariable);
        angleadd += rotateVariable;
        
        dirX = Mathf.Cos((angleadd / 180)*PI);
        dirY = Mathf.Sin((angleadd / 180)*PI);

        //Vector3 vf = new Vector3(1, dirX, 1);
        //Flighttr0.localScale.Scale(vf);
        //Flighttr1.localScale.Scale(vf);
        Flighttr1.localScale = new Vector3(0.5f, -FlightYScale * dirY, 1);
        GameObject.Find("flightmask").GetComponent<Transform>().localScale = Flighttr1.localScale;

        float Length = Mathf.Sqrt(Mathf.Pow(Vx, 2) + Mathf.Pow(Vy, 2));

        if(Length != 0)
        {
            float dvx = decreaseSpeed * Vx / Length;
            float dvy = decreaseSpeed * Vy / Length;
            Vx -= dvx;
            Vy -= dvy;
        }
        
        if (Input.GetAxis("X") == 1)
        {

        }

        if(Input.GetAxis("Boost") >0)
        {
            boostAnimator.SetInteger("StateNum", 0);
            int t = boostAnimator.GetInteger("time") + 1;
            boostAnimator.SetInteger("time", t);
            bBoosted = true;
            Vx += dirX * boostSpeed;
            Vy += dirY * boostSpeed;

            if (g.downSpeed > 0)
            {
                g.downSpeed -= 3 * g.gravity * dirY;
            }
            else g.downSpeed = 0;
            
        }
        else
        {
            boostAnimator.SetInteger("StateNum", 1);
        }

        Length = Mathf.Sqrt(Mathf.Pow(Vx, 2) + Mathf.Pow(Vy, 2));
        if(LimitSpeed < Length)
        {
            Vx = LimitSpeed * Vx / Length;
            Vy = LimitSpeed * Vy / Length;
        }

        Vector3 v = new Vector3();
        v.Set(0, 0, 1);
        tr.Rotate(v, rotateVariable);
        Quaternion q = tr.rotation;
        Vector3 pos = new Vector3();
        pos.Set(tr.position.x + Vx, tr.position.y + Vy, 0);
        tr.SetPositionAndRotation(pos, q);

        Vector3 CV = new Vector3();
        CV.Set((tr.position.x + mctr.position.x) / 2, (tr.position.y + mctr.position.y) / 2, -50);
        mctr.SetPositionAndRotation(CV, mctr.rotation);
    }
}
