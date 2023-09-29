using UnityEngine;
using UnityEngine.EventSystems;

public class CellClickHandler : MonoBehaviour, IPointerClickHandler
{
    private InventoryCell inventoryCell; // ������ �� ��������� InventoryCell, ��������� � ������ �������
    private ItemRemover itemRemover;

    private void Start()
    {
        // �������� ��������� InventoryCell, �������������� � ���� ������
        inventoryCell = GetComponent<InventoryCell>();

        // �������� ��������� ItemRemover
        itemRemover = ItemInfoPanel.Instance.GetComponentInChildren<ItemRemover>();

        // ���������, ��� ��������� InventoryCell ����������
        if (inventoryCell == null)
        {
            Debug.LogError("��������� InventoryCell �� ������ �� �������: " + gameObject.name);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || inventoryCell.IsEmpty)
        {
            return;
        }

        // ��������� ���� � ����������� � ��������
        ItemInfoPanel.Instance.ShowItemInfo(inventoryCell.GetItemSprite(), inventoryCell.GetItemName(), inventoryCell.GetItemDescription());

        // ����������� ItemRemover ��� �������� �������� �� ������� ������
        itemRemover.SetCellToRemoveItem(inventoryCell);
    }
}
