using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private Image itemIcon;      // Иконка предмета в ячейке
    [SerializeField] private TMP_Text itemCountText; // Текст для отображения количества стаков

    private string itemName;    // Название предмета
    private int stackCount;     // Количество стаков
    private bool isStackable;   // Флаг, указывающий, можно ли стакать предметы
    private string itemDescription; // Описание предмета

    // Свойство, указывающее, является ли ячейка пустой
    public bool IsEmpty { get { return string.IsNullOrEmpty(itemName) || stackCount <= 0; } }

    // Свойство, указывающее, можно ли стакать предметы в этой ячейке
    public bool IsStackable { get { return isStackable; } }

    // Свойство для получения количества стаков в ячейке
    public int StackCount { get { return stackCount; } }

    // Максимальное количество стаков 
    public int MaxStackCount { get { return 99; } }

    // Получить название предмета
    public string GetItemName()
    {
        return itemName;
    }

    // Получить описание предмета
    public string GetItemDescription()
    {
        return itemDescription;
    }

    // Получить спрайт предмета
    public Sprite GetItemSprite()
    {
        return itemIcon.sprite;
    }

    // Установить информацию о предмете
    public void SetItem(string newItemName, Sprite itemSprite, int newStackCount, bool newIsStackable, string newItemDescription)
    {
        itemName = newItemName;
        stackCount = newStackCount;
        isStackable = newIsStackable;
        itemDescription = newItemDescription;
        itemIcon.sprite = itemSprite;
        UpdateItemCountText();
    }

    // Увеличить количество стаков на указанное количество
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

    // Удаление предмета
    public void RemoveItem(int amount = 1)
    {
        if (isStackable && stackCount > 0)
        {
            stackCount -= amount;

            // Если остался всего один предмет, убираем его полностью из инвентаря
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

    // Очистить ячейку (сделать ее пустой)
    public void ClearItem()
    {
        itemName = null;
        stackCount = 0;
        itemDescription = null;
        itemIcon.sprite = null;
        UpdateItemCountText();
    }

    // Обновить отображение количества стаков в тексте
    private void UpdateItemCountText()
    {
        itemCountText.text = isStackable ? stackCount.ToString() : "";
        itemCountText.gameObject.SetActive(isStackable && stackCount > 1);
    }
}
