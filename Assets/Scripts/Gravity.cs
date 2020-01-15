using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    // Start is called before the first frame update
    public float gravity = 0.1f;
    public float downSpeed = 0.5f;
    public float LimitSpeed = 5.0f;
    private float deltaX = 0;
    private float deltaY = 0;
    private float pvx = 0;
    private float pvy = 0;

    private Transform tr;
    private Player p;
    void Start()
    {
        tr = GetComponent<Transform>();
        p = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (downSpeed < LimitSpeed)
            downSpeed += gravity;
        else downSpeed = LimitSpeed;

        Vector3 v3 = new Vector3(tr.position.x, tr.position.y-downSpeed, 0);
        tr.SetPositionAndRotation(v3, tr.rotation);
    }
}
