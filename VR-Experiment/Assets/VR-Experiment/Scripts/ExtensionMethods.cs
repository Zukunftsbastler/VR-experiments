using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class gameObject
{
    //-----------------------------------------------------------------------------------------------------------------------------------------------
    public static T GetComponentInParent<T>(this Component comp, bool includeInactive) where T : class
    {
        if(comp == null)
            return null;

        if(!includeInactive)
            return comp.GetComponentInParent<T>();

        T result = comp.GetComponent<T>();
        if(result != null)
            return result;

        return GetComponentInParent<T>(comp.transform.parent, true);
    }

    public static bool TryGetComponentInParent<T>(this Component comp, out T component, bool includeInactive = false) where T : class
    {
        component = comp.GetComponentInParent<T>(includeInactive);
        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this Component comp, out T component, bool includeInactive = false) where T : class
    {
        component = comp.GetComponentInChildren<T>(includeInactive);
        return component != null;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------
}
