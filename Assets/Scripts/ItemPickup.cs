using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Inventory inventoryComponent; // ������ �� ��������� Inventory
    [SerializeField] private bool isStackable = false;
    [SerializeField] private string itemDescription = "";

    private void Start()
    {
        // �������� ��������� Inventory ���� ��� ��� ������
        inventoryComponent = GameObject.Find("Player")?.GetComponent<Inventory>();
        if (inventoryComponent == null)
        {
            Debug.LogError("����������� ��������� Inventory �� ������� ���������.");
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
                    // �������� ������� � ���������, ������� ���� ����������� � ��������
                    inventoryComponent.AddItem(itemName, itemSprite, isStackable, itemDescription);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("��������� �����. ������ �������� ������� " + itemName + ".");
                }
            }
            else
            {
                Debug.LogError("����������� ��������� SpriteRenderer �� �������.");
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
