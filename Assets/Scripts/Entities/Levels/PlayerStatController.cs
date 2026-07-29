using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// This class keeps track of the players upgraded stats and gets the upgrade values from the inspector
/// It also handles the upgrade menu on levelup.
/// Not all upgrades are implemented yet.
/// </summary>
public class PlayerStatController : MonoBehaviour
{
    public static PlayerStatController Instance;
    
    public PlayerStats startingStats;
    
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
    [SerializeField] private GameObject UpgradeLayout;
    [SerializeField] private TextMeshProUGUI playerStatsText;
    [SerializeField] private List<GameObject> upgradesList;
    [SerializeField] private List<GameObject> specialUpgradesList;
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
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished += UpdatePlayerStatsText;
    }
    private void OnDisable()
    {
        levelController.LevelUpEvent -= InitializeUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished -= CloseUpgradeMenu;
        EventBus<PlayerUpgradeEventData>.OnNoParamEventPublished -= UpdatePlayerStatsText;
    }

    private void Start()
    {
        InitializeStats();
        Time.timeScale = 1f;
        if (upgradeUI != null) upgradeUI.SetActive(false);
    }

    public void InitializeStats()
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
        
        UpdatePlayerStatsText();
    }

    private void InitializeUpgradeMenu()
    {
        if (UpgradeLayout != null)
        {
            //clear upgrades after upgrading
            foreach (Transform transform in UpgradeLayout.GetComponentsInChildren<Transform>())
            {
                if (transform != UpgradeLayout.transform)
                    Destroy(transform.gameObject);
            }

            if (levelController.level % 5 == 0)
            {
                ChooseUpgradesFromList(specialUpgradesList, 1);
            }
            else
            {
                ChooseUpgradesFromList(upgradesList, upgradesPerLevel);
            }
            
            GameManager.Instance.PauseEvent.Invoke();
            Time.timeScale = 0f;
            upgradeUI.SetActive(true);
        }
        else {
             Debug.LogWarning($"{this.name}: No upgrade UI found");
        }
    }

    private void ChooseUpgradesFromList(List<GameObject> upgradesList, int upgradesAmount)
    {
        List<int> alreadyPickedList = new List<int>();
        for (int i = 0; i < upgradesAmount; i++)
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
            Instantiate(upgradesList[randomIndex].gameObject, UpgradeLayout.transform);
        }
    }
    
    public void CloseUpgradeMenu()
    {
        GameManager.Instance.UnPauseEvent.Invoke();
        Time.timeScale = 1f;
        upgradeUI.SetActive(false);
    }

    private void UpdatePlayerStatsText()
    {
        if (playerStatsText != null)
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

    public void InstantKillUpgrade(bool on)
    {
        if (on)
        {
            flatDamage += 999;
            damageMultiplier += 999;
        }
        else
        {
            flatDamage -= 999;
            damageMultiplier -= 999;
        }
        UpdatePlayerStatsText();
    }
}
