using UnityEngine.Events;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public float GunCooldown;

    public bool Automatic;

    private float CurrentCooldown;
    void Start()
    {
        CurrentCooldown = GunCooldown;
    }

    void Update()
    {
        if (Automatic)
        {
            if (Input.GetMouseButton(0))
            {
                OnGunShoot?.Invoke();
                CurrentCooldown = GunCooldown;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (CurrentCooldown <= 0f)
                {
                    OnGunShoot?.Invoke();
                    CurrentCooldown = GunCooldown;
                }
            }
        }
        CurrentCooldown -= Time.deltaTime;
    }
}
