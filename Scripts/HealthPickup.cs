using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
    AudioSource pickupItemSource;
    // Start is called before the first frame update
    private void Awake()
    {
        pickupItemSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable= collision.GetComponent<Damageable>();
        if(damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);
            if(wasHealed) 
            {
                if(pickupItemSource)
                    AudioSource.PlayClipAtPoint(pickupItemSource.clip, gameObject.transform.position, pickupItemSource.volume);
                Destroy(gameObject);
            }
            
        }
    }
    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
