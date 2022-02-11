using UnityEngine;
using System.Collections;
using static GameManager;

public class SpawnItemController : MonoBehaviour
{
    public enum ObjectType
    {
        TrafficCar,
        Trunk
    }
    public ObjectType objectType;

    public float speed = 1.0f;

    [Header("==== Trunk ====")]
    public float animationTime = 0.1f;
    public float animationDistance = 0.1f;

    private float originalY;
    private bool sinking;
    private float elapsedTime;

    #region BuiltInMethods
    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        //transform.position += new Vector3(speed * Time.deltaTime, transform.position.y, 0.0f);

        //======= Only For Trunk
        if (objectType == ObjectType.Trunk)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > animationTime)
            {
                sinking = false;
                transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            }

            if (sinking)
            {
                float y = Sinerp(originalY, originalY - animationDistance, (elapsedTime < animationTime) ? (elapsedTime / animationTime) : 1);
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //======= Only for Traffic car
        if (objectType == ObjectType.TrafficCar)
        {
            if (other.gameObject.tag == "Player")
            {
                Vector3 scale = other.gameObject.transform.localScale;
                other.gameObject.transform.localScale = new Vector3(scale.x, scale.y * 0.1f, scale.z);
                gameManager.gamePlayController.GameOver();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //======= Only for Trunk
        if (objectType == ObjectType.Trunk)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
            }
        }
    }
    #endregion


    #region PrivateMethods
    private float Sinerp(float min, float max, float weight)
    {
        return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
    }
    #endregion
}
