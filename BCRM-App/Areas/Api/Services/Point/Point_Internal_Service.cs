using BCRM_App.Areas.Api.Services.Point.Models;
using System;
using static BCRM_App.Areas.Api.Services.Point.Models.PointModel;

namespace BCRM_App.Areas.Api.Services.Point
{
    public interface IPointService
    {
        public EarnPoint_Resp EarnPoint(EarnPoint_Req earnPointInfo, int customerId, int? brandId = null, string brand = null);
        public BurnPoint_Resp BurnPoint(BurnPoint_Req burnPointInfo, int customerId, int? brandId = null, string brand = null);
        public VoidPoint_Resp VoidPoint(VoidPoint_Req voidPointInfo, int customerId, int? brandId = null, string brand = null);
    }

    // ยังไม่ได้ Implement
    public class Point_Internal_Service : Service_Base, IPointService
    {
        public Point_Internal_Service()
        {
            TxTimeStamp = DateTime.Now;
        }

        public BurnPoint_Resp BurnPoint(BurnPoint_Req burnPointInfo, int customerId, int? brandId = null, string brand = null)
        {
            throw new System.NotImplementedException();
        }

        public EarnPoint_Resp EarnPoint(EarnPoint_Req earnPointInfo, int customerId, int? brandId = null, string brand = null)
        {
            throw new System.NotImplementedException();
        }

        public VoidPoint_Resp VoidPoint(VoidPoint_Req voidPointInfo, int customerId, int? brandId = null, string brand = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
