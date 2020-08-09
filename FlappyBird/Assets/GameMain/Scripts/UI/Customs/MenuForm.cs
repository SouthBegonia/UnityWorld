using FlappyBird;
/// <summary>
/// 菜单界面脚本
/// </summary>
public class MenuForm : UGuiForm
{
    /// <summary>
    /// 菜单流程
    /// </summary>
    private ProcedureMenu m_ProcedureMenu = null;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        m_ProcedureMenu = (ProcedureMenu)userData;
    }

    protected override void OnClose(object userData)
    {
        m_ProcedureMenu = null;
        base.OnClose(userData);
    }

    public void OnStartButtonClick()
    {
        m_ProcedureMenu.IsStartGame = true;
    }

    public void OnSettingButtonClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
    }
}