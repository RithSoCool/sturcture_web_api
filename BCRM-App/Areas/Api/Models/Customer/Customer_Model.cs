using BCRM_App.Models.DBModels.Duchmill;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCRM_App.Areas.Api.Models
{
    public class Req_Customer_Checkpoint
    {
        [Required]
        public string UserId { get; set; } // IdentitySRef
    }

    public class Req_Customer_CheckUserByMobile
    {
        [Required]
        public string MobileNo { get; set; }
    }

    public class Resp_Customer_Checkpoint
    {
        public string UserId;
        public string Name;
        public string LastName;
        public List<Customer_Checkpoint_Brand> Brand;
    }

    public class Customer_Checkpoint_Brand
    {
        public string BrandName;
        public int PointBalance;
        public string PointUnit;
        public string Tier;
    }

    public class Customer_Information
    {
        public CRM_Customer CRMCustomer;
        public BCRM_Customer BCRMCustomer;
    }

    public class Req_Customer_SubmitTicket
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string BrandName { get; set; }

        [Required]
        public string BranchId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string Reference { get; set; }

        [Required]
        public decimal GrandTotal { get; set; }

        public decimal TicketDiscount { get; set; }

        public string DiscountRef { get; set; }

        public string DiscountType { get; set; }

        public decimal NetTotal { get; set; }

        [Required]
        public decimal CrmTotal { get; set; }

        [Required]
        public string Paymentchannel { get; set; }

        [Required]
        public string DistributionChannel { get; set; }

        [Required]
        public string StoreType { get; set; }

        [Required]
        public List<Customer_EarnPoint_Info> Info { get; set; }
    }

    public class Resp_Customer_SubmitTicket
    {
        public string UserId;
        public string Reference;
        public decimal CrmTotal;
        public string BrandId;
        public string BrandName;
        public string BranchId;
        public string BranchName;
        public int CoinEarnToDay;
        public int CurrentCoin;
        public int DiamondEarnToday;
        public int CurrentDiamond;
        public string TransRef;
    }

    public class Customer_SubmitTicket_Diamond
    {
        public int DiamondEarnToday;
        public int CurrentDiamond;
    }

    public class Req_Customer_VoidTicket
    {
        [Required]
        public string TransRef { get; set; }

        [Required]
        public string BrandId { get; set; }

        public string BrandName { get; set; }

        [Required]
        public string Branchid { get; set; }

        public string BranchName { get; set; }

        [Required]
        public string Reference { get; set; }
    }

    public class Resp_Customer_VoidTicket
    {
        public string TransRef;
        public string Reference;
        public decimal CrmTotal;
        public string Brandid;
        public string BrandName;
        public string Branchid;
        public string BranchName;
        public int VoidCoin;
        public int VoidDiamond;
        public int CurrentCoin;
        public int CurrentDiamond;
    }

    public class Customer_VoidTicket_Diamond
    {
        public int VoidDiamond;
        public int CurrentDiamond;
    }

    public class Customer_EarnPoint_Info
    {
        [Required]
        public string Sku { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal Total { get; set; }

        public string DiscountRef { get; set; }

        public decimal ItemDiscount { get; set; }

        public string DiscountType { get; set; }

        public decimal NetTotal { get; set; }
    }

    public class Message_EarnPoint
    {
        public int MessageId;
        public Req_Customer_SubmitTicket Message;
    }

    public class Req_Customer_ReservePoint
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        public string BrandName { get; set; }

        [Required]
        public string BranchId { get; set; }

        public string BranchName { get; set; }

        [Required]
        public int Point { get; set; }

        [Required]
        public string RedeemRef { get; set; }

        [Required]
        public string Reference { get; set; }
    }

    public class Log_ReservePoint
    {
        public int MessageId;
        public int? RelatedMessageId;
        public string UserId;
        public int Type;
        public string BrandId;
        public string BrandName;
        public string BranchId;
        public string BranchName;
        public string Reference;
        public string RedeemRef;
        public string ReserveRef;
        public int Point;
    }

    public class Message_ReservePoint
    {
        public int MessageId;
        public Req_Customer_ReservePoint Message;
    }

    public class Resp_Customer_ReservePoint
    {
        public string Reserve_Ref;
        public int Balance;
        public string UserId;
        public string BrandId;
        public string BrandName;
        public string BranchId;
        public string BranchName;
        public int Point;
        public string RedeemRef;
        public string Reference;
      
    }

    public class Req_Customer_CancelReservePoint
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string BranchId { get; set; }

        [Required]
        public string Reserve_Ref { get; set; }
    }

    public class Message_CancelReservePoint
    {
        public int MessageId;
        public Req_Customer_CancelReservePoint Message;
    }

    public class Resp_Customer_CancelReservePoint
    {
        public string UserId;
        public string BrandId;
        public string BranchId;
        public int Balance;
    }

    public class Req_Customer_RedeemPoint
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string Reserve_Ref { get; set; }
    }

    public class Resp_Customer_BurnPoint
    {
        public string UserId;
        public string BrandId;
        public int Point;
        public int Balance;
    }

    public class Req_Customer_VoidRedeem
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string Reserve_Ref { get; set; }
    }

    public class Resp_Customer_VoidRedeem
    {
        public string UserId;
        public string BrandId;
        public int VoidPoint;
        public int Balance;
    }
}
