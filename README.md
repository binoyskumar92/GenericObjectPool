# Generic Object Pool
This is a simple implementation of generic object pool for any type. This includes GameObjects, Queues, Lists, Customclass objects, primitives etc. Following unity project helps to show an implementation of the generic object pool. All the c# script is in Asset->Scripts and the unit testcases in Asset->Scripts->Editor folder.
Usage:
1. Copy class ObjectPool.cs into your project from Asset->Scripts folder
2. ObjectPool<T> pool = new ObjectPool<T>(< preallocatesize >, < maxpoolsize >, < FactoryMethod >); Use a class of your choice for T and provide necessary parameters for objectpool constructor.
3. Use GetObjectFromPool() method to get an object from the pool
4. Use AddBackToPool(T obj) method to add a used object back to pool.
5. Use destroyObjectsinPool() to destroy all objects int the pool
7. All the unit tests for all scenarios are covered in ObjectPollTests.cs file in Asset->Scripts->Editor folder.
8. To run the testcases use Unity testrunner. In Unity Window->TestRunner and Run all.
9. For a live use of the object pool run the scene_one in unity. A set of cubes will be initialised and thrown outwards in a loop.
  
  # Object pool in Action

![Loading ObjectPool.GIF...](https://media.giphy.com/media/vxr1aQ0EtdxHPGTlhI/giphy.gif)
