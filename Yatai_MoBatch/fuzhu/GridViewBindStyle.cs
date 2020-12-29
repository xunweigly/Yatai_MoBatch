using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace fuzhu
{
    class GridViewBindStyle
    {
        #region 窗体布局保存读取
        // 数据库读取标题,只改标题和是否更改。宽度和是否可见通过布局设置
        public static void Bind(string strTableName,GridView  dgvMain)
        {
            try
            {

                string sql = string.Format("select ccolname,iwidth ,bvisible,bedit,ccolcaption from zdy_gly_columnset where ctablename = '{0}' ", strTableName);
                //MessageBox.Show(sql);
                DataTable dt = DbHelper.ExecuteTable(sql);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string cColname = row["ccolname"].ToString();
                        //GridColumn gridCol = new GridColumn();
                        //gridCol.Name=cColname;
                        //if (dgvMain.Columns.Contains(gridCol))
                        //{
                        string cCaption = DbHelper.GetDbString(row["ccolcaption"]);
                        if (!string.IsNullOrEmpty(cCaption))
                        {
                           //dgvMain.Columns[cColname].OptionsColumn.AllowEdit = Convert.ToBoolean(row["bedit"]);
                            dgvMain.Columns[cColname].Caption = DbHelper.GetDbString(row["ccolcaption"]);
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
