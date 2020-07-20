using GameFramework;
using System;
using UnityGameFramework.Runtime;

namespace GDT
{
    public static class DataTableExtension
    {
        /// <summary>
        /// 数据行前缀名（根据项目命名空间决定）
        /// </summary>
        private const string DataRowClassPrefixName = "GDT.DR";
        private static readonly string[] ColumnSplit = new string[] { "\t" };

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <param name="dataTableName">数据表名称</param>
        /// <param name="userData">用户自定义数据</param>
        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            //获取数据表行类名
            string dataRowClassName = DataRowClassPrefixName + splitNames[0];

            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
            dataTableComponent.LoadDataTable(dataRowType, dataTableName, dataTableNameInType, AssetUtility.GetDataTableAsset(dataTableName), userData);
        }

        /// <summary>
        /// 切割数据表行文本
        /// </summary>
        /// <param name="dataRowText">数据表行文本</param>
        public static string[] SplitDataRow(string dataRowText)
        {
            return dataRowText.Split(ColumnSplit, StringSplitOptions.None);
        }
    }
}
