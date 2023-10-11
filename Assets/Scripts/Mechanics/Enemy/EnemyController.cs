using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject Enemy_1;
    [SerializeField] GameObject[] SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject spawnpoint in SpawnPoints) {
            Instantiate(Enemy_1, spawnpoint.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
