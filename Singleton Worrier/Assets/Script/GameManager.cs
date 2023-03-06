using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // FILE RELEVANCE
    
    private const string DIR_DATA = "/Text/";
    private const string FILE_LEVEL = "Level";
    private const string FILE_FILETYPE_TXT = ".txt";
    private const string FILE_STORY_START = "Story_Start";
    private const string FILE_STORY_END = "Story_End";

    private string PATH_STORY_START;
    private string PATH_STORY_END;
    
    // LEVEL RELEVANCE
    
    public static GameManager GM;
    [SerializeField] public static int currentLevel = 1;
    public static int gameObjectArray;
    private const int highestLevel = 7;             // 总共关卡数：8
    private static bool isLoadingLevel = false;
    public static bool isMovable = true;          // 是否可操作
    public GameObject levelInstanceHolder;

    public GameObject wall;
    public GameObject player;
    public GameObject exit;
    public GameObject x;

    public float xOffset;
    public float yOffset;
    
    // UI RELEVANCE
    
    public GameObject blackScreen;
    public GameObject txt;

    
    
    
    private void Awake()
    {
        
        if (GM == null)                              // Singleton
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        isLoadingLevel = false;             // 加载关卡锁解开
        Time.timeScale = 1;                // 
        

    }


    
    void Start()
    {
        if (currentLevel == 1)
        {
            StartCoroutine(StoryStart());

        }

        // 第一关：加载叙事，出现level number
        // 其他关：出现 Level number
    }


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadLevelAccordingToTxt();
            
        }
        

        
        if (gameObjectArray <= 1)
        {
            
            if ((currentLevel < highestLevel)&(!isLoadingLevel))            // 不是最后一关 且 没在加载关卡
            {
                isLoadingLevel = true;
                
                currentLevel++;
                LoadLevelAccordingToTxt();
            }

            if ((currentLevel == highestLevel)&(!isLoadingLevel))        // 最后一关
            {
                // 游戏胜利
                isLoadingLevel = true;
                StartCoroutine(StoryEnd());

            }
        }
        
        
    }




    public void LoadLevelAccordingToTxt()
    {

        int levelToLoad = currentLevel;
        string PATH_FILE_LEVELINDEX = Application.dataPath + DIR_DATA + FILE_LEVEL + levelToLoad + FILE_FILETYPE_TXT;

        if (File.Exists(PATH_FILE_LEVELINDEX))
        {
            
            // 清空当前关卡
            
            if (levelInstanceHolder == null) { levelInstanceHolder = GameObject.Find("levelInstanceHolder");}
            Destroy(levelInstanceHolder);
            levelInstanceHolder = new GameObject("levelInstanceHolder");
            levelInstanceHolder.transform.position = new Vector3(0, 0, 0);

            gameObjectArray = 0;


            // 从txt读取并放置

            string[] levelLine = File.ReadAllLines(PATH_FILE_LEVELINDEX);
            GameObject Agent = null;

            for (int yPos = 0; yPos < levelLine.Length; yPos++)
            {
                for (int xPos = 0; xPos < levelLine[yPos].Length; xPos++)
                {

                    switch (levelLine[yPos][xPos])
                    {
                        case 'W':
                        {
                            Agent = Instantiate<GameObject>(wall);
                            break;
                        }
                        case 'P':
                        {
                            Agent = Instantiate<GameObject>(player);
                            gameObjectArray++;
                            break;
                        }
                        case 'E':
                        {
                            Agent = Instantiate<GameObject>(exit);
                            break;
                        }
                        case 'X':
                        {
                            Agent = Instantiate<GameObject>(x);
                            break;
                        }
                        default: 
                            break;
                    }

                    if (Agent != null)
                    {
                        Agent.transform.position = 
                            new Vector2
                            (
                                xOffset + xPos, 
                                yOffset - yPos
                            );

                        Agent.transform.parent = levelInstanceHolder.transform;
                    }
                    
                }
                
            }
            
        }
        

    }
    
    
    

    IEnumerator StoryStart()
    {
        isMovable = false;
        
        PATH_STORY_START = Application.dataPath + DIR_DATA + FILE_STORY_START + FILE_FILETYPE_TXT;
        
        blackScreen = GameObject.Find("Black");
        txt = GameObject.Find("TXT");

        
        // file is here. file I/O

        txt.GetComponent<TMP_Text>().text = "";
        txt.GetComponent<TMP_Text>().color = new Color(1,1,1,0);
        txt.GetComponent<TMP_Text>().text = File.ReadAllText(PATH_STORY_START);
        
        
        // black screen is here
        
        if (!blackScreen.activeSelf)
        {
            blackScreen.SetActive(true);
        }
        
        blackScreen.GetComponent<RawImage>().color = Color.black;

        
        // text fade in
        
        float textFadeSpeed = 0.05f;
        float textFadeTime = 0.04f;

        while (true)
        {
            if (txt.GetComponent<TMP_Text>().color.a >= 1)  { break; }

            txt.GetComponent<TMP_Text>().color = new Color(1,1,1,txt.GetComponent<TMP_Text>().color.a + textFadeSpeed);

            yield return new WaitForSeconds(textFadeTime);
            
        }
        
        
        // 加载关卡
        LoadLevelAccordingToTxt();         
        
        
        // last 10 secs

        yield return new WaitForSeconds(11.53f);
        
        
        // text fade out
        // black screen gone

        blackScreen.SetActive(false);
        txt.SetActive(false);

        isMovable = true;
        
    }

    
    
    IEnumerator StoryEnd()
    {
        isMovable = false;
        GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
        
        PATH_STORY_END = Application.dataPath + DIR_DATA + FILE_STORY_END + FILE_FILETYPE_TXT;
        
        blackScreen = GameObject.Find("Black");
        txt = GameObject.Find("TXT");

        if(!blackScreen.activeSelf) {blackScreen.SetActive(true);}
        blackScreen.GetComponent<RawImage>().color = Color.clear;
        
        if(!txt.activeSelf){txt.SetActive(true);}
        txt.GetComponent<TMP_Text>().text = "";
        txt.GetComponent<TMP_Text>().color = new Color(1,1,1,0);
        txt.GetComponent<TMP_Text>().text = File.ReadAllText(PATH_STORY_END);
        
        
        // black fade in
        
        float blackFadeSpeed = 0.1f;
        float blackFadeTime = 0.1f;

        while (true)
        {
            if (blackScreen.GetComponent<RawImage>().color.a >= 1)
            { Debug.Log("break"); break; }

            blackScreen.GetComponent<RawImage>().color = new Color(0,0,0,blackScreen.GetComponent<RawImage>().color.a + blackFadeSpeed);
            Debug.Log("waht");

            yield return new WaitForSeconds(blackFadeTime);
            
        }
        
        // last 2 secs

        yield return new WaitForSeconds(2f);

        // text fade in 

        
        
        float textFadeSpeed = 0.05f;
        float textFadeTime = 0.04f;

        while (true)
        {
            if (txt.GetComponent<TMP_Text>().color.a >= 1)
            { Debug.Log("break"); break; }

            txt.GetComponent<TMP_Text>().color = new Color(1,1,1,txt.GetComponent<TMP_Text>().color.a + textFadeSpeed);
            Debug.Log("waht");

            yield return new WaitForSeconds(textFadeTime);
            
        }
        
    }
    
    
    
    
    
    
}
