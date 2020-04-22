using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerFixed : MonoBehaviour
{
    [SerializeField] private string sortingLayerName = string.Empty; //initialization before the methods
    [SerializeField] private int orderInLayer = 0;
    private Renderer MyRenderer;
    private void Start()
    {
        MyRenderer = GetComponent<Renderer>();
        if (sortingLayerName != string.Empty)
        {
            MyRenderer.sortingLayerName = sortingLayerName;
            MyRenderer.sortingOrder = orderInLayer;
        }
    }
}