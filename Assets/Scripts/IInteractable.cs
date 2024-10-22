using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public event EventHandler OnUpdateInteract;
    public event EventHandler OnUpdateInteractAlternate;

    public const string GRAB = "Grab ";
    public const string PLACE = "Place ";

    public bool CanInteract(Player player, out string action);
    public bool CanInteractAlternate(Player player, out string action);
    public KitchenObjectSO GetKitchenObjectSO();
}
