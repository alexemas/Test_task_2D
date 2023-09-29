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
            // ������� ������� �� ���������
            Inventory.Instance.RemoveItem(cellToRemoveItemFrom);

            // ���������, �������� �� ������� ���������
            if (!cellToRemoveItemFrom.IsStackable)
            {
                // ���� ������� �� ���������, �������� ���������� � ��������
                ItemInfoPanel.Instance.HideItemInfo();
            }
        }
    }
}
