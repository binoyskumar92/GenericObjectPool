using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolTester : MonoBehaviour {

   
    public GameObject poolobject;
    public float upForce = 1f;
    public float sideForce = 0.1f;
    ObjectPool<GameObject> _pool;
    Queue<int> a;
    private void Awake()
    {
   
        _pool = new ObjectPool<GameObject>(20,400, SimpleConstructor);
    }
    public GameObject makeObject()
    {
        Debug.Log("Inside make");
        return poolobject;
    }

    public GameObject SimpleConstructor()
    {
        GameObject obj = GameObject.Instantiate(poolobject);
        obj.SetActive(false);
        return obj;
    }

    private void Update()
    {

        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(upForce/2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);

        Vector3 force = new Vector3(xForce, yForce, zForce);
        GameObject obj = _pool.GetObjectFromPool();
        if (obj != null && !obj.Equals(default(GameObject)))
        {
            obj.SetActive(true);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.GetComponent<Rigidbody>().velocity = force;
            Debug.Log("PoolSize: " + _pool.GetNumberOfObjectsInPool());
            StartCoroutine(AddBackToPosol(obj, 15f));
        }
    }
    IEnumerator AddBackToPosol(GameObject poolobject, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        poolobject.SetActive(false);
        _pool.AddBackToPool(poolobject);
    }
}
