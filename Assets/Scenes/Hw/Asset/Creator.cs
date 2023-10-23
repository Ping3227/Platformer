using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField] GameObject Prefab;
    [SerializeField] float CreateTime;
    private float TimeCounter;
    private void Start()
    {
        TimeCounter = CreateTime;
    }
    private void Update()
    {
        TimeCounter -= Time.deltaTime;
        if (TimeCounter <= 0)
        {
            Instantiate(Prefab, transform.position, Quaternion.identity);
            TimeCounter = CreateTime;
        }
    }
}
