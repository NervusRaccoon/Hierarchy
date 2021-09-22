using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject paperPref;
    void Start()
    {
        GameObject level = GameObject.Find("Level");
        Color color = new Color(Random.Range(0.8f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 1f);
        int count = 24;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(0, 0, 0);//Random.insideUnitSphere * 0.65f;
            GameObject go = Instantiate(paperPref, pos, Quaternion.identity, level.transform);
            float paperX = Mathf.Abs(go.GetComponent<BoxCollider2D>().bounds.min.x);
            float paperY = Mathf.Abs(go.GetComponent<BoxCollider2D>().bounds.min.y);
            BoxCollider2D col = level.GetComponent<BoxCollider2D>();
            if (Random.Range(0, 9) < 6)
            {
                go.transform.localScale = new Vector3(Random.Range(150, 200), Random.Range(150, 200), 0);
                pos = new Vector3(Random.Range(col.bounds.min.x + paperX, col.bounds.max.x - paperX), 
                                Random.Range(col.bounds.min.y + paperY, col.bounds.max.y - paperY), 0);
            }
            else
            {
                go.transform.localScale = new Vector3(Random.Range(200, 300), Random.Range(200, 500), 0);
                pos = new Vector3(Random.Range(col.bounds.min.x/2 + paperX, col.bounds.max.x/2 - paperX), 
                                Random.Range(col.bounds.min.y/4 + paperY, col.bounds.max.y/4 - paperY), 0);
            }
            go.transform.position = pos;                                        
            go.GetComponent<SpriteRenderer>().color = new Color(color.r*Random.Range(0f, 0.5f), color.g*Random.Range(0f, 0.5f), color.b*Random.Range(0.5f, 1f), 1f); 
            go.GetComponent<SpriteRenderer>().sortingOrder = i;
        }        
    }
}
