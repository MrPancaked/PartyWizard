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
        levelController.XPEvent += UpdateXpBar;
        levelController.LevelUpEvent += ClearXpBar;
    }

    private void OnDisable()
    {
        levelController.XPEvent -= UpdateXpBar;
        levelController.LevelUpEvent -= ClearXpBar;
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
    private void UpdateXpBar(int xpGain)
    {
        for (int i = internalXpCounter; i <= internalXpCounter + xpGain; i++)
        {
            Animator anim;
            if (i == 0) anim = xpPointList[i].GetComponent<Animator>();
            else anim =  xpPointList[i-1].GetComponent<Animator>();
            if (i == 1)
            {
                anim.Play("XpBarStart");
            }
            else if (i > 1 && i < internalMaxXp)
            {
                anim.Play("XpBarMiddle");
            }
            else if (i == internalMaxXp)
            {
                anim.Play("XpBarEnd");
                AnimatorClipInfo[] clipInfo = xpPointList[internalMaxXp-1].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            }
            anim.Update(0f);
        }
        internalXpCounter = levelController.xpCount;
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

    private void ClearXpBar()
    {
        for (int i = 1; i <= internalMaxXp; i++)
        {
            Animator anim = xpPointList[i - 1].GetComponent<Animator>();
            if (i == 1)
            {
                anim.Play("XpStartEmpty");
            }
            else if (i < internalMaxXp)
            {
                anim.Play("XpMiddleEmpty");
            }
            else if (i == internalMaxXp)
            {
                anim.Play("XpEndEmpty");
                AnimatorClipInfo[] clipInfo = xpPointList[internalMaxXp-1].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            }
            anim.Update(0f); //needed to force update the animation for if animation gets switched twice
        }
        internalXpCounter = levelController.xpCount;
    }
}
