using System;
using System.Collections.Generic;

/// <summary>
/// A Generic Object Pool class
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : class
{
    /// <summary>
    /// Number of objects that needs to be preallocated in pool when pool is created
    /// </summary>
    private int _objectsToPreAllocate;

    /// <summary>
    /// Maximum number of objects that pool can hold at any time 
    /// </summary>
    private int _maxNumberOfObjects;

    /// <summary>
    /// Number of extra objects that can be grown other than that was preallocated
    /// </summary>
    private int _extrObjectsCount;

    /// <summary>
    /// Queue which holds all the objects in pool.
    /// </summary>
    private Queue<T> _pooledObjects;

    /// <summary>
    /// Factory method which helps in creating an object that need to be stored in pool.
    /// </summary>
    private Func<T> _factoryMethod;
  

    /// <summary>
    /// Constructor for creating a generic ObjectPool
    /// </summary>
    /// <param name="objectsToPreAllocate">Specifies the number of objects that needs to be allocated to pool on creation</param>
    /// <param name="maxNumberOfObjects">Specifies maximum number to which pool can be grown</param>
    /// <param name="func">A constructor or a method that returns the actual object that need to be stored in the pool</param>
    public ObjectPool(int objectsToPreAllocate, int maxNumberOfObjects, Func<T> func)
    {
        _objectsToPreAllocate = objectsToPreAllocate;
        SetMaxNumberOfObjects(maxNumberOfObjects);
        _factoryMethod = func;

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

    /// <summary>
    /// Method to get/retrieve an object stored in pool
    /// </summary>
    /// <returns>An object stored in pool</returns>
    public T GetObjectFromPool()
    {
        T genericObject = null;
        //Pool already has free objects
        if (GetNumberOfObjectsInPool() > 0)
        {
            genericObject = _pooledObjects.Dequeue(); ;

        }
        // Pool doesnt have free objects but it can grow as _maxNumberOfObjects > _objectsToPreAllocate
        else if (_extrObjectsCount > 0)
        {
            genericObject = _factoryMethod.Invoke();
            _extrObjectsCount--;
        }
        return genericObject;

    }

    /// <summary>
    /// Method to add an object back to pool
    /// </summary>
    /// <param name="returningObject">Object passed to be added back to pool</param>
    public void AddBackToPool(T returningObject)
    {
        // Checking if returning object is not null, current size of pool not greater than_maxNumberOfObjects and returning object type is same as pool type
        if (!returningObject.Equals(null) && GetNumberOfObjectsInPool() < _maxNumberOfObjects && returningObject.GetType().Equals(typeof(T)))
        {
            _pooledObjects.Enqueue(returningObject);
        }

    }

    /// <summary>
    /// Method to find how many objects are there currently in pool
    /// </summary>
    /// <returns>Number of objects</returns>
    public int GetNumberOfObjectsInPool()
    {
        return _pooledObjects.Count;
    }

    /// <summary>
    /// Destroys all objects in pool
    /// </summary>
    public void DestroyObjectsinPool()
    {
        _pooledObjects.Clear();
    }

    /// <summary>
    /// Set maximum number of objects a pool can hold. This helps in setting if the pool can grow or not. If _maxNumberOfObjects &lt; _objectsToPreAllocate   will not grow
    /// </summary>
    /// <param name="maxObjects">Maximum number of objects pool can hold</param>
    public void SetMaxNumberOfObjects(int maxObjects)
    {
        _maxNumberOfObjects = maxObjects;
        _extrObjectsCount = _maxNumberOfObjects - _objectsToPreAllocate;
    }

}
