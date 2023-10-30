using Platformer.Mechanics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _currentHP;
    [SerializeField] private float _maxHP;
    private SpriteRenderer _spriteRenderer;


    [Header("player hurt effect")]
    public bool IsReflecting;
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
       Debug.Log($"Enemy hurt {Time.time}");
       _currentHP -= damage;
        if (_currentHP <= 0) {
            // TODO : Enemy dead effect 
            return;
        }
        
        _spriteRenderer.material = _flashMaterial;
      
       LeanTween.color(gameObject, _flashColor, _hurtInterval).setOnComplete(() => {
           _spriteRenderer.material = _initialMaterial;
           _spriteRenderer.color = Color.white;
           Isimmune = false;
       });
    }
    
}
