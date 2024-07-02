using App.Common.Data;
using Unity;
using System;
using UnityEngine;

namespace App.Battle.Interfaces.Views
{
    public interface IDeckCardView
    {
        void Setup(string cardNumber, string name, CardLevel cardLevel, int maxHp, Sprite Sprite );
        void UpdateHp(int hp);
        void Unspawn();
    }
}