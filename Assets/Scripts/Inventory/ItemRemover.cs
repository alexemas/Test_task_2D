using UnityEngine;

public class ItemRemover : MonoBehaviour
{
    private InventoryCell cellToRemoveItemFrom;

    public void SetCellToRemoveItem(InventoryCell cell)
    {
        cellToRemoveItemFrom = cell;
    }

    public void RemoveItem()
    {
        if (cellToRemoveItemFrom != null)
        {
            // Удаляем предмет из инвентаря
            Inventory.Instance.RemoveItem(cellToRemoveItemFrom);

            // Проверяем, является ли предмет стакуемым
            if (!cellToRemoveItemFrom.IsStackable)
            {
                // Если предмет не стакуемый, скрываем информацию о предмете
                ItemInfoPanel.Instance.HideItemInfo();
            }
        }
    }
}
