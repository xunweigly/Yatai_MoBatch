using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using fuzhu;
using System.Data.SqlClient;
using System.IO;

namespace LKU8.shoukuan
{
    public partial class UserControl1 : UserControl
    {



        DataTable dt, dt2, dt3;
        string chk2;
        string XMLPath = "";
        public UserControl1()
        {
            InitializeComponent();

            
        }

        #region 加载
      private void UserControl1_Load(object sender, EventArgs e)
        {
            //DevExpress.Accessibility.AccLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressUtilsLocalizationCHS();
            //DevExpress.XtraBars.Localization.BarLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraBarsLocalizationCHS();
            //DevExpress.XtraCharts.Localization.ChartLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraChartsLocalizationCHS();
            //DevExpress.XtraEditors.Controls.Localizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraEditorsLocalizationCHS();
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraGridLocalizationCHS();
            DevExpress.XtraLayout.Localization.LayoutLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraLayoutLocalizationCHS();
            //DevExpress.XtraPivotGrid.Localization.PivotGridLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraPivotGridLocalizationCHS();
            //DevExpress.XtraPrinting.Localization.PreviewLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraPrintingLocalizationCHS();
            //DevExpress.XtraRichEdit.Localization.XtraRichEditLocalizer.Active = new DevExpress.LocalizationCHS.DevExpressXtraRichEditLocalizationCHS();

            XMLPath = Application.StartupPath + @"\\config" + "\\";
            if (!Directory.Exists(Application.StartupPath + @"\\config"))
                Directory.CreateDirectory(Application.StartupPath + @"\\config");
            //更新布局
            string fileName = XMLPath + canshu.cMenuname;
            if (File.Exists(fileName))
            {
                gridView1.RestoreLayoutFromXml(fileName);
            }
            fileName = XMLPath + canshu.cMenuname+"-2";
            if (File.Exists(fileName))
            {
                gridView2.RestoreLayoutFromXml(fileName);
            }
            fileName = XMLPath + canshu.cMenuname + "-3";
            if (File.Exists(fileName))
            {
                gridView3.RestoreLayoutFromXml(fileName);
            }
         
        }
        #endregion#region 查询
      public void Cx()
      {
          try
          {
                 SearchCondition searchObj = new SearchCondition();
                      searchObj.AddCondition("a.dTLDate", dateTimePicker4.Value.ToString("yyyy-MM-dd HH:mm"), SqlOperator.MoreThanOrEqual, dateTimePicker4.Checked == false);
                      searchObj.AddCondition("a.dTLDate", dateTimePicker5.Value.ToString("yyyy-MM-dd HH:mm"), SqlOperator.LessThanOrEqual, dateTimePicker5.Checked == false);
                      searchObj.AddCondition("c.textEdit1", textEdit1.Text, SqlOperator.Equal);
                      searchObj.AddCondition("c.cMoBatch", txtcCode.Text, SqlOperator.Equal);
                      string conditionSql = searchObj.BuildConditionSql(2);
                    string  sql = @"  SELECT b.MoCode,c.InvCode,inv.cInvName,inv.cInvStd,com.cComUnitName,
c.Qty,d.cDepName,a.cMoBatch,[dTLDate]
      ,[dFYEndTime]
      ,[dFYHour]
      ,[dWetWeight]
      ,[dDryStartTime]
      ,[dDryEndTime]
      ,[dDryHour]
      ,[dWeight]
      ,[dYield]
      ,[cYieldScope]
      ,[dSumHour]
      ,[dEstEndDate]
      ,[cMemo] FROM   zdy_yatai_MoBatch a
INNER JOIN dbo.mom_orderdetail c  ON a.imodid =c.MoDId
INNER JOIN dbo.Inventory inv ON c.InvCode=inv.cInvCode
INNER JOIN dbo.ComputationUnit com ON inv.cComUnitCode=com.cComunitCode
INNER  JOIN dbo.mom_order b ON b.MoId=c.MoId
LEFT JOIN dbo.Department d ON c.MDeptCode=d.cDepCode where 1=1  ";

                  
                      sql += conditionSql;
                 
                
                  dt = DbHelper.ExecuteTable(sql);
                  gridControl1.DataSource = dt;
                  gridView1.PopulateColumns();
                 


                  RefreshDV2();
              
           
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return;
          
          }
      }
      #endregion

      #region  自动换行
      private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
      {
          if (e.FocusedRowHandle < 0)
          {
              gridControl2.DataSource = null;
              return;
          }

          RefreshDV2();
      }

      private void RefreshDV2()
      {
          try
          {
              string id="0";
             
                  id = DbHelper.GetDbString(gridView1.GetFocusedRowCellValue("id"));
                  string sql = string.Format(@"SELECT 
      [Id]
      ,[cInvCode]
      ,[cInvName]
      ,[cComUnitName]
      ,[dQty],cBatch
      ,[cbMemo]
  FROM  [zdy_yatai_MoBatchs] where id = '{0}'", id);

                    gridControl2.DataSource = DbHelper.Execute(sql).Tables[0];
                  //MessageBox.Show(sql);
                 
             sql = string.Format(@"SELECT [dateid]
      ,[id]
      ,[dDate]
      ,[iModid]
      ,[cMoBatch]
      ,[dTLDate]
      ,[dFYEndTime]
      ,[dFYHour]
      ,[dWetWeight]
      ,[dDryStartTime]
      ,[dDryEndTime]
      ,[dDryHour]
      ,[dWeight]
      ,[dYield]
      ,[cYieldScope]
      ,[dSumHour]
      ,[dEstEndDate]
      ,[cMemo]
      ,[cMaker]
  FROM  [zdy_yatai_MoBatch_Log] where id = '{0}'", id);


             gridControl3.DataSource = DbHelper.Execute(sql).Tables[0];
           
         
           
          }
          catch (Exception ex)
          {
             
              DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return;

          }
      }
      #endregion

      #region 选择，全选
      public void Checkall()
      {
         
      }
      #endregion
        
      #region 手工同步
      private void button5_Click(object sender, EventArgs e)
      {
          GengXin();

      }

      public void GengXin()
      {
          
        
          try
          {
              
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
              //string cError = cLx + "更新错误" + ex.Message;
              //DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return;
          }
      }

      #region 使用存储过程
      /// <summary>
      /// 更新存货档案,从中间表,更新到ERP.
      /// </summary>
      private static string UpdateProc(string cTable, string cName)
      {


          #region  修改程序，改用存储过程
          try
          {

              DbHelper.ExecuteNonQuery("exec " + cTable);
              return "";
          }
          catch (Exception ex)
          {

              string cError = cName + "更新错误" + ex.Message;
              DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return cError;
          }
          #endregion
      }
      #endregion


      #endregion

      private static void UpdateXT()
      {


          #region  修改程序，改用存储过程
          try
          {

              DbHelper.ExecuteNonQuery("exec zdy_HSXT_auto");
          }
          catch (Exception ex)
          {

              string cError = "采购协同更新错误" + ex.Message;
              DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return;
          }
          #endregion
      }

      private static void UpdateXTXS()
      {


          #region  修改程序，改用存储过程
          try
          {

              DbHelper.ExecuteNonQuery("exec zdy_HSXT_auto_xs");
          }
          catch (Exception ex)
          {

              string cError = "销售协同更新错误" + ex.Message;
              DbHelper.WriteError(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, "");

              return;
          }
          #endregion
      }


       #region 布局
      #region 布局
      public void SaveBuju()
      {
         
          string fileName = XMLPath + canshu.cMenuname;
          gridView1.SaveLayoutToXml(fileName);
          fileName = XMLPath + canshu.cMenuname + "-2"; 
          gridView2.SaveLayoutToXml(fileName);
          fileName = XMLPath + canshu.cMenuname + "-3"; 
          gridView3.SaveLayoutToXml(fileName);
          CommonHelper.MsgInformation("保存布局成功！");

      }

      public void DelBuju()
      {

          string fileName = XMLPath+ canshu.cMenuname + "-2";

          if (File.Exists(fileName))
          {
              File.Delete(fileName);
          }

          fileName = XMLPath  + canshu.cMenuname + "-3"; ;
          if (File.Exists(fileName))
          {
              File.Delete(fileName);
              CommonHelper.MsgInformation("删除布局成功，请重新打开！");
          }

          fileName = XMLPath + canshu.cMenuname;
          if (File.Exists(fileName))
         {
              File.Delete(fileName);
              CommonHelper.MsgInformation("删除布局成功，请重新打开！");
          }
          else
          {
              CommonHelper.MsgInformation("布局未保存，无需删除！");
          }
      }

      #endregion

       #endregion

       #region 导出excel
       public void Excel()
       {

           SaveFileDialog saveFileDialog = new SaveFileDialog();
           saveFileDialog.Title = "导出Excel";
           saveFileDialog.Filter = "Excel文件(*.pdf)|*.pdf";
           saveFileDialog.Filter = "Excel文件(*.xls)|*.xls"; DialogResult dialogResult = saveFileDialog.ShowDialog(this);

           if (dialogResult == DialogResult.OK)
           {
               DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();

               // gridCo 
               gridView1.ExportToXls(saveFileDialog.FileName);
               DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }

       }
       #endregion
         

       private void btncx_Click(object sender, EventArgs e)
       {
           Cx();
       }


       

    









    }
}
