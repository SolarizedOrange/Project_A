using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnerCondition
{
    public bool IsTrue();

    public static ISpawnerCondition operator &(ISpawnerCondition a, ISpawnerCondition b)
    {
        var and = new AndCondition(a, b);
        return and;
    }

    public static ISpawnerCondition operator |(ISpawnerCondition a, ISpawnerCondition b)
    {
        var or = new OrCondition(a, b);
        return or;
    }

    class AndCondition : ISpawnerCondition
    {
        List<ISpawnerCondition> list = new ();
        public AndCondition(ISpawnerCondition a, ISpawnerCondition b)
        {
            list.Add(a);
            list.Add(b);
        }
        public bool IsTrue()
        {
            return list.TrueForAll(condition => condition.IsTrue());
        }
    }

    class OrCondition : ISpawnerCondition
    {
        List<ISpawnerCondition> list = new ();
        public OrCondition(ISpawnerCondition a, ISpawnerCondition b)
        {
            list.Add(a);
            list.Add(b);
        }
        public bool IsTrue()
        {
            return list.Exists(condition => condition.IsTrue());
        }
    }
}

public class ObjectSpawner : MonoBehaviour
{
    [Pool] public GameObject ObjectPrefab;
    public Transform SpawnPoint;
    public float SpawnInterval = 1f;
    public ISpawnerCondition SpawnCondition;

    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        bool isFirst = true;
        while (true)
        {
            if (SpawnCondition == null || SpawnCondition.IsTrue())
            {
                if (isFirst)
                {
                    OnStart();
                    isFirst = false;
                }
                SpawnObject();
                yield return new WaitForSeconds(SpawnInterval);
            }
            else if (isFirst == false)
            {
                OnStop();
                isFirst = true;
                yield return null;
            }
        }
    }

    protected virtual void OnStart() { }
    protected virtual void OnStop() { }

    public void SpawnObject()
    {
        ObjectPrefab.Spawn(ObjectPrefab, SpawnPoint.position, SpawnPoint.rotation);
    }
}
