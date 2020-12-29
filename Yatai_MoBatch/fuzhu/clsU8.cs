using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using fuzhu;


namespace fuzhu
{
    public class clsU8
    {
        public static DataTable GetZT()
        {
            string sql = "SELECT cAccount FROM dbo.T_OMS_Account ";
            return DbHelper.ExecuteTable(sql);
        
        
        }




        /// <summary>
        /// 获得主U8数据库的主数据库
        /// </summary>
        /// <returns></returns>
        public static string GetU8Main()
        {
            string sql = "SELECT cAccount FROM dbo.T_OMS_Account where id = 1 ";
           return  DbHelper.GetDbString(DbHelper.ExecuteScalar(sql));
           


        }

    

        /// <summary>
   

        //检查是否存在,sql ”select　count(1) from  ...
        public static bool isExist(string sql, string strConn)
        {
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql,strConn)) <= 0)
                return false;
            return true;
        }
        //判断料号是否存在
        public static bool isExistInvCode(string cinvcode, string strConn)
        {
            string sql = "select count(*) from inventory where cinvcode='" + cinvcode + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }

        //判断存货分类是否存在
        public static bool isExistInvcCode(string cinvcname, string strConn)
        {
            string sql = "select count(*) from inventoryclass where cinvcname='" + cinvcname + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }

        //取存货分类编码
        public static string getInvcCode(string cinvcname, string strConn)
        {

            try
            {
               

                string sql = "select cinvccode from inventoryclass where cinvcname='" + cinvcname + "'";
                string cInvccode = DbHelper.GetDbString(DbHelper2.ExecuteScalar(sql, strConn));
                if (string.IsNullOrEmpty(cInvccode))
                {


                    sql = "select max(convert(int,cinvccode)) from inventoryclass";
                    int iMax =DbHelper.GetDbInt(DbHelper2.ExecuteScalar(sql,strConn));
                    cInvccode = (iMax + 1).ToString().PadLeft(2,'0');

                    sql = string.Format(@"insert into inventoryclass(cInvCCode ,
          cInvCName ,
          iInvCGrade ,
          bInvCEnd ,
          cEcoCode ) values('{0}','{1}',1,1,'{0}')", cInvccode, cinvcname);
                    DbHelper2.ExecuteNonQuery(sql, strConn);


                }
                return cInvccode;
            }
            catch (Exception ex)
            {
                return "存货分类添加错误:" + ex.ToString();
            
            }

            

        }

        //判断客户编码是否存在
        public static bool isExistCusCode(string ccuscode, string strConn)
        {
            string sql = "select count(*) from customer where ccuscode='" + ccuscode + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }


        //判断供应商编码是否存在
        public static bool isExistVenCode(string cvencode, string strConn)
        {
            string sql = "select count(*) from vendor where cvencode='" + cvencode + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }


        //判断仓库是否存在
        public static bool isExistWhcode(string cwhcode, string strConn)
        {
            string sql = "select count(*) from warehouse where cwhcode='" + cwhcode + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }

        //判断部门是否存在
        public static bool isExistDepcode(string cdepcode, string strConn)
        {
            string sql = "select count(*) from department where cdepcode='" + cdepcode + "'";
            if (Convert.ToInt32(DbHelper2.ExecuteScalar(sql, strConn)) > 0)
                return true;
            return false;
        }
    }
}
