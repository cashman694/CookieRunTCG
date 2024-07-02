using App.Common.Data;
using Unity;
using System;
using UnityEngine;


namespace App.Catalog.Interfaces.Views
{
    public interface ICatalogCardView
    {
        void Setup(string cardNumber, string name, int level, int maxHp, Sprite sprite);
        void UpdateHp(int hp);
        void Unspawn();
    }
}