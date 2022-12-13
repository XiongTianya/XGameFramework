using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XGameFramework;
using XGameFramework.Procedure;
using XGameFramework.Fsm;
using ProcedureOwner = XGameFramework.Fsm.IFsm<XGameFramework.Procedure.IProcedureManager>;

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

        // ReferenceCollection pool = ReferencePool.GetReferenceCollection(test.GetType());
        // Debug.Log(pool);
        Debug.Log(ReferencePool.Count);
        Debug.Log(ReferencePool.GetReferencePoolInfo<testInfo>().infoDesc);

        FsmManager fsmManager = new FsmManager();
        ProcedureManager procedureManager = new ProcedureManager();

        procedureManager.Initialize(fsmManager, new ProcedureTest1(), new ProcedureTest2());
        procedureManager.StartProcedure<ProcedureTest1>();

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



public class ProcedureTest1 : XGameFramework.Procedure.ProcedureBase
{
    //ProcedureOwner procedureOwner = null;
    protected override internal void OnInit(ProcedureOwner procedureOwner)
    {
        procedureOwner = procedureOwner;
        base.OnInit(procedureOwner);
        Debug.Log("OnInit ProcedureTest1");
    }

    protected override internal void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        Debug.Log("OnEnter ProcedureTest1");
        ChangeState<ProcedureTest2>(procedureOwner);

    }

    protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        Debug.Log("OnUpdate ProcedureTest1");
        ChangeState<ProcedureTest2>(procedureOwner);
    }

    protected override internal void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        Debug.Log("OnLeave ProcedureTest1");
    }

}

public class ProcedureTest2 : XGameFramework.Procedure.ProcedureBase
{
    protected override internal void OnInit(ProcedureOwner procedureOwner)
    {
        base.OnInit(procedureOwner);
        Debug.Log("OnInit ProcedureTest2");
    }

    protected override internal void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);
        Debug.Log("OnEnter ProcedureTest2");
    }

    protected override internal void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
        Debug.Log("OnLeave ProcedureTest2");
    }

}
