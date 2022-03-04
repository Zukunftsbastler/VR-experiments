using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Inventory
{
    private static List<SO_Product> _products = null;

    public static List<SO_Product> Products => _products ??= Resources.LoadAll<SO_Product>("").ToList();

    public static SO_Product GetProductByName(string productName)
    {
        return Products.FirstOrDefault(p => p.Name.Equals(productName));
    }
}
