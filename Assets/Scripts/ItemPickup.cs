using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Inventory inventoryComponent; // Ссылка на компонент Inventory
    [SerializeField] private bool isStackable = false;
    [SerializeField] private string itemDescription = "";

    private void Start()
    {
        // Получаем компонент Inventory один раз при старте
        inventoryComponent = GameObject.Find("Player")?.GetComponent<Inventory>();
        if (inventoryComponent == null)
        {
            Debug.LogError("Отсутствует компонент Inventory на объекте инвентаря.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer itemRenderer = GetComponent<SpriteRenderer>();

            if (itemRenderer != null && inventoryComponent != null)
            {
                string itemName = CleanItemName(gameObject.name);
                Sprite itemSprite = itemRenderer.sprite;

                if (!inventoryComponent.IsFull())
                {
                    // Добавить предмет в инвентарь, передав флаг стакаемости и описание
                    inventoryComponent.AddItem(itemName, itemSprite, isStackable, itemDescription);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Инвентарь полон. Нельзя добавить предмет " + itemName + ".");
                }
            }
            else
            {
                Debug.LogError("Отсутствует компонент SpriteRenderer на объекте.");
            }
        }
    }

    private string CleanItemName(string originalName)
    {
        int openBracketIndex = originalName.IndexOf('(');

        if (openBracketIndex != -1)
        {
            return originalName.Substring(0, openBracketIndex).Trim();
        }

        return originalName;
    }
}
