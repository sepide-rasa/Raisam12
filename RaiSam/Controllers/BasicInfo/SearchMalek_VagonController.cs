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
    public class SearchMalek_VagonController : Controller
    {
        //
        // GET: /SearchMalek_Vagon/
        string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
        public ActionResult Index(int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read(StoreRequestParameters parameters, byte state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                //if (Permission.haveAccess(17, "", "0"))
                //{
                    UserInfo user = (UserInfo)(Session["User"]);
                    Models.RaiSamEntities m = new RaiSamEntities();
                    List<Models.prs_tblMalek_VagonInfoSelect> data = null;
                    if (filterHeaders.Conditions.Count > 0)
                    {
                      
                            string field = "";
                            string searchtext = "";
                            List<prs_tblMalek_VagonInfoSelect> data1 = null;
                            foreach (var item in filterHeaders.Conditions)
                            {
                                var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                                switch (item.FilterProperty.Name)
                                {
                                    case "fldId":
                                        searchtext = ConditionValue.Value.ToString();
                                        field = "fldId";
                                        break;
                                    case "fldNameCompany":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldNameCompany";
                                        break;
                                    case "fldCodeEghtasadi":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldCodeEghtasadi";
                                        break;
                                    case "fldShenaseMeli":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldShenaseMeli";
                                        break;
                                    case "fldTypeSamaneName":
                                        searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                        field = "fldTypeSamaneName";
                                        break;
                                }

                                if (data != null)
                                    data1 = m.prs_tblMalek_VagonInfoSelect(field, searchtext, 100).ToList();
                                else
                                    data = m.prs_tblMalek_VagonInfoSelect(field, searchtext,  100).ToList();
                            }
                            if (data != null && data1 != null)
                                data.Intersect(data1);
                        
                    }
                    else
                    {
                        data = m.prs_tblMalek_VagonInfoSelect("", "", 100).ToList();
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

                    List<prs_tblMalek_VagonInfoSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                    //-- end paging ------------------------------------------------------------

                    return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
        public ActionResult IndexChecked(int state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            var result = new Ext.Net.MVC.PartialViewResult();
            result.ViewBag.state = state;
            return result;
        }
        public ActionResult ReadChecked(StoreRequestParameters parameters, byte state)
        {
            if (Session["User"] == null)
                return RedirectToAction("Logon", "Account", new { area = "" });
            if (Request.IsAjaxRequest() == false)
            {
                return null;
            }
            else
            {
                var filterHeaders = new FilterHeaderConditions(this.Request.Params["filterheader"]);
                //if (Permission.haveAccess(17, "", "0"))
                //{
                UserInfo user = (UserInfo)(Session["User"]);
                Models.RaiSamEntities m = new RaiSamEntities();
                List<Models.prs_tblMalek_VagonInfoSelect> data = null;
                if (filterHeaders.Conditions.Count > 0)
                {

                    string field = "";
                    string searchtext = "";
                    List<prs_tblMalek_VagonInfoSelect> data1 = null;
                    foreach (var item in filterHeaders.Conditions)
                    {
                        var ConditionValue = (Newtonsoft.Json.Linq.JValue)item.ValueProperty.Value;

                        switch (item.FilterProperty.Name)
                        {
                            case "fldId":
                                searchtext = ConditionValue.Value.ToString();
                                field = "fldId";
                                break;
                            case "fldNameCompany":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldNameCompany";
                                break;
                            case "fldCodeEghtasadi":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldCodeEghtasadi";
                                break;
                            case "fldShenaseMeli":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldShenaseMeli";
                                break;
                            case "fldTypeSamaneName":
                                searchtext = "%" + ConditionValue.Value.ToString() + "%";
                                field = "fldTypeSamaneName";
                                break;
                        }

                        if (data != null)
                            data1 = m.prs_tblMalek_VagonInfoSelect(field, searchtext, 0).ToList();
                        else
                            data = m.prs_tblMalek_VagonInfoSelect(field, searchtext, 0).ToList();
                    }
                    if (data != null && data1 != null)
                        data.Intersect(data1);

                }
                else
                {
                    data = m.prs_tblMalek_VagonInfoSelect("", "", 0).ToList();
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

                List<prs_tblMalek_VagonInfoSelect> rangeData = (parameters.Start < 0 || limit < 0) ? data : data.GetRange(parameters.Start, limit);
                //-- end paging ------------------------------------------------------------

                return this.Store(rangeData, data.Count);
                //}
                //else
                //{
                //    return RedirectToAction("Error", "Home", new { Code = "403" });
                //}
            }
        }
    }
}
