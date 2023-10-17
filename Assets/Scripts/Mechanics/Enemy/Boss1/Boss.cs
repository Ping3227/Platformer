
using Platformer.Mechanics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject ImmobolizePrefab;
    public BoxCollider2D Area;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Animator anim;
    private bool IsFacingRight = true;
    private void Start()
    {
        rb =gameObject.GetComponent<Rigidbody2D>();
        
        IsFacingRight = true;
        anim= GetComponent<Animator>();
    }
    public void Move() {
        anim.SetFloat("Distance", Vector2.Distance(targetPosition, GameController.player.transform.position));
        rb.position = targetPosition;
        
        CheckTurn();
        
    }
    public void SetNextPosition(Vector2 NextPosition) {
        Debug.Log(NextPosition);
        targetPosition = NextPosition;
        
    }
    private void CheckTurn() { 
        if(GameController.player.transform.position.x > targetPosition.x && !IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = true;
        }
        if (GameController.player.transform.position.x < targetPosition.x && IsFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = false;
        }
    }
     public void CreateImmobolize() {
        Instantiate(ImmobolizePrefab,targetPosition,Quaternion.identity);
        anim.SetFloat("Distance", Vector2.Distance(transform.position, GameController.player.transform.position));

    }
}
