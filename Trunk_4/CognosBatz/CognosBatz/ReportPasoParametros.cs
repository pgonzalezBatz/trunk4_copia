/**
 * PassDateRange.cs
 *
 * Licensed Material - Property of IBM
 * © Copyright IBM Corp. 2003, 2010
 *
 * Description: Technote 1344372 - SDK Sample to pass a date range parameter to a report
 *
 * Tested with: IBM Cognos BI 10.1, MS VS 2005
 */

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Services;
using cognosdotnet_2_0;

namespace CognosBatz
{
	
	public class ReportPasoParametros
	{
         reportService1 reportService = new reportService1();
		 contentManagerService1 cmService = new contentManagerService1();
		
		public baseParameter[] reportParameters;

        public ReportPasoParametros(String sendPoint)
		{	
			try
			{
                cmService.Url = sendPoint;
                reportService.Url = sendPoint;
			}
			catch (System.Exception e)
			{	Console.Out.WriteLine(e);
			}
		}

		

        public virtual parameterValue[] populateMultipleParm(string[] parametros, string[] valores)
        {
            // pass an array of parameters.
            parameterValue[] parameters = new parameterValue[reportParameters.Length];
            // Loop to populate all parameters.
            if (parametros.Length==reportParameters.Length && parametros.Length==valores.Length)
            {
                for (int i = 0; i < reportParameters.Length; i++ )
                {
                    //String parameterName = reportParameters[i].name;
                    String parameterName = parametros[i];

                    simpleParmValueItem item = new simpleParmValueItem();
                    item.use = valores[i]; // hard coded value for the parameter.
                    item.display = valores[i];
                    item.inclusive = true;

                    // below is a sample of setting a dateTime. passed as a string.
                    //item.setDisplay("Feb 2 2004 12:00 AM");
                    //item.setUse("2004-02-02T00:00:00");

                    parmValueItem[] pvi = new parmValueItem[1];
                    pvi[0] = item;

                    parameters[i] = new parameterValue();
                    parameters[i].name = parameterName;
                    parameters[i].value = pvi;
                }
            }
            
            return parameters;
        }

        public virtual System.IO.FileInfo executeReport_FI(String reportName, String format, string[] parametros, string[] valores)
		{
			String textOutput = null;
			asynchReply rsr;
			asynchDetailReportOutput reportOutput = null;
			
			runOption[] runOptions = new runOption[3];
			
			runOptionBoolean rob = new runOptionBoolean();
			runOptionStringArray rosa = new runOptionStringArray();
			runOptionBoolean rop = new runOptionBoolean();
			
			// We do not want to save this output
			rob.name=runOptionEnum.saveOutput;
			rob.value=false;
			
			//What format do we want the report in: PDF, HTML, or XML?
			rosa.name=runOptionEnum.outputFormat;
			rosa.value=new String[]{format};
			
			//Set the report not to prompt as we pass the parameter if any
			rop.name=runOptionEnum.prompt;
			rop.value=false;
			
			// Fill the array with the run options.
			runOptions[0] = rob;
			runOptions[1] = rosa;
			runOptions[2] = rop;
			
			option[] options = new option[runOptions.Length];
			
			for (int i = 0; i < runOptions.Length; i++)
			{
				options[i] = runOptions[i];
			}
			
			try
			{
				parameterValue[] parameters = null;
				searchPathSingleObject spSingle = new searchPathSingleObject();
				spSingle.Value=reportName;
				reportParameters = getParameters(spSingle);
				int countOfParameters = reportParameters.Length;

                if (countOfParameters > 0)
                    //parameters = populateBoundedRangeParm();
                    //parameters = populateSingleParm();
                    parameters = populateMultipleParm(parametros, valores);
                else
                    parameters = new parameterValue[] { };
				
				rsr = reportService.run(spSingle, parameters, options);
				
				// If it has not yet completed, keep waiting until it is done.
				// We could be waiting forever.
				//int count = 1;
				while (rsr.status != asynchReplyStatusEnum.complete && rsr.status != asynchReplyStatusEnum.conversationComplete)
				{
					rsr = reportService.wait(rsr.primaryRequest, parameters, runOptions);
					//System.Console.Out.WriteLine("waiting: " + count++);
				}
				
				for (int i = 0; i < rsr.details.Length; i++)
				{
					if (rsr.details[i] is asynchDetailReportOutput)
					{
						reportOutput = (asynchDetailReportOutput) rsr.details[i];
						break;
					}
				}
				
				textOutput = reportOutput.outputPages[0];
				
				// Write the report output to a temporary file and print the output to stdout
				System.IO.FileInfo oFile = new System.IO.FileInfo("c:\\temp\\reportOutput." + format);
				System.IO.FileStream fos = new System.IO.FileStream(oFile.FullName, System.IO.FileMode.Create);

                if (format.Equals("PDF") || format.Equals("CSV"))
                {
                    byte[] binaryOut = Convert.FromBase64String(textOutput);	
                    if (binaryOut != null)
                        fos.Write(binaryOut, 0, binaryOut.Length);
                }
                else
                {
                    if (textOutput != null)
                    {
                        byte[] hunk_data = UTF8Encoding.UTF8.GetBytes(textOutput);
                        fos.Write(hunk_data, 0, hunk_data.Length);
                    }
                }
				fos.Flush();
				fos.Close();

                return oFile;
			}
			catch (System.Exception e)
			{
                throw new Exception("Error in writing " + e.Message);
			}
		}

        public virtual System.IO.FileStream executeReport_FS(String reportName, String format, string[] parametros, string[] valores)
        {
            String textOutput = null;
            asynchReply rsr;
            asynchDetailReportOutput reportOutput = null;

            runOption[] runOptions = new runOption[3];

            runOptionBoolean rob = new runOptionBoolean();
            runOptionStringArray rosa = new runOptionStringArray();
            runOptionBoolean rop = new runOptionBoolean();

            // We do not want to save this output
            rob.name = runOptionEnum.saveOutput;
            rob.value = false;

            //What format do we want the report in: PDF, HTML, or XML?
            rosa.name = runOptionEnum.outputFormat;
            rosa.value = new String[] { format };

            //Set the report not to prompt as we pass the parameter if any
            rop.name = runOptionEnum.prompt;
            rop.value = false;

            // Fill the array with the run options.
            runOptions[0] = rob;
            runOptions[1] = rosa;
            runOptions[2] = rop;

            option[] options = new option[runOptions.Length];

            for (int i = 0; i < runOptions.Length; i++)
            {
                options[i] = runOptions[i];
            }

            try
            {
                parameterValue[] parameters = null;
                searchPathSingleObject spSingle = new searchPathSingleObject();
                spSingle.Value = reportName;
                reportParameters = getParameters(spSingle);
                int countOfParameters = reportParameters.Length;

                if (countOfParameters > 0)
                    //parameters = populateBoundedRangeParm();
                    //parameters = populateSingleParm();
                    parameters = populateMultipleParm(parametros, valores);
                else
                    parameters = new parameterValue[] { };

                rsr = reportService.run(spSingle, parameters, options);

                // If it has not yet completed, keep waiting until it is done.
                // We could be waiting forever.
                //int count = 1;
                while (rsr.status != asynchReplyStatusEnum.complete && rsr.status != asynchReplyStatusEnum.conversationComplete)
                {
                    rsr = reportService.wait(rsr.primaryRequest, parameters, runOptions);
                    //System.Console.Out.WriteLine("waiting: " + count++);
                }

                for (int i = 0; i < rsr.details.Length; i++)
                {
                    if (rsr.details[i] is asynchDetailReportOutput)
                    {
                        reportOutput = (asynchDetailReportOutput)rsr.details[i];
                        break;
                    }
                }

                textOutput = reportOutput.outputPages[0];

                // Write the report output to a temporary file and print the output to stdout
                System.IO.FileInfo oFile = new System.IO.FileInfo("c:\\temp\\reportOutput." + format);
                System.IO.FileStream fos = new System.IO.FileStream(oFile.FullName, System.IO.FileMode.Create);

                if (format.Equals("PDF") || format.Equals("CSV"))
                {
                    byte[] binaryOut = Convert.FromBase64String(textOutput);
                    if (binaryOut != null)
                        fos.Write(binaryOut, 0, binaryOut.Length);
                }
                else
                {
                    if (textOutput != null)
                    {
                        byte[] hunk_data = UTF8Encoding.UTF8.GetBytes(textOutput);
                        fos.Write(hunk_data, 0, hunk_data.Length);
                    }
                }
                fos.Flush();
                fos.Close();

                return fos;
            }
            catch (System.Exception e)
            {
                throw new Exception("Error in writing " + e.Message);
            }
        }
		
		//get report parameters
		public virtual baseParameter[] getParameters(searchPathSingleObject spSingle)
		{
			baseParameter[] parm = new parameter[]{};
			asynchReply response = null;
			
			try
			{
				response = reportService.getParameters(spSingle, new parameterValue[]{}, new option[]{});
				if (!response.status.Equals(asynchReplyStatusEnum.conversationComplete))
				{
					while (!response.status.Equals(asynchReplyStatusEnum.conversationComplete))
					{
						response = reportService.wait(response.primaryRequest, new parameterValue[]{}, new option[]{});
					}
				}
			}
			catch (System.Runtime.Remoting.RemotingException e)
			{
                throw new Exception("Error in remoting " + e.Message);
			}
			
			for (int i = 0; i < response.details.Length; i++)
				if (response.details[i] is asynchDetailParameters)
					parm = ((asynchDetailParameters) response.details[i]).parameters;
			
			return parm;
		}
		
		
		//this method will login the user to Cognos
        public virtual String quickLogon(String userNamespace, String uid, String pwd)
		{
            System.Text.StringBuilder credentialXML = new System.Text.StringBuilder("<credential>");
            credentialXML.AppendFormat("<namespace>{0}</namespace>", userNamespace);
            credentialXML.AppendFormat("<username>{0}</username>", uid);
            credentialXML.AppendFormat("<password>{0}</password>", pwd);
            credentialXML.Append("</credential>");

            //The csharp toolkit encodes the credentials
            string encodedCredentials = credentialXML.ToString();
            xmlEncodedXML xmlEncodedCredentials = new xmlEncodedXML();
            xmlEncodedCredentials.Value = encodedCredentials;
            searchPathSingleObject[] emptyRoleSearchPathList = new searchPathSingleObject[0];

            try
            {
                cmService.logon(xmlEncodedCredentials, null);
                reportService.biBusHeaderValue = cmService.biBusHeaderValue;
            }
            catch (System.Exception e)
            {
                  System.Console.Out.WriteLine(e);
            }             
			
			return ("Logon successful as " + uid);
		}
		
		
	}
}