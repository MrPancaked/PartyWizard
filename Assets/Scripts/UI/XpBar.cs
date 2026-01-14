using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class manages modular XpBars, by changing out the modular HpBar Sprites based on the attached LevelControllers xpCount variable.
/// It can be used to generate various modular xpBars and in this project it is used to generate the xp bar for the player.
/// Similar to the HealthBar class but this one switches out animations instead of sprites and is not required (yet) to change its amount of xp points after initialization
/// It also doesn't require a separate method for the boss like the HealthBar Class has
/// </summary>
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
        //make sure the Xp bar is cleared
        ClearHorizontalLayout();
        xpPointList = new List<GameObject>();
        internalXpCounter = levelController.xpCount;
        internalMaxXp = levelController.xpData.xpForLevel;
        
        for (int i = 1; i <= internalMaxXp; i++) //spawn the right modular xp point assets
        {
            GameObject xpObject;
            if (internalMaxXp == 1)
            {
                xpObject = null; //I did not create a sprite / animation object for when the player has a singular xp point.
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
    //clears the layout in which the xp points are spawned
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
    //Updates the Xp bar based on the LevelControllers current xp count
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
