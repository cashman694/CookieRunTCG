using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardOrder : MonoBehaviour
    {
        [SerializeField] Renderer[] backRenderers;
        [SerializeField] Renderer[] middleRenderers1;
        [SerializeField] Renderer[] middleRenderers2;
        [SerializeField] Renderer[] middleRenderers3;
        [SerializeField] Renderer[] middleRenderers4;
        int originOrder;

        public void SetOriginOrder(int originOrder)
        {
            this.originOrder = originOrder;
            SetOrder(originOrder);
        }

        public void SetOrder(int order)
        {
            int mulOrder = order * 10;
            foreach (var renderer in backRenderers)
            {
                renderer.sortingOrder = mulOrder;
            }
            foreach (var renderer in middleRenderers1)
            {
                renderer.sortingOrder = mulOrder + 1;
            }
            foreach (var renderer in middleRenderers2)
            {
                renderer.sortingOrder = mulOrder + 2;
            }
            foreach (var renderer in middleRenderers3)
            {
                renderer.sortingOrder = mulOrder + 3;
            }
            foreach (var renderer in middleRenderers4)
            { 
                renderer.sortingOrder = mulOrder + 4;
            }

        }
    }
}