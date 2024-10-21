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

namespace RaiSam.Controllers.Transaction
{
    public class EventTablesController : Controller
    {
        //
        // GET: /EventTables/
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetNametable()
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
                
                var q = m.prs_tblNameTablesSelect("","",0).ToList().Select(l => new { fldId = l.fldId, fldName = l.fldFaName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEventType()
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

                var q = m.prs_tblEventTypeSelect("", "", 0).ToList().Select(l => new { fldId = l.fldId, fldName = l.fldName });
                return this.Store(q);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Models.prs_tblEventTablesSelect EventTables)
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
                string Msg = "", MsgTitle = ""; var Er = 0;
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {

                    /* if (EventTables.fldDesc == null)
                         EventTables.fldDesc = "";*/
                    // EventTables.fldUserId = user.UserId;
                    // EventTables.fldInputId = user.UserInputId;
                    if (EventTables.fldId == 0)
                    {
                        //ذخیره
                        if (Permission.haveAccess(28, "tblEventTables", null))
                        {
                            m.prs_tblEventTablesInsert(EventTables.fldNameTablesId, EventTables.fldEventTypeId, EventTables.fldFlag);

                            Msg = "ذخیره با موفقیت انجام شد";
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
                        //ویرایش
                        if (Permission.haveAccess(35, "tblEventTables", EventTables.fldId.ToString()))
                        {
                            m.prs_tblEventTablesUpdate(EventTables.fldId, EventTables.fldNameTablesId, EventTables.fldEventTypeId, EventTables.fldFlag);

                            Msg = "ویرایش با موفقیت انجام شد";
                            MsgTitle = "ویرایش موفق";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                string Msg = "", MsgTitle = ""; byte Er = 0;
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                try
                {//حذف
                    if (Permission.haveAccess(36, "tblEventTables", id.ToString()))
                    {
                        m.prs_tblEventTablesDelete(id);

                        Msg = "حدف با موفقیت انجام شد";
                        MsgTitle = "حذف موفق";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int Id)
        {
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                
                var q = m.prs_tblEventTablesSelect("fldId", Id.ToString(),1).FirstOrDefault();
                var flag = "0";
                if (q.fldFlag)
                    flag = "1";
                return Json(new
                {
                    fldId = q.fldId,
                    fldNameTablesId = q.fldNameTablesId,
                    fldEventTypeId = q.fldEventTypeId,
                    fldFlag = flag/*,
                fldDesc = q.fldDesc*/
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            Models.RaiSamEntities m = new RaiSamEntities();
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                if (Permission.haveAccess(33,"","0"))
                {
                    var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                    UserInfo user = (UserInfo)(Session["User"]);
                    List<prs_tblEventTablesSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                        string field = "";
                        string searchtext = "";
                        List<prs_tblEventTablesSelect> data1 = null;
                        foreach (var item in filterHeaders.Conditions)
                        {
                            var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                            switch (item.FilterProperty.Name)
                            {
                                case "fldId":
                                    searchtext = ConditionValue.Value.ToString();
                                    field = "fldId";
                                    break;
                                case "fldFaName":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldFaName";
                                    break;
                                case "fldNameType":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameType";
                                    break;
                                case "fldNameFlag":
                                    searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                    field = "fldNameFlag";
                                    break;

                            }
                            
                            if (data != null)
                                data1 = m.prs_tblEventTablesSelect(field, searchtext,100).ToList();
                            else
                                data = m.prs_tblEventTablesSelect(field, searchtext, 100).ToList();
                        }
                        if (data != null && data1 != null)
                            data.Intersect(data1);
                    }
                    else
                    {
                       
                        data = m.prs_tblEventTablesSelect("","",100).ToList();
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

                    List<prs_tblEventTablesSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Code = "403" });
                }
            }
        }
    }
}