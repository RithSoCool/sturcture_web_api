using BCRM_App.Services.RemoteInternal.Repository;
using System;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Areas.Api.Services.Repository.Customer
{
    public class Line_Repository : Respository_Base<DuchmillModel.BCRM_Line_Info>
    {
        public Line_Repository(DuchmillModel.BCRM_36_Entities dbContext) : base(dbContext)
        {
            TxTimeStamp = DateTime.Now;
        }
    }
}
