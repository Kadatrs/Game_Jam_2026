using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> items = new List<string>();
    public GameObject playerInventoryPanel; // Panel kho đồ cá nhân (ấn phím để mở)

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Đã thêm: " + itemName);
    }

    void Update()
    {
        // Kiểm tra phím tắt Ctrl + C để thoát mọi Menu
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            CloseAllMenus();
        }
    }

    public void CloseAllMenus()
    {
        // Ẩn tất cả Panel UI
        if (playerInventoryPanel != null) playerInventoryPanel.SetActive(false);

        // Tìm và ẩn UI của rương đang mở
        ChestAnimationController activeChest = FindObjectOfType<ChestAnimationController>();
        if (activeChest != null) activeChest.CloseChest();

        // Trả lại trạng thái bình thường
        ResumeGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột lại tâm màn hình

        // Bật lại di chuyển
        var playerMove = GetComponent<PlayerMovement>();
        if (playerMove != null) playerMove.enabled = true;

        // Reset trạng thái rương để có thể mở lại lần sau
        ChestAnimationController activeChest = FindObjectOfType<ChestAnimationController>();
        // Nếu bạn muốn rương đóng lại ngay khi thoát UI:
        // if (activeChest != null) activeChest.ResetChest(); 
    }
}