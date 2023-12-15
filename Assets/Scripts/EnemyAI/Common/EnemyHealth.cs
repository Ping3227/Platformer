using Platformer.Mechanics;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Platformer.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health of stage")]
    private float _currentHP;
    [SerializeField] private float _maxHP;
    [SerializeField] private bool IsInvincible;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector] public PracticeSpawner Spawner;

    [Header("player hurt effect")]
    private  bool Isimmune;
    private Material _initialMaterial;
    private Color _initialColor;
    [Tooltip("The material when player hurt")]
    [SerializeField] Material _flashMaterial;
    [Tooltip("The color when player hurt")]
    [SerializeField] Color _flashColor;
    [SerializeField] float _hurtInterval;

    [Header("Enemy dead effect")]
    [SerializeField] AnimationClip _deadAnimation;
    [SerializeField] ParticleSystem _deadEffect;
    [SerializeField] AudioClip _deadSound;
    [SerializeField] PracticeSpawner _spawner;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialMaterial = _spriteRenderer.material;
        _initialColor = _spriteRenderer.color;
        _currentHP = _maxHP;
       
    }
    
    public void hurt(float damage) {
        if (Isimmune) return;
        else{
            Isimmune = true;
        }
        if (!IsInvincible) _currentHP -= damage;       
        if (_currentHP <= 0) {
                if (_deadEffect != null){
                    _deadEffect.Play();
                }
            if (_deadAnimation != null)
            {
                LeanTween.delayedCall(gameObject, _deadAnimation.length, () =>
                {
                    _spawner?.OnObjectDeath();
                    Destroy(gameObject);

                });
            }
            else {
                LeanTween.delayedCall(gameObject, _hurtInterval, () =>
                {
                    _spawner?.OnObjectDeath();
                    Destroy(gameObject);
                });
            }
                
            }
        _spriteRenderer.material = _flashMaterial;
      
       LeanTween.color(gameObject, _flashColor, _hurtInterval).setOnComplete(() => {
           _spriteRenderer.material = _initialMaterial;
           _spriteRenderer.color = _initialColor;
           Isimmune = false;
       });
    }
    
}

