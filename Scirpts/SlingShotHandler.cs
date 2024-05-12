using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotHandler : MonoBehaviour
{
    [Header("LineRenderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;
    [Header("Transforms")]
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private Transform idleTransform;
    [Header("SlingShotConfig")]
    [SerializeField] private float maxLength = 5f;
    [SerializeField] private SlingShotArea slingShotArea;
    [Header("Birb")]
    [SerializeField] private GameObject birbPrefab;
    private Vector3 linesPosition;
    private GameObject birb;
    private bool clickedWithinArea = false;
    private void Awake()
    {
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        SpawnBird();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && slingShotArea.IsWithinSlingShotArea())
        {
            clickedWithinArea = true;
        }
        if (Mouse.current.leftButton.isPressed && clickedWithinArea)
        {
            DrawSlingShot();
            positionAndRotateAngie();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            clickedWithinArea = false;
            ClearSlingShot();
        }

    }

    #region slingshot
    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        linesPosition = Vector3.ClampMagnitude(touchPosition - centerTransform.position, maxLength) + centerTransform.position;
        SetLines(linesPosition, linesPosition);
    }

    private void SetLines(Vector3 rightPosition, Vector3 leftPosition)
    {
        if (!leftLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
        }
        if (!rightLineRenderer.enabled)
        {
            rightLineRenderer.enabled = true;
        }

        rightLineRenderer.SetPosition(0, rightPosition);
        rightLineRenderer.SetPosition(1, rightTransform.position);
        leftLineRenderer.SetPosition(0, leftPosition);
        leftLineRenderer.SetPosition(1, leftTransform.position);

    }

    private void ClearSlingShot()
    {
        SetLines(rightTransform.position, leftTransform.position);
    }
    #endregion

    #region birb
    private void SpawnBird()
    {
        SetLines(idleTransform.position, idleTransform.position);

        Instantiate(birbPrefab, idleTransform.position, Quaternion.identity);

    }

    private void positionAndRotateAngie()
    {
        birb.transform.position = linesPosition;
    }
    #endregion
}

