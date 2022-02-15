using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductSpawnBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _productPrefab = new GameObject[3];
    [SerializeField] private RevolvingStageBehaviour _stage;

    public void SpawnProductOne()
    {
        SpawnProduct(0);
    }

    public void SpawnProductTwo()
    {
        SpawnProduct(1);
    }

    public void SpawnProductThree()
    {
        SpawnProduct(2);
    }

    private void SpawnProduct(int index)
    {
        if(index >= _productPrefab.Length)
            return;

        if(_stage.HasActiveItem)
        {
            DestroyImmediate(_stage.ActiveProduct);
        }

        _stage.ActiveProduct = Instantiate(_productPrefab[index], _stage.transform.position, Quaternion.identity);
    }
}
