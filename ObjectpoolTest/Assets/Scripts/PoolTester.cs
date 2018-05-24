using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class to test the working of Generic object pool
/// </summary>
public class PoolTester : MonoBehaviour
{

    public GameObject _poolobject;
    public float _upForce = 1f;
    public float _sideForce = 0.1f;
    ObjectPool<GameObject> _pool;
 
    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(20, 400, SimpleGameObjectConstructor);
    }

    private void Update()
    {
        //Generates a random force for 3 axis
        float xForce = Random.Range(-_sideForce, _sideForce);
        float yForce = Random.Range(_upForce / 2f, _upForce);
        float zForce = Random.Range(-_sideForce, _sideForce);
        Vector3 force = new Vector3(xForce, yForce, zForce);

        //Now get an object from pool and apply the force on the rigidbody component
        GameObject obj = _pool.GetObjectFromPool();
        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = new Vector3(0f, 0f, 0f);
            obj.GetComponent<Rigidbody>().velocity = force;
            // Add object back to pool after an interval of 12s. Modify this to see the effect of growing pool
            StartCoroutine(AddBackToPosol(obj, 12f));
        }
    }

    /// <summary>
    /// Method that helps in creating a gameobject
    /// </summary>
    /// <returns></returns>
    public GameObject SimpleGameObjectConstructor()
    {
        GameObject obj = GameObject.Instantiate(_poolobject);
        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Helper method to add objects back to pool after an interval
    /// </summary>
    /// <param name="poolobject">Object to be added back to pool</param>
    /// <param name="delayTime">Interval after which objects need to be added back to pool</param>
    /// <returns></returns>
    IEnumerator AddBackToPosol(GameObject poolobject, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        poolobject.SetActive(false);
        _pool.AddBackToPool(poolobject);
    }
}
