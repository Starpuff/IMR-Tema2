using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshPro scoreText;
    private int score = 0;

    private int currentScore = 0;
    private float currentTime = 0;
    private float scoreWindow = 2.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        if (Time.time - currentTime > scoreWindow)
        {
            AddScore(currentScore);
            currentScore = 0;
        }
    }

    public void UpdateScore(int points)
    {
		if(currentScore == 0)
		{
        	currentScore = points;
        	currentTime = Time.time;
		}
    }

    void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}
