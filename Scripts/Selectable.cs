using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Color selectedColor = Color.green;
    public Color deselectedColor = Color.white;
    private Renderer objectRenderer;
    private bool isSelected = false;

    public void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        Deselect();
    }

    public void Select()
    {
        isSelected = true;
        //objectRenderer.material.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;
        //objectRenderer.material.color = deselectedColor;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
