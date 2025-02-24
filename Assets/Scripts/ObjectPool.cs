using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{    
    private Queue<T> pool;
    private T prefab;

    

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        //���Ҵ��� �Ϸ��� ���׸��� ������ ���� �־���ϴµ� �� ������ �𸣴� ���׸��̱� ������ �����ڿ� ����.
        pool = new Queue<T>();

        for (int i = 0; i < initialSize; i++)
        {
            T instance = Object.Instantiate(prefab);
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    public T GetObject()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        // Ǯ�� ���� ������Ʈ�� ������ ���� ����
        T newObj = Object.Instantiate(prefab);
        return newObj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
