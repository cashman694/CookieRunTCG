using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Battle.Views
{
    public class CardOrder : MonoBehaviour
    {
        [SerializeField] private Renderer[] backRenderers;
        [SerializeField] private Renderer[] middleRenderers1;
        [SerializeField] private Renderer[] middleRenderers2;

        private int originOrder;

        public void SetOriginOrder(int originOrder)
        {
            this.originOrder = originOrder;
            SetOrder(originOrder);
        }

        public void SetMostFrontOrder(bool isMostFront)
        {
            SetOrder(isMostFront ? 200 : originOrder);
        }

        // sortingOrder값이 작을수록 나중에 그려진다 
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
        }
    }
}