using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        InitiateHealthBar();
    }
    private void OnEnable()
    {
        hpController.TakeDamageEvent += UpdateHealthBar;
        hpController.HealEvent += UpdateHealthBar;
        if (hpController.isPlayer)
        {
            EventBus<ExtraHpUpgradeEventData>.OnNoParamEventPublished += InitiateHealthBar;
        }
    }
    private void OnDisable()
    {
        hpController.TakeDamageEvent -= UpdateHealthBar;
        hpController.HealEvent -= UpdateHealthBar;
        if (hpController.isPlayer)
        {
            EventBus<ExtraHpUpgradeEventData>.OnNoParamEventPublished -= InitiateHealthBar;
        }
    }

    private void InitiateHealthBar()
    {
        ClearHorizontalLayout();
        hpPointList = new List<Image>();
        internalHpCounter = hpController.hp;
        internalMaxHp = hpController.maxHp;

        for (int i = 0; i < internalMaxHp; i++)
        {
            GameObject hpObject = Instantiate(healthPointObject, horizontalLayoutGroup.transform);
            hpPointList.Add(hpObject.GetComponent<Image>());
        }
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
}
