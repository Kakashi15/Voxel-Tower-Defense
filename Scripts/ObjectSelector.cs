using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSelector : MonoBehaviour
{
    public List<UnitController> selectableObjects;
    private bool objectsSelected = false;
    private bool objectCanMove = false;
    public Vector3 targetPosition;

    private Vector3 mousePosition1;
    private Vector3 mousePosition2Update;
    private Vector3 mousePosition2;

    Rect selectionRect;
    Texture2D selectionTexture;
    Color selectionColor;
    bool drawSelectionBox;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition1 = Input.mousePosition;
            objectsSelected = false;
            drawSelectionBox = true;
            DeselectAll();
        }

        if (Input.GetMouseButton(0))
        {
            mousePosition2Update = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePosition2 = Input.mousePosition;
            drawSelectionBox = false;
            SelectObjects();
        }

        //if (objectsSelected && Input.GetMouseButtonDown(1))
        //{
        //    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    targetPosition.y = 0f;
        //    objectCanMove = true;
        //
        //}

        if (objectsSelected && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                //Selectable clickedSelectable = hit.transform.GetComponent<Selectable>();
                Tree tree = hit.transform.GetComponent<Tree>();
                if (tree == null)
                    targetPosition = hit.point;
                //navMeshAgent.SetDestination(hit.point);
            }
            objectCanMove = true;

            foreach (UnitController selectable in selectableObjects)
            {
                if (selectable.IsSelected())
                {
                    // Get the position of the clicked object
                    RaycastHit hit2;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, Mathf.Infinity))
                    {
                        // Check if the clicked object is selectable and is different from the current object
                        Selectable clickedSelectable = hit2.transform.GetComponent<Selectable>();
                        if (clickedSelectable != null && clickedSelectable != selectable && clickedSelectable.GetComponent<Tree>() == null)
                        {
                            // Move to the clicked object
                            selectable.TargetPosition = clickedSelectable.transform.position;
                        }
                    }
                }
            }
        }

        if (objectCanMove && targetPosition != Vector3.negativeInfinity)
        {
            MoveAllSelectedObjects();
        }
        if (mousePosition1 != mousePosition2Update)
            selectionRect = GetSelectionRect(mousePosition1, mousePosition2Update);
    }

    void DeselectAll()
    {
        foreach (UnitController selectable in selectableObjects)
        {
            selectable.Deselect();
            objectsSelected = false;
            objectCanMove = false;
            targetPosition = Vector3.negativeInfinity;
        }
    }

    void SelectObjects()
    {
        Bounds selectionBounds = GetViewportBounds(mousePosition1, mousePosition2);
        foreach (UnitController selectable in selectableObjects)
        {
            if (selectionBounds.Contains(Camera.main.WorldToViewportPoint(selectable.transform.position)))
            {
                selectable.GetComponent<UnitController>().Select();
                objectsSelected = true;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<UnitController>() != null)
            {
                hit.transform.gameObject.GetComponent<UnitController>().Select();
                objectsSelected = true;
            }
            //navMeshAgent.SetDestination(hit.point);
        }

    }

    void OnMouseDown()
    {

    }

    void MoveAllSelectedObjects()
    {
        foreach (UnitController selectable in selectableObjects)
        {
            if (selectable.GetComponent<UnitController>().IsSelected())
            {
                selectable.GetComponent<UnitController>().TargetPosition = targetPosition;
            }
        }
    }

    void MoveObjects()
    {
        foreach (UnitController selectable in selectableObjects)
        {
            if (selectable.GetComponent<Renderer>().material.color == Color.green)
            {
                selectable.GetComponent<NavMeshAgent>().SetDestination(targetPosition);
                //selectable.transform.position = Vector3.MoveTowards(selectable.transform.position, targetPosition, Time.deltaTime * 5f);
            }
        }
    }

    Bounds GetViewportBounds(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 lowerLeft = Camera.main.ScreenToViewportPoint(screenPosition1);
        Vector3 upperRight = Camera.main.ScreenToViewportPoint(screenPosition2);

        Vector3 min = Vector3.Min(lowerLeft, upperRight);
        Vector3 max = Vector3.Max(lowerLeft, upperRight);

        min.z = Camera.main.nearClipPlane;
        max.z = Camera.main.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }

    private void OnGUI()
    {
        // Create a Rect from the mouse positions


        // Draw the selection rectangle
        DrawSelectionRect(selectionRect);
    }

    private Rect GetSelectionRect(Vector3 startPosition, Vector3 endPosition)
    {
        // Calculate the top-left position and size of the rectangle
        Vector3 topLeft = Vector3.Min(startPosition, endPosition);
        Vector3 bottomRight = Vector3.Max(startPosition, endPosition);
        Vector3 size = bottomRight - topLeft;

        // Create a Rect from the calculated values
        return new Rect(topLeft.x, Screen.height - topLeft.y, size.x, -size.y);
    }

    private void DrawSelectionRect(Rect rect)
    {
        // Set the color and transparency for the selection rectangle
        selectionColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);
        if (drawSelectionBox && mousePosition1 != mousePosition2Update)
        {
            selectionTexture = new Texture2D(1, 1);
            selectionTexture.SetPixel(0, 0, selectionColor);
            selectionTexture.Apply();
            GUI.DrawTexture(rect, selectionTexture);
        }
        else
        {
            rect = new Rect(0, 0, 0, 0);
            selectionTexture = new Texture2D(0, 0);
            selectionTexture.SetPixel(0, 0, selectionColor);
            selectionTexture.Apply();
            GUI.DrawTexture(rect, selectionTexture);
        }
    }
}
