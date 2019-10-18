# AudioPlay

简单实现Unity音频管理脚本，便于长短音频的播放控制和音量控制等。
项目地址：[AudioPlay - SouthBegonia](https://github.com/SouthBegonia/UnityWorld/AudioPlay)

--------

### 包含方法：
- 持有所有 AudioSource
- 播放短音频（特效、技能等）
- 播放长音频（BGM）
- Slider调控音量

### 如何使用：
1. 导入目录下的 **AudioManager.unitypackage**
2. 新建空物体AudioManager，并挂载同名脚本（也可直接使用Prefabs内的预制体）
3. 在AudioManager上设定`AudioClipArray`大小，并从Project中拖拽音频文件
4. 创建Slider组件，挂载到AudioManger上；Slider上设定`AudioManager.SetVolume()`方法
5. 通过调用 `PlayEffect(string acName)` 、 `PlayBGM(string acName)` 、 `StopBGMPlay()` 等方法对音频播放进行操控
