using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HONDA.Models;
using HONDA.Database_Access_Layer;
using System.Web.Script.Serialization;

namespace HONDA.Database_Access_Layer
{
    public class AdminDB
    {
        #region General Information         
        private SqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        db dblayer = new db();
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
        }
        #endregion
        #region Methods
        public string CreateMasterSetting(DataTable objMasterSettingModel)
        {            
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Prc_MST_MASTER_SETTING_INSERT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MST_MASTER_SETTING_TYPE", objMasterSettingModel);                    
                    SqlParameter oblogin = new SqlParameter();
                    oblogin.ParameterName = "@message";
                    oblogin.SqlDbType = SqlDbType.NVarChar;
                    oblogin.Size = 50;
                    oblogin.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(oblogin);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(oblogin.Value);
                    con.Close();
                    return res;
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }        
        public DataTable GetMasterSetting_History(int type)
        {
            try
            {                                   
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_MASTERSETTING_HISTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);                                                       
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public CommonSettingModel GetMasterSettingDetails(int id)
        {
            CommonSettingModel mastersetting = new CommonSettingModel();
            try
            {
                connection();                
                using (SqlCommand cmd = new SqlCommand("PRC_GETDETAILS_MASTERSETTING", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {                        
                        mastersetting.F_DESCRIPTION = dr["F_DESCRIPTION"].ToString();
                        mastersetting.PROJECT_FAMILY = dr["PROJECT_FAMILY"].ToString();
                        mastersetting.PROJECT_FLOW = dr["PROJECT_FLOW"].ToString();
                        mastersetting.WORK_CATEGORY = dr["WORK_CATEGORY"].ToString();
                        mastersetting.STATUS = dr["STATUS"].ToString();                       
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return mastersetting;
        }
        public string Update_MasterSetting(int ID, string F_DESCRIPTION, string WORK_CATEGORY, string PROJECT_FAMILY, string PROJECT_FLOW, string STATUS)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_MasterSettingUpdate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@F_DESCRIPTION", F_DESCRIPTION);
                    cmd.Parameters.AddWithValue("@WORK_CATEGORY", WORK_CATEGORY);
                    cmd.Parameters.AddWithValue("@PROJECT_FAMILY", PROJECT_FAMILY);
                    cmd.Parameters.AddWithValue("@PROJECT_FLOW", PROJECT_FLOW);
                    cmd.Parameters.AddWithValue("@STATUS", STATUS);
                    SqlParameter objResignation = new SqlParameter();
                    objResignation.ParameterName = "@message";
                    objResignation.SqlDbType = SqlDbType.NVarChar;
                    objResignation.Size = 50;
                    objResignation.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(objResignation);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    res = Convert.ToString(objResignation.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;                
            }
            return res;
        }
        

        

        #endregion
    }
}