using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage; // ������ �� ��������� ����������� ��������
    [SerializeField] private TMP_Text itemNameText; // ������ �� ��������� ���� ��� ����� ��������
    [SerializeField] private TMP_Text itemDescriptionText; // ������ �� ��������� ���� ��� �������� ��������
    [SerializeField] private Button deleteButton; // ������ ��� �������� ��������
    [SerializeField] private ItemRemover itemRemover; // ������ ��� �������� ��������

    public static ItemInfoPanel Instance; // ������ �� ��������� ������ ���������� � ��������, ��������� �� ������ ��������

    private void Awake()
    {
        Instance = this; // ������������� ��������� ������ ���������� ��� � ��������
        gameObject.SetActive(false); // ��� ������ �������� ������ ����������
        deleteButton.onClick.AddListener(OnDeleteButtonClick); // ������������� ��������� ��� ������� ������ "Delete"
    }

    // ����������� ���������� � ��������
    public void ShowItemInfo(Sprite itemSprite, string itemName, string itemDescription)
    {
        itemImage.sprite = itemSprite; // ������������� ����������� ��������
        itemNameText.text = itemName; // ������������� ��� ��������
        itemDescriptionText.text = itemDescription; // ������������� �������� ��������
        gameObject.SetActive(true); // ���������� ������ ����������
    }

    // ������� ������ ���������� � ��������
    public void HideItemInfo()
    {
        gameObject.SetActive(false); // �������� ������ ����������
    }

    // ���������� ������� ������ "Delete" ��� �������� ��������
    private void OnDeleteButtonClick()
    {
        itemRemover.RemoveItem(); // �������� ����� ��� �������� �������� �� ���������
    }
}
