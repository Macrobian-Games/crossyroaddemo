using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class GamePlayController : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public bool isGameOver = false;

    #region BuiltInMethods
    private void OnEnable()
    {
        score =  0;
        scoreText.text = score.ToString();
    }
    #endregion


    #region PublicMethods
    public void GameOver()
    {
        gameManager.gameOverController.gameObject.SetActive(true);
        gameManager.characterController.canMove = false;
        gameManager.cameraController.moving = false;
        isGameOver = true;
        gameObject.SetActive(false);
    }
    #endregion
}
