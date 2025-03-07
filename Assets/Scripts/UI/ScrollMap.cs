using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ScrollMap : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject contentContainer;
    public float scrollSpeed = 5f;

    private bool isDragging = false;
    private bool isPointerInside = false;

    private float maxY = 0f;
    private float minY = -2560;

    void Update()
    {
        Vector2 pos = contentContainer.GetComponent<RectTransform>().anchoredPosition;

        // Clamp the position so it stays within bounds
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        contentContainer.GetComponent<RectTransform>().anchoredPosition = pos;

        if (Input.GetMouseButtonDown(0))
            isDragging = true;

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

    }
    
}
