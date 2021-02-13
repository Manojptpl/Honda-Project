using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HONDA.Models
{
    public class TranExitModel
    {
        public int EMP_ID { get; set; }
        public DateTime SEPERATION_DATE { get; set; }
        public string P_REASON { get; set; }
        public string JOB_EXPACTATIONS { get; set; }        
    }
    public class TranExitFormModel
    {
        public int emp_id { get; set; }
        public string first_name { get; set; }
        public string job_name { get; set; }
        public string designation { get; set; }
        public string department_name { get; set; }
        public string strDOJ { set; get; }
    }
    public class TranExitDissatisfactionModel
    {
        public int EXIT_ID { get; set; }
        public int WORK_TYPE { get; set; }
        public int WORK_COND { get; set; }
        public int PAY { get; set; }
        public int SUPERVISOR { get; set; }
        public int LOCATION { get; set; }
        public int COST_LIVING { get; set; }
        public int COMMUTE { get; set; }        
    }
    public class TranExitAnotherJobModel
    {
        public int EXIT_ID { get; set; }
        public string NEW_EMPLOYER { get; set; }
        public string LOCATION { get; set; }
        public string POSITION { get; set; }
        public string WORK_NATURE { get; set; }
        public int SALARY_OF_POSITION { get; set; }
        public int ORGANIZATION_OFFER { get; set; }        
    }
    public class TRANEXITFORMJOBASPECTSModel
    {
        public int EXIT_ID { get; set; }
        public int TYPE_OF_WORK { get; set; }
        public int FAIRNESS_OF_WORKLOAD { get; set; }
        public int SALARY { get; set; }
        public int WORKING_CONDTIONS { get; set; }
        public int TOOLS_PROVIDED { get; set; }
        public int TRAINING_RECEIVED { get; set; }
        public int CO_WORKKERS { get; set; }
        public int SUPERVISION_RECEIVED { get; set; }
        public int LEVEL_OF_iNPUT { get; set; }        			
    }

    public class TRANEXITFORMORGANIZATIONASPECTSModel
    {
        public int EXIT_ID { get; set; }
        public int RECRUITMENT_PROCESS { get; set; }
        public int EMPLOYEE_ORIENTATION { get; set; }
        public int TRAINING_OPPORTUNITIES { get; set; }
        public int CAREER_DEVELOPMENT { get; set; }
        public int EMPLOYEE_MORALE { get; set; }
        public int FAIR_TREAT_EMPLOYEE { get; set; }
        public int RECOGNITION_FOR_JOB { get; set; }
        public int SUPPORT_FOR_WORK { get; set; }
        public int CORPRATION { get; set; }
        public int COMMUNICATION { get; set; }
        public int PERFORMANCE { get; set; }
        public int INTEREST { get; set; }
        public int COMMITMENT { get; set; }
        public int CONCERN { get; set; }
        public int ADMINISTRATIVE { get; set; }        
	
    }
    public class TRANEXITFORMSUPERVISORModel
    {
        public int EXIT_ID { get; set; }
        public int PERFORMANCE_FEEDBACK { get; set; }
        public int RECOGNISED_ACCOMP { get; set; }
        public int COMMUNICATED_EXPECT { get; set; }
        public int TREATED { get; set; }
        public int TRAINED { get; set; }
        public int LEADERSHIP { get; set; }
        public int ENCOURAGED { get; set; }
        public int CONCERNS { get; set; }
        public int SUGGESTIONS { get; set; }
        public int KEPT_INFORMED { get; set; }
        public int WORK_LIFE_BALANCE { get; set; }
        public int ASSIGNMENT { get; set; }       
    }
    public class TranExit
    {
        public List<TranExitModel>  tran_exitModel { get; set; }
        public List<TranExitDissatisfactionModel>  tran_exitDissatisfactionModel { get; set; }
        public List<TranExitAnotherJobModel>  tran_exitAnotherJobModel { get; set; }
        public List<TRANEXITFORMJOBASPECTSModel> tran_exitJOB_ASPECTSModel { get; set; }
        public List<TRANEXITFORMORGANIZATIONASPECTSModel> tran_exitORGANIZATION_ASPECTSModel { get; set; }
        public List<TRANEXITFORMSUPERVISORModel> tran_exitSUPERVISORModel { get; set; }
    }
}