using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LKU8.shoukuan;

namespace fuzhu
{
    class RD01API
    {
   
        public string InsertProduct(DataTable dt1,  DataTable dt2,string cAcc,string conStr)
        {

            SqlTransaction tran = DbHelper2.BeginTrans(conStr);
            try
            {
                              
                int iRdid = 1;
                int iRdsid = 1;

                //获取主键 id
                SqlParameter[] param = new SqlParameter[]{ 
                                         new SqlParameter("@RemoteId","00"),
                                         new SqlParameter("@cAcc_Id",cAcc),
                                         new SqlParameter("@cVouchType","RD"),
                                         new SqlParameter("@iAmount",dt2.Rows.Count),
                                         new SqlParameter("@iFatherId",iRdid),
                                         new SqlParameter("@iChildId",iRdsid)
                                         
          };
                param[4].Direction = ParameterDirection.Output;
                param[5].Direction = ParameterDirection.Output;
                DbHelper2.ExecuteSqlWithTrans("sp_getid", param, CommandType.StoredProcedure, tran);
                iRdid = DbHelper.GetDbInt(param[4].Value);
                iRdsid = DbHelper.GetDbInt(param[5].Value);
                iRdsid = iRdsid - dt2.Rows.Count;


                string cWhcode = DbHelper.GetDbString(DbHelper.ExecuteScalar("select cvalue from zdy_baoye_para where cname = 'cwhcode2'"));
                string cVencode = DbHelper.GetDbString(DbHelper.ExecuteScalar("select cvalue from zdy_baoye_para where cname = 'cvencode'"));
                string cRdcode = DbHelper.GetDbString(DbHelper.ExecuteScalar("select cvalue from zdy_baoye_para where cname = 'crdcode2'"));
                string cDepcode = DbHelper.GetDbString(DbHelper.ExecuteScalar("select cvalue from zdy_baoye_para where cname = 'cdepcode2'"));
                string cPoscode = DbHelper.GetDbString(DbHelper.ExecuteScalar("select cvalue from zdy_baoye_para where cname = 'cposcode2'"));
                    //写数据库，刷新

                    string sql1 = @"INSERT INTO RdRecord(ID,bRdFlag,cVouchType,cBusType,cSource,cWhCode,DDATE,CCODE,cVenCode,cOrderCode,bTransFlag,
cMaker,bpufirst,biafirst,VT_ID,bIsSTQc,ipurorderid,iTaxRate,iExchRate,cExch_Name,bOMFirst,bFromPreYear,
bIsComplement,iDiscountTaxType,ireturncount,iverifystate,iswfcontrolled,bredvouch,bCredit,
cHandler,dVeriDate,dnmaketime, dnverifytime,cdefine11,crdcode)
VALUES(@ID,1,'01','普通采购','库存',@cWhCode,@DDATE,@CCODE,@cVenCode,null,0,
@cMaker,0,0,'27',0,null,@iTaxRate,@iExchRate,@cExch_Name,0,0,
0,0,0,0,0,0,0,
@cHandler,@dVeriDate,getdate(),getdate(),@cdefine11,@crdcode)";
                                        DbHelper2.ExecuteSqlWithTrans(sql1, new SqlParameter[]{ 
                                                             new SqlParameter("@id",iRdid), 
                                                               new SqlParameter("@cWhCode",cWhcode),
                                                                 new SqlParameter("@DDATE", dt1.Rows[0]["DDATE"]),
                                                                 new SqlParameter("@CCODE", "RK" +DbHelper.GetDbString(dt1.Rows[0]["CCODE"])),
                                                                 new SqlParameter("@cVenCode",  cVencode),
                                                                 new SqlParameter("@cMaker",dt1.Rows[0]["cMaker"]),
                                                                 new SqlParameter("@iTaxRate", dt1.Rows[0]["iTaxRate"]),
                                                                 new SqlParameter("@iExchRate", dt1.Rows[0]["iExchRate"]),
                                                                 new SqlParameter("@cExch_Name", dt1.Rows[0]["cExch_Name"]),
                                                                 new SqlParameter("@cHandler",  dt1.Rows[0]["cHandler"]),
                                                                  new SqlParameter("@cdefine11",  dt1.Rows[0]["id"]),
                                                                 new SqlParameter("@dVeriDate",  dt1.Rows[0]["dVeriDate"]),
                                                                  new SqlParameter("@dnverifytime",  dt1.Rows[0]["dnverifytime"]),
                                                                   new SqlParameter("@crdcode", cRdcode)
                                        }, CommandType.Text, tran);

                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            iRdsid += 1;
                                            sql1 = string.Format(@"INSERT INTO rdrecords(AUTOID,ID,cInvCode,iQuantity,iUnitCost,iPrice,iAPrice,cBatch,iFlag,iSQuantity,iSNum,iMoney
,fACost,iNQuantity,bTaxCost,cPOID,
iMatSettleState,iBillSettleCount,bLPUseFree,iOriTrackID,bCosting,iExpiratDateCalcu,iordertype,
cDefine26,cDefine30,cDefine31,cdefine32, cdefine33, cdefine34,cfree1,cfree2,cfree3,inum,iinvexchrate,cPosition,cAssUnit)
VALUES( @AUTOID,@ID,@cInvCode,@iQuantity,@iUnitCost,@iPrice,@iAPrice,@cBatch,0,0,0,0
,@fACost,@iNQuantity,1,@cPOID,
0,0,0,0,1,0,0,@cDefine26,@cDefine30,@cDefine31,@cdefine32, @cdefine33, @cdefine34,@cfree1,@cfree2,@cfree3,@inum,@iinvexchrate,@cPosition,@cAssUnit)");
                                            DbHelper2.ExecuteSqlWithTrans(sql1, new SqlParameter[]{ 
                                                             new SqlParameter("@ID",iRdid),
                                                             new SqlParameter("@autoid",iRdsid),  
                                                             new SqlParameter("@cInvCode",dt2.Rows[i]["cinvcode"]),   
                                                             new SqlParameter("@iUnitCost",(DbHelper.GetDbdecimal(dt2.Rows[i]["iPrice"])*1.1m)/DbHelper.GetDbdecimal(dt2.Rows[i]["iQuantity"])),
                                                               new SqlParameter("@iQuantity", dt2.Rows[i]["iQuantity"]),
                                                                new SqlParameter("@iPrice", (DbHelper.GetDbdecimal(dt2.Rows[i]["iPrice"])*1.1m).ToString("0.00")),
                                                                 new SqlParameter("@iAPrice", dt2.Rows[i]["iAPrice"]),
                                                                  new SqlParameter("@cBatch", dt2.Rows[i]["cBatch"]),
                                                                   new SqlParameter("@fACost", (DbHelper.GetDbdecimal(dt2.Rows[i]["iPrice"])*1.1m)/DbHelper.GetDbdecimal(dt2.Rows[i]["iQuantity"])),
                                                                    new SqlParameter("@iNQuantity", dt2.Rows[i]["iNQuantity"]),
                                                                          new SqlParameter("@cPOID", DBNull.Value),
                                                                 new SqlParameter("@cDefine26",dt2.Rows[i]["cDefine26"]),
                                                                 new SqlParameter("@cDefine30",dt2.Rows[i]["cDefine30"]),
                                                                 new SqlParameter("@cDefine31",dt2.Rows[i]["cDefine31"]),
                                                                 new SqlParameter("@cdefine32",dt2.Rows[i]["cdefine32"]),
                                                                 new SqlParameter("@cdefine33",dt2.Rows[i]["cdefine33"]),
                                                                 new SqlParameter("@cdefine34",dt2.Rows[i]["cdefine34"]),
                                                                 new SqlParameter("@cfree1",dt2.Rows[i]["cfree1"]),
                                                                 new SqlParameter("@cfree2",dt2.Rows[i]["cfree2"]),
                                                                 new SqlParameter("@cfree3",dt2.Rows[i]["cfree3"]),
                                                                 new SqlParameter("@inum",dt2.Rows[i]["inum"]),
                                                                 new SqlParameter("@iinvexchrate",dt2.Rows[i]["iinvexchrate"]),
                                                                 new SqlParameter("@cPosition",DbHelper.ToDbValue(cPoscode)),
                                                                 new SqlParameter("@cAssUnit",dt2.Rows[i]["cAssUnit"])
                                        }, CommandType.Text, tran);
                                    }

                    //更新现存量

                    sql1= string.Format(@"
 insert into CurrentStock(cWhCode,cInvCode,cBatch,cFree1,cFree2,cFree3,cFree4,ItemId,iSoType,isodid,iQuantity,bStopFlag,BGSPSTOP)
select distinct c.cWHCode,a.cinvcode,isnull(a.cbatch,'') as cbatch,a.cFree1,a.cFree2,a.cFree3,a.cFree4,b.id,0 as isotype,'' as isodid,0 as iquantity,0 as bstockflag,0 as bgspstock 
from rdrecords a,SCM_Item b,rdrecord c
where  a.ID = c.ID and a.ID ={0} and  a.cinvcode = b.cinvcode and isnull(a.cFree1,'')=isnull(b.cFree1,'') and isnull(a.cFree2,'')=isnull(b.cFree2,'') and isnull(a.cFree3,'')=isnull(b.cFree3,'') and isnull(a.cFree4,'')=isnull(b.cFree4,'')
and not exists(select 1 from CurrentStock z where c.cwhcode = z.cWhCode and a.cinvcode = z.cInvCode and ISNULL(a.cbatch,'') = ISNULL(z.cbatch,'') 
and isnull(a.cFree1,'')=isnull(z.cFree1,'') and isnull(a.cFree2,'')=isnull(z.cFree2,'') and isnull(a.cFree3,'')=isnull(z.cFree3,'') and isnull(a.cFree4,'')=isnull(z.cFree4,''))

update a set a.iQuantity = a.iQuantity + b.iquantity 
from CurrentStock a,(select cwhcode,cinvcode,isnull(cbatch,'') as cbatch,SUM(iQuantity) as iquantity,cFree1,cFree2,cFree3,cFree4 
from rdrecord a,rdrecords b where a.ID = b.ID and a.ID = {0}  group by cwhcode,cinvcode,cFree1,cFree2,cFree3,cFree4 ,ISNULL(cbatch,'')) as b
where a.cWhCode = b.cwhcode and a.cInvCode = b.cinvcode and ISNULL(a.cbatch,'') = ISNULL(b.cbatch,'') 
and isnull(a.cFree1,'')=isnull(b.cFree1,'') and isnull(a.cFree2,'')=isnull(b.cFree2,'') and isnull(a.cFree3,'')=isnull(b.cFree3,'') and isnull(a.cFree4,'')=isnull(b.cFree4,'')
 
", iRdid); ;
DbHelper2.ExecuteSqlWithTrans(sql1,  tran);

                //插入货位
if (!string.IsNullOrEmpty(cPoscode))
{
    sql1 = @"INSERT into invposition([RdsID]
           ,[RdID]
           ,[cWhCode]
           ,[cPosCode]
           ,[cInvCode]
           ,[cBatch]
           ,[cFree1]
           ,[cFree2]
           ,[dVDate]
           ,[iQuantity]
           ,[iNum]
           ,[cMemo]
           ,[cHandler]
           ,[dDate]
           ,[bRdFlag]
           ,[cSource]
           ,[cFree3]
           ,[cFree4]
           ,[cFree5]
           ,[cFree6]
           ,[cFree7]
           ,[cFree8]
           ,[cFree9]
           ,[cFree10]
           ,[cAssUnit]
           ,[cBVencode]
           ,[iTrackId]
           ,[dMadeDate]
           ,[iMassDate]
           ,[cMassUnit]
           ,[cvmivencode]
           ,[iExpiratDateCalcu]
           ,[cExpirationdate]
           ,[dExpirationdate])
        select  autoid,a.id
           ,[cWhCode]
           ,b.cPosition
           ,[cInvCode]
           ,[cBatch]
           ,isnull(cFree1,'')
           ,isnull(cFree2,'')
           ,[dVDate]
           ,[iQuantity]
           ,[iNum]
           ,[cMemo]
           ,[cHandler]
           ,[dDate]
           ,[bRdFlag]
           ,''
           ,isnull(cFree3,'')
           ,isnull(cFree4,'')
           ,isnull(cFree5,'')
           ,isnull(cFree6,'')
           ,isnull(cFree7,'')
           ,isnull(cFree8,'')
           ,isnull(cFree9,'')
           ,isnull(cFree10,'')
           ,[cAssUnit]
           ,[cBVencode]
           ,b.iOriTrackID
           ,[dMadeDate]
           ,[iMassDate]
           ,[cMassUnit]
           ,''
           ,[iExpiratDateCalcu]
           ,[cExpirationdate]
           ,[dExpirationdate]
           FROM rdrecord a, rdrecords b WHERE a.id = b.id  and isnull(b.cPosition,'')<>''  and a.id = @id
          and AutoID not in (select rdsid from invposition)  ";


    DbHelper2.ExecuteSqlWithTrans(sql1, new SqlParameter[]{ 
                                         new SqlParameter("@id",iRdid)
                    }, CommandType.Text, tran);
}
           

                DbHelper2.CommitTransAndCloseConnection(tran);

                sql1 = string.Format(@"update rdrecord set cdefine11 ='{0}' where id='{1}' ", "RK" + DbHelper.GetDbString(dt1.Rows[0]["CCODE"]), dt1.Rows[0]["id"]);
                DbHelper.ExecuteNonQuery(sql1);
                return ("生成成功！");
            }
            catch (Exception exception)
            {
                DbHelper2.RollbackAndCloseConnection(tran);
                return ("生成失败：" + exception.Message);
            }

            //return "";
        }
        /// <summary>
        /// 表头
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable1(string Constr)
        {

            string sql = "select * from rdrecord01 where 1=2";

            return DbHelper2.Execute(sql, Constr).Tables[0];
        }

        /// <summary>
        /// 表体
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable2(string Constr)
        {
            string sql = "select * from rdrecords01 where 1=2";

            return DbHelper2.Execute(sql, Constr).Tables[0];
        }


    }
}
