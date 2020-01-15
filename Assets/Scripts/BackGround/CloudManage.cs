using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManage : MonoBehaviour
{
    //구름크기 : 80 ~ 120
    //구름움직임 : -10 ~ 10
    //구름깊이 : -1 ~ 1 ||  1 : 0~1 || 0 : 0~-1
    // Start is called before the first frame update
    ArrayList[] CloudArray;
    public int CloudNum; // 구름의 개수 - 0 기준
    Camera c;
    public GameObject CloudObj;
    public uint CloudNumSize = 0; // 총 구름 개수
    public Rect MapRect;
    public Vector2 MapPos; // MapRect를 기준으로 한 좌표

    private GameObject player;

    ArrayList[] MapList; // 할당되어야 하는 공간
    ArrayList[] CheckList; // 할당된 공간 - 이둘은 Vector3으로 저장 (X, Y, delta);

    bool bRectInPoint(Rect rt, Vector2 pos)
    {
        if (rt.xMin <= pos.x && pos.x <= rt.xMax &&
            rt.yMin <= pos.y && pos.y <= rt.yMax)
            return true;
        else return false;
    }

    //realLocation Parameter
    void AddCloud(Rect cloudrt, int cloudNum, int idepth)
    {
        //구름이 몇개가 필요한지 needAdd에 저장됨
        for (int k = 0; k < cloudNum; k++)
        {
            float ins_depth = (float)(idepth-1) + ((float)Random.Range(0, 100) / (float)100);
            Rect rt = cloudrt;
            GameObject cloud = Instantiate(CloudObj, new Vector3(), new Quaternion());
            cloud.GetComponent<Cloud>().SetCloud(ins_depth, Random.Range(-0.1f, 0.1f), Random.Range(90, 110), Random.Range(90, 110),
                new Vector3(Random.Range(rt.xMin, rt.xMax), Random.Range(rt.yMin, rt.yMax), -ins_depth));
            cloud.GetComponent<SpriteRenderer>().sortingLayerName = GetLayerFromDepth(ins_depth);
            cloud.GetComponent<SpriteRenderer>().sortingOrder = (int)(ins_depth * 100);
            cloud.transform.parent = this.gameObject.transform;
            CloudArray[idepth].Add(cloud);
            CloudNumSize++;
        }
    }

    void DestroyCloud(Rect cloudrt, int idepth)
    {
    }

    void StartCloud()
    {
        for(int i = 0; i < 2; i++)
        {
            Rect rt = GetMappingRect(0, 0);
            AddCloud(rt, CloudNum, i);
        }
    }

    string GetLayerFromDepth(float depth) //depth는 -4 ~ 1 기준값 
    {
        if(depth >= -4 && depth < -2)
        {
            return "Cloud0";
        }
        else if(depth >= -2 && depth < 0)
        {
            return "Cloud1";
        }
        else if (depth >= 0 && depth < 1)
        {
            return "Front";
        }
        else return "Player";
    }

    void UpdateCloud()
    {
        for(int d = 0; d < 2; d++)
        {
            int x1, x2;
            int y1, y2;
            Rect prt = GetPlayerShowRect(player.transform.position.x, player.transform.position.y);
            prt = new Rect(prt.center.x - (d + 1) * prt.width / 2, prt.center.y - (d + 1) * prt.height / 2,
                (d + 1) * prt.width, (d + 1) * prt.height);

            for(int i = 0; i < CloudArray[d].Count; i++)
            {
                Vector2 v2 = ((GameObject)CloudArray[d][i]).GetComponent<Cloud>().realLocation;
                if (!bRectInPoint(prt, v2))
                {
                    GameObject o;
                    o = (GameObject)CloudArray[d][i];
                    Destroy(o);
                    CloudArray[d].RemoveAt(i);
                    CloudNumSize--;
                }
            }

            x1 = (int)((prt.xMin - MapRect.xMin) / MapRect.width);
            x2 = (int)((prt.xMax - MapRect.xMin) / MapRect.width + 1);
            y1 = (int)((prt.yMin - MapRect.yMin) / MapRect.height);
            y2 = (int)((prt.yMax - MapRect.yMin) / MapRect.height + 1);

            for (int i = x1; i < x2; i++)
            {
                for (int k = y1; k < y2; k++)
                {
                    MapList[d].Add(new Vector2Int(i, k));
                    bool bgotocheck = true;
                    for(int m = 0; m < CheckList[d].Count; m++)
                    {
                        if(new Vector2Int(i, k).Equals((Vector2Int)CheckList[d][m]))
                        {
                            bgotocheck = false;
                            Debug.Log(bgotocheck);
                        }
                    }

                    if (bgotocheck)
                    {
                        CheckList[d].Add(new Vector2Int(i, k));

                        Rect rt = GetMappingRect(i, k);
                        AddCloud(rt, CloudNum, d);
                    }
                }
            }

            for(int i = 0; i < CheckList[d].Count; i++)
            {
                for(int k = 0; k < MapList[d].Count; k++)
                {
                    bool bgotoremov = true;
                    if (((Vector2Int)MapList[d][k]).Equals((Vector2Int)CheckList[d][i]))
                    {
                        bgotoremov = false;
                    }

                    if (bgotoremov)
                    {
                        CheckList[d].RemoveAt(i);
                    }
                }
            }
            //항상 마지막
            MapList[d].Clear();
        }
        
    }

    Rect GetMappingRect(int x, int y)
    {
        return new Rect(MapRect.xMin + x * MapRect.width, MapRect.yMin + y * MapRect.height, MapRect.width, MapRect.height);
    }

    Rect GetPlayerShowRect(float x, float y)
    {
        return new Rect(x-MapRect.width/2, y-MapRect.height/2, x + MapRect.width / 2, y + MapRect.height / 2);
    }

    void Start()
    {
        c = GameObject.Find("Main Camera").GetComponent<Camera>();
        CloudArray = new ArrayList[2];
        CloudArray[0] = new ArrayList();
        CloudArray[1] = new ArrayList();
        player = GameObject.Find("Player");
        MapList = new ArrayList[2];
        MapList[0] = new ArrayList();
        MapList[1] = new ArrayList();
        CheckList = new ArrayList[2];
        CheckList[0] = new ArrayList();
        CheckList[1] = new ArrayList();
        StartCloud();
    }

    // Update is called once per frame
    void Update()
    {
        MapPos.x = (player.transform.position.x - MapRect.xMin) / MapRect.width;
        MapPos.y = (player.transform.position.y - MapRect.yMin) / MapRect.height;
        UpdateCloud();
    }
}
