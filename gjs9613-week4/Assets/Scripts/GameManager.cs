using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    const string DIR_LOGS = "/Logs";
    const string FILE_HIGH_SCORES = DIR_LOGS + "/highscores.txt";

    string FILE_PATH_HIGH_SCORES;

    public static GameManager instance;
    private float startTime =  0;
    private bool isGame;
    public GameObject target;
    public GameObject player;

    //UI vars
    public Text timerText;
    public Text scoreText;
    public Text finalScoreText;
    public Text highScoreText;
    public GameObject panel;
    
    private int score;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    
    private List<int> highScores;

    // public List<int> HighScores
    // {
    //     get
    //     {
    //         if (File.Exists(FILE_PATH_HIGH_SCORES))
    //         {
    //             string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);
    //             highScores = Int32.Parse(fileContents);
    //         }
    //         return highScores;
    //     }
    //     set
    //     {
    //         highScores = value;
    //     }
    // }

    void Awake()
    {
        if (instance == null) 
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = 0;
        isGame = true;

        FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_HIGH_SCORES;
        
        // timerText = GameObject.Find("TimerText").GetComponent<Text>();
        // scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        // panel = GameObject.Find("GameOverPanel");
    }

    // Update is called once per frame
    void Update()
    {
        startTime += Time.deltaTime;
        int timeLeft = 10 - (int)startTime;
        timerText.text = "TIME REMAINING: " + timeLeft;
        scoreText.text = "SCORE: " + score;

        if (!GameObject.FindWithTag("Target"))
        {
            Instantiate(target, 
                new Vector3(
                    player.transform.position.x + Random.Range(-10,10), 
                    player.transform.position.y + Random.Range(0,10), 
                    player.transform.position.z + Random.Range(-10,10)), 
                    Quaternion.identity
                );
        }

        if (timeLeft <= 0 && isGame)
        {
            panel.SetActive(true);
            // finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
            isGame = false;
            finalScoreText.text = "FINAL SCORE: " + score;
            UpdateHighScores();
        }

        if (!isGame)
        {
            string highScoreString = "";
            for (int i = 0; i < highScores.Count; i++)
            {
                highScoreString += highScores[i] + "\n–*–\n";
            }

            highScoreText.text = "HIGH SCORES\n*******\n" + highScoreString;
        }
    }

    public void Replay()//TODO: fix reference bug that makes replay impossible
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateHighScores()
    {
        // if (!File.Exists(FILE_PATH_HIGH_SCORES))
        // {
        //     File.Create(FILE_PATH_HIGH_SCORES);
        // }
        
        if (highScores == null) //if there arent any high scores, load em from file
        {
            highScores = new List<int>();

            string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);
            string[] fileScores = fileContents.Split(',');

            for (int i = 0; i < fileScores.Length - 1; i++)
            {
                highScores.Add(Int32.Parse(fileScores[i]));
            }
        }
        
        for (int i = 0; i < highScores.Count; i++)
        {
            if (score >= highScores[i])
            {
                highScores.Insert(i, score);
                break;
            }
        }

        string saveHighScoreString = "";

        for (int i = 0; i < highScores.Count; i++)
        {
            saveHighScoreString += highScores[i] + ",";
        }

        File.WriteAllText(FILE_PATH_HIGH_SCORES,saveHighScoreString + "");
    }
}
