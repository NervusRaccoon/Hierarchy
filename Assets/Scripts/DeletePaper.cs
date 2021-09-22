using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePaper : MonoBehaviour
{
    private string layerToCheck = "Default";
    private List<GameObject> paper = new List<GameObject>();
    public static bool win = false;
    public static bool lose = false;
    private GameObject img;
    private GameObject level;
    public Text stepText;
    public Text levelText;
    public static int countTouch = 0;

    void Start()
    {
        levelText.text = "LEVEL: " + ImageCheck.levelStrength.ToString() + "-" + ImageCheck.levelCount.ToString();
    }
    void Update()
    {
        if ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Ended) && !ImageCheck.startPanel.activeSelf)
        {
            if (!win && !lose)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);
                GameObject result = GetHighestRaycastTarget(touchPos2D);
                if (result == null)
                {
                    lose = true;
                }
                else
                {
                    countTouch += 1;
                    if (result.name == "imagePref")
                    {
                        win = true;
                        return; 
                    }
                    StartCoroutine(Anim(result));
                    if (countTouch > ImageCheck.maxStep)
                    {
                        lose = true;
                        stepText.text = "STEP: 0";
                        return; 
                    }
                    stepText.text = "STEP: " + (ImageCheck.maxStep - countTouch).ToString();
                    return;
                }
            }
        }
    }

    public static IEnumerator Anim(GameObject result)
    {
        if (result != null)
        {
            result.GetComponent<Animator>().SetBool("delete", true);
            yield return new WaitForSeconds(1.4f);
            result.SetActive(false);
            result.GetComponent<Animator>().SetBool("delete", false);
        }
    }
    private GameObject GetHighestRaycastTarget(Vector2 touchPos)
    {
        GameObject topLayer = null;
        RaycastHit2D[] hit = Physics2D.RaycastAll(touchPos, Vector2.zero);

        foreach (RaycastHit2D ray in hit)
        {
            SpriteRenderer spriteRenderer = ray.transform.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null
                && spriteRenderer.sortingLayerName == layerToCheck 
                && ((ray.transform.name != "imagePref" && !ray.transform.GetComponent<Animator>().GetBool("delete")) || ray.transform.name == "imagePref"))
            {
                if (topLayer == null)
                {
                    topLayer = spriteRenderer.transform.gameObject;
                }

                if (spriteRenderer.sortingOrder >= topLayer.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    topLayer = ray.transform.gameObject;
                }
            }
        }

        level = GameObject.Find(ImageCheck.levelName);
        img = GameObject.Find("imagePref");
        bool top = true;
        for (int i = 0; i < level.transform.childCount; i++)
        {
            GameObject child = level.transform.GetChild(i).gameObject;
            if (topLayer.GetComponent<Collider2D>().IsTouching(child.GetComponent<Collider2D>()))
            {
                if (child.transform.GetComponent<SpriteRenderer>().sortingOrder > topLayer.transform.GetComponent<SpriteRenderer>().sortingOrder 
                                                && child.activeInHierarchy == true && (!child.GetComponent<Animator>().GetBool("delete") && child.name != "imagePref"))
                {
                    top = false;
                }
            }
        }
        if (top == true)
        {
            return topLayer;
        }
        else
        {
            return null;
        }
    } 
}
