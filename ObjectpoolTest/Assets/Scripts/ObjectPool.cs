using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    /*  _pooledObject - GameObject if to be instatiated [optional]
        _objectsToPreAllocate - Number of Objects in pool to be preallocated
        _maxNumberOfObjects - Max size of pool can grow upto
        _extrObjectsCount; -  how much pool can grow extra from preallocated size
        willGrow = true - boolean to toggle if pool grows to max size 
        _pooledObjects -   Queue which keeps all the queued objects
        _objectType - type of T at runtime
        _parameters - additional _parameters for new object creation
     */
    private int _objectsToPreAllocate;
    public int _maxNumberOfObjects;
    private int _extrObjectsCount;
    private Queue<T> _pooledObjects;
    private Func<T> _factoryMethod;

    public ObjectPool(int objectsToPreAllocate, int maxNumberOfObjects,Func<T> func)
    {
        
        _objectsToPreAllocate = objectsToPreAllocate;
        _maxNumberOfObjects = maxNumberOfObjects;
        _factoryMethod= func;       
        _extrObjectsCount = _maxNumberOfObjects - _objectsToPreAllocate;

        // Queue which keeps all the queued objects
        _pooledObjects = new Queue<T>();
        T genericObject;
        for (int i = 0; i < _objectsToPreAllocate; i++)
        {
            genericObject = _factoryMethod.Invoke();
            if (genericObject != null)
            {
                _pooledObjects.Enqueue(genericObject);
            }

        }
    }

    //method to get an object from pool
    public T GetObjectFromPool()
    {
        T genericObject = null;
        if (GetNumberOfObjectsInPool() > 0)
        {
            genericObject = _pooledObjects.Dequeue(); ;
            
        }else if (_extrObjectsCount > 0)
        {
            try
            {
                genericObject = _factoryMethod.Invoke();
                _extrObjectsCount--;
                Debug.Log("Objects that can be grown: "+_extrObjectsCount);
            }
            catch(Exception e)
            {
                Debug.Log("Error in creating a generic object while pool is growing: "+e.Message);
            }
        }
        return genericObject;

    }

    //method to add object back to pool
    public void AddBackToPool(T returningObject)
    {
        if (!returningObject.Equals(null) && GetNumberOfObjectsInPool() < _maxNumberOfObjects)
        {
            _pooledObjects.Enqueue(returningObject);
        }

    }

    //method to get count of objects currently in pool
    public int GetNumberOfObjectsInPool()
    {
        return _pooledObjects.Count;
    }

    //method to destroy object sin pool
    public void DestroyObjectsinPool()
    {
        _pooledObjects.Clear();
    }
    public void SetMaxNumberOfObjects(int maxObjects)
    {
        _maxNumberOfObjects = maxObjects;
        _extrObjectsCount = _maxNumberOfObjects - _objectsToPreAllocate;
    }

}
