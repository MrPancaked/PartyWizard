using Player;
using UnityEngine;

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
        hpController.DeathEvent += DropItem;
    }

    private void OnDisable()
    {
        hpController.DeathEvent -= DropItem;
    }
    private void DropItem()
    {
        foreach (GameObject item in items)
        {
            GameObject droppedItem = Instantiate(item, transform.position, Quaternion.identity);
            Debug.Log($"dropped item: {droppedItem.name}");
            Rigidbody2D itemRb = droppedItem.GetComponent<Rigidbody2D>();
            if (itemRb != null) itemRb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
        }
    }
}
