using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HpController playerHpController;
    [SerializeField] private Sprite[] healthBarSprites;
    [SerializeField] private Image image;

    private void OnEnable()
    {
        playerHpController.TakeDamageEvent += UpdateHealthBar;
    }
    private void OnDisable()
    {
        playerHpController.TakeDamageEvent -= UpdateHealthBar;
    }
    private void UpdateHealthBar()
    {
        if (playerHpController.hp > 0)
        {
            image.sprite = healthBarSprites[playerHpController.hp];
        }
    }
}
