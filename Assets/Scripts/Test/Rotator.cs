using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float angularSpeed = 1f;
    private void Update()
    {
        transform.Rotate(Vector3.forward, angularSpeed * Time.deltaTime);
    }
}
