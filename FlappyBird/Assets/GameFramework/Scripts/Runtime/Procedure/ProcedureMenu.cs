using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace FlappyBird
{
    /// <summary>
    /// 菜单流程
    /// </summary>
    public class ProcedureMenu : ProcedureBase
    {
        public bool IsStartGame { get; set; }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            IsStartGame = false;
            
            // TODO 打开UI页面
            //GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if(IsStartGame)
            {
                procedureOwner.SetData<VarInt>(ConstantForce.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Main"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}