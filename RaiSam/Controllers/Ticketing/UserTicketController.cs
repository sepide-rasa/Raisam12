﻿using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace RaiSam.Controllers.Ticketing
{
    public class UserTicketController : Controller
    {
        //
        // GET: /UserTicket/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        UserInfo u = (UserInfo)System.Web.HttpContext.Current.Session["User"];

        public ActionResult Index()
        {
            //باز شدن پنجره
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            Models.RaiSamEntities m = new RaiSamEntities();
            Models.prs_tblTicketSelect lastTicket = new Models.prs_tblTicketSelect();
            var DefaultCategory = "0";
            var ValidFormat = "";
            if (Session["User"] != null)//کاربر
            {
                var UserInfo = (UserInfo)(Session["User"]);

                var user = m.prs_tblUserSelect("fldId", UserInfo.UserSecondId.ToString(),"",1).FirstOrDefault();

                lastTicket = m.prs_tblTicketSelect("CheckCategory", user.fldShakhsId.ToString(),"",1).FirstOrDefault();
                PartialView.ViewBag.AshkhasId = user.fldShakhsId;

                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                foreach (var item in MojazNew)
                {
                    ValidFormat = ValidFormat + item + ";";
                }
                PartialView.ViewBag.ValidFormat = ValidFormat;
            }
            //else//مأمورین موظف
            //{
            //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
            //    param.FieldName = "CheckCategory";
            //    param.Value = MovazafInfo.AshkhasId.ToString();
            //    param.Value2 = "";
            //    param.Top = 1;
            //    lastTicket = service.SelectTicket_Movazaf(param, MovazafInfo.UserKey, IP).ListTicket.FirstOrDefault();
            //    PartialView.ViewBag.AshkhasId = MovazafInfo.AshkhasId;

            //    param.FieldName = "";
            //    param.Value = "";
            //    param.Top = 0;
            //    var Mojaz = service.SelectTicketFormatFile_Movazaf(param, MovazafInfo.UserKey, IP).TicketFormatFileList.Select(l => l.fldPassvand.ToLower()).ToList();
            //    var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
            //    foreach (var item in MojazNew)
            //    {
            //        ValidFormat = ValidFormat + item + ";";
            //    }
            //    PartialView.ViewBag.ValidFormat = ValidFormat;
            //}
            if (lastTicket != null)
            {
                DefaultCategory = lastTicket.fldTicketCategoryId.ToString();
            }
            PartialView.ViewBag.DefaultCategory = DefaultCategory;
            return PartialView;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCategory()
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                //if (Session["User"] != null)//کاربر
                //{
                    var UserInfo = (UserInfo)(Session["User"]);
                    var q = m.prs_tblTicketCategorySelect("fldType_UserId", UserInfo.UserSecondId.ToString(), "", 0).ToList().Select(n => new { ID = n.fldId, Name = n.fldTitle });
                    return this.Store(q);
                //}
               
                //else
                //{
                //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
                //    param.FieldName = "fldType";
                //    param.Value = "0";
                //    param.Value2 = "";
                //    param.Top = 0;
                //    var q = service.SelectTicketCategory_Movazaf(param, MovazafInfo.UserKey, IP).TicketCategoryList.ToList().Select(n => new { ID = n.fldId, Name = n.fldTitle });
                //    return this.Store(q);
                //}
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadChat(int Id, int? CategoryId, int countUserTicket)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            List<Models.prs_tblTicketSelect> q = new List<Models.prs_tblTicketSelect>();
            List<Models.prs_tblTicketSelect> q1 = new List<Models.prs_tblTicketSelect>();
            List<Models.prs_tblTicketSelect> q2 = new List<Models.prs_tblTicketSelect>();

            int AshkhasId = 0;
            //Idدر واقع همان آیدی شخص است اینجا
            if ((Session["User"] != null && Permission.haveAccess(52,null,null)) || Session["Movazaf"] != null)
            {
                var type = "";
                var matn = "";
                var att = "";
                var time = "";
                var seen = "";
                var UserIdd = "";
                var Forwarded = "";
                var ChatId = "";
                if (Session["User"] != null)
                {
                    var UserInfo = (UserInfo)(Session["User"]);
                    if (CategoryId != null)
                        q = m.prs_tblTicketSelect("fldAshkhasId", Id.ToString(), CategoryId.ToString(), countUserTicket).ToList();

                    var user = m.prs_tblUserSelect("fldId", UserInfo.UserSecondId.ToString(),"",1).FirstOrDefault();
                    AshkhasId = Convert.ToInt32(user.fldShakhsId);
                }
                //else
                //{

                //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
                //    q = m.SelectTicket_Movazaf(param, MovazafInfo.UserKey, IP).ListTicket.ToList();
                //    AshkhasId = MovazafInfo.AshkhasId;
                //}

                if (Session["User"] != null)
                {
                    var UserInfo = (UserInfo)(Session["User"]);
                    q1 = m.prs_tblTicketSelect("fldAshkhasId_NotSeen", AshkhasId.ToString(),"",0).ToList();
                }
                //else
                //{
                //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
                //    q1 = service.SelectTicket_Movazaf(param, MovazafInfo.UserKey, IP).ListTicket.ToList();
                //}

              
                if (Session["User"] != null)
                {
                    var UserInfo = (UserInfo)(Session["User"]);
                    q2 = m.prs_tblTicketSelect("Permmision", AshkhasId.ToString(),"",0).ToList();
                }
                //else
                //{
                //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
                //    q2 = service.SelectTicket_Movazaf(param, MovazafInfo.UserKey, IP).ListTicket.ToList();
                //}

                foreach (var item in q)
                {
                    if (item.fldUserId != null)
                    {
                        UserIdd = UserIdd + item.fldUserId + "|";
                    }
                    else
                    {
                        UserIdd = UserIdd + "0|";
                    }
                    int? haveAtt = 0;
                    if (item.fldFileId != null)
                        haveAtt = item.fldFileId;
                    if (item.fldSeen)
                        seen = seen + "1" + "|";
                    else
                        seen = seen + "0" + "|";
                    var t = 1;
                    if (item.fldUserId != null)
                        t = 2;
                    type = type + t + "|";

                    var matnHtml = item.fldHTML;
                    if (item.fldSourceReplyId != null)
                    {
                        var UserInfo = (UserInfo)(Session["User"]);
                       
                        var tt = m.prs_tblTicketSelect("fldId", item.fldSourceReplyId.ToString(),"",0).FirstOrDefault();
                        matnHtml = "<div style='background-color: #bbd2f0;cursor: pointer;height: 50px;' onclick='FnShowTiketHtml(" + item.fldSourceReplyId.ToString() + ")' title='" + tt.Matn + "' >پاسخ به: " + item.fldMatnReply + "</div>" + matnHtml;
                    }
                    matn = matn + matnHtml + "|";

                    att = att + haveAtt + "|";
                    time = time + item.fldTarikh + "|";
                    Forwarded = Forwarded + item.fldSourceForwardId + "|";
                    ChatId = ChatId + item.fldId + "|";
                }
                return Json(new
                {
                    type = type,
                    matn = matn,
                    att = att,
                    time = time,
                    seen = seen,
                    UserIdd = UserIdd,
                    CountUser = q1.Count,
                    CountAdmin = q2.Count,
                    Count = countUserTicket,
                    Forwarded = Forwarded,
                    ChatId = ChatId
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Error", "Home", new { Code = "403" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public FileContentResult DownloadAttach(string Id)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return null;
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            Models.prs_tblFileSelect f = new Models.prs_tblFileSelect();
            if (Session["User"] != null)
            {
                var UserInfo = (UserInfo)(Session["User"]);
                f = m.prs_tblFileSelect("fldId", Id.ToString(),1).FirstOrDefault();
            }
            //else
            //{
            //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
            //    f = service.SelectFile_Movazaf(param, MovazafInfo.UserKey, IP).FileList.FirstOrDefault();
            //}
            if (f != null)
            {
                MemoryStream st = new MemoryStream(f.fldFile);
                return File(st.ToArray(), MimeType.Get(System.IO.Path.GetExtension(f.fldPasvand)), "File." + f.fldPasvand);
            }
            return null;
        }
        [HttpPost]
        public ActionResult Upload()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            try
            {
                if (Session["savePathUserTicket"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathUserTicket"].ToString());
                    Session.Remove("savePathUserTicket");
                    Session.Remove("PasvandUserTicket");
                    Session.Remove("FileNameUserTicket");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var Allowed = false;
              
                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                if (MojazNew.Contains(extension.Substring(1)))
                {
                    switch (extension.ToLower())
                    {
                        case ".pdf":
                            Allowed = FileInfoExtensions.IsPDF(Request.Files[0]);
                            break;
                        case ".xls":
                        case ".xlsx":
                            Allowed = FileInfoExtensions.IsExcel(Request.Files[0]);
                            break;
                        case ".doc":
                        case ".docx":
                            Allowed = FileInfoExtensions.IsWord(Request.Files[0]);
                            break;
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".tiff":
                        case ".tif":
                        case ".gif":
                        case ".bmp":
                            Allowed = FileInfoExtensions.IsImage(Request.Files[0]);
                            break;
                        case ".mp4":
                            Allowed = FileInfoExtensions.IsMP4(Request.Files[0]);
                            break;
                        default:
                            Allowed = FileInfoExtensions.blockList(Request.Files[0]);
                            break;
                    }
                }
                if (Allowed == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".pdf"
                    || extension == ".tif" || extension == ".tiff" || extension == ".ico" || extension == ".bmp"
                    || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".xls"
                    || extension == ".xlsx" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        var Name = Guid.NewGuid();
                        string savePath = Server.MapPath(@"~\Uploaded\" + Name + extension);
                        string e = Path.GetExtension(savePath);
                        file.SaveAs(savePath);
                        Session["savePathUserTicket"] = savePath;
                        Session["PasvandUserTicket"] = file.FileName.Split('.').Last();
                        Session["FileNameUserTicket"] = file.FileName.Split('.').First();
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "فایل با موفقیت آپلود شد.",
                            IsValid = true
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathUserTicket"] != null)
                {
                    System.IO.File.Delete(Session["savePathUserTicket"].ToString());
                    Session.Remove("savePathUserTicket");
                    Session.Remove("PasvandUserTicket");
                    Session.Remove("FileNameUserTicket");
                }
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {

                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(string Ticket1, string HaveFile1, int? SourceId)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0; int output = 0;
                var s = m.prs_GetDate().FirstOrDefault();
                var d = s.fldTarikh + "-" + s.fldTime.Substring(0, 5);
                try
                {
                    string Pasvand = "";
                    string FileName = "";
                    byte[] report_file = null;
                    Models.prs_tblTicketSelect Ticket = JsonConvert.DeserializeObject<Models.prs_tblTicketSelect>(Ticket1);
                    var template = new { HaveFile = false };
                    bool HaveFile = JsonConvert.DeserializeAnonymousType(HaveFile1, template).HaveFile;

                    Ticket.fldHTML = System.Uri.UnescapeDataString(Ticket.fldHTML);
                    if (HaveFile == true && Session["savePathUserTicket"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathUserTicket"].ToString()));
                        report_file = stream.ToArray();
                        Pasvand = Session["PasvandUserTicket"].ToString();
                        FileName = Session["FileNameUserTicket"].ToString();
                    }
                    if (Ticket.fldHTML == null)
                        Ticket.fldHTML = "";

                    System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));


                    if (Session["User"] != null)
                    {
                        var UserInfo = (UserInfo)(Session["User"]);
                        Ticket.fldInputID = UserInfo.UserInputId;


                        m.prs_tblTicketInsert(Id,null, Ticket.fldHTML, false, report_file, Pasvand, null, "", Ticket.fldTicketCategoryId, Ticket.fldAshkhasId, FileName,
                            u.UserInputId, null, null, SourceId);

                        output = Convert.ToInt32(Id.Value);
                    }
                    //else
                    //{
                    //    var Movazaf = (MovazafInfo)(Session["Movazaf"]);
                    //    Ticket.fldInputId = null;
                    //    Ticket.fldInputId_Personal = Movazaf.PrsInputId;
                    //    output = service.InsertTicket_Movazaf(Ticket, Movazaf.UserKey, IP);
                    //}

                  
                    if (Session["savePathUserTicket"] != null)
                    {
                        System.IO.File.Delete(Session["savePathUserTicket"].ToString());
                        Session.Remove("savePathUserTicket");
                        Session.Remove("PasvandUserTicket");
                        Session.Remove("FileNameUserTicket");
                    }
                }
                catch (Exception x)
                {
                    var InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                    if (Session["savePathUserTicket"] != null)
                    {
                        System.IO.File.Delete(Session["savePathUserTicket"].ToString());
                        Session.Remove("savePathUserTicket");
                        Session.Remove("PasvandUserTicket");
                        Session.Remove("FileNameUserTicket");
                    }
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    time = d,
                    FileId = output
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowPic(int id, byte State)
        {//برگرداندن عکس 
            //if (id != 0)
            //{
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null && Session["Movazaf"] == null)
                return null;
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            byte[] FilePerson = null;
            var image = "";
            
            if (Session["User"] != null)
            {
                var UserInfo = (UserInfo)(Session["User"]);
                if (State == 1)
                {//عکس کاربر
                    var user = m.prs_tblUserSelect("fldId", id.ToString(),"",1).FirstOrDefault();

                    var shakhs = m.prs_tblAshkhasSelect("fldId", user.fldShakhsId.ToString(), "", "", "", 0).FirstOrDefault();
                    if (shakhs.fldFileId != null)
                    {
                        FilePerson = m.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(),1).FirstOrDefault().fldFile;
                    }
                }
                else
                {//عکس شخص

                    var shakhs = m.prs_tblAshkhasSelect("fldId", id.ToString(), "", "", "", 1).FirstOrDefault();
                    if (shakhs.fldFileId != null)
                    {
                        FilePerson = m.prs_tblFileSelect("fldId", shakhs.fldFileId.ToString(), 1).FirstOrDefault().fldFile;
                    }
                }
            }
            //else
            //{
            //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
            //    if (State == 1)
            //    {//عکس کاربر
            //        var user = service.SelectUser_Movazaf(param, MovazafInfo.UserKey, IP).UserList.FirstOrDefault();

            //        param.FieldName = "fldId";
            //        param.Value = user.fldShakhsId.ToString();
            //        param.Value2 = "";
            //        param.Top = 1;
            //        var shakhs = service.SelectAshkhas_Movazaf(param, MovazafInfo.UserKey, IP).AshkhasList.FirstOrDefault();
            //        if (shakhs.fldFileId != null)
            //        {
            //            param.FieldName = "fldId";
            //            param.Value = shakhs.fldFileId.ToString();
            //            param.Top = 1;
            //            FilePerson = service.SelectFile_Movazaf(param, MovazafInfo.UserKey, IP).FileList.FirstOrDefault().fldFile;
            //        }
            //    }
            //    else
            //    {//عکس شخص

            //        var shakhs = service.SelectAshkhas_Movazaf(param, MovazafInfo.UserKey, IP).AshkhasList.FirstOrDefault();
            //        if (shakhs.fldFileId != null)
            //        {
            //            param.FieldName = "fldId";
            //            param.Value = shakhs.fldFileId.ToString();
            //            param.Top = 1;
            //            FilePerson = service.SelectFile_Movazaf(param, MovazafInfo.UserKey, IP).FileList.FirstOrDefault().fldFile;
            //        }
            //    }
            //}
            if (FilePerson == null)
            {
                var Image = Server.MapPath("~/Content/user.png");
            if (State == 2)
                Image = Server.MapPath("~/Content/user1.png"); 
                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                image = Convert.ToBase64String(stream.ToArray());
            }
            else
            {
                image = Convert.ToBase64String(FilePerson);
                if (image == "IA==" || image == "ICAgICAgIA==" || image == "ICA=" || image == "IAAgAA==")
                {
                    var Image = Server.MapPath("~/Content/user.png");
                    if (State == 2)
                        Image = Server.MapPath("~/Content/user1.png"); 
                    MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Image.ToString()));
                    image = Convert.ToBase64String(stream.ToArray());
                }
            }
            //}
            //else
            //{
            //    param.FieldName = "";
            //    param.Value = "";
            //    param.Top = 1;
            //    var q = service.SelectSetting(param, u.UserKey, IP).settingList.FirstOrDefault();
            //    if (q != null)
            //    {
            //        if (q.fldFile != null)
            //        {
            //            return File((byte[])q.fldFile, "jpg");
            //        }
            //    }
            //}
            return new JsonResult()
            {
                Data = new
                {
                    image = image
                },
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void ReloadFromUser()
        {
            SignalrHub h = new SignalrHub();
            h.Send("");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSeenUser(int Id, int? CategoryId)
        {
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
         
            if (Session["User"] != null)
            {
                var UserInfo = (UserInfo)(Session["User"]);
                if (CategoryId != null)
                    m.prs_tblTicketUpdateBySetadUserId(Id, CategoryId, "User");
            }
            //else
            //{
            //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
            //    service.TicketUpdateBySetadUserId_Movazaf(ticket, MovazafInfo.UserKey, IP);
            //}
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult OpenReplyWin(int chatId)
        {//باز شدن پنجره
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Session["User"] == null && Session["Movazaf"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                var ValidFormat = "";
                var Mojaz = new List<string>();
              
                if (Session["User"] != null)
                {
                    UserInfo user = (UserInfo)(Session["User"]);
                    Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                }
                //else
                //{
                //    var MovazafInfo = (MovazafInfo)(Session["Movazaf"]);
                //    Mojaz = service.SelectTicketFormatFile_Movazaf(param, MovazafInfo.UserKey, IP).TicketFormatFileList.Select(l => l.fldPassvand.ToLower()).ToList();
                //}
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                foreach (var item in MojazNew)
                {
                    ValidFormat = ValidFormat + item + ";";
                }
                PartialView.ViewBag.ValidFormat = ValidFormat;
                PartialView.ViewBag.chatId = chatId;
                return PartialView;
            }
        }
        [HttpPost]
        public ActionResult UploadReply()
        {
            UserInfo userSession = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
            try
            {

                if (Session["savePathReply"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("PasvandReply");
                    Session.Remove("FileNameReply");
                    System.IO.File.Delete(physicalPath);
                }
                var extension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var Allowed = false;
              
                var Mojaz = m.prs_tblTicketFormatFileSelect("","",0).Select(l => l.fldPassvand.ToLower()).ToList();
                var MojazNew = Mojaz.SelectMany(s => s.Split(',')).ToList();
                if (MojazNew.Contains(extension.Substring(1)))
                {
                    switch (extension.ToLower())
                    {
                        case ".pdf":
                            Allowed = FileInfoExtensions.IsPDF(Request.Files[0]);
                            break;
                        case ".xls":
                        case ".xlsx":
                            Allowed = FileInfoExtensions.IsExcel(Request.Files[0]);
                            break;
                        case ".doc":
                        case ".docx":
                            Allowed = FileInfoExtensions.IsWord(Request.Files[0]);
                            break;
                        case ".png":
                        case ".jpg":
                        case ".jpeg":
                        case ".tiff":
                        case ".tif":
                        case ".gif":
                        case ".bmp":
                            Allowed = FileInfoExtensions.IsImage(Request.Files[0]);
                            break;
                        case ".mp4":
                            Allowed = FileInfoExtensions.IsMP4(Request.Files[0]);
                            break;
                        default:
                            Allowed = FileInfoExtensions.blockList(Request.Files[0]);
                            break;
                    }
                }
                if (Allowed == true/*extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".pdf"
                    || extension == ".tif" || extension == ".tiff" || extension == ".ico" || extension == ".bmp"
                    || extension == ".doc" || extension == ".docx" || extension == ".txt" || extension == ".xls"
                    || extension == ".xlsx" || extension == ".gif"*/)
                {
                    if (Request.Files[0].ContentLength <= 10485760)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        string savePath = Server.MapPath(@"~\Uploaded\" + file.FileName);
                        string e = Path.GetExtension(savePath);
                        file.SaveAs(savePath);
                        Session["FileNameReply"] = file.FileName;
                        Session["savePathReply"] = savePath;
                        Session["PasvandReply"] = file.FileName.Split('.').Last();
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "فایل با موفقیت آپلود شد.",
                            IsValid = true
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    }
                    else
                    {
                        object r = new
                        {
                            success = true,
                            name = Request.Files[0].FileName,
                            Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد.",
                            IsValid = false
                        };
                        return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                        //X.Msg.Show(new MessageBoxConfig
                        //{
                        //    Buttons = MessageBox.Button.OK,
                        //    Icon = MessageBox.Icon.ERROR,
                        //    Title = "خطا",
                        //    Message = "حجم فایل انتخابی می بایست کمتر از ده مگابایت باشد."
                        //});
                        //DirectResult result = new DirectResult();
                        //return result;
                    }
                }
                else
                {
                    object r = new
                    {
                        success = true,
                        name = Request.Files[0].FileName,
                        Message = "فرمت فایل انتخاب شده غیر مجاز است.",
                        IsValid = false
                    };
                    return Content(string.Format("<textarea>{0}</textarea>", JSON.Serialize(r)));
                    //X.Msg.Show(new MessageBoxConfig
                    //{
                    //    Buttons = MessageBox.Button.OK,
                    //    Icon = MessageBox.Icon.ERROR,
                    //    Title = "خطا",
                    //    Message = "فرمت فایل انتخاب شده غیر مجاز است."
                    //});
                    //DirectResult result = new DirectResult();
                    //return result;
                }
            }
            catch (Exception x)
            {
                if (Session["savePathReply"] != null)
                {
                    System.IO.File.Delete(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("PasvandReply");
                    Session.Remove("FileNameReply");
                }
                string InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReply(string Data/*string MatnHtml, bool? HaveFile, int SourceId*/)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            Models.RaiSamEntities m = new RaiSamEntities();
            var UserInfo = (UserInfo)(Session["User"]);
            string Msg = "پاسخ با موفقیت ارسال شد.", MsgTitle = ""; var Er = 0;

            var template = new { MatnHtml = "", HaveFile = false, SourceId = "" };
            var MatnHtml = System.Uri.UnescapeDataString(JsonConvert.DeserializeAnonymousType(Data, template).MatnHtml);
            var HaveFile = JsonConvert.DeserializeAnonymousType(Data, template).HaveFile;
            var SourceId = JsonConvert.DeserializeAnonymousType(Data, template).SourceId;

            var s = m.prs_GetDate().FirstOrDefault();
            var d = s.fldTarikh + "-" + s.fldTime.Substring(0, 5);
            string Pasvand = ""; string FileName = "";
            var FileId = 0;
            try
            {
                byte[] report_file = null;
                if (MatnHtml == null)
                    MatnHtml = "";

                if (HaveFile == true)
                {
                    if (Session["savePathReply"] != null)
                    {
                        MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(Session["savePathReply"].ToString()));
                        report_file = stream.ToArray();
                        Pasvand = Session["PasvandReply"].ToString();
                    }
                    else
                    {
                        return Json(new
                        {
                            MsgTitle = "خطا",
                            Msg = "لطفا فایل را مجددا آپلود نمایید.",
                            Er = 1
                        });
                    }
                }
                else if (MatnHtml == "")
                {
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "لطفا متن پاسخ را وارد نمایید.",
                        Er = 1
                    });
                }


                var Ticket = m.prs_tblTicketSelect("fldId", SourceId.ToString(),"",0).FirstOrDefault();

                MsgTitle = "عملیات موفق";

                System.Data.Entity.Core.Objects.ObjectParameter Id = new System.Data.Entity.Core.Objects.ObjectParameter("FileId", typeof(int));


                m.prs_tblTicketInsert(Id,null, MatnHtml, false, report_file, Pasvand, null, "", Ticket.fldTicketCategoryId, Ticket.fldAshkhasId, FileName,
                    UserInfo.UserInputId, null, null, Convert.ToInt32(SourceId));


                FileId = Convert.ToInt32(Id.Value);
             

                if (Session["savePathReply"] != null)
                {
                    string physicalPath = System.IO.Path.Combine(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("FileNameReply");
                    Session.Remove("PasvandReply");
                    System.IO.File.Delete(physicalPath);
                }
            }
            catch (Exception x)
            {
                var InnerException = "";
                if (x.InnerException != null)
                    InnerException = x.InnerException.Message;
                else
                    InnerException = x.Message;
                System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                var Input = m.prs_tblInputInfoSelect("CheckUser", u.UserKey, IP, false, 0).FirstOrDefault();
                m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
                if (Session["savePathReply"] != null)
                {
                    System.IO.File.Delete(Session["savePathReply"].ToString());
                    Session.Remove("savePathReply");
                    Session.Remove("PasvandReply");
                    Session.Remove("FileNameReply");
                }
                return Json(new
                {
                    MsgTitle = "خطا",
                    Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                    Er = 1
                });
            }
            return Json(new
            {
                Msg = Msg,
                MsgTitle = MsgTitle,
                Err = Er,
                time = d,
                FileId = FileId
            }, JsonRequestBehavior.AllowGet);
        }

    }
}