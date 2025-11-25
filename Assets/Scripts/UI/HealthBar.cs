using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField] private GameObject startFull;
    [SerializeField] private GameObject startEmpty;
    [SerializeField] private GameObject middleFull;
    [SerializeField] private GameObject middleEmpty;
    [SerializeField] private GameObject endFull;
    [SerializeField] private GameObject endEmpty;

    private List<GameObject> hpPointList;
    private int internalHpCounter;
    private int internalMaxHp;

    private void Start()
    {
        hpPointList = new List<GameObject>();
        internalHpCounter = hpController.hp;
        internalMaxHp = hpController.hpData.maxHp;
        for (int i = 0; i < internalMaxHp; i++)
        {
            GameObject hpPoint;
            if (i == 0) 
            {
                if (internalHpCounter == 0) hpPoint = startEmpty;
                else hpPoint = startFull;
            }
            else if (i < internalMaxHp - 1)
            {
                if (i < internalHpCounter) hpPoint = middleFull;
                else hpPoint = middleEmpty;
            }
            else 
            {
                if (internalHpCounter == internalMaxHp) hpPoint = endFull;
                else hpPoint = endEmpty;
            }
            hpPointList.Add(hpPoint);
        }
        DrawHealthBar(hpPointList, horizontalLayoutGroup);
    }
    private void OnEnable()
    {
        hpController.TakeDamageEvent += UpdateHealthBar;
    }
    private void OnDisable()
    {
        hpController.TakeDamageEvent -= UpdateHealthBar;
    }
    private void UpdateHealthBar(int damage)
    {
        for (int i = internalHpCounter; i > internalHpCounter - damage; i--)
        {
            if (i - 1 == 0) 
            {
                hpPointList[i-1] = startEmpty;
            }
            else if (i == internalMaxHp) 
            {
                hpPointList[i-1] = endEmpty;
            }
            else 
            {
                hpPointList[i-1] = middleEmpty;
            }
            Debug.Log($"healthpoint {i} updated: {hpPointList[i]}");
        }
        internalHpCounter = hpController.hp;
        DrawHealthBar(hpPointList, horizontalLayoutGroup);
    }

    private void DrawHealthBar(List<GameObject> hpPointList, HorizontalLayoutGroup horizontalLayout)
    {
        List<Image> sceneHpPointList = horizontalLayout.GetComponentsInChildren<Image>().ToList();
        int i = 0;
        while (sceneHpPointList.Count > 0 && i < 100)
        {
            Destroy(sceneHpPointList[0].gameObject);
            sceneHpPointList.RemoveAt(0);
            i++;
            //Debug.Log($"HLG lenght: {sceneHpPointList.Count}, i = {i}");
        }
        foreach (GameObject hpPoint in hpPointList)
        {
            Instantiate(hpPoint, horizontalLayout.transform);
            Debug.Log($"hp point: {hpPoint.name}");
        }
    }
}
