# Generic Object Pool
A generic object pool for any kind of reference types. This includes GameObjects, Queues, Lists, Customclasse objects etc. Following unity porjects helps to show an implementation of the generic object pool. All the c# script is in Asset->Scripts and the unit testcases in Asset->Scripts->Editor folder.
Usage:
1. Copy class ObjectPool.cs into your project from Asset->Scripts folder
2. ObjectPool<T> pool = new ObjectPool<T>(<preallocatesize>, <maxpoolsize>, <new object[] { }>, <willgrow>); Use a class of your choice for T and provide necessary parameters for objectpool constructor.
3. Use GetObjectFromPool() method to get an object from the pool
4. Use AddBackToPool(T obj) method to add a used object back to pool.
5. use destroyObjectsinPool() to destroy all objects int he pool
6. In case of Gameobject pool, provide an optional parameter, a game object while initialising the pool whose clone wil be created and stored in the pool.
7. In case of GameObject type pool make sure to use the method InstantiateGameObjects() so as to initialise all gameobjects and store. Else exception wil be thrown.
8. All the unit tests for all scenarios are covered in ObjectPollTests.cs file in Asset->Scripts->Editor folder.

