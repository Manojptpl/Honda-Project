using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HONDA.Models
{
    public class DALWorkStructure
    {

        DataTable dt = new DataTable();
        private SqlConnection con;
        private string connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["conn"].ToString();
            con = new SqlConnection(constr);
            return constr;

        }
        public List<SelectListItem> GetCompany()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetCompanyList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["BU_FULL_NAME"].ToString(),
                            Value = dr["BU_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return countryli;
        }

        public List<SelectListItem> GetCountry()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection())){
                using (SqlCommand cmd = new SqlCommand("Prc_GetCountryList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["COUNTRY_NAME"].ToString(),
                            Value = dr["COUNTRY_ID"].ToString()
                        });
                                             
                    }
                    con.Close();
                }
            }
            return countryli;
        }

        public List<SelectListItem> GetZone()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetRegionList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["REGION_NAME"].ToString(),
                            Value = dr["REGION_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return countryli;
        }

        public List<SelectListItem> GetState()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetStateList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["DZONGKHAG_NAME"].ToString(),
                            Value = dr["DZONGKHAG_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return countryli;
        }

        public List<SelectListItem> GetStore()
        {
            List<SelectListItem> countryli = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetStoreList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        countryli.Add(new SelectListItem
                        {
                            Text = dr["STORE_NAME"].ToString(),
                            Value = dr["STORE_ID"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return countryli;
        }
        public string GetMapedStoreName(string id)
        {
            string val = "";
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_GetMapedRegion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                               val = dr["REGION_ID"].ToString(); 
                        }
                        con.Close();
                    }
                }
                return val;
            
          
           
        }

        //Business Unit
        public string AddUpdateBusinessUnit(BusinessUnit BU, string Createdby)
        {
            string res = "";
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_BusinessUnit", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Buid", BU.ID);
                        cmd.Parameters.AddWithValue("@companyname", BU.CompanyName);
                        cmd.Parameters.AddWithValue("@compshortname", BU.CompanyShortName);
                        cmd.Parameters.AddWithValue("@incorporationdate", BU.IncorporationDate);
                        cmd.Parameters.AddWithValue("@pannumber", BU.PANNumber);
                        cmd.Parameters.AddWithValue("@tannumber", BU.TANNumber);
                        cmd.Parameters.AddWithValue("@address", BU.Address);
                        cmd.Parameters.AddWithValue("@countryid", BU.CountryID);
                        cmd.Parameters.AddWithValue("@zoneid", BU.ZoneID);
                        cmd.Parameters.AddWithValue("@stateid", BU.State_ProvinceID);
                        cmd.Parameters.AddWithValue("@cityid", BU.CityID);
                        cmd.Parameters.AddWithValue("@postalcode", BU.PostalCode);
                        cmd.Parameters.AddWithValue("@companyemail", BU.CompanyEmail);
                        cmd.Parameters.AddWithValue("@phonenumber", BU.PhoneNumber);
                        cmd.Parameters.AddWithValue("@contactperson", BU.ContactPerson);
                        cmd.Parameters.AddWithValue("@contactemail", BU.ContactEmail);
                        cmd.Parameters.AddWithValue("@mobilenumber", BU.MobileNumber);
                        cmd.Parameters.AddWithValue("@website", BU.Website);
                        cmd.Parameters.AddWithValue("@financialyearfrom", BU.FinancialYearFrom);
                        cmd.Parameters.AddWithValue("@financialyearto", BU.FinancialYearTo);
                        cmd.Parameters.AddWithValue("@calendaryearfrom", BU.CalendarYearFrom);
                        cmd.Parameters.AddWithValue("@calendaryearto", BU.CalendarYearTo);
                        cmd.Parameters.AddWithValue("@status", "");
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
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
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        public BusinessUnit GetBusinessUnit()
        {
            BusinessUnit businessunit = new BusinessUnit();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetBusinessUnit1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        businessunit.ID = Convert.ToInt32(dr["BU_ID"]);
                        businessunit.CompanyName = dr["BU_FULL_NAME"].ToString();
                        businessunit.CompanyShortName = dr["BU_SHORT_NAME"].ToString();
                        businessunit.IncorporationDate = dr["INCORPORATION_DATE"].ToString();
                        businessunit.PANNumber = dr["PAN_NO"].ToString();
                        businessunit.TANNumber = dr["TAN_NO"].ToString();
                        businessunit.Address = dr["BU_ADDRESS"].ToString();
                        businessunit.Countryname = dr["COUNTRY_ID"].ToString();
                        businessunit.ResionName = dr["REGION_ID"].ToString();
                        businessunit.Dzongkhagname = dr["DZONGKHAG_ID"].ToString();
                        businessunit.Locationname = dr["LOCATION_ID"].ToString();
                        businessunit.PostalCode = dr["POSTCODE"].ToString();
                        businessunit.CompanyEmail = dr["BU_EMAIL"].ToString();
                        businessunit.PhoneNumber = dr["PHONE_NUMBER"].ToString();
                        businessunit.ContactPerson = dr["CONTACT_PERSON"].ToString();
                        businessunit.ContactEmail = dr["CONTACT_EMAIL"].ToString();
                        businessunit.MobileNumber= dr["MOBILE_NUMBER"].ToString();
                        businessunit.FinancialYearFrom = dr["FINANCIAL_YEAR_FROM"].ToString();
                        businessunit.FinancialYearTo = dr["FINANCIAL_YEAR_TO"].ToString();
                        businessunit.CalendarYearFrom = dr["CALENDAR_YEAR_FROM"].ToString();
                        businessunit.CalendarYearTo = dr["CALENDAR_YEAR_TO"].ToString();
                        businessunit.Website = dr["WEBSITE"].ToString();
                    }
                    con.Close();
                }
            }
            return businessunit;
        }

        //Geography
        public DataSet GetGeography()
        {
            DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_GeographyList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                        
                        con.Open();
                        using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                        {
                            adp.Fill(ds);
                        }                        
                        con.Close();
                    }
                }
                return ds;
         }
       
        //Store Location Mapping
        public string AddStoreLocationMapping(StoreLocationMapping slmapping, string Createdby)
        {
            string res = "";
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_StoreLocationMapping", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@slmappingid", 0);
                        cmd.Parameters.AddWithValue("@storeid", slmapping.storeid);
                        cmd.Parameters.AddWithValue("@locationid", slmapping.locationid);
                        cmd.Parameters.AddWithValue("@bunameid", slmapping.bunameid);
                        cmd.Parameters.AddWithValue("@storelatitude", slmapping.storelatitude);
                        cmd.Parameters.AddWithValue("@storelongitude", slmapping.storelongitude);
                        cmd.Parameters.AddWithValue("@radius", slmapping.radius);
                        cmd.Parameters.AddWithValue("@status", "");                        
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
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
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        public string UpdateStoreLocationMapping(StoreLocationMapping slmapping, string Createdby)
        {
            string res = "";
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_StoreLocationMapping", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@slmappingid", slmapping.ID);
                        cmd.Parameters.AddWithValue("@storeid", slmapping.storeid);
                        cmd.Parameters.AddWithValue("@locationid", slmapping.locationid);
                        cmd.Parameters.AddWithValue("@bunameid", slmapping.bunameid);
                        cmd.Parameters.AddWithValue("@storelatitude", slmapping.storelatitude);
                        cmd.Parameters.AddWithValue("@storelongitude", slmapping.storelongitude);
                        cmd.Parameters.AddWithValue("@radius", slmapping.radius);
                        cmd.Parameters.AddWithValue("@status", "");
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
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
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        public List<StoreLocationMapping> EditStoreLocationMapping(int slmappingid)
        {

            List<StoreLocationMapping> StoreLocationMappingli = new List<StoreLocationMapping>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_EditStoreLocationMapping", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@slmappingid", slmappingid);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        StoreLocationMapping storelocationmapping = new StoreLocationMapping();
                        storelocationmapping.ID = Convert.ToInt32(dr["STORE_LOCATION_ID"].ToString());
                        storelocationmapping.storeid = Convert.ToInt32(dr["STORE_ID"].ToString());
                        storelocationmapping.locationid = Convert.ToInt32(dr["LOCATION_ID"].ToString());
                        storelocationmapping.bunameid = Convert.ToInt32(dr["BU_ID"].ToString());
                        storelocationmapping.radius = Convert.ToDouble(dr["RADIUS"].ToString());
                        storelocationmapping.storelatitude = Convert.ToDouble(dr["LATITUDE"].ToString());
                        storelocationmapping.storelongitude = Convert.ToDouble(dr["LONGITUDE"].ToString());
                        storelocationmapping.status = dr["STATUS"].ToString();
                        StoreLocationMappingli.Add(storelocationmapping);
                    }

                    con.Close();
                }
            }
            return StoreLocationMappingli;
        }       

         public List<StoreLocationMapping> GetStoreLocationMapping()
        {
            List<StoreLocationMapping> StoreLocationMappingli = new List<StoreLocationMapping>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_StoreLocationMappingList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        StoreLocationMapping storelocationmapping = new StoreLocationMapping();
                        storelocationmapping.ID = Convert.ToInt32(dr["STORE_LOCATION_ID"].ToString());
                        storelocationmapping.storename = dr["storename"].ToString();
                        storelocationmapping.StoreLocation =dr["StoreLocation"].ToString();
                        storelocationmapping.BUName = dr["BUName"].ToString();
                        storelocationmapping.radius = Convert.ToDouble(dr["RADIUS"].ToString());
                        storelocationmapping.storelatitude = Convert.ToDouble(dr["LATITUDE"].ToString());
                        storelocationmapping.storelongitude = Convert.ToDouble(dr["LONGITUDE"].ToString());
                        storelocationmapping.status = dr["STATUS"].ToString();
                        StoreLocationMappingli.Add(storelocationmapping);
                    }

                    con.Close();
                }
            }
            return StoreLocationMappingli;



        }

        //Holiday
        public string AddHoliday(Holiday holiday, string Createdby)
        {
            string res = "";
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_Holiday", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@holidayid", 0);
                        cmd.Parameters.AddWithValue("@holidayname", holiday.holiday_name);
                        cmd.Parameters.AddWithValue("@holidaytype", holiday.holiday_type);
                        cmd.Parameters.AddWithValue("@holidayregion", holiday.holiday_region == null ? "" : holiday.holiday_region);
                        cmd.Parameters.AddWithValue("@holidaysdate", holiday.holiday_sdate);
                        cmd.Parameters.AddWithValue("@holidayedate", holiday.holiday_edate);
                        cmd.Parameters.AddWithValue("@description", holiday.holiday_desc==null?"": holiday.holiday_desc);
                        cmd.Parameters.AddWithValue("@year", holiday.holiday_year);
                        cmd.Parameters.AddWithValue("@starthalfday", holiday.startdatehalfday==null?"": holiday.startdatehalfday);
                        cmd.Parameters.AddWithValue("@endhalfday", holiday.enddatehalfday==null?"": holiday.enddatehalfday);
                        cmd.Parameters.AddWithValue("@noofdays", holiday.noofdays);
                        
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
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
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }

        public string UpdateHoliday(Holiday holiday, string Createdby)
        {
            string res = "";
            try
            {

                using (SqlConnection con = new SqlConnection(connection()))
                {
                    using (SqlCommand cmd = new SqlCommand("Prc_Holiday", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@holidayid", holiday.ID);
                        cmd.Parameters.AddWithValue("@holidayname", holiday.holiday_name);
                        cmd.Parameters.AddWithValue("@holidaytype", holiday.holiday_type);
                        cmd.Parameters.AddWithValue("@holidayregion", holiday.holiday_region == null ? "" : holiday.holiday_region);
                        cmd.Parameters.AddWithValue("@holidaysdate", holiday.holiday_sdate);
                        cmd.Parameters.AddWithValue("@holidayedate", holiday.holiday_edate);
                        cmd.Parameters.AddWithValue("@description", holiday.holiday_desc);
                        cmd.Parameters.AddWithValue("@year", holiday.holiday_year);
                        cmd.Parameters.AddWithValue("@starthalfday", holiday.startdatehalfday == null ? "" : holiday.startdatehalfday);
                        cmd.Parameters.AddWithValue("@endhalfday", holiday.enddatehalfday == null ? "" : holiday.enddatehalfday);
                        cmd.Parameters.AddWithValue("@noofdays", holiday.noofdays);
                        cmd.Parameters.AddWithValue("@createdby", Createdby);
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
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }

        }
        public List<Holiday> GetHoliday()
        {

            List<Holiday> holidayli = new List<Holiday>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetHolidayList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Holiday holiday = new Holiday();
                        holiday.holiday_name = dr["HOLIDAY_NAME"].ToString();
                        holiday.holiday_region_name2 = dr["top2Region"].ToString();
                        holiday.holiday_typename = dr["HOLIDAY_TYPE_NAME"].ToString();
                        holiday.holiday_desc = dr["DESCRIPTION"].ToString();
                        holiday.holiday_day = dr["HOLIDAY_DAY"].ToString();
                        holiday.status = dr["STATUS"].ToString();
                        holiday.holiday_year = dr["YEAR"].ToString();
                        holiday.ID = Convert.ToInt32(dr["HOLIDAY_ID"].ToString());
                        holiday.holiday_sdate = dr["HOLIDAY_SDATE"].ToString();
                        holiday.holiday_edate = dr["HOLIDAY_EDATE"].ToString();
                        holiday.startdatehalfday = dr["STARTHALFDAY"].ToString();
                        holiday.enddatehalfday = dr["ENDHALFDAY"].ToString();
                        holiday.noofdays = Convert.ToDouble(dr["NO_OF_DAYS"].ToString());
                        holiday.holiday_region_name = dr["Regionlist"].ToString();
                        if (DBNull.Value.Equals(dr["EFFECTIVE_START_DATE"]) || DBNull.Value.Equals(dr["EFFECTIVE_END_DATE"]))
                        {
                            holiday.currentdate = "";
                        }
                        else if (Convert.ToDateTime(dr["EFFECTIVE_START_DATE"]) < DateTime.Now)
                        {
                            holiday.currentdate = "";
                        }
                        else
                        {
                            holiday.currentdate = "Edit";
                        }

                        holidayli.Add(holiday);
                    }

                    con.Close();
                }
            }
            return holidayli;



        }
        public List<Holiday> GetHoliday(string year)
        {

            List<Holiday> holidayli = new List<Holiday>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetHolidayList_HR", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@year", year);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Holiday holiday = new Holiday();
                        holiday.holiday_name = dr["HOLIDAY_NAME"].ToString();
                        holiday.holiday_region_name2 = dr["top2Region"].ToString();
                        holiday.holiday_typename = dr["HOLIDAY_TYPE_NAME"].ToString();
                        holiday.holiday_desc = dr["DESCRIPTION"].ToString();
                        holiday.holiday_day = dr["HOLIDAY_DAY"].ToString();
                        holiday.status = dr["STATUS"].ToString();
                        holiday.holiday_year = dr["YEAR"].ToString();
                        holiday.ID = Convert.ToInt32(dr["HOLIDAY_ID"].ToString());
                        holiday.holiday_sdate = dr["HOLIDAY_SDATE"].ToString();
                        holiday.holiday_edate = dr["HOLIDAY_EDATE"].ToString();
                        holiday.startdatehalfday = dr["STARTHALFDAY"].ToString();
                        holiday.enddatehalfday = dr["ENDHALFDAY"].ToString();
                        holiday.noofdays = Convert.ToDouble(dr["NO_OF_DAYS"].ToString());
                        holiday.holiday_region_name = dr["Regionlist"].ToString();
                        if (DBNull.Value.Equals(dr["EFFECTIVE_START_DATE"]) || DBNull.Value.Equals(dr["EFFECTIVE_END_DATE"]))
                        {
                            holiday.currentdate = "";
                        }
                        else if (Convert.ToDateTime(dr["EFFECTIVE_START_DATE"]) < DateTime.Now)
                        {
                            holiday.currentdate = "";
                        }
                        else
                        {
                            holiday.currentdate = "Edit";
                        }

                        holidayli.Add(holiday);
                    }

                    con.Close();
                }
            }
            return holidayli;



        }

        public List<Holiday> Emp_Get_Holiday(string emp_id,int year)
        {

            List<Holiday> holidayli = new List<Holiday>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetHolidayList_EmployeeWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Employee_ID", emp_id);
                    cmd.Parameters.AddWithValue("@year", year);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Holiday holiday = new Holiday();
                        holiday.holiday_name = dr["HOLIDAY_NAME"].ToString();
                        holiday.holiday_region_name2 = dr["top2Region"].ToString();
                        holiday.holiday_typename = dr["HOLIDAY_TYPE_NAME"].ToString();
                        holiday.holiday_desc = dr["DESCRIPTION"].ToString();
                        holiday.holiday_day = dr["HOLIDAY_DAY"].ToString(); 
                        holiday.status = dr["STATUS"].ToString();
                        holiday.holiday_year = dr["YEAR"].ToString();
                        holiday.ID = Convert.ToInt32(dr["HOLIDAY_ID"].ToString());
                        holiday.holiday_sdate = dr["HOLIDAY_SDATE"].ToString();
                        holiday.holiday_edate = dr["HOLIDAY_EDATE"].ToString();
                        holiday.startdatehalfday = dr["STARTHALFDAY"].ToString();
                        holiday.enddatehalfday = dr["ENDHALFDAY"].ToString();
                        holiday.noofdays =Convert.ToDouble( dr["NO_OF_DAYS"].ToString());
                        holiday.holiday_region_name = dr["Regionlist"].ToString();
                        if (DBNull.Value.Equals(dr["EFFECTIVE_START_DATE"]) || DBNull.Value.Equals(dr["EFFECTIVE_END_DATE"]))
                        {
                            holiday.currentdate = "";
                        }
                        else  if (Convert.ToDateTime(dr["EFFECTIVE_START_DATE"]) < DateTime.Now)
                        {
                            holiday.currentdate = "";
                        }
                        else
                        {
                            holiday.currentdate = "Edit";
                        }

                        holidayli.Add(holiday);
                    }

                    con.Close();
                }
            }
            return holidayli;



        }

        public List<Holiday> EditHoliday(int holidayid)
        {

            List<Holiday> holidayli = new List<Holiday>();

            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_EditHolidayList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@holidayid", holidayid);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Holiday holiday = new Holiday();
                        holiday.holiday_name = dr["HOLIDAY_NAME"].ToString();
                        holiday.holiday_type = Convert.ToInt16(dr["HOLIDAY_TYPE_ID"].ToString());
                        holiday.holiday_date = dr["HOLIDAY_DATE"].ToString();
                        holiday.holiday_sdate = dr["HOLIDAY_SDATE"].ToString();
                        holiday.holiday_edate = dr["HOLIDAY_EDATE"].ToString();

                        holiday.noofdays = Convert.ToDouble(dr["NO_OF_DAYS"].ToString());
                        holiday.holiday_desc = dr["DESCRIPTION"].ToString();
                        holiday.holiday_day = dr["HOLIDAY_DAY"].ToString();
                        holiday.status = dr["STATUS"].ToString();
                        holiday.holiday_year = dr["YEAR"].ToString();
                        holiday.startdatehalfday = dr["STARTHALFDAY"].ToString();
                        holiday.enddatehalfday = dr["ENDHALFDAY"].ToString();
                        holiday.ID = Convert.ToInt32(dr["HOLIDAY_ID"].ToString());
                        if (DBNull.Value.Equals(dr["EFFECTIVE_START_DATE"]) || DBNull.Value.Equals(dr["EFFECTIVE_END_DATE"]))
                        {
                            holiday.currentdate = "";
                        }
                        else if (Convert.ToDateTime(dr["EFFECTIVE_START_DATE"]) < DateTime.Now)
                        {
                            holiday.currentdate = "";
                        }
                        else
                        {
                            holiday.currentdate = "Edit";
                        }

                        holidayli.Add(holiday);
                    }

                    con.Close();
                }
            }
            return holidayli;



        }
        public List<Region> EditHolidayregion(int holidayid)
        {
            DataSet ds = new DataSet();
       
            List<Region> regionli = new List<Region>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_EditHolidayList_Region", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@holidayid", holidayid);
                    con.Open();
                 
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Region region = new Region();
                        region.HolidayID = dr["holidayid"].ToString();
                        region.RegionID =  dr["REGIONID"].ToString();
                        regionli.Add(region);
                    }

                    con.Close();
                }
            }        
            return regionli;



        }
        //Department
        public DataSet GetDepartment()
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_BindList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataTable GetDepartmentById(int dept_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_id", dept_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public string update_department(int department_id, int head_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Udepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@department_id", department_id);
                cmd.Parameters.AddWithValue("@head_id", head_id);
                SqlParameter objdept = new SqlParameter();
                objdept.ParameterName = "@message";
                objdept.SqlDbType = SqlDbType.NVarChar;
                objdept.Size = 50;
                objdept.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(objdept);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(objdept.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        //Designation
        public List<Designation> GetDesignation()
        {
            List<Designation> designationli = new List<Designation>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetDesignation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Designation designation = new Designation();
                        designation.designationname = dr["DESIGNATION_NAME"].ToString();
                        designation.designationcode = dr["DESIGNATION_CODE"].ToString();
                        designation.status = dr["STATUS"].ToString();
                        designationli.Add(designation);
                    }
                    con.Close();
                }
            }
            return designationli;
        }

        //Grade
        public List<Grade> GetGrade()
        {
            List<Grade> gradeli = new List<Grade>();
            using (SqlConnection con = new SqlConnection(connection()))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_GetGrade", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Grade grade = new Grade();
                        grade.gradename = dr["GRADE_NAME"].ToString();
                        grade.gradecode = dr["GRADE_CODE"].ToString();
                        grade.status = dr["STATUS"].ToString();
                        gradeli.Add(grade);
                    }
                    con.Close();
                }
            }
            return gradeli;
        }

        public DataTable GetSection(int section_id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_GetSection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@section_id", section_id);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public string update_section(int section_id, int head_id)
        {
            string res = "";
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("Prc_Usection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@section_id", section_id);
                cmd.Parameters.AddWithValue("@head_id", head_id);
                SqlParameter objdept = new SqlParameter();
                objdept.ParameterName = "@message";
                objdept.SqlDbType = SqlDbType.NVarChar;
                objdept.Size = 50;
                objdept.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(objdept);
                con.Open();
                cmd.ExecuteNonQuery();
                res = Convert.ToString(objdept.Value);
                con.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
    }
}