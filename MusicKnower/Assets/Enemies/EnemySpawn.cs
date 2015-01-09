using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private int _beatsPerSpawn = 5;
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

            enemy.GetComponent<Enemy>().MoveToPosition(
                _targetObject.transform.position,
                () => _targetObject.TakeDamage(1.0f)
           );
        }
    }
}
