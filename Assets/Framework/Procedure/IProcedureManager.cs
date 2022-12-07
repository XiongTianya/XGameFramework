using XGameFramework.Fsm;
using System;
namespace XGameFramework.Procedure
{
    public interface IProcedureManager
    {
        ProcedureBase CurrentProcedure
        {
            get;
        }

        float CurrentProcedureTime
        {
            get;
        }

        //void Initialize(IFsmManager)
    }
}