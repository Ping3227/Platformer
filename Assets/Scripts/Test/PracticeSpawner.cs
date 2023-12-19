using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSpawner : MonoBehaviour
{
    [SerializeField] Flys SpawnPrefab;
    private Flys SpawnObject;
    [SerializeField] Transform SpawnPoint;
    [Header("AutoSpawn")]
    [SerializeField] float SpawnTime;
    private float SpawnCounter;
    [SerializeField] bool AutoSpawn;
    [SerializeField] BoxCollider2D DefaultArea;
    [SerializeField] BoxCollider2D AttackArea;
    
    [Header("interaction")]
    private GameObject _popup;
    [SerializeField] string information;
    [SerializeField] GameObject popup;
    [SerializeField] float PopOutTime;
    [SerializeField] float Size;
    private bool IsActive;
    private void Awake()
    {
        _popup= Instantiate(popup, transform.position, Quaternion.identity);
       
        _popup.SetActive(false);
    }
    private void Update()
    {
        
        if (AutoSpawn)
        {
            if (SpawnObject == null) {
                SpawnCounter += Time.deltaTime;
            }
            if (SpawnCounter >= SpawnTime)
            {
                SpawnCounter = 0;
                
                SpawnObject = Instantiate(SpawnPrefab, SpawnPoint.position, Quaternion.identity);
                
                SpawnObject.GetComponent<EnemyHealth>().Spawner = this;
                SpawnObject.PatrolArea = DefaultArea;
                SpawnObject.AttackArea = AttackArea;

            }
        }
        else if (SpawnObject==null && IsActive && UserInput.instance.controls.Interact.Interact.WasPressedThisFrame() )
        {
            Debug.Log("spawned");
            SpawnObject = Instantiate(SpawnPrefab, SpawnPoint.position, Quaternion.identity);
            SpawnObject.GetComponent<EnemyHealth>().Spawner = this;
            
            SpawnObject.PatrolArea = DefaultArea;
            SpawnObject.AttackArea = AttackArea;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!AutoSpawn&& collision.gameObject.CompareTag("Player")){
            _popup.SetActive(true);
            IsActive = true;
            LeanTween.cancel(_popup);
            LeanTween.scale(_popup, new Vector3(1, 1, 1)*Size, PopOutTime);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!AutoSpawn && collision.gameObject.CompareTag("Player"))
        {
            IsActive = false;
            LeanTween.cancel(_popup);
            LeanTween.scale(_popup, Vector3.zero, PopOutTime).setOnComplete(() => _popup.SetActive(false));
        }
    }
    public void OnObjectDeath() {
        SpawnObject = null;
    }
    
}
