using GameFramework.Fsm;
using GameFramework.Procedure;

namespace FlappyBird
{
    /// <summary>
    /// 主流程
    /// </summary>
    public class ProcedureMain : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Entity.ShowBg(new BgData(GameEntry.Entity.GenerateSerialId(), 1, 1f, -10));
        }

    }
}