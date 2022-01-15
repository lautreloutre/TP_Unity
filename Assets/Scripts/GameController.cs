using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //public static GameController instance;
    public int countdownTime;
    private float startTime;
    public Text countdownDisplay;
    public bool gamePlaying;
    public GameObject HUD, GameOverObj, CounterObj, CrosshairObj, GameDefeatObj;
    public AISpawner[] spawners;
    public PlayerMove[] player;


    /*private void Awake()
    {
        instance = this;
    }*/

    private void Start()
    {
        GameStart();
    }


    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString(); //le text du cddisplay sera la variable cdtime ToString -> transforme la variable en char

            yield return new WaitForSeconds(1f); //stop la coroutine pendant 1s avant de continuer (IEnumerator)

            countdownTime--;

            //cdTime est > 3 donc on affiche "3" puis on attend 1s puis on -1 cdTime etc
        }

        countdownDisplay.text = "START";
        gamePlaying = true;
        CounterObj.SetActive(true);
        CrosshairObj.SetActive(true);


        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false); //attend 1s et enlève le "START"
    }

    public void GameStart()
    {
        gamePlaying = false;
        PlayerMove.defeat = false;
        StartCoroutine(CountdownToStart());
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        gamePlaying = false;
        Time.timeScale = 0;
        ShowGameOverScreen();
    }

    public void GameDefeat()
    {
        gamePlaying = false;
        ShowDefeatScreen();
        Time.timeScale = 0;
        ScoreScript.kills = 0;
    }

    private void ShowGameOverScreen()
    {
        GameOverObj.SetActive(true);
        HUD.SetActive(false);
    }

    private void ShowDefeatScreen()
    {
        GameDefeatObj.SetActive(true);
        HUD.SetActive(false);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (ScoreScript.kills >= 20)
        {
            EndGame();
        }
        if (gamePlaying == false)
        {
            for (int i = 0; i < spawners.Length; i++)//
            {
                spawners[i].enabled = false;
            }
            for (int i = 0; i < player.Length; i++)
            {
                player[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].enabled = true;
            }
            for (int i = 0; i < player.Length; i++)
            {
                player[i].enabled = true;
            }
        }
        if (PlayerMove.defeat == true)
            GameDefeat();
        if (gamePlaying == true)
            Time.timeScale = 1;

        if ((Input.GetButtonDown("Jump")) && (PlayerMove.defeat == true))
            ResetScene();
    }
}

