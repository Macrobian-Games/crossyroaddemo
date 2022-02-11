using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager = null;

    public MenuController menuController;
    public CharacterController characterController;
    public CameraController cameraController;
    public GamePlayController gamePlayController;
    public GameOverController gameOverController;
    public GroundController groundController;

    #region BuiltInMethods
    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;
    }
    #endregion
}
