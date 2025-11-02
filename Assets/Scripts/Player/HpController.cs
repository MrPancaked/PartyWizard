using UnityEngine;

public class HpController : MonoBehaviour
{
    public int hp;
    public int shield;

    private PlayerController playerController;
    
    [HideInInspector] public HpData hpData; //public so playercontroller can update the controller data classes
    
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        hpData = playerController.hpData;
        Initialize();
    }
    public void Initialize()
    {
        hp = hpData.startHp;
        shield = hpData.startShield;
    }
}
