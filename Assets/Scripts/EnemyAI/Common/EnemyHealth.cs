using Platformer.Mechanics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _currentHP;
    [SerializeField] private float _maxHP;
    private SpriteRenderer _spriteRenderer;

    [Tooltip("CumulateDamage for ability use")]
    public float _cumulateDamage { private set; get; } 

    [Header("player hurt effect")]
    [HideInInspector]public bool IsReflecting;
    private  bool Isimmune;
    private Material _initialMaterial;
    [Tooltip("The material when player hurt")]
    [SerializeField] Material _flashMaterial;
    [Tooltip("The color when player hurt")]
    [SerializeField] Color _flashColor;
    [SerializeField] float _hurtInterval;
    [SerializeField] float hurtSound;

    [Header("Enemy dead effect")]
    [SerializeField] AnimationClip _deadAnimation;
    [SerializeField] ParticleSystem _deadEffect;
    [SerializeField] AudioClip _deadSound;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialMaterial = _spriteRenderer.material;
        _currentHP = _maxHP;
    }
    
    public void hurt(float damage) {
        if (IsReflecting)
        {
            GameController.player.Reflected(damage);
            return;
        }
        if (Isimmune) return;
        else{
            Isimmune = true;
        }
       _currentHP -= damage;
        _cumulateDamage += damage;
        if (_currentHP <= 0) {
            // delete object after delay
            LeanTween.delayedCall(gameObject,_hurtInterval, () =>{
                Destroy(gameObject);
            });
            // delay after dead 
        }
        
        _spriteRenderer.material = _flashMaterial;
      
       LeanTween.color(gameObject, _flashColor, _hurtInterval).setOnComplete(() => {
           _spriteRenderer.material = _initialMaterial;
           _spriteRenderer.color = Color.white;
           Isimmune = false;
       });
    }
    public void ResetCumulateDamage() {
        _cumulateDamage = 0;
    }
    
}
