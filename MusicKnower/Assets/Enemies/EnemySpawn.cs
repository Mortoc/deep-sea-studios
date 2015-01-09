using UnityEngine;
using System.Collections;

using Procedural;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private int _beatsPerSpawn = 5;

    [SerializeField]
    private Procedural.LoftComponent _enemyPath;

    private Target _targetObject;
    void Start()
    {
        GameObject.FindObjectOfType<MusicKnower>().OnBeat += SpawnUnit;
        _targetObject = GameObject.FindObjectOfType<Target>();
    }

    void SpawnUnit(MusicKnower.BeatInfo bi)
    {
        if (bi.BeatNumber % _beatsPerSpawn == 0)
        {
            var enemy = (GameObject)Instantiate(_enemyPrefab);
            enemy.transform.position = transform.position;
            enemy.GetComponent<Enemy>().Init(_enemyPath.Path, _targetObject);
        }
    }
}
