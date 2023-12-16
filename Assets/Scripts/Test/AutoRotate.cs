using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AutoRotate : MonoBehaviour
{

    public bool IsRotating = false;
    public bool ResetAngle = false;
    [SerializeField] Transform target;
    [SerializeField] float RotateTime;
    private int RotateDirection;
    [SerializeField] float RotateAngle;

    [Header("Effect")]
    [SerializeField] ParticleSystem ExplodeEffect;
    [SerializeField] bool Explodeable = false;
    [SerializeField] float ExplodeDelay;
    private RaycastHit2D hit;
    [SerializeField] LayerMask whichToHit;
    void Update()
    {
        if(ResetAngle == true)
        {
            
            transform.rotation = Quaternion.Euler(0, 0, 90);
            
            ResetAngle = false;
        }
        // Only rotate z axis toward target
        if (IsRotating == true) {
            
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // compare angle with transforom.rotation.z
            if (angle < 0) {
               angle += 360;
            }
            //Debug.Log("angle: " + angle + " minus " + transform.rotation.eulerAngles.z );
            if (angle - transform.rotation.eulerAngles.z > 0 && angle - transform.rotation.eulerAngles.z<180)
            {
                RotateDirection = 1;
            }
            else
            {
                RotateDirection = -1;
            }
            //Debug.Log("RotateDirection:" +RotateDirection+"target angle: " + angle + " Transform start " + transform.rotation.eulerAngles.z + " to " + transform.rotation.eulerAngles.z + RotateDirection * RotateAngle);
            LeanTween.cancel(gameObject);
            LeanTween.rotateZ(gameObject, transform.rotation.eulerAngles.z + RotateDirection * RotateAngle, RotateTime);
            IsRotating = false;
        }
        hit= Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)),20,whichToHit);
        
        //Debug.Log("Location: "+transform.position+" Direction: " + new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)));
        //if(hit)
        //Debug.Log(hit.collider.name + "hit point" + hit.point);
        if(hit && Explodeable && ExplodeEffect != null)
        {
            //Debug.Log("Explode");
            LeanTween.delayedCall(ExplodeDelay, () => {
                ExplodeEffect.transform.position = hit.point;
                ExplodeEffect.Play();
            });
        }
    }

    
}
