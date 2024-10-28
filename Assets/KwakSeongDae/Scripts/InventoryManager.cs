using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> items;
    public List<GameObject> Items
    {
        get { return items; }
        set
        {
            items = value;
            OnItemsChanged?.Invoke(items);
        }
    }

    private UnityAction<List<GameObject>> OnItemsChanged;

    public void InventoryUpdate()
    {

    }
}
