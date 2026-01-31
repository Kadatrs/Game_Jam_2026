using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestAnimationController : MonoBehaviour
{
    [Header("Inventory Settings")]
    public List<GameObject> itemsInChest;
    public GameObject chestUIPanel;

    [Header("Auto Close Settings")]
    [SerializeField] private float closeDelay = 3f; // Thời gian chờ để đóng rương

    private Animator animator;
    private bool isPlayerNearby = false;
    private bool IsOpen = false;
    private Coroutine closeCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (isPlayerNearby && !IsOpen)
        {
            Debug.Log("<color=cyan>Chest:</color> Người chơi đã click mở rương.");
            OpenChest();
        }
        else if (!isPlayerNearby)
        {
            Debug.Log("<color=yellow>Chest:</color> Click thành công nhưng người chơi ở quá xa!");
        }
    }

    void OpenChest()
    {
        if (IsOpen) return;
        IsOpen = true;
        animator.SetBool("IsOpen", true);

        // Nếu rương đang trong quá trình chờ đóng, hãy hủy bỏ nó
        if (closeCoroutine != null) StopCoroutine(closeCoroutine);

        StartCoroutine(ShowUIRoutine(0.8f));
    }

    IEnumerator ShowUIRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (chestUIPanel != null)
        {
            chestUIPanel.SetActive(true);
            PauseGame();
            Debug.Log("<color=green>Chest UI:</color> Đã hiển thị kho đồ và Pause game.");
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        var playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (playerMove != null) playerMove.enabled = false;
    }

    // Hàm để đóng rương hoàn toàn
    public void CloseChest()
    {
        if (!IsOpen) return;

        IsOpen = false;
        animator.SetBool("IsOpen", false);

        if (chestUIPanel != null) chestUIPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (playerMove != null) playerMove.enabled = true;

        Debug.Log("<color=red>Chest:</color> Rương đã đóng và tiếp tục game.");
    }

    IEnumerator WaitAndCloseRoutine()
    {
        Debug.Log("<color=orange>Chest:</color> Bắt đầu đếm ngược " + closeDelay + " giây để đóng rương...");

        // Sử dụng Realtime vì game có thể đang bị Pause (Time.timeScale = 0)
        yield return new WaitForSecondsRealtime(closeDelay);

        CloseChest();
    }

    public void PickItem(GameObject itemButton)
    {
        string name = itemButton.name;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().AddItem(name);
        itemButton.SetActive(false);
        Debug.Log("<color=white>Inventory:</color> Đã nhặt vật phẩm: " + name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("<color=blue>System:</color> Player đã đi vào vùng rương.");

            // Nếu người chơi quay lại, ngừng đếm ngược đóng rương
            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
                Debug.Log("<color=blue>System:</color> Hủy bỏ đếm ngược đóng rương.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("<color=blue>System:</color> Player đã rời xa vùng rương.");

            // Nếu rương đang mở, bắt đầu đếm ngược để tự đóng
            if (IsOpen)
            {
                if (closeCoroutine != null) StopCoroutine(closeCoroutine);
                closeCoroutine = StartCoroutine(WaitAndCloseRoutine());
            }
        }
    }
}