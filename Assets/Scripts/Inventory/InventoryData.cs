using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<InventoryItemData> items = new List<InventoryItemData>();
}

[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int stackCount; // Добавьте поле для количества стаков
    public bool isStackable;
    public string itemDescription;
    public string spriteName;

    public InventoryItemData(string itemName, int stackCount, bool isStackable, string itemDescription, string spriteName)
    {
        this.itemName = itemName;
        this.stackCount = stackCount; // Инициализируйте поле количества стаков
        this.isStackable = isStackable;
        this.itemDescription = itemDescription;
        this.spriteName = spriteName;
    }
}
