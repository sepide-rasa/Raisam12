using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RaisamWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            timScheduledTask.Interval = 24 * 60 * 60 * 1000;    

            timScheduledTask.Enabled = true;

            timScheduledTask.Elapsed += new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);

        }
        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
                RaiSamEntities m = new RaiSamEntities();
                var g=m.prs_GetDate().FirstOrDefault();

                try
                {
                    getSajamData();
                    //Select
                    var sms=m.prs_tblSafSMSSelect("Pass3Day", "", 0).ToList();
                    foreach (var item in sms)
                    {
                    //ersal payam
                        CallWebServiceSms(item.fldMatn, item.fldMobile);
                    //Update
                        m.prs_tblSafSMSUpdate(2, item.fldAshkhasId, item.fldCherkheFirstEghdamId, 1);
                        //insert mojadad hamun record
                        m.prs_tblSafSMSInsert(item.fldMatn,1,item.fldAshkhasId,item.fldCherkheFirstEghdamId, 1);
                        
                    }
                }
                catch (Exception x)
                {

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(sms For Cartabl)");
                }

            //sv.GetGroupPersonelInfo("1", 1);
        }

        protected override void OnStop()
        {
        }

        void getSajamData()
        {
            RaiSamEntities m = new RaiSamEntities();
            var g = m.prs_GetDate().FirstOrDefault();
            try
            {
                var kk = "";
                SejamService.SejamService sv = new SejamService.SejamService();
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                try
                {
                    var MoveTran = sv.GetMoveTranDetails(g.fldTarikh, g.fldTarikh, null, null);

                    List<MoveTranDetails> a = new List<MoveTranDetails>();
                    if (MoveTran.Err == 0)
                    {
                        foreach (var item in MoveTran.data)
                        {

                            if (/*item.PelakNo != 0 &&*/ item.SalonNo != 0)
                                m.prs_InsertFromWebServiceSajam(item.TranNo, item.MoveDate, item.MoveTime, item.PelakNo, item.SalonNo, item.SourceStation, item.TargetStation);

                        }
                        sv.CallWebServiceRaja();
                    }
                    else
                    {
                        m.prs_tblErrorInsert(ErrorId, MoveTran.Msg, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(SajamWinService1)");
                    }
                }
                catch (Exception x)
                {

                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, 1, "تاریخ: " + g.fldTarikh + "(SajamWinService2)");
                }



                m.Dispose();
                sv.Dispose();
            }
            catch (Exception x)
            {
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId2 = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                m.prs_tblErrorInsert(ErrorId2, InnerException, DateTime.Now, 1, "SajamWinService3");
                m.Dispose();
            }
        }
        public void CallWebServiceSms(string Matn, string Mobile)
        {

            RaiSamEntities m = new RaiSamEntities();
            var haveSmsPanel = m.prs_tblSMSSettingSelect("", "", 1).FirstOrDefault();


            //Reading input values from console
            //string a = "s";
            //string b = "e";
            //Calling InvokeService method
            InvokeService(haveSmsPanel.fldUserName, haveSmsPanel.fldPassword, Matn, Mobile, "0", 1, 2, null, "RailWay", null, 0, 0, "", "");
        }
        public void InvokeService(string cUserName, string cPassword, string cBody, string cSmsnumber, string cGetid, int nCMessage, int nTypeSent, string m_SchedulDate, string cDomainname, string cFromNumber, int nSpeedsms, int nPeriodmin, string cstarttime, string cEndTime)
        {
            //Calling CreateSOAPWebRequest method
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
             <soap:Body>
                <wbsLogin xmlns=""http://tempuri.org/"">
                    <cUserName>" + cUserName + @"</cUserName>
                      <cPassword>" + cPassword + @"</cPassword>
                      <cBody>" + cBody + @"</cBody>
                      <cSmsnumber>" + cSmsnumber + @"</cSmsnumber>
                      <cGetid>" + cGetid + @"</cGetid>
                      <nCMessage>" + nCMessage + @"</nCMessage>
                      <nTypeSent>" + nTypeSent + @"</nTypeSent>
                      <m_SchedulDate>" + m_SchedulDate + @"</m_SchedulDate>
                      <cDomainname>" + cDomainname + @"</cDomainname>
                      <cFromNumber>" + cFromNumber + @"</cFromNumber>
                      <nSpeedsms>" + nSpeedsms + @"</nSpeedsms>
                      <nPeriodmin>" + nPeriodmin + @"</nPeriodmin>
                      <cstarttime>" + cstarttime + @"</cstarttime>
                      <cEndTime>" + cEndTime + @"</cEndTime>
                </wbsLogin>
              </soap:Body>
            </soap:Envelope>");


            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + @"\Uploaded\smsOut.txt", ServiceResult);
                    Console.WriteLine(ServiceResult);
                    Console.ReadLine();
                }
            }
        }
        public HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request
            //HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://madeh12.rai.ir/SejamService.asmx");
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://sms.rai.ir/webservice/service.asmx");
            //SOAPAction
            //Req.Headers.Add(@"SOAPAction:http://tempuri.org/TestService");
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/SendSms");
            //Content_type
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method
            Req.Method = "POST";
            //return HttpWebRequest
            return Req;
        }
    }
}
