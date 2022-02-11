using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class MenuController : MonoBehaviour
{
    #region PublicMethods
    public void TapToStartClick()
    {
        gameManager.cameraController.moving = gameManager.characterController.canMove = true;
        gameManager.gamePlayController.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion
}
