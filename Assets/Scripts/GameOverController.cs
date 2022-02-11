using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public Text scoreText;

    #region BuiltInMethods
    private void OnEnable()
    {
        scoreText.text = gameManager.characterController.score.ToString();
    }
    #endregion


    #region PublicMethods
    public void BtnHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
