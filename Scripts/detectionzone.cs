using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class detectionzone : MonoBehaviour
{
    public UnityEvent noColliderRemains;
    Collider2D col;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    // Start is called before the first frame update
    void Awake()
    {
        col= GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
        if(detectedColliders.Count <= 0)
        {
            noColliderRemains.Invoke();
        }
    }
}
