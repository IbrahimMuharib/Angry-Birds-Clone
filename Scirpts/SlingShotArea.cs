using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotArea;
    public bool IsWithinSlingShotArea()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        return Physics2D.OverlapPoint(touchPosition, slingShotArea);


    }
}
