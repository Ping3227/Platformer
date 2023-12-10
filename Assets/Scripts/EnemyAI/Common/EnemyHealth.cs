using Platformer.Mechanics;
using System;

using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health of stage")]
    private float _currentHP;
    private Boos1 boss;
    [SerializeField] StateInfo[] _stateInfo;
    private int State=0;
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
    [SerializeField] Slider HealthBar;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialMaterial = _spriteRenderer.material;
        _currentHP = _stateInfo[0]._maxHP;
        boss = GetComponent<Boos1>();
        
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
        if (HealthBar != null)
        {
            HealthBar.value = _currentHP / _stateInfo[State]._maxHP;
        }
        _cumulateDamage += damage;
        if (_currentHP <= 0) {
            if (State == _stateInfo.Length - 1){
                // play animation and death
                boss.NextStage(_stateInfo[State].end_animation,null);
                LeanTween.delayedCall(gameObject, _stateInfo[State].end_animation.length, () =>
                {
                    Destroy(gameObject);
                });
            }
            else {
                
                boss.NextStage(_stateInfo[State].end_animation, _stateInfo[State].BT_scripts);
                if (HealthBar != null)
                {
                    State++;
                    _currentHP = _stateInfo[State]._maxHP;
                    HealthBar.value = _currentHP / _stateInfo[State]._maxHP;
                    
                    
                }
            }
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
    [Serializable]
    class StateInfo
    {
        public float _maxHP;
        public AnimationClip end_animation;
        public InteractActor[] InteractionObjects;
        public TextAsset[] BT_scripts;
        
    }
}

