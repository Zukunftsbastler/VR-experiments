using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemListCallbackListener
{
    public void OnItemToggleInvoked(bool isActive, string itemName);
}
