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

    private GameObject start;
    private GameObject middle;
    private GameObject end;

    private void OnEnable()
    {
        levelController.XPEvent += UpdateXPBar;
    }

    private void OnDisable()
    {
        levelController.XPEvent -= UpdateXPBar;
    }
    private void InitiateHealthBar()
    {
        ClearHorizontalLayout();
        xpPointList = new List<GameObject>();
        internalXpCounter = levelController.xpData.xpCount;
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
                Instantiate(xpObject, horizontalLayoutGroup.transform);
                xpPointList.Add(xpObject);
            }
            else Debug.Log($"no single XP sprite found");
        }
    }
    private void UpdateXPBar(int xpGain)
    {
        for (int i = internalXpCounter; i < internalXpCounter + xpGain; i++)
        {
            if (i == 1)
            {
                xpPointList[i].GetComponent<Animation>().Play("XpStartFull");
            }

            if (i < internalMaxXp)
            {
                xpPointList[i].GetComponent<Animation>().Play("XpMiddleFull");
            }

            if (i == internalMaxXp)
            {
                xpPointList[i].GetComponent<Animation>().Play("XpEndFull");
            }
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
}
