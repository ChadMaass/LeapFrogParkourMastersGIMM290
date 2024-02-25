using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _scoreText2;
    [SerializeField] private Text winnerText;
    //[SerializeField] private int _score;
    //[SerializeField] private int _score2;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Player 1 Score: " + 0;
        _scoreText2.text = "Player 2 Score: " + 0;
        winnerText.text = "";
    }

    public void AddScore(int _killPoint)
    {
        //_score += _killPoint;
        _scoreText.text = "Player 1 Score: " + _killPoint.ToString();
    }  

    public void AddScore2(int _killPoint)
    {
        //_score2 += _killPoint;
        _scoreText2.text = "Player 2 Score: " + _killPoint.ToString();
    }

    public void DisplayWinner(string playerName)
    {
        // Update the text property with the name of the winning player
        winnerText.text = playerName + " Wins!";
        StartCoroutine(ResetWinnerTextAfterDelay(5));
    }

    IEnumerator ResetWinnerTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        winnerText.text = "";
    }
}
