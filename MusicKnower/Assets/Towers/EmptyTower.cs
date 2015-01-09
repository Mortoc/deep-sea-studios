using UnityEngine;
using System.Collections;

public class EmptyTower : MonoBehaviour
{
    public GameObject _towerPrefab;

    void OnMouseDown()
    {
        var tower = Instantiate<GameObject>(_towerPrefab);
        tower.transform.position = transform.position;
        tower.transform.rotation = transform.rotation;
        tower.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}