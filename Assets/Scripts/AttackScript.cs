using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GameObject knifePrefab;
    public GameObject gunPrefab;

    private GameObject activeKnife;

    public StaminaController stats;

    public int attack1Cost = 5;
    public int attack2Cost = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack attack = new SpikeHand(knifePrefab, transform, this);
            attack.UseAttack();
            //stats.TakeStam(attack1Cost);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Attack attack = new Spikes(gunPrefab, transform);
            attack.UseAttack();
        }

        // Keep knife locked to player
        if (activeKnife != null)
        {
            activeKnife.transform.position = transform.position;
            activeKnife.transform.rotation = transform.rotation;
        }
    }

    public void SetActiveKnife(GameObject knife)
    {
        if (activeKnife != null)
        {
            Destroy(activeKnife);
        }
        activeKnife = knife;
    }

    public abstract class Attack
    {
        protected int damage;
        protected float duration;
        protected float cooldown;

        public abstract void UseAttack();

        protected void StartCooldown()
        {
            Debug.Log("Starting cooldown for: " + cooldown + " seconds.");
        }
    }

    public class Melee : Attack
    {
        public override void UseAttack()
        {
            Debug.Log("Using melee attack.");
            StartCooldown();
        }
    }

    public class SpikeHand : Melee
    {
        private GameObject knifePrefab;
        private Transform playerTransform;
        private Attacks attackController;

        public SpikeHand(GameObject prefab, Transform transform, Attacks controller)
        {
            knifePrefab = prefab;
            playerTransform = transform;
            attackController = controller;
        }

        public override void UseAttack()
        {
            Debug.Log("Spawning knife locked to player.");
            GameObject knife = Object.Instantiate(knifePrefab, playerTransform.position, playerTransform.rotation);
            attackController.SetActiveKnife(knife);
            base.UseAttack();
        }
    }

    public class Ranged : Attack
    {
        public override void UseAttack()
        {
            Debug.Log("Using ranged attack.");
            StartCooldown();
        }
    }

    public class Spikes : Ranged
    {
        private GameObject gunPrefab;
        private Transform playerTransform;

        public Spikes(GameObject prefab, Transform transform)
        {
            gunPrefab = prefab;
            playerTransform = transform;
        }

        public override void UseAttack()
        {
            Debug.Log("Spawning gun at player position.");
            Object.Instantiate(gunPrefab, playerTransform.position, Quaternion.identity);

            // Simulate damage
            Debug.Log("All characters would lose a life here.");
            // Example: foreach (var enemy in FindObjectsOfType<Enemy>()) enemy.TakeDamage(damage);

            base.UseAttack();
        }
    }
}
