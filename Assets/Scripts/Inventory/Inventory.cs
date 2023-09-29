using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Inventory : MonoBehaviour
{
    private string saveFilePath;
    [SerializeField] private List<InventoryCell> cells = new List<InventoryCell>(); // ������ ����� ���������

    public static Inventory Instance; // ������ �� ��������� ���������, ��������� �� ������ ��������
    private InventoryData inventoryData = new InventoryData(); // ��������� ���� ��� �������� ������ ���������

    private void Awake()
    {
        Instance = this; // ������������� ��������� ��������� ��� ��� ��������
        saveFilePath = Path.Combine(Application.persistentDataPath, "Inventory.json");
        LoadInventory();
    }

    // ��������� ��������� � ����
    public void SaveInventory()
    {
        InventoryData inventoryData = new InventoryData();
        for (int i = 0; i < cells.Count; i++)
        {
            InventoryCell cell = cells[i];
            if (!cell.IsEmpty)
            {
                InventoryItemData itemData = new InventoryItemData(
                    cell.GetItemName(),
                    cell.StackCount,
                    cell.IsStackable,
                    cell.GetItemDescription(),
                    cell.GetItemSprite().name
                );
                inventoryData.items.Add(itemData);
            }
        }

        string json = JsonUtility.ToJson(inventoryData);
        File.WriteAllText(saveFilePath, json);
    }

    // ��������� ��������� �� �����
    public void LoadInventory()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

            foreach (var itemData in inventoryData.items)
            {
                Sprite itemSprite = Resources.Load<Sprite>("Items/" + itemData.spriteName); // �������� ������� �� �����
                AddItem(itemData.itemName, itemSprite, itemData.isStackable, itemData.itemDescription);
                InventoryCell[] matchingCells = cells.Where(cell => !cell.IsEmpty && cell.GetItemName() == itemData.itemName).ToArray();
                foreach (var cell in matchingCells)
                {
                    if (cell.StackCount < itemData.stackCount)
                    {
                        int quantityToAdd = Mathf.Min(itemData.stackCount - cell.StackCount, 1);
                        cell.IncreaseStack(quantityToAdd);
                    }
                }
            }
        }
    }

    // ���������� �������� � ���������
    public void AddItem(string itemName, Sprite itemSprite, bool isStackable, string itemDescription)
    {
        // ����� ���� ����� � ����� �� ���������
        List<InventoryCell> matchingCells = cells.Where(cell => !cell.IsEmpty && cell.GetItemName() == itemName).ToList();

        if (matchingCells.Count > 0)
        {
            foreach (var cell in matchingCells)
            {
                if (isStackable)
                {
                    if (cell.StackCount < 99)
                    {
                        int spaceLeftInStack = 99 - cell.StackCount;
                        int quantityToAdd = Mathf.Min(spaceLeftInStack, 1);
                        cell.IncreaseStack(quantityToAdd);

                        // ��������� ���������� � InventoryItemData
                        foreach (var itemData in inventoryData.items)
                        {
                            if (itemData.itemName == itemName)
                            {
                                itemData.stackCount = cell.StackCount; // ��������� ���������� ������
                                break;
                            }
                        }

                        Debug.Log("��������� " + quantityToAdd + " ��������� " + itemName + " � ���������.");
                        SaveInventory();
                        return; // ������� �� ������ ����� ���������� ��������
                    }
                }
            }
        }

        // ���� ��� ���������� ������ ��� ����� ��� ������� �� ���������
        InventoryCell emptyCell = cells.FirstOrDefault(cell => cell.IsEmpty);

        if (emptyCell != null)
        {
            emptyCell.SetItem(itemName, itemSprite, 1, isStackable, itemDescription);

            // ������� ����� InventoryItemData ��� ��������
            InventoryItemData newItemData = new InventoryItemData(
                itemName,
                emptyCell.StackCount, // ���������� ���������� ������ �� ������
                isStackable,
                itemDescription,
                itemSprite.name
            );

            inventoryData.items.Add(newItemData);

            Debug.Log("������� " + itemName + " �������� � ���������.");
            SaveInventory();
        }
        else
        {
            Debug.Log("��������� �����. ������ �������� ������� " + itemName + ".");
        }
    }

    // �������� ����� ������ ��� ��������
    private void CreateNewStack(string itemName, Sprite itemSprite, bool isStackable, string itemDescription)
    {
        InventoryCell newCell = cells.FirstOrDefault(cell => cell.IsEmpty);
        if (newCell != null)
        {
            newCell.SetItem(itemName, itemSprite, 1, isStackable, itemDescription);
        }
    }

    // ��������, ����� �� ���������
    public bool IsFull()
    {
        return cells.All(cell => !cell.IsEmpty);
    }

    // �������� �������� �� ���������
    public void RemoveItem(InventoryCell cellToRemove)
    {
        if (cells.Contains(cellToRemove))
        {
            cellToRemove.RemoveItem();
            SaveInventory();
        }
    }

    // ��������� ����� � ��������� ������������� ����
    public InventoryCell[] GetAmmoCells(string ammoType)
    {
        return cells.Where(cell => !cell.IsEmpty && cell.GetItemName() == ammoType).ToArray();
    }
}
