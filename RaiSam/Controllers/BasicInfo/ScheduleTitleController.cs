﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using Ext.Net.Utilities;
using RaiSam.Models;
using System.IO;
using Newtonsoft.Json;

namespace RaiSam.Controllers.BasicInfo
{
    public class ScheduleTitleController : Controller
    {
        //
        // GET: /ScheduleTitle/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];

        public ActionResult Index(string containerId)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult
            {
                WrapByScriptTag = true,
                ContainerId = containerId,
                RenderMode = RenderMode.AddTo
            };

            this.GetCmp<TabPanel>(containerId).SetLastTabAsActive();
            return result;
        }
        public ActionResult New(int id)
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            else
            {
                Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
                PartialView.ViewBag.Id = id;
                return PartialView;
            }
        }
        public ActionResult Help()
        {//باز شدن پنجره
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult ShowHelpScheduleTitle()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "43", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFilePdfId.ToString(), 1).FirstOrDefault();
            return File((byte[])q.fldFile, "application/pdf");
        }
        public ActionResult VideoWinScheduleTitle()
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Ext.Net.MVC.PartialViewResult PartialView = new Ext.Net.MVC.PartialViewResult();
            return PartialView;
        }
        public FileContentResult DownloadFileVideoScheduleTitle()
        {
            if (Session["User"] == null)
                return null;
            UserInfo user = (UserInfo)(Session["User"]);
            Models.RaiSamEntities m = new RaiSamEntities();
            var Help = m.prs_tblHelpSelect("fldId", "43", 1).FirstOrDefault();
            var q = m.prs_tblFileSelect("fldId", Help.fldFileVideoId.ToString(), 1).FirstOrDefault();

            MemoryStream st = new MemoryStream(q.fldFile);
            return File(st.ToArray(), MimeType.Get(q.fldPasvand), q.fldFileName + q.fldPasvand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                var q = m.prs_tblScheduleTitleSelect("fldId", Id.ToString(), 0).FirstOrDefault();
                return Json(new
                {
                    fldId = q.fldId,
                    fldTitle = q.fldTitle,
                    fldTimeStamp = q.fldTimeStamp,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int TimeStamp)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; int Er = 0; var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();


                string jsonstr = "", title = "";
                title = m.prs_tblScheduleTitleSelect("fldId", id.ToString(), 0).FirstOrDefault().fldTitle;
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("عنوان", title);
                try
                {
                    if (Permission.haveAccess(237, "tblScheduleTitle", id.ToString()))
                    {
                        var q = m.prs_tblScheduleTitleDelete(id, TimeStamp).FirstOrDefault();

                        if (q.flag == 1)
                        {
                            Msg = "حذف با موفقیت انجام شد";
                            MsgTitle = "حذف موفق";
                        }
                        else if (q.flag == 0)
                        {
                            Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                            MsgTitle = "هشدار";
                            Er = 1;
                            Change = 1;
                        }
                        else if (q.flag == 2)
                        {
                            Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان دوباره حذف وجود ندارد.";
                            MsgTitle = "خطا";
                            Er = 1;
                            Change = 2;
                        }

                        if (Er == 1)
                        {
                            parameters.Add("متن خطا", "ScheduleTitle:Delete: " + Msg);
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, false, id);
                        }

                        else
                        {
                            jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_LogDelete(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, true, id);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Msg = "شما مجاز به دسترسی نمی باشید.",
                            MsgTitle = "خطا",
                            Er = 1
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ScheduleTitle:Delete: " + ErMsg);
                    jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);

                    m.prs_LogDelete(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, false, id);
                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(prs_tblScheduleTitleSelect Group)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; var Er = 0;
                var Change = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {
                    if (Group.fldId == 0)
                    {
                        if (Permission.haveAccess(235, "tblScheduleTitle", null))
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();

                            parameters.Add("عنوان", Group.fldTitle);
                            string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                            m.prs_tblScheduleTitleInsert(Group.fldTitle, user.UserInputId, jsonstr);

                            Msg = "ذخیره با موفقیت انجام شد.";
                            MsgTitle = "دخیره موفق";
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (Permission.haveAccess(236, "tblScheduleTitle", Group.fldId.ToString()))
                        {
                            var q = m.prs_tblScheduleTitleUpdate(Group.fldId, Group.fldTitle, user.UserInputId, Group.fldTimeStamp).FirstOrDefault();

                            if (q.flag == 1)
                            {
                                Msg = "ویرایش با موفقیت انجام شد";
                                MsgTitle = "ویرایش موفق";
                            }
                            else if (q.flag == 0)
                            {
                                Msg = "مورد انتخاب شده قبلا تغییر کرده است لطفا منتظر بارگذاری جدید باشید";
                                MsgTitle = "هشدار";
                                Er = 1;
                                Change = 1;
                            }
                            else if (q.flag == 2)
                            {
                                Msg = "مورد انتخاب شده قبلا حذف گردیده است لذا امکان تغییر وجود ندارد.";
                                MsgTitle = "خطا";
                                Er = 1;
                                Change = 2;
                            }

                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            parameters.Add("عنوان", Group.fldTitle);

                            if (Er == 1)
                            {
                                parameters.Add("متن خطا", "ScheduleTitle:Save: " + Msg);
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, false, Group.fldId);
                            }

                            else
                            {
                                string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                                m.prs_LogUpdate(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, true, Group.fldId);
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                Msg = "شما مجاز به دسترسی نمی باشید.",
                                MsgTitle = "خطا",
                                Er = 1
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }


                }
                catch (Exception x)
                {
                    var ErMsg = "";
                    if (x.InnerException != null)
                        ErMsg = x.InnerException.Message;
                    else
                        ErMsg = x.Message;
                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, ErMsg, DateTime.Now, Input.fldId, "");

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("عنوان", Group.fldTitle);
                    parameters.Add("کد خطا", ErrorId.Value);
                    parameters.Add("متن خطا", "ScheduleTitle:Save: " + ErMsg);
                    string jsonstr = Newtonsoft.Json.JsonConvert.SerializeObject(parameters, Newtonsoft.Json.Formatting.Indented);
                    if (Group.fldId == 0)
                    {
                        m.prs_LogInsert(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, false);
                    }
                    else
                    {
                        m.prs_LogUpdate(user.UserInputId, "dbo.tblScheduleTitle", jsonstr, false, Group.fldId);
                    }

                    return Json(new
                    {
                        MsgTitle = "خطا",
                        Msg = "خطایی با شماره: " + ErrorId.Value + " رخ داده است لطفا با پشتیبانی تماس گرفته و کد خطا را اعلام فرمایید.",
                        Er = 1,
                        Change = Change
                    });
                }
                return Json(new
                {
                    Msg = Msg,
                    MsgTitle = MsgTitle,
                    Er = Er,
                    Change = Change
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(234, "", "0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<prs_tblScheduleTitleSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblScheduleTitleSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldTitle":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldTitle";
                                    break;

                            }
                            if (data != null)
                                data1 = m.prs_tblScheduleTitleSelect(field, searchtext, 100).ToList();
                            else
                                data = m.prs_tblScheduleTitleSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                        data = m.prs_tblScheduleTitleSelect("", "", 100).ToList();
                    }

                    var fc = new FilterHeaderConditions(this.Request.Params["filterheader"]);

                    //FilterConditions fc = parameters.GridFilters;

                    //-- start filtering ------------------------------------------------------------
                    if (fc != null)
                    {
                        foreach (var condition in fc.Conditions)
                        {
                            string field = condition.FilterProperty.Name;
                            var value = (Newtonsoft.Json.Linq.JValue)condition.ValueProperty.Value;

                            data.RemoveAll(
                                item =>
                                {
                                    object oValue = item.GetType().GetProperty(field).GetValue(item, null);
                                    return !(oValue.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0);
                                }
                            );
                        }
                    }
                    //-- end filtering ------------------------------------------------------------

                    //-- start paging ------------------------------------------------------------
                    int limit = parameters.Limit;

                    if ((parameters.Start + parameters.Limit) > data.Count)
                    {
                        limit = data.Count - parameters.Start;
                    }

                    List<prs_tblScheduleTitleSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
        public ActionResult SaveOrder(string Order)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                string Msg = "", MsgTitle = ""; var Er = 0;
                try
                {
                    List<OBJ_UpdateOrder> Order1 = JsonConvert.DeserializeObject<List<OBJ_UpdateOrder>>(Order);
                    foreach (var item in Order1)
                    {

                        m.prs_UpdateOrder("ScheduleTitle", item.fldId, item.order);

                    }
                    Msg = "ویرایش با موفقیت انجام شد.";
                    MsgTitle = "ویرایش موفق";
                }
                catch (Exception x)
                {
                    string InnerException = "";
                    if (x.InnerException != null)
                        InnerException = x.InnerException.Message;
                    else
                        InnerException = x.Message;

                    System.Data.Entity.Core.Objects.ObjectParameter ErrorId = new System.Data.Entity.Core.Objects.ObjectParameter("fldID", typeof(int));
                    var Input = m.prs_tblInputInfoSelect("CheckUser", user.UserKey, IP, false, 0).FirstOrDefault();
                    m.prs_tblErrorInsert(ErrorId, InnerException, DateTime.Now, Input.fldId, "");
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
                    Er = Er
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
