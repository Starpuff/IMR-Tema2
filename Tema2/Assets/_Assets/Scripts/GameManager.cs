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
	private int currentDistance = 0;
    private float currentTime = 0;
    private float scoreWindow = 3.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Update()
    {
        if (currentScore!=0 && Time.time - currentTime > scoreWindow)
        {
            AddScore(currentScore, currentDistance);
            currentScore = 0;
			currentDistance = 0;
        }
    }

    public void UpdateScore(int points, int distance)
    {
		if(currentScore == 0)
		{
        	currentScore = points;
			currentDistance = distance;
        	currentTime = Time.time;
		}
    }

    void AddScore(int points, int distance)
    {
        score += points;
        scoreText.text = "The distance was " + distance + "m\n" + "Target score: " + points + "\nTotal Score: " + points * distance;
    }
}
