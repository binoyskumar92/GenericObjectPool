using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class ObjectPoolTests
{

    [Test]
    public void GetObjectFromPoolTest()
    {

        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, ()=>new Pool(1,'b') ); 
        Pool[] obj = new Pool[20];
        for (int i = 0; i < 20; i++)
        {
            obj[i] = (Pool)pool.GetObjectFromPool();
            Assert.AreEqual(obj[i].GetType(), typeof(Pool));
        }
    }
    [Test]
    public void ObjectPoolGrowTest()
    {
        //Pool grows by default if willgrouw=true
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(1, 'b'));
        pool.DestroyObjectsinPool();

        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj.GetType(), typeof(Pool));

        //Pool does not grow when pool does not have free objects and willgrow=false
        pool.DestroyObjectsinPool();
        pool.SetMaxNumberOfObjects(0);
        obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj, null);

    }

    [Test]
    public void ObjectPreallocateTest()
    {
        try
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(1, 'b')); 
            Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 20);
        }
        catch (Exception e)
        {
            Debug.Log("Exception caused: " + e.Message);
        }
    }

    [Test]
    public void MaxPoolCapacityTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(1, 'b'));
        Pool[] obj = new Pool[50];
        for (int i = 0; i < 50; i++)
        {
            obj[i] = (Pool)pool.GetObjectFromPool();
        }
        for (int i = 0; i < 50; i++)
        {
            if (obj[i] != null)
            {
                pool.AddBackToPool(obj[i]);
            }
        }
        Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 40);

    }
    [Test]
    public void DestroyObjectsinPoolTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(1, 'b'));
        Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 20);
        pool.DestroyObjectsinPool();
        Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 0);
    }

    [Test]
    public void AddBackToPoolTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(1, 'b'));
        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 19);
        pool.AddBackToPool(obj);
        Assert.AreEqual(pool.GetNumberOfObjectsInPool(), 20);
    }

    [Test]
    public void objectPoll2()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, () => new Pool(10, 'a'));
        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj.a, 10);
        Assert.AreEqual(obj.b, 'a');
    }
    [Test]
    public void objectPoll3()
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(20, 40, SimpleConstructor);
        GameObject obj = (GameObject)pool.GetObjectFromPool();
        Assert.AreEqual(obj.GetType(), typeof(GameObject));
     
    }
    public GameObject SimpleConstructor()
    {
        GameObject obj = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Plane));
        obj.SetActive(false);
        return obj;
    }
}
