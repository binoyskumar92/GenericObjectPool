using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectPool<T> where T:class
{
    public T pooledObject;
    public int objectsToPreAllocate = 20;
    public int maxNumberOfObjects;
    public int extrObjectsCount;
    public bool willGrow = true;
    public Queue<T> pooledObjects;
    public Type ObjectType;
    public object[] parameters;

    public bool destroyInProgress { get; private set; }
    
    public ObjectPool(int objectsToPreAllocate,int maxNumberOfObjects,object[] parameters, bool willGrow,GameObject poolobj)
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

        for(int i=0;i< this.objectsToPreAllocate; i++)
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


    // helper method to create a pool object and add to pool
    private T createGenericObject()
    {
      
        T obj;
        //no need to create an  instance of gameobjects
        if (ObjectType == typeof(GameObject))
        {
            GameObject gameobject = GameObject.Instantiate(pooledObject as GameObject);
            gameobject.SetActive(true);
            return gameobject as T;
        }
        if (parameters.Length > 0)
            obj =(T) Activator.CreateInstance(ObjectType,parameters);
        else
            obj = (T)Activator.CreateInstance(ObjectType);

        return obj;
            
    }

    public void InstantiateGameObjects()
    {
        if (ObjectType != typeof(GameObject))
        {
            throw new Exception("Method can be invoked only for GameObject pool");
        }
        else
        {
            for(int i = 0; i < pooledObjects.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(pooledObjects.Dequeue() as GameObject);
                obj.SetActive(false);
                pooledObjects.Enqueue(obj as T);
            }
        }

    }

    //helper to get an object from pool
    public T GetObjectFromPool()
    {
       
        
            if (getNumberOfObjectsInPool() > 0 && !destroyInProgress)
            {
                T obj= pooledObjects.Dequeue(); ;
            return obj;
            }
            else if (willGrow && extrObjectsCount>0 && !destroyInProgress)
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
    //helper to add object back to pool
    public void AddBackToPool(T returningObject)
    {
        if (!returningObject.Equals(null) && getNumberOfObjectsInPool() < maxNumberOfObjects && !destroyInProgress)
        {
           pooledObjects.Enqueue(returningObject);
        }
        
    }
    public int getNumberOfObjectsInPool()
    {
        return pooledObjects.Count;
    }
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
