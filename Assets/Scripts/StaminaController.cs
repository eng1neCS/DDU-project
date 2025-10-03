using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    public float dashCost = 15f;
    [HideInInspector] public bool hasRegenerated = true;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina UI Elementer")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private void Update()
    {
        if (playerStamina < maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            playerStamina = Mathf.Clamp(playerStamina, 0, maxStamina);
            UpdateStamina(1);

            if (playerStamina >= maxStamina)
            {
                sliderCanvasGroup.alpha = 0;
                hasRegenerated = true;
            }
        }
    }

    // Bruges af PlayerController til at tjekke og trække stamina
    public bool UseStamina(float amount)
    {
        if (playerStamina >= amount)
        {
            playerStamina -= amount;
            UpdateStamina(1);
            return true;
        }
        return false;
    }

    private void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        sliderCanvasGroup.alpha = value == 0 ? 0 : 1;
    }
}
