using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour 
{
    [SerializeField]
    private GameObject _minionPrefab;

    public int _numMinions = 3;

	void Start () 
    {
        SpawnMinion(_numMinions);
	}

    void SpawnMinion(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var minion = (GameObject)Instantiate(_minionPrefab);
            minion.transform.position = transform.position;
        }
    }
	
}
