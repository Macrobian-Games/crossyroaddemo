using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GroundController : MonoBehaviour
{
    public List<GameObject> lineList;

    public int minZ = 3;
    public int remainLineForward = 40;
    public int remainLineBackward = 20;

    private Dictionary<int, GameObject> lines;
    private Transform player;

    #region BuiltInMethods
    private void Start()
    {
        player = gameManager.characterController.transform;
        lines = new Dictionary<int, GameObject>();
    }

    private void Update()
    {
        var playerZ = (int)player.transform.position.z;
        for (var z = Mathf.Max(minZ, playerZ - remainLineBackward); z <= playerZ + remainLineForward; z += 1)
        {
            if (!lines.ContainsKey(z))
            {
                var line = Instantiate(lineList[Random.Range(0, lineList.Count)]
                    ,new Vector3(0, 0, z * 3 - 5),Quaternion.identity,transform);
                lines.Add(z, line);
            }
        }

        foreach (var line in new List<GameObject>(lines.Values))
        {
            if (line != null)
            {
                var lineZ = line.transform.position.z;

                if (lineZ < playerZ - remainLineBackward)
                {
                    lines.Remove((int)lineZ);
                    Destroy(line);
                }
            }
        }
    }
    #endregion
}
