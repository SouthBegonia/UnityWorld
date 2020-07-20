using GameFramework;
using GameFramework.DataTable;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace GDT
{
    public static class SoundExtension
    {
        private const float FadeVolumeDuration = 1f;
        private static int? s_MusicSerialId = null;

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="musicId">音乐编号</param>
        /// <param name="userData">用户自定义数据</param>
        public static int? PlayMusic(this SoundComponent soundComponent, int musicId, object userData = null)
        {
            
            soundComponent.StopMusic();

            //获取音乐数据表，然后根据音乐编号获取到对应的数据表行对象
            IDataTable<DRMusic> dtMusic = GameEntry.DataTable.GetDataTable<DRMusic>();
            DRMusic drMusic = dtMusic.GetDataRow(musicId);
            if (drMusic == null)
            {
                Log.Warning("Can not load music '{0}' from data table.", musicId.ToString());
                return null;
            }

            //创建播放声音参数对象
            PlaySoundParams playSoundParams = new PlaySoundParams
            {
                Priority = 64,
                Loop = true,
                VolumeInSoundGroup = 1f,
                FadeInSeconds = FadeVolumeDuration,
                SpatialBlend = 0f,
            };

            s_MusicSerialId = soundComponent.PlaySound(AssetUtility.GetMusicAsset(drMusic.AssetName), "Music",playSoundParams.Priority,playSoundParams,userData);
            return s_MusicSerialId;
        }

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (!s_MusicSerialId.HasValue)
            {
                return;
            }

            soundComponent.StopSound(s_MusicSerialId.Value, FadeVolumeDuration);
            s_MusicSerialId = null;
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundId">声音编号</param>
        /// <param name="bindingEntity">声音绑定的实体</param>
        /// <param name="userData">用户自定义数据</param>
        public static int? PlaySound(this SoundComponent soundComponent, int soundId, Entity bindingEntity = null, object userData = null)
        {
            IDataTable<DRSound> dtSound = GameEntry.DataTable.GetDataTable<DRSound>();
            DRSound drSound = dtSound.GetDataRow(soundId);
            if (drSound == null)
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = new PlaySoundParams
            {
                Priority = drSound.Priority,
                Loop = drSound.Loop,
                VolumeInSoundGroup = drSound.Volume,
                SpatialBlend = drSound.SpatialBlend,
            };

            return soundComponent.PlaySound(AssetUtility.GetSoundAsset(drSound.AssetName), "Sound",playSoundParams.Priority, playSoundParams, bindingEntity != null ? bindingEntity.Entity : null, userData);
        }

        /// <summary>
        /// 播放界面声音
        /// </summary>
        /// <param name="uiSoundId">界面声音编号</param>
        /// <param name="userData">用户自定义数据</param>
        public static int? PlayUISound(this SoundComponent soundComponent, int uiSoundId, object userData = null)
        {
            IDataTable<DRUISound> dtUISound = GameEntry.DataTable.GetDataTable<DRUISound>();
            DRUISound drUISound = dtUISound.GetDataRow(uiSoundId);
            if (drUISound == null)
            {
                Log.Warning("Can not load UI sound '{0}' from data table.", uiSoundId.ToString());
                return null;
            }

            PlaySoundParams playSoundParams = new PlaySoundParams
            {
                Priority = drUISound.Priority,
                Loop = false,
                VolumeInSoundGroup = drUISound.Volume,
                SpatialBlend = 0f,
            };

            return soundComponent.PlaySound(AssetUtility.GetUISoundAsset(drUISound.AssetName), "UISound",playSoundParams.Priority, playSoundParams, userData);
        }

        /// <summary>
        /// 是否静音
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        public static bool IsMuted(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return true;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return true;
            }

            return soundGroup.Mute;
        }

        /// <summary>
        /// 静音
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="mute">是否静音</param>
        public static void Mute(this SoundComponent soundComponent, string soundGroupName, bool mute)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Mute = mute;

            GameEntry.Setting.SetBool(string.Format(Constant.Setting.SoundGroupMuted, soundGroupName), mute);
            GameEntry.Setting.Save();
        }

        /// <summary>
        /// 获取音量
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="volume">音量大小</param>
        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            GameEntry.Setting.SetFloat(string.Format(Constant.Setting.SoundGroupVolume, soundGroupName), volume);
            GameEntry.Setting.Save();
        }
    }
}
