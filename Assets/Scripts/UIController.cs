using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null) scoreText.text = score.ToString();
    }
}
