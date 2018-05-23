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
    private T _pooledObject;
    private int _objectsToPreAllocate;
    public int _maxNumberOfObjects;
    private int _extrObjectsCount;
    private bool _gameObjectsInitialised = false;
    private Queue<T> _pooledObjects;
    private Type _objectType;
    private object[] _parameters;

    public bool _destroyInProgress { get; private set; }

    public ObjectPool(int objectsToPreAllocate, int maxNumberOfObjects, object[] parameters, bool willGrow, GameObject poolobj = null)
    {

        _objectsToPreAllocate = objectsToPreAllocate;
        _maxNumberOfObjects = maxNumberOfObjects;
        _parameters = parameters;
        _objectType = typeof(T);
        _destroyInProgress = false;
        _extrObjectsCount = _maxNumberOfObjects - _objectsToPreAllocate;

        if (poolobj != null)
        {
            this._pooledObject = poolobj as T;
        }

        // Queue which keeps all the queued objects
        _pooledObjects = new Queue<T>();
        for (int i = 0; i < this._objectsToPreAllocate; i++)
        {
            if (_objectType == typeof(GameObject))
            {
                _pooledObjects.Enqueue(_pooledObject);
            }
            else
            {
                _pooledObjects.Enqueue(createGenericObject());
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
            if (_objectType == typeof(GameObject))
            {
                GameObject gameobject = GameObject.Instantiate(_pooledObject as GameObject);
                gameobject.SetActive(true);
                return gameobject as T;
            }
            if (_parameters.Length > 0)
            {
                obj = (T)Activator.CreateInstance(_objectType, _parameters);

            }
            else
            {
                obj = (T)Activator.CreateInstance(_objectType);
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
        if (_objectType != typeof(GameObject))
        {
            throw new Exception("Method can be invoked only for pool type GameObject");
        }
        else
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(_pooledObjects.Dequeue() as GameObject);
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj as T);
            }
            _gameObjectsInitialised = true;
        }
    }

    //method to get an object from pool
    public T GetObjectFromPool()
    {
        if (_objectType == typeof(GameObject) && !_gameObjectsInitialised)
        {
            throw new Exception("Please initialize gameobjects using InstantiateGameObjects method on pool reference");
        }

        if (getNumberOfObjectsInPool() > 0 && !_destroyInProgress)
        {
            T obj = _pooledObjects.Dequeue(); ;
            return obj;
        }
        else if (_extrObjectsCount > 0 && !_destroyInProgress)
        {
            _extrObjectsCount--;
            Debug.Log("Pool count: " + _extrObjectsCount);
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
        if (!returningObject.Equals(null) && getNumberOfObjectsInPool() < _maxNumberOfObjects && !_destroyInProgress)
        {
            _pooledObjects.Enqueue(returningObject);
        }

    }

    //method to get count of objects currently in pool
    public int getNumberOfObjectsInPool()
    {
        return _pooledObjects.Count;
    }

    //method to destroy object sin pool
    public void destroyObjectsinPool()
    {
        _destroyInProgress = true;
        while (getNumberOfObjectsInPool() > 0)
        {
            _pooledObjects.Dequeue();
        }
        _destroyInProgress = false;
    }


}
