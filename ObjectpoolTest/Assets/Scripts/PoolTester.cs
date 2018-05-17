using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTester : MonoBehaviour {

	// Use this for initialization
 
   
    public ObjectPool<int> abc;
    private void Awake()
    {
        abc = new ObjectPool<int>(typeof(int), 20,40, true);
    }
    void Start () {

        StartCoroutine(destroyAfter(5));
    }
	
	// Update is called once per frame
	void Update () {
        if (abc.getNumberOfObjectsInPool() > 40)
        {
            abc.willGrow = false;
        }
        int obje = (abc.GetObjectFromPool()) != null ? (int)abc.GetObjectFromPool():0;
       
        //StartCoroutine(waitAndRun(obje));
        
        Debug.Log(obje + " : "+ abc.getNumberOfObjectsInPool());
    }
    public IEnumerator waitAndRun(int obje)
    {
        // WAIT FOR 3 SEC
        yield return new WaitForSeconds(Time.deltaTime*2);
        // RUN YOUR CODE HERE ...
        abc.AddBackToPool(obje);
    }
    public IEnumerator destroyAfter(int seconds)
    {
        // WAIT FOR 3 SEC
        yield return new WaitForSeconds(seconds);
        // RUN YOUR CODE HERE ...
        abc.destroyObjectsinPool();
    }
}
