using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBartext;
    Damageable playerDamagealbe;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.Log("No tag 'Player' found.");
        }
        playerDamagealbe= player.GetComponent<Damageable>();
        
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth/maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamagealbe.Health, playerDamagealbe.MaxHealth);
        healthBartext.text = "HP: " + playerDamagealbe.Health + "/" + playerDamagealbe.MaxHealth;
    }
    private void OnEnable()
    {
        playerDamagealbe.healthChanged.AddListener(OnPlayerHealthChanged);
    }
    private void OnDisable()
    {
        playerDamagealbe.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBartext.text = "HP: " + newHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
