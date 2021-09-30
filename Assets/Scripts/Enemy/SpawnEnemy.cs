using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    List<GameObject> prefabList = new List<GameObject>();
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    // Start is called before the first frame update
    void Start()
    {
        prefabList.Add(prefab1);
        prefabList.Add(prefab2);
        prefabList.Add(prefab3);
    }

    // Update is called once per frame
    void Update()
    {
        //When the return key is pressed pick a random enemy prefab and spawn it.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int prefabIndex = Random.Range(0, 3);
            Instantiate(prefabList[prefabIndex], transform.position, Quaternion.identity);
        }
    }
}
