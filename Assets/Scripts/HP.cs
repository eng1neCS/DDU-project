using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public int health;
    public int maxHealth = 1;
    public Slider hslider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        hslider.maxValue = maxHealth;
        hslider.value = health;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
