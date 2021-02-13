using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HONDA.Models
{
    public class roleModel
    {
        public int ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public DateTime EFFECTIVE_START_DATE { get; set; }
        public DateTime EFFECTIVE_END_DATE { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime LAST_UPDATE_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public string LAST_UPDATE_LOGIN { get; set; }
        public int ROLEACTIVEINDEX { get; set; }
        public int status { set; get; }
        public string SuccessMsg { set; get; }
        public string ErrorMsg { set; get; }
    }

    public class moduleModel
    {
        public int MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public string STATUS { get; set; }
        public DateTime EFFECTIVE_START_DATE { get; set; }
        public DateTime EFFECTIVE_END_DATE { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime LAST_UPDATE_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public string LAST_UPDATE_LOGIN { get; set; }
        public int ROLEACTIVEINDEX { get; set; }
    }

    public class pagesPermissionModel
    {
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }
        public int PAGE_ID { get; set; }
        public int PERMISSION_ID { get; set; }
        public int MODULE_ID { get; set; }
        public string STATUS { get; set; }
        public string PAGE_NAME { get; set; }
        public string PERMISSION_NAME { get; set; }
    }

    public class employeeModel
    {
        public Int64 EMPLOYEE_ID { get; set; }
        public int USER_ID { get; set; }
        public string ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string EMP_NAME { get; set; }
        public string EMPLOYEE_CODE { get; set; }        
        public string EMPLOYEE_EMAIL_ADDRESS { get; set; }
        public string NATIONALITY { get; set; }
        public string ORIGINAL_DATE_OF_HIRE { get; set; }
        public String CREATION_DATE { get; set; }

        public string USER_PASSWORD { get; set; }
        public string STORE_ID { get; set; }
        public string STORE_NAME { get; set; }
        public string USER_CODE { get; set; }
        public string STATUS { get; set; }
        public string IS_NAVONE_USER { get; set; }
        
    }

    public class UserPermissions
    {
        public DataTable DATATABLE_VALUE { get; set; }
        public int NO_FIXED_COLUMN { get; set; }
        public int ROLE_ID { get; set; }
        public int EMPLOYEE_ID { get; set; }
    }

    public class SystemUserPartial
    {
        public DataTable DATATABLE_VALUE { get; set; }
    }

    public class RolePermissionsStatus
    {
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }        
        public String CHECKED_STATUS { get; set; }
    }

    public class UserPermissionsStatus
    {
        public Int64 EMPLOYEE_ID { get; set; }
        public int ROLE_PERMISSION_ID { get; set; }
        public int ROLE_ID { get; set; }        
        public String CHECKED_STATUS { get; set; }
    }




}