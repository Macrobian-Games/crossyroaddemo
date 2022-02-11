using UnityEngine;
using static GameManager;

public class CameraController : MonoBehaviour
{
    public float minZ = 0.0f;
    public float zIncrement = 1.0f;
    public float zOffsetSpeed = 4.0f;
    public bool moving = false;

    private CharacterController characterController;
    private Vector3 initialOffset = new Vector3(5f, 10.0f, -7.5f);
    private Vector3 offset;

    #region BuiltInMethods
    private void Start()
    {
        characterController = gameManager.characterController;
        offset = initialOffset = transform.position;
	}
	
    private void Update()
    {
        if (moving)
        {
            Vector3 playerPosition = characterController.transform.position;
            transform.position = new Vector3(playerPosition.x, 0, Mathf.Max(minZ, playerPosition.z)) + offset;

            // for moving camera
            offset.z += zIncrement * Time.deltaTime;

            // Increase/decrease z when player is moving forward/backward.
            if (characterController.IsMoving)
            {
                if (characterController.MoveDirection == "forward")
                {
                    offset.z -= zOffsetSpeed * Time.deltaTime;
                }
            }

            if(transform.position.z >= (playerPosition.z+2))
                gameManager.gamePlayController.GameOver();
        }
	}
    #endregion
}
