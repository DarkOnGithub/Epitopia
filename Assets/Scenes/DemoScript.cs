using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemToPickup;

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemToPickup[id]);
        if (result)
        {
            Debug.Log("Item added");
        }
        else
        {
            Debug.Log("Item not added");
        }
    }

    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectecItem(false);
        if (receivedItem != null)
        {

            Debug.Log("Received item" + receivedItem);
        }
        else
        {
            Debug.Log("No item received");
        }
    }

    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectecItem(true);
        if (receivedItem != null)
        {

            Debug.Log("Used item" + receivedItem);
        }
        else
        {
            Debug.Log("No item used");
        }
    }
}
