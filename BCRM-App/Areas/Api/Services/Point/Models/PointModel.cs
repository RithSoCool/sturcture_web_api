namespace BCRM_App.Areas.Api.Services.Point.Models
{
    public class PointModel
    {
        public class EarnPoint_Req
        {

        }

        public class BurnPoint_Req
        {

        }

  

        public class VoidPoint_Req
        {

        }

        public class EarnPoint_Resp
        {

        }

        public class BurnPoint_Resp
        {
            public int PointBalance { get; set; }
        }

        public class VoidPoint_Resp
        {
            public int PointBalance { get; set; }
        }
    }
}
