using GameFramework;
using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace GDT
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        /// <summary>
        /// 获取实体逻辑脚本
        /// </summary>
        /// <param name="entityId">实体编号</param>
        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        /// <summary>
        /// 隐藏实体
        /// </summary>
        /// <param name="entity">实体逻辑脚本</param>
        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        /// <summary>
        /// 附加子实体
        /// </summary>
        /// <param name="entity">要附加的子实体的逻辑脚本</param>
        /// <param name="ownerId">被附加的父实体的实体编号</param>
        /// <param name="parentTransformPath">相对于被附加父实体的位置</param>
        /// <param name="userData">用户自定义数据</param>
        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

       
       
        /// <summary>
        /// 显示实体
        /// </summary>
        /// <param name="logicType">实体逻辑类型</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="data">实体数据</param>
        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }
            //获取实体数据表
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            
            //根据实体数据对象的类型ID，获取对应的实体数据表行
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }
            //显示实体
            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, data);
        }

        //TODO：简便显示实体的封装示例
        //public static void ShowAircraft(this EntityComponent entityComponent, AircraftData data)
        //{
        //    entityComponent.ShowEntity(typeof(Aircraft), "Aircraft", data);
        //}

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
