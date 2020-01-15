using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    public enum BulletType
    {
        bt_dot,
        bt_laser,
        bt_Bom
    }

    private BulletType bulletType = BulletType.bt_dot;
    private Vector2 bulletDir = new Vector2(0, 0);
    private float bulletSpeed = 3;
    private Transform ptr;
    // Start is called before the first frame update
    private Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
        ptr = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(bulletSpeed * bulletDir.x, bulletSpeed * bulletDir.y, 0);

        ptr = GameObject.Find("Player").GetComponent<Transform>();
        float l = Mathf.Sqrt(Mathf.Pow(ptr.position.x - tr.position.x, 2) + Mathf.Pow(ptr.position.y - tr.position.y, 2));
        if (l > 1000)
        {
            Destroy(tr.gameObject);
        }
    }

    public void Setbullet(float dx, float dy, float speed, BulletType bt)
    {
        bulletType = bt;
        bulletDir.x = dx;
        bulletDir.y = dy;
        bulletSpeed = speed;
    }
}
