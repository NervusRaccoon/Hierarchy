using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCheck : MonoBehaviour
{ 
    private GameObject menuPanel;
    private GameObject winPanel;
    private GameObject gameoverPanel;
    public static GameObject startPanel;
    private GameObject winText;
    private GameObject loseText;
    public static string levelName = "Level1";
    private GameObject level;
    private GameObject imagePref;
    public static int levelCount = 1;
    public static int levelStrength = 1;
    public static int maxStep = 0;
    private List<GameObject> list;
    private List<int> levelColor = new List<int>() {1, 3, 1, 2, 2, 1, 3, 1};
    private bool pause = false;
    private GameObject toMenuButton;
    public Sprite fun;
    void Start()
    {   
        list = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            list.Add(GameObject.Find("Level" + (i+1).ToString()));
            if (i != 0)
            {
                list[i].SetActive(false);
            }
        }
        winPanel = GameObject.FindGameObjectWithTag("WinPanel");
        menuPanel = GameObject.FindGameObjectWithTag("Menu");
        startPanel = GameObject.FindGameObjectWithTag("Play");
        gameoverPanel = GameObject.Find("GameOverPanel");
        toMenuButton = GameObject.Find("ToMenuButton");
        winText = GameObject.Find("WinText");
        loseText = GameObject.Find("LoseText");
        imagePref = GameObject.Find("imagePref");
        gameoverPanel.SetActive(false);
        level = list[levelCount-1];
        maxStep = 10;
        GameObject.Find("StepText").GetComponent<Text>().text = "STEP: " + maxStep.ToString();
        GameObject.Find("LevelText").GetComponent<Text>().text = "LEVEL: " + ImageCheck.levelStrength.ToString() + "-" + ImageCheck.levelCount.ToString();   
        HideElement(null, true);
        menuPanel.SetActive(false);
        ToMenuButton();
    }
    void HideElement(GameObject text, bool panel)
    {
        if (panel)
        {
            loseText.SetActive(false);
            winText.SetActive(false);
            winPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        else
        {
            winPanel.GetComponent<Animation>().Play("winstart");
            menuPanel.SetActive(false);
            winPanel.SetActive(true);
            text.SetActive(true);
        }
    }
    void Update()
    {
        if (!winPanel.activeSelf && !startPanel.activeSelf)
        {
            if (DeletePaper.win && !pause)
            {
                GameObject.Find("imagePref").GetComponent<SpriteRenderer>().sprite = fun;
                pause = true;
                StartCoroutine(Wait());
            }
            if (DeletePaper.lose && !pause)
            {
                pause = true;
                HideElement(loseText, false);
            }
        }
    }

    public void WinButton()
    {
        StartCoroutine(WaitWin());
    }

    public void MusicButton()
    {
        if (GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("MusicButton").GetComponent<Animator>().SetBool("isPlaying", false);
            GetComponent<AudioSource>().Stop();
        }
        else
        {
            GameObject.Find("MusicButton").GetComponent<Animator>().SetBool("isPlaying", true);
            GetComponent<AudioSource>().Play();
        }
    }

    public void StartGameButton()
    {
        StartCoroutine(WaitToGame());
    }

    IEnumerator WaitToGame()
    {
        startPanel.GetComponent<Animation>().Play("finishpanel");
        yield return new WaitForSeconds(0.3f);
        startPanel.SetActive(false);
        toMenuButton.SetActive(true);
        HideElement(null, true);
    }

    public void LoseButton()
    {
        StartCoroutine(WaitLose());
    }

    public void ToMenuButton()
    {
        if (DeletePaper.win)
        {
            WinButton();
        }
        else if (DeletePaper.lose)
        {
            LoseButton();
        }
        startPanel.GetComponent<Animation>().Play("panel");
        menuPanel.SetActive(false);
        StartCoroutine(WaitToMenu());
    }
    IEnumerator WaitToMenu()
    {
        if (DeletePaper.win)
        {
            yield return WaitWin();
        }
        else if (DeletePaper.lose)
        {
            yield return WaitLose();
        }
        startPanel.SetActive(true);
        toMenuButton.SetActive(false);
    }

    public void ReturnButton()
    {
        StartCoroutine(WaitLose());
    }

    IEnumerator WaitLose()
    {
        winPanel.GetComponent<Animation>().Play("winfinish");
        yield return new WaitForSeconds(0.4f);
        DeletePaper.win = false;
        DeletePaper.lose = false;
        DeletePaper.countTouch = 0;
        for (int i = 0; i < level.transform.childCount; i++)
        {
            if (level.transform.GetChild(i).gameObject.name != "imagePref")
            {
                if (level.transform.GetChild(i).gameObject.GetComponent<Animator>().GetBool("delete"))
                {
                    StopCoroutine(DeletePaper.Anim(level.transform.GetChild(i).gameObject));
                }
            }
            level.transform.GetChild(i).gameObject.SetActive(true);
        }
        HideElement(null, true);
        GameObject.Find("StepText").GetComponent<Text>().text = "STEP: " + maxStep.ToString();
        pause = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        HideElement(winText, false);
        if (levelCount == 4 && levelStrength == 1)
        {
            levelStrength += 1;
            levelCount = 1;
        }
        else
        {
            levelCount += 1;
        }
    }
    IEnumerator WaitWin()
    {
        winPanel.GetComponent<Animation>().Play("winfinish");
        yield return new WaitForSeconds(0.4f);
        DeletePaper.win = false;
        DeletePaper.lose = false;
        DeletePaper.countTouch = 0;
        if (levelCount == 5 && levelStrength == 2)
        {
            winPanel.SetActive(false);
            gameoverPanel.SetActive(true);
            Application.Quit();
        }
        else
        {
            HideElement(null, true);
            level.SetActive(false);
            int c = 0;
            if (levelStrength == 2)
            {
                c = 4 + levelCount;
                maxStep = 20 + levelCount;
                if (c == 8)    
                {
                    maxStep = 20 + levelCount - 1;   
                } 
            }
            else
            {
                c = levelCount;
                maxStep = 10 + levelCount - 1;
            }
            for (int i = 0; i < levelColor.Count; i++)
            {
                if (c-1 == i)
                {
                    if (levelColor[i] == 1)
                    {
                        Camera.main.GetComponent<Camera>().backgroundColor = new Color(24f / 255f, 14f / 255f, 46f  / 255f, 0f);
                    }
                    else if (levelColor[i] == 2)
                    {
                        Camera.main.GetComponent<Camera>().backgroundColor = new Color(9f / 255f, 48f / 255f, 38f  / 255f, 0f);
                    }
                    else if (levelColor[i] == 3)
                    {
                        Camera.main.GetComponent<Camera>().backgroundColor = new Color(34f / 255f, 10f / 255f, 13f  / 255f, 0f);
                    }
                }
            }
            levelName = "Level" + c.ToString();
            
            level = list[c-1];
            imagePref = GameObject.Find("imagePref");
            GameObject.Find("StepText").GetComponent<Text>().text = "STEP: " + maxStep.ToString();
            GameObject.Find("LevelText").GetComponent<Text>().text = "LEVEL: " + ImageCheck.levelStrength.ToString() + "-" + ImageCheck.levelCount.ToString();        
            level.SetActive(true);
            pause = false;
        }
    }
}
