using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cognosdotnet_2_0;
using System.Web.Services.Protocols;

namespace CognosBatz
{
    public class Cognos
    {
        private string _url;
        private string _userName;
        private string _password;
        private string _dominio;

        public Cognos(string url, string userName, string password, string dominio)
        {
            _url = url;
            _userName = userName;
            _password = password;
            _dominio = dominio;
        }
        //private CognosConnection cgn;

        public bool executeReport(string reportPath,string outputFormat, string emailTo, string subject, string body)
        {  
            
            CognosConnection cgn = new CognosConnection();
            cgn.conectar(_url, _userName, _password, _dominio);
            BaseClassWrapper report = buildSingleReport(cgn, reportPath);
            return emailReport(cgn, report,outputFormat, emailTo, subject, body);
        }

        public string getParametros(string reportPath)
        {
            string parametros="";
            CognosConnection cgn = new CognosConnection();
            cgn.conectar(_url, _userName, _password, _dominio);

            return parametros;

           
        }

        public BaseClassWrapper buildSingleReport(CognosConnection connection, string reportPath)
        {
            return buildReport(connection, reportPath)[0];
        }

        public BaseClassWrapper[] buildReportList(CognosConnection connection, string reportPath)
        {
            return buildReport(connection, reportPath);
        }

        private BaseClassWrapper[] buildReport(CognosConnection connnection, string reportPath)
        {
            baseClass[] reports = new baseClass[0];
            propEnum[] props = new propEnum[] { propEnum.searchPath, propEnum.defaultName, propEnum.objectClass, propEnum.parent };
            sort[] sortOptions = { new sort() };
            sortOptions[0].order = orderEnum.ascending;
            sortOptions[0].propName = propEnum.defaultName;
            searchPathMultipleObject reportsPath = new searchPathMultipleObject();
            reportsPath.Value = reportPath; //"/content/folder[@name='BATZ']/folder[@name='98 - Monedas']/report[@name='02 - RS 1 EURO son .....']";
            reports = connnection.CBICMS.query(reportsPath, props, sortOptions, new queryOptions());
            BaseClassWrapper[] reportQueryList = new BaseClassWrapper[reports.GetLength(0)]; //+ queries.GetLength(0)
            int nbReports = 0;
            if ((reports != null) && (reports.GetLength(0) > 0))
            {
                nbReports = reports.GetLength(0);
                for (int i = 0; i < nbReports; i++)
                {
                    reportQueryList[i] = new BaseClassWrapper(reports[i]);
                }
            }
            return reportQueryList;
        }


       
        public class BaseClassWrapper
        {
            public BaseClassWrapper[] reportAndQueryList = new BaseClassWrapper[0];
            private cognosdotnet_2_0.baseClass m_BaseClassObject = null;

            public BaseClassWrapper()
            {
            }

            public BaseClassWrapper(cognosdotnet_2_0.baseClass wrapThis)
            {
                m_BaseClassObject = wrapThis;
            }

            public override string ToString()
            {
                return m_BaseClassObject.defaultName.value;
            }

            public cognosdotnet_2_0.stringProp searchPath
            {
                get
                {
                    return m_BaseClassObject.searchPath;
                }
            }

            public cognosdotnet_2_0.tokenProp defaultName
            {
                get
                {
                    return m_BaseClassObject.defaultName;
                }
            }
        }


        private bool emailReport(CognosConnection cgn, BaseClassWrapper reportPath,string outputFormat, string emailTo, string subject, string body)
        {

            Email emailObject = new Email();
            // 1. check the list of contacts in the content store

            return emailObject.sendEmail(cgn, reportPath,outputFormat, emailTo, subject, body);

        }

        class Email
        {
            public Email() { }

            public bool sendEmail(CognosConnection connection, BaseClassWrapper reportOrQueryObject, string outputFormat,string emailAddress, string emailSubject, string emailBody)
		        {
                    try
                    {
			            if (connection == null)
			            {
				            return false; //"...the connection is invalid.\n";
			            }
			            option[] emailRunOptions = new option[4]; // holds the run options for running the report
			            option[] emailOptions = new option[5];    // holds all other email options for delivering the output
			
			            // 1. Set the run options
			            runOptionStringArray deliveryFormat = new runOptionStringArray();
			            string[] formatList = new string[1];
			            formatList[0] = outputFormat;
			            deliveryFormat.name = runOptionEnum.outputFormat;
			            deliveryFormat.value = formatList;
			            emailRunOptions[0] = deliveryFormat;

			            runOptionBoolean promptFlag = new runOptionBoolean();
			            promptFlag.name = runOptionEnum.prompt;
			            promptFlag.value = false;
			            emailRunOptions[1] = promptFlag;

			            // Set the primary wait threshold
			            asynchOptionInt primaryWaitRunOpts = new asynchOptionInt();
			            primaryWaitRunOpts.name = asynchOptionEnum.primaryWaitThreshold;
			            primaryWaitRunOpts.value = 0;
			            emailRunOptions[2] = primaryWaitRunOpts;

			            asynchOptionBoolean alwaysIncludePrimaryRequestFlag = new asynchOptionBoolean();
			            alwaysIncludePrimaryRequestFlag.name = asynchOptionEnum.alwaysIncludePrimaryRequest;
			            alwaysIncludePrimaryRequestFlag.value = true;
			            emailRunOptions[3] = alwaysIncludePrimaryRequestFlag;

			            // 2. Set the delivery options
                        // a) specify email delivery
                        runOptionBoolean emailDelivery = new runOptionBoolean();
                        emailDelivery.name = runOptionEnum.email;
                        emailDelivery.value = true;
                        emailOptions[0] = emailDelivery;

			            // b) set the output to be an email attachment
			            runOptionBoolean emailAttach = new runOptionBoolean();
			            emailAttach.name = runOptionEnum.emailAsAttachment;
			            emailAttach.value = true;
			            emailOptions[1] = emailAttach;

			            // c) Set the email body
			            memoPartString memoText = new memoPartString();
			            memoText.name = "Body";
			            memoText.text = emailBody;
			            memoText.contentDisposition = smtpContentDispositionEnum.inline;
			            deliveryOptionMemoPart bodyMemo = new deliveryOptionMemoPart();
			            bodyMemo.name = deliveryOptionEnum.memoPart;
			            bodyMemo.value = memoText;
			            emailOptions[2] = bodyMemo;

                        char[] sep=new char[1] {';'};
                        string[] emails = emailAddress.Split(sep);
                        addressSMTP singleAddress;
                        addressSMTP[] addressValue = addressValue = new addressSMTP[emails.GetLength(0)];
                        int i=0;
                        foreach(string emailAd in emails)
                        {
                            singleAddress = new addressSMTP();
                            singleAddress.Value = emailAd;
                            addressValue[i] = singleAddress;
                            i += 1;
                        }                            
				
			            // c) i. Option is set to email to a specific user
			            deliveryOptionAddressSMTPArray addressDeliveryOption = new deliveryOptionAddressSMTPArray();
			            addressDeliveryOption.name = deliveryOptionEnum.toAddress;
			            addressDeliveryOption.value = addressValue;
			            emailOptions[3] = addressDeliveryOption;
			

			            // e) Set the email subject
			            deliveryOptionString subjectOptionString = new deliveryOptionString();
			            subjectOptionString.name = deliveryOptionEnum.subject;
			            subjectOptionString.value = emailSubject;
			            emailOptions[4] = subjectOptionString;

			            searchPathSingleObject reportPath = new searchPathSingleObject();
			            reportPath.Value = reportOrQueryObject.searchPath.value;

			            // execute the report
			            // sn_dg_sdk_method_reportService_deliver_start_0


                        asynchReply sendEmailResponse = connection.CBIRS.run(reportPath, new parameterValue[] { }, emailRunOptions);
                        connection.CBIRS.deliver(sendEmailResponse.primaryRequest, new parameterValue[] { }, emailOptions);
                        // sn_dg_sdk_method_reportService_deliver_end_0

                        //If the request has not yet completed, keep waiting until it has finished
                        while ((sendEmailResponse.status != asynchReplyStatusEnum.complete) && (sendEmailResponse.status != asynchReplyStatusEnum.conversationComplete))
                        {
                            sendEmailResponse = connection.CBIRS.wait(sendEmailResponse.primaryRequest, new parameterValue[] { }, new option[] { });
                        }
			            return true;//"...the report : \"" + reportOrQueryObject.defaultName.value + "\" was successfully emailed.\n";
		            }
                    catch
                    {return false;}
               }

        }
    }
}
