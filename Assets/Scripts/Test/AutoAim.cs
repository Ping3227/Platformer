using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoAim : MonoBehaviour
{
    [SerializeField] Transform target;

    

    void Update(){
        // Only rotate z axis toward target
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    
}
