using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GameObject knifePrefab;
    public GameObject Gun;

    private GameObject activeKnife;

    public HP stats;

    public int attack1Cost = 5;
    public int attack2Cost = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack attack = new Slash(knifePrefab, transform, this);
            attack.UseAttack();
            stats.TakeStam(attack1Cost);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Attack attack = new GunShot(GunPrefab, BulletPrefab, transform);
            attack.UseAttack();
            stats.TakeStam(attack2Cost);
        }
    }