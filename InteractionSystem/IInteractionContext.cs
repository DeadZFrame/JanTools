using Jan.Interaction;
using UnityEngine;

namespace Jan.Core
{
    public interface IInteractionContext
    {
        IPickable HeldItem { get; }
        Transform HandTransform { get; }

        void PickUp(IPickable pickable);
        void Drop();
    }
}