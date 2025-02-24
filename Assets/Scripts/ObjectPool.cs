using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{    
    private Queue<T> pool;
    private T prefab;

    

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        //뉴할당을 하려면 제네릭에 데이터 형을 넣어야하는데 뭘 만들지 모르는 제네릭이기 떄문에 생성자에 넣음.
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

        // 풀에 남은 오브젝트가 없으면 새로 생성
        T newObj = Object.Instantiate(prefab);
        return newObj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
