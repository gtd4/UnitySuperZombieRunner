using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Text continueText;
    public Text scoreText;

    private float timeElapsed = 0f;
    private float bestTime = 0f;
    private float blinkTime = 0f;
    private bool shouldBlink;
    private GameObject floor;
    private Spawner spawner;
    private GameObject player;
    private TimeManager timeManager;
    public bool gameStarted;
    private bool beatBestTime;

    private void Awake()
    {
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GetComponent<TimeManager>();
    }

    // Use this for initialization
    private void Start()
    {
        var floorheight = floor.transform.localScale.y;

        var pos = floor.transform.position;
        pos.x = 0;
        pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + (floorheight / 2);

        floor.transform.position = pos;
        floor.layer = 30;

        spawner.isActive = false;

        Time.timeScale = 0;

        bestTime = PlayerPrefs.GetFloat("BestTime");
        continueText.text = "Press Any Button To Start";
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameStarted && Time.timeScale == 0)
        {
            if (Input.anyKeyDown)
            {
                timeManager.ManipulateTime(1, 1f);
                ResetGame();
            }
        }

        if (!gameStarted)
        {
            blinkTime++;

            if (blinkTime % 40 == 0)
            {
                shouldBlink = !shouldBlink;
            }

            continueText.canvasRenderer.SetAlpha(shouldBlink ? 0 : 1);
            var textColor = beatBestTime ? "#FF0" : "#FFF";

            scoreText.text = "Time: " + FormatTime(timeElapsed) + "\n<color=" + textColor + ">Best: " + FormatTime(bestTime) + "</color>";
        }
        else
        {
            timeElapsed += Time.deltaTime;
            scoreText.text = "Time: " + FormatTime(timeElapsed);
        }
    }

    private void ResetGame()
    {
        //Clear away all obstacles
        gameStarted = true;
        spawner.isActive = true;

        player = GameObjectUtil.Instantiate(playerPrefab, new Vector3(0, (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + 100, 0));
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();

        playerDestroyScript.DestroyCallback += OnPlayerKilled;

        continueText.canvasRenderer.SetAlpha(0f);

        timeElapsed = 0;
    }

    private void OnPlayerKilled()
    {
        spawner.isActive = false;

        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();

        playerDestroyScript.DestroyCallback -= OnPlayerKilled;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeManager.ManipulateTime(0, 5.5f);
        gameStarted = false;

        continueText.text = "Press any key to restart";

        if (timeElapsed > bestTime)
        {
            bestTime = Math.Max(bestTime, timeElapsed);
            PlayerPrefs.SetFloat("BestTime", bestTime);
            beatBestTime = true;
        }
    }

    private string FormatTime(float value)
    {
        TimeSpan t = TimeSpan.FromSeconds(value);

        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}