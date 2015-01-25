using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour 
{
    [SerializeField]
    private GameObject _minionPrefab;

    [SerializeField]
    private Procedural.BezierComponent _enemyPath;

    public int _numMinions = 3;
    public int _spawnTimer = 10;

	void Start () 
    {
        StartCoroutine(SpawnMinions(_numMinions));
	}

    IEnumerator SpawnMinions(int num)
    {
        while (true)
        {
            for (int i = 0; i < num; i++)
            {
                var minion = (GameObject)Instantiate(_minionPrefab);
                minion.transform.position = transform.position;
                minion.GetComponent<Minion>().Init(_enemyPath, gameObject.layer);
            }
            for (float timer = 0; timer < _spawnTimer; timer += Time.deltaTime)
                yield return 0;
        }
    }
}
