using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryCallbackListener
{
    public void OnInventoryProductInvoked(bool isActive, string productName);
}
