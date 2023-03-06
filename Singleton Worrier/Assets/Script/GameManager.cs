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
    public static GameManager GM;
    public static int currentLevel = 0;
    public static int gameObjectArray;
    private const int highestLevel = 7;             // 总共关卡数：8
    private static bool isLoadingLevel = false;
    public static bool isMovable = true;          // 是否可操作

    public GameObject blackScreen;
    public GameObject txt;

    private const string DIR_DATA = "/Data/";
    private const string FILE_STORY_START = "Story_Start.txt";
    private const string FILE_STORY_END = "Story_End.txt";

    private string PATH_STORY_START;
    private string PATH_STORY_END;
    
    

    private void Awake()
    {
        
        if (GM == null)
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

        currentLevel = SceneManager.GetActiveScene().buildIndex;        //让 currentlevel 值等于当前场景编号
        
        gameObjectArray = GameObject.FindGameObjectsWithTag("Player").Length;        // initialize gameobject array
        Debug.Log("at awake, gameobjectarray:"+gameObjectArray);
        
        Debug.Log("level"+currentLevel+"awaked" );
        

        

        
        

    }


    
    void Start()
    {
        if (currentLevel == 0)
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
            SceneManager.LoadScene(GameManager.currentLevel, LoadSceneMode.Single);
            
        }
        

        
        if (gameObjectArray <= 1)
        {
            
            if ((currentLevel < highestLevel)&(!isLoadingLevel))            // 不是最后一关 且 没在加载关卡
            {
                isLoadingLevel = true;
                Debug.Log("is loading level:"+ isLoadingLevel);
                currentLevel++;
                SceneManager.LoadScene(GameManager.currentLevel, LoadSceneMode.Single);
            }

            if ((currentLevel == highestLevel)&(!isLoadingLevel))        // 最后一关
            {
                // 游戏胜利
                isLoadingLevel = true;
                StartCoroutine(StoryEnd());
                
                Debug.Log("shit" );
            }
        }
        
        
    }


    IEnumerator StoryStart()
    {
        isMovable = false;
        
        PATH_STORY_START = Application.dataPath + DIR_DATA + FILE_STORY_START;
        
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
            if (txt.GetComponent<TMP_Text>().color.a >= 1)
            { Debug.Log("break"); break; }

            txt.GetComponent<TMP_Text>().color = new Color(1,1,1,txt.GetComponent<TMP_Text>().color.a + textFadeSpeed);
            Debug.Log("waht");

            yield return new WaitForSeconds(textFadeTime);
            
        }
        
        
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
        
        PATH_STORY_END = Application.dataPath + DIR_DATA + FILE_STORY_END;
        
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
