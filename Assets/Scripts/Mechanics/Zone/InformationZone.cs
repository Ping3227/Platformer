using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationZone : MonoBehaviour
{
    [SerializeField] string information;
    
    private GameObject _PopOutWord;
    [SerializeField] GameObject PopOutWord;
    [SerializeField] float PopOutTime;
    [SerializeField] float Size;
 
    private void Awake()
    {
        _PopOutWord= Instantiate(PopOutWord, transform.position, Quaternion.identity);
        
        _PopOutWord.SetActive(false);
        _PopOutWord.gameObject.GetComponentInChildren<TextMeshPro>().text = information;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _PopOutWord.SetActive(true);
            LeanTween.cancel(_PopOutWord);
            LeanTween.scale(_PopOutWord, new Vector3(Size, Size, Size), PopOutTime);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LeanTween.cancel(_PopOutWord);
            LeanTween.scale(_PopOutWord, Vector3.zero, PopOutTime).setOnComplete(() => _PopOutWord.SetActive(false));
        }
    }
    
}
