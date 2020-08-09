using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace FlappyBird
{
    /// <summary>
    /// 设置界面脚本
    /// </summary>
    public class SettingForm : UGuiForm
    {
        public void OnCloseButtonClick()
        {
            Close();
        }

        public void OnMusicSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Music", value);
        }

        public void OnSoundSettingValueChange(float value)
        {
            GameEntry.Sound.SetVolume("Sound", value);
            GameEntry.Sound.SetVolume("UISound", value);
        }
    }
}