using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
    private List<GameObject> xpPointList;
    private int internalXpCounter;
    private int internalMaxXp;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject end;

    private void Start()
    {
        InitiateXpBar();
    }
    private void OnEnable()
    {
        levelController.XpEvent += UpdateXpBar;
        levelController.LevelUpEvent += UpdateXpBar;
    }

    private void OnDisable()
    {
        levelController.XpEvent -= UpdateXpBar;
        levelController.LevelUpEvent -= UpdateXpBar;
    }
    private void InitiateXpBar()
    {
        ClearHorizontalLayout();
        xpPointList = new List<GameObject>();
        internalXpCounter = levelController.xpCount;
        internalMaxXp = levelController.xpData.xpForLevel;
        
        for (int i = 1; i <= internalMaxXp; i++)
        {
            GameObject xpObject;
            if (internalMaxXp == 1)
            {
                xpObject = null;
            }
            else if (i == 1)
            {
                xpObject = start;
            }
            else if (i < internalMaxXp)
            {
                xpObject = middle;
            }
            else
            {
                xpObject = end;
            }
            if (xpObject != null)
            {
                GameObject instantiatedXP = Instantiate(xpObject, horizontalLayoutGroup.transform);
                xpPointList.Add(instantiatedXP);
            }
            else Debug.Log($"no single XP sprite found");
        }
    }
    private void ClearHorizontalLayout()
    {
        List<Image> sceneXpPointList = horizontalLayoutGroup.GetComponentsInChildren<Image>().ToList(); //if I do gameobject type instead of image it gives error
        int i = 0;
        while (sceneXpPointList.Count > 0 && i < 100) //increase 100 in i < 100 if you need enemies with more than 100 hp. this only makes sure there is no infinite loop
        {
            Destroy(sceneXpPointList[0].gameObject);
            sceneXpPointList.RemoveAt(0);
            i++;
        }
    }
    private void UpdateXpBar()
    {
        internalXpCounter = levelController.xpCount;
        internalMaxXp = levelController.xpData.xpForLevel;
        for (int i = 0; i < internalMaxXp; i++)
        {
            Animator anim = xpPointList[i].GetComponent<Animator>();
            if (i == 0)
            {
                if (internalXpCounter == 0) anim.Play("XpStartEmpty");
                else anim.Play("XpBarStart");
            }
            else if (i < internalMaxXp - 1)
            {
                if  (i < internalXpCounter) anim.Play("XpBarMiddle");
                else anim.Play("XpMiddleEmpty");
            }
            else if (i == internalMaxXp - 1)
            {
                if (internalXpCounter == internalMaxXp) anim.Play("XpBarEnd");
                else anim.Play("XpEndEmpty");
            }
            anim.Update(0f);
        }
    }
}
