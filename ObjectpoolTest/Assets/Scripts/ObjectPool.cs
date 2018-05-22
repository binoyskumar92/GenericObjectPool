using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    /*  pooledObject - GameObject if to be instatiated [optional]
        objectsToPreAllocate - Number of Objects in pool to be preallocated
        maxNumberOfObjects - Max size of pool can grow upto
        extrObjectsCount; -  how much pool can grow extra from preallocated size
        willGrow = true - boolean to toggle if pool grows to max size 
        pooledObjects -   Queue which keeps all the queued objects
        ObjectType - type of T at runtime
        parameters - additional parameters for new object creation
     */
    private T pooledObject;
    private int objectsToPreAllocate = 20;
    private int maxNumberOfObjects;
    private int extrObjectsCount;
    public bool willGrow = true;
    private bool gameObjectsInitialised = false;
    private Queue<T> pooledObjects;
    private Type ObjectType;
    private object[] parameters;

    public bool destroyInProgress { get; private set; }

    public ObjectPool(int objectsToPreAllocate, int maxNumberOfObjects, object[] parameters, bool willGrow, GameObject poolobj = null)
    {

        this.objectsToPreAllocate = objectsToPreAllocate;
        this.maxNumberOfObjects = maxNumberOfObjects;
        this.parameters = parameters;
        this.willGrow = willGrow;
        this.ObjectType = typeof(T);
        this.destroyInProgress = false;
        extrObjectsCount = maxNumberOfObjects - objectsToPreAllocate;

        if (poolobj != null)
        {
            this.pooledObject = poolobj as T;
        }

        // Queue which keeps all the queued objects
        pooledObjects = new Queue<T>();
        for (int i = 0; i < this.objectsToPreAllocate; i++)
        {
            if (ObjectType == typeof(GameObject))
            {
                pooledObjects.Enqueue(pooledObject);
            }
            else
            {
                pooledObjects.Enqueue(createGenericObject());
            }
        }
    }

    // helper method to create a pool object 
    private T createGenericObject()
    {

        T obj;
        try
        {
            //no need to create an  instance of gameobjects instead instantiate one object reference
            if (ObjectType == typeof(GameObject))
            {
                GameObject gameobject = GameObject.Instantiate(pooledObject as GameObject);
                gameobject.SetActive(true);
                return gameobject as T;
            }
            if (parameters.Length > 0)
            {
                obj = (T)Activator.CreateInstance(ObjectType, parameters);

            }
            else
            {
                obj = (T)Activator.CreateInstance(ObjectType);
            }
            return obj;
        }
        catch (Exception e)
        {
            Debug.Log("Exception caused at generic object creation: " + e.Message);
            return default(T);
        }

    }

    //method to instantiate game object pool
    public void InstantiateGameObjects()
    {
        if (ObjectType != typeof(GameObject))
        {
            throw new Exception("Method can be invoked only for pool type GameObject");
        }
        else
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(pooledObjects.Dequeue() as GameObject);
                obj.SetActive(false);
                pooledObjects.Enqueue(obj as T);
            }
            gameObjectsInitialised = true;
        }
    }

    //method to get an object from pool
    public T GetObjectFromPool()
    {
        if (ObjectType == typeof(GameObject) && !gameObjectsInitialised)
        {
            throw new Exception("Please initialize gameobjects using InstantiateGameObjects method on pool reference");
        }

        if (getNumberOfObjectsInPool() > 0 && !destroyInProgress)
        {
            T obj = pooledObjects.Dequeue(); ;
            return obj;
        }
        else if (willGrow && extrObjectsCount > 0 && !destroyInProgress)
        {
            extrObjectsCount--;
            return createGenericObject();

        }
        else
        {
            return default(T);
            // return null;
        }

    }

    //method to add object back to pool
    public void AddBackToPool(T returningObject)
    {
        if (!returningObject.Equals(null) && getNumberOfObjectsInPool() < maxNumberOfObjects && !destroyInProgress)
        {
            pooledObjects.Enqueue(returningObject);
        }

    }

    //method to get count of objects currently in pool
    public int getNumberOfObjectsInPool()
    {
        return pooledObjects.Count;
    }

    //method to destroy object sin pool
    public void destroyObjectsinPool()
    {
        destroyInProgress = true;
        while (getNumberOfObjectsInPool() > 0)
        {
            pooledObjects.Dequeue();
        }
        destroyInProgress = false;
    }


}
