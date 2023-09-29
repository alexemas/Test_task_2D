using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private Image itemIcon;      // ������ �������� � ������
    [SerializeField] private TMP_Text itemCountText; // ����� ��� ����������� ���������� ������

    private string itemName;    // �������� ��������
    private int stackCount;     // ���������� ������
    private bool isStackable;   // ����, �����������, ����� �� ������� ��������
    private string itemDescription; // �������� ��������

    // ��������, �����������, �������� �� ������ ������
    public bool IsEmpty { get { return string.IsNullOrEmpty(itemName) || stackCount <= 0; } }

    // ��������, �����������, ����� �� ������� �������� � ���� ������
    public bool IsStackable { get { return isStackable; } }

    // �������� ��� ��������� ���������� ������ � ������
    public int StackCount { get { return stackCount; } }

    // ������������ ���������� ������ 
    public int MaxStackCount { get { return 99; } }

    // �������� �������� ��������
    public string GetItemName()
    {
        return itemName;
    }

    // �������� �������� ��������
    public string GetItemDescription()
    {
        return itemDescription;
    }

    // �������� ������ ��������
    public Sprite GetItemSprite()
    {
        return itemIcon.sprite;
    }

    // ���������� ���������� � ��������
    public void SetItem(string newItemName, Sprite itemSprite, int newStackCount, bool newIsStackable, string newItemDescription)
    {
        itemName = newItemName;
        stackCount = newStackCount;
        isStackable = newIsStackable;
        itemDescription = newItemDescription;
        itemIcon.sprite = itemSprite;
        UpdateItemCountText();
    }

    // ��������� ���������� ������ �� ��������� ����������
    public void IncreaseStack(int amount = 1)
    {
        if (isStackable && stackCount < MaxStackCount)
        {
            int spaceLeftInStack = MaxStackCount - stackCount;
            int quantityToAdd = Mathf.Min(spaceLeftInStack, amount);
            stackCount += quantityToAdd;
            UpdateItemCountText();
        }
    }

    // �������� ��������
    public void RemoveItem(int amount = 1)
    {
        if (isStackable && stackCount > 0)
        {
            stackCount -= amount;

            // ���� ������� ����� ���� �������, ������� ��� ��������� �� ���������
            if (stackCount == 0)
            {
                ClearItem();
            }
            else
            {
                UpdateItemCountText();
            }
        }
        else
        {
            ClearItem();
        }
    }

    // �������� ������ (������� �� ������)
    public void ClearItem()
    {
        itemName = null;
        stackCount = 0;
        itemDescription = null;
        itemIcon.sprite = null;
        UpdateItemCountText();
    }

    // �������� ����������� ���������� ������ � ������
    private void UpdateItemCountText()
    {
        itemCountText.text = isStackable ? stackCount.ToString() : "";
        itemCountText.gameObject.SetActive(isStackable && stackCount > 1);
    }
}
