using System;
using UniRx;
using UnityEngine;

namespace App.Field.Interfaces.Presenters
{
    public interface IPlayerFieldPresenter
    {
        Transform DeckTransform { get; }
        Transform HandTransform { get; }
        Transform[] CookieTransforms { get; }
        Transform[] HpTransforms { get; }
        Transform SupportAreaTransform { get; }
        Transform StageAreaTransform { get; }
        Transform BreakAreaTransform { get; }
        Transform TrashTransform { get; }

        IObservable<int> OnCookieAreaClicked { get; }
        IObservable<Unit> OnStageAreaClicked { get; }
        IObservable<Unit> OnTrashClicked { get; }
    }
}