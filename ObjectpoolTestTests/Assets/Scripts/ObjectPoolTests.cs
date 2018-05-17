using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Tests
{
    [TestClass()]
    public class ObjectPoolTests
    {
    

        [TestMethod()]
        public void GetObjectFromPoolTest()
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(typeof(Pool), 20, 40, true);
            Pool[] obj = new Pool[20];
            for(int i = 0; i < 20; i++)
            {
                obj[i] = (Pool)pool.GetObjectFromPool();
                Assert.AreEqual(obj[i].GetType(), typeof(Pool));
            }
           

        }

        [TestMethod()]
        public void ObjectPreallocateTest()
        {
            ObjectPool<int> pool = new ObjectPool<int>(typeof(int), 20, 40, true);
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
        }

        [TestMethod()]
        public void MaxPoolCapacityTest()
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(typeof(Pool), 20, 40, true);
            Pool[] obj = new Pool[50];
            for (int i = 0; i < 50; i++)
            {
                obj[i] = (Pool)pool.GetObjectFromPool();
            }
            for (int i = 0; i < 50; i++)
            {
                pool.AddBackToPool(obj[i]);
            }
            Assert.AreEqual(pool.getNumberOfObjectsInPool(),40);

        }
        [TestMethod()]
        public void DestroyObjectsinPoolTest()
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(typeof(Pool), 20, 40, true);
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
            pool.destroyObjectsinPool();
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 0);
        }

        [TestMethod()]
        public void AddBackToPoolTst()
        {
            ObjectPool<Pool> pool = new ObjectPool<Pool>(typeof(Pool), 20, 40, true);
            Pool obj = (Pool)pool.GetObjectFromPool();
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 19);
            pool.AddBackToPool(obj);
            Assert.AreEqual(pool.getNumberOfObjectsInPool(), 20);
        }
    }
}

