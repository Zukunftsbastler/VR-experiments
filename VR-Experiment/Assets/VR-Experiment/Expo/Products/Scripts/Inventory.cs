using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VR_Experiment.Expo.Product
{
    public static class Inventory
    {
        private static List<SO_Product> _products = null;

        public static List<SO_Product> Products => _products ??= Resources.LoadAll<SO_Product>("").ToList();

        /// <summary>
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>
        /// <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource}, System.Func{TSource, bool})">FirstOrDefaut </see>
        /// product called <paramref name="productName"/>.
        /// </returns>
        public static SO_Product GetProductByName(string productName)
        {
            return Products.FirstOrDefault(p => p.Name.Equals(productName));
        }
    }
}
