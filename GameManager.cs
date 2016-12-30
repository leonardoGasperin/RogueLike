using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2.0f;
    public float turnDelay = .1f;
    public static GameManager instance = null;//to can be acessed by others scripts on game
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //add one to our lvl number
        level++;
        //call Initgame to initialize our level.
        InitGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        if (enabled == false)
        {
            enabled = true;
            level = 0;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;//we play here
        else if (instance != this)
            Destroy(gameObject);//destroy the map(or instance) duplicated

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();    
        boardScript = GetComponent<BoardManager>();
        //InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day" + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        enemies.Clear();//reset enemies to next lvl
        boardScript.SetupLevel(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    
    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved" + '\n' + '\n' + "Press any key to restart.";
        levelImage.SetActive(true);
        enabled = false;
        doingSetup = true;
    }

    // Update is called once per frame
    void Update()
    {
            if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
