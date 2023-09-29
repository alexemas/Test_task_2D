using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Inventory : MonoBehaviour
{
    private string saveFilePath;
    [SerializeField] private List<InventoryCell> cells = new List<InventoryCell>(); // Список ячеек инвентаря

    public static Inventory Instance; // Ссылка на экземпляр инвентаря, доступный из других скриптов
    private InventoryData inventoryData = new InventoryData(); // Объявляем поле для хранения данных инвентаря

    private void Awake()
    {
        Instance = this; // Устанавливаем экземпляр инвентаря при его создании
        saveFilePath = Path.Combine(Application.persistentDataPath, "Inventory.json");
        LoadInventory();
    }

    // Сохранить инвентарь в файл
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

    // Загрузить инвентарь из файла
    public void LoadInventory()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

            foreach (var itemData in inventoryData.items)
            {
                Sprite itemSprite = Resources.Load<Sprite>("Items/" + itemData.spriteName); // Загрузка спрайта по имени
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

    // Добавление предмета в инвентарь
    public void AddItem(string itemName, Sprite itemSprite, bool isStackable, string itemDescription)
    {
        // Поиск всех ячеек с таким же предметом
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

                        // Обновляем информацию в InventoryItemData
                        foreach (var itemData in inventoryData.items)
                        {
                            if (itemData.itemName == itemName)
                            {
                                itemData.stackCount = cell.StackCount; // Обновляем количество стаков
                                break;
                            }
                        }

                        Debug.Log("Добавлено " + quantityToAdd + " предметов " + itemName + " в инвентарь.");
                        SaveInventory();
                        return; // Выходим из метода после добавления предмета
                    }
                }
            }
        }

        // Если нет подходящей ячейки для стака или предмет не стакуемый
        InventoryCell emptyCell = cells.FirstOrDefault(cell => cell.IsEmpty);

        if (emptyCell != null)
        {
            emptyCell.SetItem(itemName, itemSprite, 1, isStackable, itemDescription);

            // Создаем новый InventoryItemData для предмета
            InventoryItemData newItemData = new InventoryItemData(
                itemName,
                emptyCell.StackCount, // Используем количество стаков из ячейки
                isStackable,
                itemDescription,
                itemSprite.name
            );

            inventoryData.items.Add(newItemData);

            Debug.Log("Предмет " + itemName + " добавлен в инвентарь.");
            SaveInventory();
        }
        else
        {
            Debug.Log("Инвентарь полон. Нельзя добавить предмет " + itemName + ".");
        }
    }

    // Создание новой ячейки для предмета
    private void CreateNewStack(string itemName, Sprite itemSprite, bool isStackable, string itemDescription)
    {
        InventoryCell newCell = cells.FirstOrDefault(cell => cell.IsEmpty);
        if (newCell != null)
        {
            newCell.SetItem(itemName, itemSprite, 1, isStackable, itemDescription);
        }
    }

    // Проверка, полон ли инвентарь
    public bool IsFull()
    {
        return cells.All(cell => !cell.IsEmpty);
    }

    // Удаление предмета из инвентаря
    public void RemoveItem(InventoryCell cellToRemove)
    {
        if (cells.Contains(cellToRemove))
        {
            cellToRemove.RemoveItem();
            SaveInventory();
        }
    }

    // Получение ячеек с патронами определенного типа
    public InventoryCell[] GetAmmoCells(string ammoType)
    {
        return cells.Where(cell => !cell.IsEmpty && cell.GetItemName() == ammoType).ToArray();
    }
}
