using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStatController : MonoBehaviour
{
    public static PlayerStatController Instance;
    
    [SerializeField] private PlayerStats startingStats;
    
    //player stat variables
    [HideInInspector] public int extraHp;
    [HideInInspector] public int flatDamage;
    [HideInInspector] public int damageMultiplier;
    [HideInInspector] public int flatKnockback;
    [HideInInspector] public int knockbackMultiplier;
    [HideInInspector] public float pickUpRange;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public int xpMultiplier;
    [HideInInspector] public int inventorySlots;
    [HideInInspector] public float spellCastDelay;
    [HideInInspector] public int spellCastAmount;
    
    

    [Header("HP")]
    public int extraHpUpgrade;
    [Header("Damage")]
    public int flatDamageUpgrade;
    public int damageMultiplierUpgrade;
    [Header("Knockback")]
    public int flatKnockbackUpgrade;
    public int knockbackMultiplierUpgrade;
    [Header("Movement")]
    public float movementSpeedUpgrade;
    [Header("Items")]
    public float pickUpRangeUpgrade;
    public int xpMultiplierUpgrade;
    public int inventorySlotsUpgrade;
    [Header("SpellCast")]
    public float spellCastDelayUpgrade;
    public int spellCastAmountUpgrade;
    
    [SerializeField] private LevelController levelController;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private TextMeshProUGUI playerStatsText;
    [SerializeField] private List<GameObject> upgradesList;
    [SerializeField] private int upgradesPerLevel;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; } // Singleton
        Instance = this;
    }

    private void OnEnable()
    {
        levelController.LevelUpEvent += InitializeUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished += CloseUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished += UpdatePlayerStats;
    }
    private void OnDisable()
    {
        levelController.LevelUpEvent -= InitializeUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished -= CloseUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished -= UpdatePlayerStats;
        
    }

    private void Start()
    {
        InitializeStats();
        Time.timeScale = 1f;
        upgradeUI.SetActive(false);
    }

    private void InitializeStats()
    {
        extraHp  = startingStats.extraHp;
        
        flatDamage = startingStats.flatDamage;
        damageMultiplier = startingStats.damageMultiplier;
        
        flatKnockback = startingStats.flatKnockback;
        knockbackMultiplier = startingStats.knockbackMultiplier;
        
        movementSpeed = startingStats.movementSpeed;
        
        xpMultiplier = startingStats.xpMultiplier;
        pickUpRange = startingStats.pickUpRange;
        inventorySlots = startingStats.inventorySlots;
        
        spellCastDelay = startingStats.spellCastDelay;
        spellCastAmount = startingStats.spellCastAmount;
        
        UpdatePlayerStats();
    }

    private void InitializeUpgradeMenu()
    {
        //clear upgrades after upgrading
        foreach (Transform transform in upgradeUI.GetComponentsInChildren<Transform>())
        {
            if (transform != upgradeUI.transform)
                Destroy(transform.gameObject);
        }
        
        List<int> alreadyPickedList = new List<int>();
        for (int i = 0; i < upgradesPerLevel; i++)
        {
            int randomIndex = Random.Range(0, upgradesList.Count);
            if (alreadyPickedList.Count < upgradesList.Count) //to prevent it going in an infitite loop
            {
                while (alreadyPickedList.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, upgradesList.Count);
                }
            }
            alreadyPickedList.Add(randomIndex);
            Instantiate(upgradesList[randomIndex], upgradeUI.transform);
        }
        
        Time.timeScale = 0f;
        upgradeUI.SetActive(true);
    }
    
    private void CloseUpgradeMenu()
    {
        Time.timeScale = 1f;
        upgradeUI.SetActive(false);
    }

    private void UpdatePlayerStats()
    {
        playerStatsText.text = $"+{extraHp}\n \n" +
                               $"+{flatDamage}\n" +
                               $"x{damageMultiplier}\n \n" +
                               $"+{flatKnockback}\n" +
                               $"x{knockbackMultiplier}\n \n" +
                               $"+{movementSpeed}\n \n" +
                               $"x{xpMultiplier}\n" +
                               $"+{pickUpRange}\n \n" +
                               $"+{spellCastAmount}\n" +
                               $"{spellCastDelay}";
    }
}
