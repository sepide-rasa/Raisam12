﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaisamWindowsService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class RaiSamEntities : DbContext
    {
        public RaiSamEntities()
            : base("name=RaiSamEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int prs_MosaferiInfoInsert(string uicWagonNo, Nullable<int> trainNumber, string trainDate, string moveTime, string persianTrainDate, Nullable<int> wagonNumber, Nullable<int> compartmentNumber, Nullable<int> seatNumber, Nullable<int> ticketNumber, Nullable<decimal> seirStartStationCode, string seirStartStation, Nullable<decimal> seirEndStationCode, string seirEndStation, string nameCompany, Nullable<decimal> totalDistance)
        {
            var uicWagonNoParameter = uicWagonNo != null ?
                new ObjectParameter("uicWagonNo", uicWagonNo) :
                new ObjectParameter("uicWagonNo", typeof(string));
    
            var trainNumberParameter = trainNumber.HasValue ?
                new ObjectParameter("trainNumber", trainNumber) :
                new ObjectParameter("trainNumber", typeof(int));
    
            var trainDateParameter = trainDate != null ?
                new ObjectParameter("trainDate", trainDate) :
                new ObjectParameter("trainDate", typeof(string));
    
            var moveTimeParameter = moveTime != null ?
                new ObjectParameter("moveTime", moveTime) :
                new ObjectParameter("moveTime", typeof(string));
    
            var persianTrainDateParameter = persianTrainDate != null ?
                new ObjectParameter("persianTrainDate", persianTrainDate) :
                new ObjectParameter("persianTrainDate", typeof(string));
    
            var wagonNumberParameter = wagonNumber.HasValue ?
                new ObjectParameter("wagonNumber", wagonNumber) :
                new ObjectParameter("wagonNumber", typeof(int));
    
            var compartmentNumberParameter = compartmentNumber.HasValue ?
                new ObjectParameter("compartmentNumber", compartmentNumber) :
                new ObjectParameter("compartmentNumber", typeof(int));
    
            var seatNumberParameter = seatNumber.HasValue ?
                new ObjectParameter("seatNumber", seatNumber) :
                new ObjectParameter("seatNumber", typeof(int));
    
            var ticketNumberParameter = ticketNumber.HasValue ?
                new ObjectParameter("ticketNumber", ticketNumber) :
                new ObjectParameter("ticketNumber", typeof(int));
    
            var seirStartStationCodeParameter = seirStartStationCode.HasValue ?
                new ObjectParameter("seirStartStationCode", seirStartStationCode) :
                new ObjectParameter("seirStartStationCode", typeof(decimal));
    
            var seirStartStationParameter = seirStartStation != null ?
                new ObjectParameter("seirStartStation", seirStartStation) :
                new ObjectParameter("seirStartStation", typeof(string));
    
            var seirEndStationCodeParameter = seirEndStationCode.HasValue ?
                new ObjectParameter("seirEndStationCode", seirEndStationCode) :
                new ObjectParameter("seirEndStationCode", typeof(decimal));
    
            var seirEndStationParameter = seirEndStation != null ?
                new ObjectParameter("seirEndStation", seirEndStation) :
                new ObjectParameter("seirEndStation", typeof(string));
    
            var nameCompanyParameter = nameCompany != null ?
                new ObjectParameter("nameCompany", nameCompany) :
                new ObjectParameter("nameCompany", typeof(string));
    
            var totalDistanceParameter = totalDistance.HasValue ?
                new ObjectParameter("totalDistance", totalDistance) :
                new ObjectParameter("totalDistance", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prs_MosaferiInfoInsert", uicWagonNoParameter, trainNumberParameter, trainDateParameter, moveTimeParameter, persianTrainDateParameter, wagonNumberParameter, compartmentNumberParameter, seatNumberParameter, ticketNumberParameter, seirStartStationCodeParameter, seirStartStationParameter, seirEndStationCodeParameter, seirEndStationParameter, nameCompanyParameter, totalDistanceParameter);
        }
    
        public virtual int prs_tblErrorInsert(ObjectParameter fldID, string fldMatn, Nullable<System.DateTime> fldTarikh, Nullable<int> fldInputID, string fldDesc)
        {
            var fldMatnParameter = fldMatn != null ?
                new ObjectParameter("fldMatn", fldMatn) :
                new ObjectParameter("fldMatn", typeof(string));
    
            var fldTarikhParameter = fldTarikh.HasValue ?
                new ObjectParameter("fldTarikh", fldTarikh) :
                new ObjectParameter("fldTarikh", typeof(System.DateTime));
    
            var fldInputIDParameter = fldInputID.HasValue ?
                new ObjectParameter("fldInputID", fldInputID) :
                new ObjectParameter("fldInputID", typeof(int));
    
            var fldDescParameter = fldDesc != null ?
                new ObjectParameter("fldDesc", fldDesc) :
                new ObjectParameter("fldDesc", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prs_tblErrorInsert", fldID, fldMatnParameter, fldTarikhParameter, fldInputIDParameter, fldDescParameter);
        }
    
        public virtual ObjectResult<prs_GetDate> prs_GetDate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prs_GetDate>("prs_GetDate");
        }
    
        public virtual ObjectResult<string> prs_InsertFromWebServiceSajam(Nullable<int> trainNum, string moveDateTrain, string moveTimeTrain, Nullable<int> pelakNo, Nullable<int> salonNo, string startSation, string endStation)
        {
            var trainNumParameter = trainNum.HasValue ?
                new ObjectParameter("TrainNum", trainNum) :
                new ObjectParameter("TrainNum", typeof(int));
    
            var moveDateTrainParameter = moveDateTrain != null ?
                new ObjectParameter("MoveDateTrain", moveDateTrain) :
                new ObjectParameter("MoveDateTrain", typeof(string));
    
            var moveTimeTrainParameter = moveTimeTrain != null ?
                new ObjectParameter("MoveTimeTrain", moveTimeTrain) :
                new ObjectParameter("MoveTimeTrain", typeof(string));
    
            var pelakNoParameter = pelakNo.HasValue ?
                new ObjectParameter("PelakNo", pelakNo) :
                new ObjectParameter("PelakNo", typeof(int));
    
            var salonNoParameter = salonNo.HasValue ?
                new ObjectParameter("SalonNo", salonNo) :
                new ObjectParameter("SalonNo", typeof(int));
    
            var startSationParameter = startSation != null ?
                new ObjectParameter("StartSation", startSation) :
                new ObjectParameter("StartSation", typeof(string));
    
            var endStationParameter = endStation != null ?
                new ObjectParameter("EndStation", endStation) :
                new ObjectParameter("EndStation", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("prs_InsertFromWebServiceSajam", trainNumParameter, moveDateTrainParameter, moveTimeTrainParameter, pelakNoParameter, salonNoParameter, startSationParameter, endStationParameter);
        }
    
        public virtual ObjectResult<prs_tblSMSSettingSelect> prs_tblSMSSettingSelect(string fieldname, string value, Nullable<int> h)
        {
            var fieldnameParameter = fieldname != null ?
                new ObjectParameter("fieldname", fieldname) :
                new ObjectParameter("fieldname", typeof(string));
    
            var valueParameter = value != null ?
                new ObjectParameter("Value", value) :
                new ObjectParameter("Value", typeof(string));
    
            var hParameter = h.HasValue ?
                new ObjectParameter("h", h) :
                new ObjectParameter("h", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prs_tblSMSSettingSelect>("prs_tblSMSSettingSelect", fieldnameParameter, valueParameter, hParameter);
        }
    
        public virtual int prs_tblSafSMSInsert(string fldMatn, Nullable<byte> fldStatus, Nullable<int> fldAshkhasId, Nullable<int> fldCherkheFirstEghdamId, Nullable<int> fldInputId)
        {
            var fldMatnParameter = fldMatn != null ?
                new ObjectParameter("fldMatn", fldMatn) :
                new ObjectParameter("fldMatn", typeof(string));
    
            var fldStatusParameter = fldStatus.HasValue ?
                new ObjectParameter("fldStatus", fldStatus) :
                new ObjectParameter("fldStatus", typeof(byte));
    
            var fldAshkhasIdParameter = fldAshkhasId.HasValue ?
                new ObjectParameter("fldAshkhasId", fldAshkhasId) :
                new ObjectParameter("fldAshkhasId", typeof(int));
    
            var fldCherkheFirstEghdamIdParameter = fldCherkheFirstEghdamId.HasValue ?
                new ObjectParameter("fldCherkheFirstEghdamId", fldCherkheFirstEghdamId) :
                new ObjectParameter("fldCherkheFirstEghdamId", typeof(int));
    
            var fldInputIdParameter = fldInputId.HasValue ?
                new ObjectParameter("fldInputId", fldInputId) :
                new ObjectParameter("fldInputId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prs_tblSafSMSInsert", fldMatnParameter, fldStatusParameter, fldAshkhasIdParameter, fldCherkheFirstEghdamIdParameter, fldInputIdParameter);
        }
    
        public virtual ObjectResult<prs_tblSafSMSSelect> prs_tblSafSMSSelect(string fieldname, string value, Nullable<int> h)
        {
            var fieldnameParameter = fieldname != null ?
                new ObjectParameter("fieldname", fieldname) :
                new ObjectParameter("fieldname", typeof(string));
    
            var valueParameter = value != null ?
                new ObjectParameter("value", value) :
                new ObjectParameter("value", typeof(string));
    
            var hParameter = h.HasValue ?
                new ObjectParameter("h", h) :
                new ObjectParameter("h", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<prs_tblSafSMSSelect>("prs_tblSafSMSSelect", fieldnameParameter, valueParameter, hParameter);
        }
    
        public virtual int prs_tblSafSMSUpdate(Nullable<byte> fldStatus, Nullable<int> fldAshkhasId, Nullable<int> fldCherkheFirstEghdamId, Nullable<int> fldInputId)
        {
            var fldStatusParameter = fldStatus.HasValue ?
                new ObjectParameter("fldStatus", fldStatus) :
                new ObjectParameter("fldStatus", typeof(byte));
    
            var fldAshkhasIdParameter = fldAshkhasId.HasValue ?
                new ObjectParameter("fldAshkhasId", fldAshkhasId) :
                new ObjectParameter("fldAshkhasId", typeof(int));
    
            var fldCherkheFirstEghdamIdParameter = fldCherkheFirstEghdamId.HasValue ?
                new ObjectParameter("fldCherkheFirstEghdamId", fldCherkheFirstEghdamId) :
                new ObjectParameter("fldCherkheFirstEghdamId", typeof(int));
    
            var fldInputIdParameter = fldInputId.HasValue ?
                new ObjectParameter("fldInputId", fldInputId) :
                new ObjectParameter("fldInputId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("prs_tblSafSMSUpdate", fldStatusParameter, fldAshkhasIdParameter, fldCherkheFirstEghdamIdParameter, fldInputIdParameter);
        }
    }
}
