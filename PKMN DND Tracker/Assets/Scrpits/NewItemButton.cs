using UnityEngine;

public class NewItemButton : MonoBehaviour
{
    public void GoToNewItemMenu()
    {
        UIManager.Instance.ShowInventory(false);
    }
}
