using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage; // Ссылка на компонент изображения предмета
    [SerializeField] private TMP_Text itemNameText; // Ссылка на текстовое поле для имени предмета
    [SerializeField] private TMP_Text itemDescriptionText; // Ссылка на текстовое поле для описания предмета
    [SerializeField] private Button deleteButton; // Кнопка для удаления предмета
    [SerializeField] private ItemRemover itemRemover; // Скрипт для удаления предмета

    public static ItemInfoPanel Instance; // Ссылка на экземпляр панели информации о предмете, доступный из других скриптов

    private void Awake()
    {
        Instance = this; // Устанавливаем экземпляр панели информации при её создании
        gameObject.SetActive(false); // При старте скрываем панель информации
        deleteButton.onClick.AddListener(OnDeleteButtonClick); // Устанавливаем слушатель для нажатия кнопки "Delete"
    }

    // Отображение информации о предмете
    public void ShowItemInfo(Sprite itemSprite, string itemName, string itemDescription)
    {
        itemImage.sprite = itemSprite; // Устанавливаем изображение предмета
        itemNameText.text = itemName; // Устанавливаем имя предмета
        itemDescriptionText.text = itemDescription; // Устанавливаем описание предмета
        gameObject.SetActive(true); // Показываем панель информации
    }

    // Скрытие панели информации о предмете
    public void HideItemInfo()
    {
        gameObject.SetActive(false); // Скрываем панель информации
    }

    // Обработчик нажатия кнопки "Delete" для удаления предмета
    private void OnDeleteButtonClick()
    {
        itemRemover.RemoveItem(); // Вызываем метод для удаления предмета из инвентаря
    }
}
