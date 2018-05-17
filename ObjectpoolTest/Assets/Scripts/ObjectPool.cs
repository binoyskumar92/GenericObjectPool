using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    public static ObjectPool<T> current;
    public T pooledObject;
    public int objectsToPreAllocate = 20;
    public int maxNumberOfObjects;
    public bool willGrow = true;
    public Queue<object> pooledObjects;
    public Type ObjectType;

    public bool destroyInProgress { get; private set; }
    
    public ObjectPool(Type ObjectType, int objectsToPreAllocate,int maxNumberOfObjects, bool willGrow){

        this.objectsToPreAllocate = objectsToPreAllocate;
        this.maxNumberOfObjects = maxNumberOfObjects;
        this.willGrow = willGrow;
        this.ObjectType = ObjectType;
        this.destroyInProgress = false;

        // Queue which keeps all the queued objects
        pooledObjects = new Queue<object>();

        for(int i=0;i< this.objectsToPreAllocate; i++)
        {
            createGenericObjectAndAddtoQueue(); 
        }
        
    }
    // helper method to create a pool object and add to pool
    private object createGenericObjectAndAddtoQueue()
    {     
        // T obj =  (T)Activator.CreateInstance(typeof(T));
       // object obj = Activator.CreateInstance(TheType,new object[] { 1, 'a' });
        object obj = Activator.CreateInstance(ObjectType);
        pooledObjects.Enqueue(obj);
        return obj;
            
    }
    //helper to get an object from pool
    public object GetObjectFromPool()
    {
        
            if (getNumberOfObjectsInPool() > 0 && !destroyInProgress)
            {
                return pooledObjects.Dequeue();
            }
            else if (willGrow && getNumberOfObjectsInPool() < maxNumberOfObjects && !destroyInProgress)
            {
                return createGenericObjectAndAddtoQueue();
            }
            else
            {
                //return default(T);
                return null;
            }
    }
    //helper to add object back to pool
    public void AddBackToPool(object returningObject)
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
