using TMPro;
using UnityEngine;

public class ItemShower : MonoBehaviour
{
    public int index;
    public int quantity;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;

    public void SetUpValues(string name, int quantity, int index)
    {
        this.index = index;
        itemNameText.text = name;
        this.quantity = quantity;
        itemQuantityText.text = "x" + quantity;
    }

    public void UpdateQuantityAndIndex(int quantity, int index)
    {
        this.index = index;
        this.quantity = quantity;
        itemQuantityText.text = "x" + quantity;
    }
    public void ChangeIndex(int newIndex)
    {
        index = newIndex;
        UpdateQuantityAndIndex(quantity, index);
    }
    public void ChangeQuantity(int quantityVal)
    {
        quantity += (quantityVal* UIManager.Instance.itemsMode);
        if (quantity > 0)
        {
            UpdateQuantityAndIndex(quantity, index);
        }
        else
        {
            CharacterManager.Instance.DeleteItem(Pkmn.Instance.pkmnName, index);
            Destroy(gameObject);
        }
    }

}
