using UnityEngine;
using static GameManager;

public class CharacterController : MonoBehaviour
{
    public float timeForMove = 0.2f;
    public float jumpHeight = 1.0f;
    public float leftRotation = -45.0f;
    public float rightRotation = 90.0f;

    public int minX = -4;
    public int maxX = 4;
    public int jumpStep = 3;

    public bool canMove = false;

    private bool moving;
    private float elapsedTime;
    private float startY;

    private Vector3 currentPos;
    private Vector3 target;
   
    private Rigidbody rigidbody;
    private Transform body;

    [HideInInspector]
    public int score;


    #region BuitInMethods
    private void Start()
    {
        currentPos = transform.position;
        moving = false;
        startY = transform.position.y;
        rigidbody = GetComponent<Rigidbody>();
        body = transform.GetChild(0);
        score = 0;
    }


    private void Update()
    {
        if (moving)
        {
            MoveCharacter();
        }
        else
        {
            currentPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

            if (canMove)
                Inputs();
        }
        score = Mathf.Max(score, ((int)currentPos.z)/ jumpStep);
        gameManager.gamePlayController.scoreText.text = score.ToString();
    }
    #endregion



    #region PrivateMethods
    private void Inputs()
    {
        //====== Mobile Input
        if (Input.GetMouseButtonDown(0))
        {
            MouseClick();
            return;
        }

        //====== Keyboard Input
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(new Vector3(0, 0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (Mathf.RoundToInt(currentPos.x) > minX)
                Move(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (Mathf.RoundToInt(currentPos.x) < maxX)
                Move(new Vector3(1, 0, 0));
        }
    }


    private void MouseClick()
    {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit))
        {
			var direction = hit.point - transform.position;
			var x = direction.x;
			var z = direction.z;
			
			if (Mathf.Abs(z) > Mathf.Abs(x))
            {
				if (z > 0)
					Move(new Vector3(0, 0, jumpStep));
                else
					Move(new Vector3(0, 0, -jumpStep));
			}
            else
            {
				if (x > 0)
                {
					if (Mathf.RoundToInt(currentPos.x) < maxX)
						Move(new Vector3(jumpStep, 0, 0));
				}
                else
                { 
					if (Mathf.RoundToInt(currentPos.x) > minX)
						Move(new Vector3(-jumpStep, 0, 0));
				}
			}
        }
	}

    private void Move(Vector3 distance)
    {
        var newPosition = currentPos + distance;
        target = newPosition;

        moving = true;
        elapsedTime = 0;
        rigidbody.isKinematic = true;

        switch (MoveDirection)
        {
            case "forward":
                body.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "backward":
                body.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "right":
                body.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case "left":
                body.rotation = Quaternion.Euler(0, 90, 0);
                break;
            default:
                break;
        }
    }

    private void MoveCharacter()
    {
        elapsedTime += Time.deltaTime;

        float weight = (elapsedTime < timeForMove) ? (elapsedTime / timeForMove) : 1;

        float x = Lerp(currentPos.x, target.x, weight);
        float z = Lerp(currentPos.z, target.z, weight);
        float y = Sinerp(currentPos.y, startY + jumpHeight, weight);

        Vector3 result = new Vector3(x, y, z);
        transform.position = result; 

        if (result == target)
        {
            moving = false;
            currentPos = target;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(0, -10, 0, ForceMode.VelocityChange);
        }
    }

    private float Lerp(float min, float max, float weight)
    {
        return min + (max - min) * weight;
    }

    private float Sinerp(float min, float max, float weight)
    {
        return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
    }
    #endregion




    #region PublicMethods
    public void GameOver()
    {
        canMove = false;
        gameManager.gamePlayController.gameObject.SetActive(false);
        gameManager.gameOverController.gameObject.SetActive(true);
    }
    #endregion



    #region Properties
    public bool IsMoving
    {
        get { return moving; }
    }

    public string MoveDirection
    {
        get
        {
            if (moving)
            {
                float dx = target.x - currentPos.x;
                float dz = target.z - currentPos.z;

                if (dz > 0)
                    return "forward";
                else if (dz < 0)
                    return "backward";
                else if (dx > 0)
                    return "left";
                else
                    return "right";
            }
            else
                return null;
        }
    }
    #endregion
}
