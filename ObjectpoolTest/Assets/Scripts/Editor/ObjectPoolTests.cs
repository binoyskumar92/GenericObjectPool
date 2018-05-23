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

        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
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
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
        pool.destroyObjectsinPool();

        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj.GetType(), typeof(Pool));

        //Pool does not grow when pool does not have free objects and willgrow=false
        pool.destroyObjectsinPool();
        pool._maxNumberOfObjects = 0;
        obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj, null);

    }

    [Test]
    public void ObjectPreallocateTest()
    {
        try
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
        }
        catch (Exception e)
        {
            Debug.Log("Exception caused: " + e.Message);
        }
    }

    [Test]
    public void MaxPoolCapacityTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
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
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 40);

    }
    [Test]
    public void DestroyObjectsinPoolTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
        pool.destroyObjectsinPool();
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 0);
    }

    [Test]
    public void AddBackToPoolTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] { }, true);
        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 19);
        pool.AddBackToPool(obj);
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
    }

    [Test]
    public void ObjectInitiliaseTest()
    {
        GameObject obj;
        //if exception is thrown if objects are not initialised
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(20, 40, new object[] { }, true, GameObject.CreatePrimitive(PrimitiveType.Plane));
        try
        {
            obj = (GameObject)pool.GetObjectFromPool();
        }
        catch (Exception e)
        {
            Assert.AreEqual(e.Message, "Please initialize gameobjects using InstantiateGameObjects method on pool reference");
        }
        pool.InstantiateGameObjects();
        obj = (GameObject)pool.GetObjectFromPool();
        Assert.AreEqual(19, pool.getNumberOfObjectsInPool());
        pool.AddBackToPool(obj);
        Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
    }

    [Test]
    public void ObjectPoolCreationWithParametersPassedTest()
    {
        ObjectPool<Pool> pool = new ObjectPool<Pool>(20, 40, new object[] {10,'a'}, true);
        Pool obj = (Pool)pool.GetObjectFromPool();
        Assert.AreEqual(obj.a, 10);
        Assert.AreEqual(obj.b, 'a');
    }
}
