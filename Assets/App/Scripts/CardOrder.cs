using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOrder : MonoBehaviour
{
    [SerializeField] Renderer[] backRenderers;
    [SerializeField] Renderer[] middleRenderers1;
    [SerializeField] Renderer[] middleRenderers2;
    [SerializeField] Renderer[] middleRenderers3;
    [SerializeField] Renderer[] middleRenderers4;
    [SerializeField] string sortingLayerName;
    int originOrder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    // Update is called once per frame
    public void SetOrder(int order)
    {
        int mulOrder = order * 10;
        foreach (var renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }
        foreach (var renderer in middleRenderers1)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
        foreach (var renderer in middleRenderers2)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 2;
        }
        foreach (var renderer in middleRenderers3)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 3;
        }
        foreach (var renderer in middleRenderers4)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 4;
        }

    }
}
