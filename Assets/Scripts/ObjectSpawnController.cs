using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameManager;

public class ObjectSpawnController : MonoBehaviour
{
    public enum ObjectType
    {
        RoadLine,
        WaterLine
    }
    public ObjectType objectType;

    public enum Direction { Left = -1, Right = 1 };
    public Direction direction;

    public GameObject[] spawnPrefebs;

    public bool randomizeValues = false;
    public float speed = 2.0f;
    public float interval = 6.0f;
    public float leftX = -20.0f;
    public float rightX = 20.0f;

    //==== For WaterLine
    private float length = 2.0f;

    private float elapsedTime;
    private List<GameObject> cloneObjLst = new List<GameObject>();

    #region BuiltInMethods
    private void Start()
    {
        if (randomizeValues)
        {
            direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(2.0f, 4.0f);

            if (objectType == ObjectType.RoadLine)
                interval = Random.Range(5.0f, 9.0f);
            else
            {
                length = Random.Range(0.5f, 1.1f);
                interval = length / speed + Random.Range(2.0f, 4.0f);
            }
        }
        elapsedTime = 0.0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > interval)
        {
            elapsedTime = 0.0f;

            Vector3 position = Vector3.zero;
            if (objectType == ObjectType.RoadLine)
            {
                 position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 1.5f, 0);
            }
            else
            {
                 position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 0, 0);
            }

            GameObject clone =Instantiate(spawnPrefebs[Random.Range(0, spawnPrefebs.Length)], position, Quaternion.identity);
            clone.GetComponent<SpawnItemController>().speed = (int)direction * speed;

            if (objectType == ObjectType.WaterLine)
            {
                Vector3 scale = clone.transform.localScale;
                //clone.transform.localScale = new Vector3(scale.x * length, scale.y, scale.z * 3);
                clone.transform.localScale = new Vector3(scale.x * length, scale.y, scale.z);
            }

            cloneObjLst.Add(clone);
            clone.transform.parent = transform;
        }

        foreach (GameObject item in cloneObjLst.ToArray())
        {
            if (direction == Direction.Left && item.transform.position.x < leftX || direction == Direction.Right && item.transform.position.x > rightX)
            {
                Destroy(item);
                cloneObjLst.Remove(item);
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (objectType == ObjectType.WaterLine)
        {
            if (other.tag == "Player")
            {
                gameManager.gamePlayController.GameOver();
            }
        }
    }
    #endregion
}
