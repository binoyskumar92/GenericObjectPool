using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolTester : MonoBehaviour {

    // Use this for initialization


    //   public ObjectPool<int> abc;
    //   private void Awake()
    //   {
    //       abc = new ObjectPool<int>(typeof(int), 20,40, true);
    //   }
    //   void Start () {

    //       StartCoroutine(destroyAfter(5));
    //   }

    //// Update is called once per frame
    //void Update () {
    //       if (abc.getNumberOfObjectsInPool() > 40)
    //       {
    //           abc.willGrow = false;
    //       }
    //       int obje = (abc.GetObjectFromPool()) != null ? (int)abc.GetObjectFromPool():0;

    //       //StartCoroutine(waitAndRun(obje));

    //       Debug.Log(obje + " : "+ abc.getNumberOfObjectsInPool());
    //   }
    //   public IEnumerator waitAndRun(int obje)
    //   {
    //       // WAIT FOR 3 SEC
    //       yield return new WaitForSeconds(Time.deltaTime*2);
    //       // RUN YOUR CODE HERE ...
    //       abc.AddBackToPool(obje);
    //   }
    //   public IEnumerator destroyAfter(int seconds)
    //   {
    //       // WAIT FOR 3 SEC
    //       yield return new WaitForSeconds(seconds);
    //       // RUN YOUR CODE HERE ...
    //       abc.destroyObjectsinPool();
    //   }

    public GameObject poolobject;
    public float upForce = 1f;
    public float sideForce = 0.1f;
    ObjectPool<GameObject> pool;
    Queue<int> a;
    private void Awake()
    {
      pool = new ObjectPool<GameObject>(20, 40,new object[]{ }, true, poolobject);
      pool.InstantiateGameObjects();
    }
    private void Update()
    {

        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce/2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);
        GameObject obj = pool.GetObjectFromPool();
        if (obj != null && !obj.Equals(default(GameObject)))
        {
            obj.SetActive(true);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.GetComponent<Rigidbody>().velocity = force;
            Debug.Log("PoolSize: " + pool.getNumberOfObjectsInPool());
            StartCoroutine(AddBackToPosol(obj, 2f));
        }

        //if (obj != null && !obj.Equals(default(GameObject)))
        //{
        //    Instantiate(obj);
        //    obj.SetActive(true);
        //    obj.GetComponent<Rigidbody>().velocity = force;
        //    //  StartCoroutine(AddBackToPosol(obj, 2f));

        //}


    }
    IEnumerator AddBackToPosol(GameObject poolobject, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        poolobject.SetActive(false);
        pool.AddBackToPool(poolobject);
    }
}
