using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolTester : MonoBehaviour {

   
    public GameObject poolobject;
    public float upForce = 1f;
    public float sideForce = 0.1f;
    ObjectPool<GameObject> pool;
    Queue<int> a;
    private void Awake()
    {
      pool = new ObjectPool<GameObject>(20, 400,new object[]{ }, true, poolobject);
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
            StartCoroutine(AddBackToPosol(obj, 15f));
        }
    }
    IEnumerator AddBackToPosol(GameObject poolobject, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        poolobject.SetActive(false);
        pool.AddBackToPool(poolobject);
    }
}
