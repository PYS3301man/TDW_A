using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    [Header("UI")]
    public Image[] slots;

    [Header("Colors")]
    public Color normalColor =
        new Color(0f, 0f, 0f, 0.5f);

    public Color selectedColor =
        new Color(1f, 1f, 1f, 0.8f);

    [Header("Objects")]
    public GameObject[] objects;

    private int selectedSlot = -1;

    void Start()
    {
        UpdateVisual();
        UpdateObjects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ToggleSlot(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ToggleSlot(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ToggleSlot(2);
    }

    void ToggleSlot(int slot)
    {
        if (selectedSlot == slot)
            selectedSlot = -1;
        else
            selectedSlot = slot;

        UpdateVisual();
        UpdateObjects();
    }

    void UpdateVisual()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].color =
                i == selectedSlot
                ? selectedColor
                : normalColor;
        }
    }

    void UpdateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActive(
                    i == selectedSlot);
            }
        }
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }

    public bool IsSlotSelected(int slot)
    {
        return selectedSlot == slot;
    }
}