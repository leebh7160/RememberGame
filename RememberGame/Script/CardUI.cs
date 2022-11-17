using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private GameManager gameManager;
    public GameManager GameManager
    {
        set
        {
            gameManager = value;
        }
    }
    [SerializeField]
    private TextMeshProUGUI TimeText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private Button RestartButton;
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Image BlackGround;

    private int score = 0;
    private float time = 30;
    private bool isGameStart = false;

    private void Update()
    {
        if (time > 0 && isGameStart == true)
        {
            time -= Time.deltaTime;
            TimeText.text = "TIME : " + time.ToString("F0");
        }
        else if (time <= 0)
            EndGame();
    }
    public void ScorePlus()
    {
        score += 1;
        ScoreText.text = "SCORE : " + score.ToString();
    }

    public void Event_GameStartButtonClick()
    {
        TimeText.gameObject.SetActive(true);
        ScoreText.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
        StartGame_MoveUI();

        isGameStart = true;
        BlackGround.gameObject.SetActive(false);
        BlackGround.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        gameManager.GameStart();
    }

    public void Event_GameRestartButtonClick()
    {
        StartGame_MoveUI();
        BlackGround.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        gameManager.Card_RePlay();
        score = 0;
        time = 30;
    }
    private void EndGame()
    {
        BlackGround.gameObject.SetActive(true);
        EndGame_MoveUI();
    }

    private void EndGame_MoveUI()
    {
        TimeText.gameObject.SetActive(false);
        ScoreText.transform.localPosition       = new Vector2(0,0);
        RestartButton.transform.localPosition   = new Vector2(0,-50);
    }

    private void StartGame_MoveUI()
    {
        TimeText.transform.localPosition        = new Vector2(-530, 440);
        ScoreText.transform.localPosition       = new Vector2(-530, 400);
        RestartButton.transform.localPosition   = new Vector2(-530, 350);
    }
}
