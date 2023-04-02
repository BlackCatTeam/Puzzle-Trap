using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioSource music;

    public bool startPlaying;

    public BeatScroller BS;
    public Text scoreText;
    public Text multText;
    public int currentScore;
    public int ScorePerNote = 100;
    public int ScorePerGoodNote = 125;
    public int ScorePerPerfectNote = 150;

    public int currentMultiplier = 1;
    public int multiplierTracker;

    public int[] multiplierThresholds;


    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;


    [Header("Score Screen")]
    public GameObject resultsScreen;
    public Text normalHitText;
    public Text goodHitText;
    public Text perfectHitText;
    public Text missedHitText;
    public Text percentageHitText;
    public Text finalScoreText;
    public Text rankText;


    void Start()
    {
        instance = this;
        currentScore = 0;
        currentMultiplier = 1;

        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + currentScore.ToString();
        multText.text = "x" + currentMultiplier.ToString();

        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                BS.hasStarted = true;
                music.Play();
            }
        }
        else
        {
            if (!music.isPlaying && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);
                UpdateScores();
            }
        }


    }

    private void UpdateScores()
    {
        normalHitText.text = normalHits.ToString();
        goodHitText.text = goodHits.ToString();
        percentageHitText.text =perfectHits.ToString();
        missedHitText.text = missedHits.ToString();

        float totalHit = normalHits + goodHits + perfectHits;
        float percentHit = (totalNotes / totalHit) / 100f;

        percentageHitText.text = percentHit.ToString("F1") + "%";

        finalScoreText.text = currentScore.ToString();


        string RankVal = "F";

        switch (percentHit)
        {
            case  _ => _== 41: break;
        }
        if (percentHit > 40)
        {
            RankVal = "D";
        }
    }

    public void NoteHit()
    {
        Debug.Log("Perfect!");

        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

    }

    public void NormalHit()
    {
        currentScore += ScorePerNote * currentMultiplier;
        normalHits++;
        NoteHit();
    }
    public void GoodHit()
    {
        currentScore += ScorePerGoodNote * currentMultiplier;
        goodHits++;
        NoteHit();
    }
    public void PerfectHit()
    {
        currentScore += ScorePerPerfectNote * currentMultiplier;
        perfectHits++;
        NoteHit();
    }
    public void NoteMissed()
    {
        missedHits++;
        currentMultiplier = 1;
        multiplierTracker = 0;
    }
}
