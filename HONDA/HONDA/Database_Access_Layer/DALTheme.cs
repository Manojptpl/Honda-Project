using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class DALTheme
    {
        private SqlConnection objCon;
        private string connection()
        {
            string conStr = null;
            conStr = Convert.ToString(ConfigurationManager.ConnectionStrings["conn"]);
            objCon = new SqlConnection(conStr);
            return conStr;
        }
        public string ProcessDataInsertUpdate(ThemeModel objTheme)
        {
            string res = "";
            try
            {
                SqlConnection objCon = new SqlConnection(connection());
                using (SqlCommand objCmd = new SqlCommand("PRC_MSTTHEMESETTINGS_INSERTUPDATE", objCon))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@pTHEME_ID", objTheme.THEME_ID);
                    objCmd.Parameters.AddWithValue("@pCOMPANY_NAME", objTheme.COMPANY_NAME);
                    objCmd.Parameters.AddWithValue("@pLOGO", objTheme.LOGO);
                    objCmd.Parameters.AddWithValue("@pFAVICON", objTheme.FAVICON);
                    SqlParameter attobj = new SqlParameter();
                    attobj.ParameterName = "@pmessage";
                    attobj.SqlDbType = SqlDbType.NVarChar;
                    attobj.Size = 100;
                    attobj.Direction = ParameterDirection.Output;
                    objCmd.Parameters.Add(attobj);
                    objCon.Open();
                    //iResult = Convert.ToInt32(objCmd.ExecuteNonQuery());
                    objCmd.ExecuteNonQuery();
                    res = Convert.ToString(attobj.Value);
                    objCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public ThemeModel GetThemeSettings()
        {
            ThemeModel objTheme = new ThemeModel();
            try
            {
                using (SqlConnection objCon = new SqlConnection(connection()))
                {
                    using (SqlCommand objCmd = new SqlCommand("PRC_GETMST_THEME_SETTINGS", objCon))
                    {
                        objCmd.CommandType = CommandType.StoredProcedure;
                        objCon.Open();
                        SqlDataReader objDr = objCmd.ExecuteReader();
                        if (objDr.Read())
                        {
                            objTheme.THEME_ID = Convert.ToInt32(objDr["THEME_ID"].ToString());
                            objTheme.COMPANY_NAME = objDr["COMPANY_NAME"].ToString();
                            objTheme.LOGO = objDr["LOGO"].ToString();
                            objTheme.FAVICON = objDr["FAVICON"].ToString();
                        }
                        objCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objTheme;
        }
    }
}