using Player;
using UnityEngine;
/// <summary>
/// This class drops an array of items when its HpController dies
/// </summary>
public class DropItems : MonoBehaviour
{
    [SerializeField] private HpController hpController;
    [SerializeField] private GameObject[] items;

    private void Awake()
    {
        hpController = GetComponent<HpController>();
    }

    private void OnEnable()
    {
        if (hpController != null) 
            hpController.DeathEvent += DropItem;
    }

    private void OnDisable()
    {
        if  (hpController != null)
            hpController.DeathEvent -= DropItem;
    }
    public void DropItem()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        foreach (GameObject item in items)
        {
            GameObject droppedItem = Instantiate(item, transform.position, Quaternion.identity);
            Debug.Log($"dropped item: {droppedItem.name}");
            Rigidbody2D itemRb = droppedItem.GetComponent<Rigidbody2D>();
            if (itemRb != null) itemRb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse); // make item drop in a random direction
        }
    }
}
