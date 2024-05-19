using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private Color usedColor = Color.gray;

    public void UseShot(int shotNumber)
    {
        if (shotNumber >= 1 && shotNumber <= icons.Length)
        {
            icons[shotNumber - 1].color = usedColor;
        }
    }
}
