using UnityEngine;
using UnityEngine.EventSystems;

public class CellClickHandler : MonoBehaviour, IPointerClickHandler
{
    private InventoryCell inventoryCell; // Ссылка на компонент InventoryCell, связанный с данной ячейкой
    private ItemRemover itemRemover;

    private void Start()
    {
        // Получаем компонент InventoryCell, присоединенный к этой ячейке
        inventoryCell = GetComponent<InventoryCell>();

        // Получаем компонент ItemRemover
        itemRemover = ItemInfoPanel.Instance.GetComponentInChildren<ItemRemover>();

        // Проверяем, что компонент InventoryCell существует
        if (inventoryCell == null)
        {
            Debug.LogError("Компонент InventoryCell не найден на объекте: " + gameObject.name);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || inventoryCell.IsEmpty)
        {
            return;
        }

        // Открываем окно с информацией о предмете
        ItemInfoPanel.Instance.ShowItemInfo(inventoryCell.GetItemSprite(), inventoryCell.GetItemName(), inventoryCell.GetItemDescription());

        // Настраиваем ItemRemover для удаления предмета из текущей ячейки
        itemRemover.SetCellToRemoveItem(inventoryCell);
    }
}
