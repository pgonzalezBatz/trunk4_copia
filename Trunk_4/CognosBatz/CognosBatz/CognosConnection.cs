using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Services.Protocols;
using cognosdotnet_2_0;

namespace CognosBatz
{

    public class CognosConnection
    {
        // sn_dg_prm_smpl_connect_start_0
        private agentService cBIAS = null;
        // sn_dg_prm_smpl_connect_end_0
        private batchReportService1 cBIBRS = null;
        private contentManagerService1 cBICMS = null;
        private dataIntegrationService1 cBIDIS = null;
        private deliveryService1 cBIDS = null;
        private eventManagementService1 cBIEMS = null;
        private jobService1 cBIJS = null;
        private monitorService1 cBIMS = null;
        private reportService1 cBIRS = null;
        private systemService1 cBISS = null;

        private string cBIUrl = "";
        private string errorText = null;
        private bool connectedToServer = false;
        private static string savedUserName = "";
        private static string savedUserPassword = "";
        private static string savedNamespace = "";
        private static string savedAccountSearchPath = "";


        public CognosConnection()
        {
        }

        public void conectar(string Url, string userName, string password, string dominio)
        {

            cBICMS = new contentManagerService1();

            if (cBICMS != null)
            {
                cBICMS.Url = Url;
                cBIBRS = new batchReportService1();
                cBIBRS.Url = Url;
                cBIBRS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                cBIRS = new reportService1();
                cBIRS.Url = Url;                
                cBIRS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                searchPathMultipleObject homeDirSearchPath = new searchPathMultipleObject();
                homeDirSearchPath.Value = "~";
                //baseClass[] bc = cBICMS.query(homeDirSearchPath, new propEnum[] { }, new sort[] { }, new queryOptions());

                if (!specificUserLogon(userName, password, dominio))
                {
                    connectedToServer = false;
                    return;
                }
                else
                {
                    connectedToServer = true;
                    savedUserName = userName;
                    savedUserPassword = password;
                    savedNamespace = dominio;
                    savedAccountSearchPath = getLogonAccount(this).searchPath.value;
                }

            }
        }

        public bool specificUserLogon(string userName, string userPassword, string userNamespace)
        {
            try
            {
                // sn_dg_prm_sdk_method_contentManagerService_logon_start_0
                System.Text.StringBuilder credentialXML = new System.Text.StringBuilder("<credential>");
                credentialXML.AppendFormat("<namespace>{0}</namespace>", userNamespace);
                credentialXML.AppendFormat("<username>{0}</username>", userName);
                credentialXML.AppendFormat("<password>{0}</password>", userPassword);
                credentialXML.Append("</credential>");

                //The csharp toolkit encodes the credentials
                string encodedCredentials = credentialXML.ToString();
                xmlEncodedXML xmlEncodedCredentials = new xmlEncodedXML();
                xmlEncodedCredentials.Value = encodedCredentials;
                searchPathSingleObject[] emptyRoleSearchPathList = new searchPathSingleObject[0];
                cBICMS.logon(xmlEncodedCredentials, null);
                // sn_dg_prm_sdk_method_contentManagerService_logon_end_0

                return true;
            }
            catch (SoapException ex)
            {
                string msm = ex.ToString();
                return false;
                //SamplesException.ShowExceptionMessage(ex, guiMode, "Unable To Logon");
            }
            catch (System.Exception ex)
            {
                string msm = ex.ToString();
                return false;
                //SamplesException.ShowExceptionMessage(ex.Message, guiMode, "Unable To Logon");
            }
        }

        public bool IsConnectedToCBI()
        {
            return connectedToServer;
        }

        public string GetAccountPath()
        {
            return savedAccountSearchPath;
        }

        public string GetErrorText()
        {
            return errorText;
        }

        public string getUserName()
        {
            return savedUserName;
        }

        public string getUserPassword()
        {
            return savedUserPassword;
        }

        public string getNamespace()
        {
            return savedNamespace;
        }

                
        public static account getLogonAccount(CognosConnection connectedAs)
        {
            propEnum[] props =
                new propEnum[] { propEnum.searchPath, propEnum.defaultName, propEnum.policies };
            account myAccount = null;

            if (connectedAs.CBICMS == null)
            {
                Console.WriteLine("Invalid parameter passed to function logon.");
                return myAccount;
            }

            try
            {
                searchPathMultipleObject dummy = new searchPathMultipleObject();
                dummy.Value = "~";
                baseClass[] bc =
                    connectedAs.CBICMS.query(dummy, props, new sort[] { }, new queryOptions());

                if ((bc != null) && (bc.Length == 1))
                {
                    myAccount = (account)bc[0];
                }
            }
            catch (Exception ex)
            {
                //An exception here likely indicates the client is not currently
                //logged in, so the query fails.
                Console.WriteLine("Caught Exception:\n" + ex.Message);
            }
            return myAccount;
        }

        

        //property definition for the server URL 
        public string CBIURL
        {
            get
            {
                return cBIUrl;
            }
        }

        public string credentialString
        {
            get
            {
                string credentialXML = "<credential>";

                //Namespace
                if (savedNamespace != "")
                {
                    credentialXML += "<namespace>";
                    credentialXML += savedNamespace;
                    credentialXML += "</namespace>";
                }

                //Username
                credentialXML += "<username>";
                if (savedUserName == "")
                {
                    credentialXML += "Anonymous";
                }
                else
                {
                    credentialXML += savedUserName;
                }
                credentialXML += "</username>";

                //Password
                if (savedUserPassword != "")
                {
                    credentialXML += "<password>";
                    credentialXML += savedUserPassword;
                    credentialXML += "</password>";
                }
                credentialXML += "</credential>";
                return credentialXML;
            }
        }
        //property definitions to give read access the various service objects
        //
        //header and url information is retrieved from the contentManagerService1 object
        //as this it the service used to connect and logon.
        public agentService CBIAS
        {
            get
            {
                // sn_dg_prm_smpl_connect_start_2
                if (cBIAS == null)
                {
                    cBIAS = new agentService();
                    cBIAS.Url = cBICMS.Url;
                }
                if (cBIAS.biBusHeaderValue == null)
                {
                    cBIAS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                // sn_dg_prm_smpl_connect_end_2
                return cBIAS;
            }
        }

        public batchReportService1 CBIBRS
        {
            get
            {
                if (cBIBRS == null)
                {
                    cBIBRS = new batchReportService1();
                    cBIBRS.Url = cBICMS.Url;
                }
                if (cBIBRS.biBusHeaderValue == null)
                {
                    cBIBRS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIBRS;
            }
        }
        public contentManagerService1 CBICMS
        {
            //This service is the initial connect and logon point.
            //It _must_ be initialized before calling this accessor.
            get
            {
                return cBICMS;
            }
        }
        public dataIntegrationService1 CBIDIS
        {
            get
            {
                if (cBIDIS == null)
                {
                    cBIDIS = new dataIntegrationService1();
                    cBIDIS.Url = cBICMS.Url;
                }
                if (cBIDIS.biBusHeaderValue == null)
                {
                    cBIDIS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIDIS;
            }
        }
        public deliveryService1 CBIDS
        {
            get
            {
                if (cBIDS == null)
                {
                    cBIDS = new deliveryService1();
                    cBIDS.Url = cBICMS.Url;
                }
                if (cBIDS.biBusHeaderValue == null)
                {
                    cBIDS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIDS;
            }
        }
        public eventManagementService1 CBIEMS
        {
            get
            {
                if (cBIEMS == null)
                {
                    cBIEMS = new eventManagementService1();
                    cBIEMS.Url = cBICMS.Url;
                }
                if (cBIEMS.biBusHeaderValue == null)
                {
                    cBIEMS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIEMS;
            }
        }
        public jobService1 CBIJS
        {
            get
            {
                if (cBIJS == null)
                {
                    cBIJS = new jobService1();
                    cBIJS.Url = cBICMS.Url;
                }
                if (cBIJS.biBusHeaderValue == null)
                {
                    cBIJS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIJS;
            }
        }
        public monitorService1 CBIMS
        {
            get
            {
                if (cBIMS == null)
                {
                    cBIMS = new monitorService1();
                    cBIMS.Url = cBICMS.Url;
                }
                if (cBIMS.biBusHeaderValue == null)
                {
                    cBIMS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIMS;
            }
        }
        public reportService1 CBIRS
        {
            get
            {
                if (cBIRS == null)
                {
                    cBIRS = new reportService1();
                    cBIRS.Url = cBICMS.Url;
                }
                if (cBIRS.biBusHeaderValue == null)
                {
                    cBIRS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBIRS;
            }
        }
        public systemService1 CBISS
        {
            get
            {
                if (cBISS == null)
                {
                    cBISS = new systemService1();
                    cBISS.Url = cBICMS.Url;
                }
                if (cBISS.biBusHeaderValue == null)
                {
                    cBISS.biBusHeaderValue = cBICMS.biBusHeaderValue;
                }
                return cBISS;
            }
        }

        public baseParameter[] getReportParameters(string reportPath)
        {
            searchPathSingleObject cmReportPath = new searchPathSingleObject();
            cmReportPath.Value = reportPath;

            // sn_dg_prm_smpl_runreport_P1_start_1
            // sn_dg_sdk_method_reportService_getParameters_start_0
            asynchReply gpReply = cBIRS.getParameters(cmReportPath, new parameterValue[] { }, new runOption[] { });
            // sn_dg_sdk_method_reportService_getParameters_end_0
            // sn_dg_prm_smpl_runreport_P1_end_1

            if ((gpReply.status != asynchReplyStatusEnum.complete)
                && (gpReply.status != asynchReplyStatusEnum.conversationComplete))
            {
                while ((gpReply.status != asynchReplyStatusEnum.complete)
                    && (gpReply.status != asynchReplyStatusEnum.conversationComplete))
                {
                    gpReply = cBIRS.wait(gpReply.primaryRequest, new parameterValue[] { }, new option[] { });
                }

            }

            if (gpReply.details == null)
            {
                return null;
            }

            // sn_dg_sdk_method_reportService_getParameters_start_1
            for (int i = 0; i < gpReply.details.Length; i++)
            {
                if (gpReply.details[i] is asynchDetailParameters)
                {
                    return ((asynchDetailParameters)gpReply.details[i]).parameters;
                }
            }
            // sn_dg_sdk_method_reportService_getParameters_end_1

            return null;
        }

    }
}
