using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class manages modular HpBars, by changing out the modular HpBar Sprites based on the attached HpControllers hp variable.
 * It can be used to generate various modular healthbars and in this project it is used to generate the healthbar for the player, boss and general enemies.
 */
public class HealthBar : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField] private Sprite singleFull;
    [SerializeField] private Sprite startFull;
    [SerializeField] private Sprite startEmpty;
    [SerializeField] private Sprite middleFull;
    [SerializeField] private Sprite middleEmpty;
    [SerializeField] private Sprite endFull;
    [SerializeField] private Sprite endEmpty;
    [SerializeField] private GameObject healthPointObject;
    
    [SerializeField] private AnimationCurve animationCurve;

    private List<Image> hpPointList;
    private int internalHpCounter;
    private int internalMaxHp;

    private void Start()
    {
        ClearHorizontalLayout();
        if (hpController != null) InitiateHealthBar();
    }
    private void OnEnable()
    {
        if (gameObject.CompareTag("BossHealthBar")) //there is a specific healthbar for the boss that needs to be instantiated while playing the game
        {
            HpController.InitiateBossHealthBar += InitiateBossHealthBar;
        }
        else
        {
            hpController.TakeDamageEvent += UpdateHealthBar;
            hpController.HealEvent += UpdateHealthBar;
            hpController.DeathEvent += ClearHorizontalLayout;
            hpController.UpdatedMaxHealth += InitiateHealthBar;
        }
        
    }
    private void OnDisable()
    {
        if (gameObject.CompareTag("BossHealthBar"))
        {
            HpController.InitiateBossHealthBar -= InitiateBossHealthBar;
        }
        else
        {
            hpController.TakeDamageEvent -= UpdateHealthBar;
            hpController.HealEvent -= UpdateHealthBar;
            hpController.DeathEvent -= ClearHorizontalLayout;
            hpController.UpdatedMaxHealth -= InitiateHealthBar;
        }
    }

    private void InitiateHealthBar()
    {
        //make sure the HpBar is empty
        ClearHorizontalLayout();
        hpPointList = new List<Image>();
        internalHpCounter = hpController.hp;
        internalMaxHp = hpController.maxHp;

        //creating the list of hp bar images to be able to swap out the sprites later
        for (int i = 0; i < internalMaxHp; i++)
        {
            GameObject hpObject = Instantiate(healthPointObject, horizontalLayoutGroup.transform);
            hpPointList.Add(hpObject.GetComponent<Image>());
        }
        
        //assigning sprites to the images of the hpbar
        for (int i = 0; i < internalMaxHp; i++)
        {
            if (internalMaxHp == 1) hpPointList[i].sprite = singleFull;
            else if (i == 0)
            {
                if (internalHpCounter == 0) hpPointList[i].sprite = startEmpty;
                else hpPointList[i].sprite = startFull;
            }
            else if (i < internalMaxHp - 1)
            {
                if (i < internalHpCounter) hpPointList[i].sprite = middleFull;
                else hpPointList[i].sprite = middleEmpty;
            }
            else 
            {
                if (internalHpCounter == internalMaxHp) hpPointList[i].sprite = endFull;
                else hpPointList[i].sprite = endEmpty;
            }
        }
    }

    //the healthpoints are initialized inside a horizontal layout for automatic positioning, this layour needs to be cleared first
    private void ClearHorizontalLayout()
    {
        List<Image> sceneHpPointList = horizontalLayoutGroup.GetComponentsInChildren<Image>().ToList(); //if I do gameobject type instead of image it gives error
        int i = 0;
        while (sceneHpPointList.Count > 0 && i < 100) //increase 100 in i < 100 if you need enemies with more than 100 hp. this only makes sure there is no infinite loop
        {
            Destroy(sceneHpPointList[0].gameObject);
            sceneHpPointList.RemoveAt(0);
            i++;
        }
    }

    //updating the healthbar when HpController takes damage (or heals)
    private void UpdateHealthBar(TakeDamageData damageData)
    {
        internalHpCounter = hpController.hp;
        internalMaxHp = hpController.maxHp;
        for (int i = 0; i < internalMaxHp; i++)
        {
            if (i == 0)
            {
                if (internalHpCounter == 0) hpPointList[i].sprite = startEmpty;
                else hpPointList[i].sprite = startFull;
            }
            else if (i < internalMaxHp - 1)
            {
                if  (i < internalHpCounter) hpPointList[i].sprite = middleFull;
                else hpPointList[i].sprite = middleEmpty;
            }
            else if (i == internalMaxHp - 1)
            {
                if (internalHpCounter == internalMaxHp) hpPointList[i].sprite = endFull;
                else hpPointList[i].sprite = endEmpty;
            }
        }
    }

    //the Boss' healthbar needs to be initialized while playing
    private void InitiateBossHealthBar()
    {
        hpController = GameObject.FindGameObjectWithTag("Boss").GetComponent<HpController>();
        if (hpController != null)
        {
            hpController.TakeDamageEvent += UpdateHealthBar;
            hpController.HealEvent += UpdateHealthBar;
            hpController.DeathEvent += ClearHorizontalLayout;
            hpController.UpdatedMaxHealth += InitiateHealthBar;
            
            InitiateHealthBar();
        }
    }
}
