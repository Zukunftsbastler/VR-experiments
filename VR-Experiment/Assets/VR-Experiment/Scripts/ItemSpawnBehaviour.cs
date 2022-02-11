using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemPrefab = new GameObject[3];
    [SerializeField] private PresenterBehaviour _presenter;

    public void SpawnItemOne()
    {
        SpawnItem(0);
    }

    public void SpawnItemTwo()
    {
        SpawnItem(1);
    }

    public void SpawnItemThree()
    {
        SpawnItem(2);
    }

    private void SpawnItem(int index)
    {
        if(index >= _itemPrefab.Length)
            return;

        if(_presenter.HasActiveItem)
        {
            DestroyImmediate(_presenter.ActiveItem);
        }

        _presenter.ActiveItem = Instantiate(_itemPrefab[index], _presenter.transform.position, Quaternion.identity);
    }
}
