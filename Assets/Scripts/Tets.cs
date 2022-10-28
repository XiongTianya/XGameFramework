using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGameFramework;

public class Tets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        testInfo test = ReferencePool.Acquire<testInfo>();
        testInfo test2 = ReferencePool.Acquire<testInfo>();

        ReferencePool.Release(test);
        testInfo test3 = ReferencePool.Acquire<testInfo>();
        ReferencePool.Release(test2);
        ReferencePool.Release(test3);
        ReferencePool.Release(test3);

        // ReferenceCollection pool = ReferencePool.GetReferenceCollection(test.GetType());
        // Debug.Log(pool);
        Debug.Log(ReferencePool.Count);
        Debug.Log(ReferencePool.GetReferencePoolInfo<testInfo>().infoDesc);

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class testInfo : IReference
{
    public void Clear()
    {

    }

    public testInfo()
    {

    }
}