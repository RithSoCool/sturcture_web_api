using BCRM_App.Configs;
using System;
using static BCRM.IAM.Constants.BCRM_IAM_Const.Permission.Lookup;

namespace BCRM_App.Constants
{
    public static class AppConstants
    {
        public static class Authentication
        {
            public static class Line
            {
                public static class Status
                {
                    public const int Line_Request = 1;
                    public const int Line_Callback = 2;
                    public const int Logined = 10;

                    public const int Logout = 40;
                    public const int Failed = 41;
                }
            }

            public static class Scope
            {
                public const string BCRM_API = "bcrm-api";
                public const string ThirdPartyApi = "third-party-api";
            }
        }

        public class Privilege
        {
            public class Status
            {
                public const int Active = 1;
                public const int InActive = 2;
                public const int Expired = 3;
                public const int Used = 4;
                public const string ActiveDesc = "ACT";
                public const string InActiveDesc = "IACT";
                public const string UsedDesc = "Used";
                public const string ExpiredDesc = "Exp";

                public static string GetDesc(int type)
                {
                    switch (type)
                    {
                        case Status.Active: return ActiveDesc;
                        case Status.InActive: return InActiveDesc;
                        case Status.Used: return UsedDesc;
                        case Status.Expired: return ExpiredDesc;
                        default: return "none";
                    }
                }
            }

            public class RewardRedemptionHistory
            {
                public class ActionType
                {
                    public const int All = 0;
                    public const int CouponCode = 1;
                    public const int PhysicalReward = 2;
                    public const int CouponCodeExpired = 3;
                }
            }

            public static partial class Fulfillment
            {
                public static class Status
                {
                    public const int Not_Specified = 0;
                    public const int Pending = 1;
                    public const int Preparing = 2;
                    public const int Packed = 5;
                    public const int Shipped = 7;
                    public const int Received = 9;

                    public class Desc
                    {
                        public const String Not_Specified = "จัดเตรียม";
                        public const String Pending = "จัดเตรียม";
                        public const String Preparing = "จัดเตรียม";
                        public const String Packed = "จัดเตรียม";
                        public const String Shipped = "อยู่ระหว่างการจัดส่ง";
                        public const String Received = "ส่งสำเร็จ";
                    }

                    public static String Get_Desc(int Status)
                    {
                        switch (Status)
                        {
                            case Fulfillment.Status.Not_Specified:
                                return Fulfillment.Status.Desc.Not_Specified;
                            case Fulfillment.Status.Pending:
                                return Fulfillment.Status.Desc.Pending;
                            case Fulfillment.Status.Preparing:
                                return Fulfillment.Status.Desc.Preparing;
                            case Fulfillment.Status.Packed:
                                return Fulfillment.Status.Desc.Packed;
                            case Fulfillment.Status.Shipped:
                                return Fulfillment.Status.Desc.Shipped;
                            case Fulfillment.Status.Received:
                                return Fulfillment.Status.Desc.Received;
                        }
                        return String.Empty;
                    }
                }
            }
        }

        public static class Database
        {
            public static class ConnectionString
            {
                public static string Dutchmill = "";
            }
        }

        public class Blob
        {
            public class Path
            {
                public static string CustomerProfile_Fulll => "bcrm-36-BR3U7352RHLM/customer-files/images/profiles";
                public static string CustomerProfile => "images/profiles";
            }
        }

        public static class Customer
        {
            public static class Role
            {
                public static class Filter
                {
                    public const string CustomerOnly = "customerOnly";
                }
            }

            public static class Address
            {

                public static class Status
                {
                    public const int Active = 1;
                    public const int InActive = 2;
                }

                public static class Type
                {
                    public const int Address = 1;
                    public const int ShippingAddress = 9;
                    public const string ShippingAddressLabel = "Shipping Address";
                    public const string AddressLabel = "Address";

                    public static string GetDesc(int type)
                    {
                        switch (type)
                        {
                            case Address:
                                return AddressLabel;
                            default:
                                return ShippingAddressLabel;
                        }
                    }
                }
            }

            public static class PointHistory
            {
                public class Type
                {
                    public const int EarnPoint = 1;
                    public const int BurnPoint = 2;

                    public const string EarnPointDesc = "Earn Point";
                    public const string BurnPointDesc = "Burn Point";


                    public static string GetStatusDesc(int statusType)
                    {
                        switch (statusType)
                        {
                            case EarnPoint: return EarnPointDesc;
                            case BurnPoint: return BurnPointDesc;
                            default:
                                return "None";
                        }
                    }
                }

            }
        }

        public static class Token
        {
            public static class Scope
            {
                public const string Register = "bcrm-register";
                public const string OTP = "bcrm-otp";
                public const string API = "bcrm-api";
            }
        }

        public static class RouteData
        {
            public const string IdentityId = BCRM.Common.Constants.BCRM_Core_Const.Api.RouteData.Key.Api_Token_IdentityId;
            public const string Identity_SRef = BCRM.Common.Constants.BCRM_Core_Const.Api.RouteData.Key.Api_Token_Identity_SRef;
            public const string CustomerId = "customerId";
            public const string Customer_Ref = "customer_ref";
            public const string Customer_MobileNo = "customer_mobileno";
            public const string Scope = "scope";
            public const string TokenId = "tokenid";
            public const string Member = "member";

            public class Line
            {
                public const string LineId = "bcrm_api_lineid";
                public const string Linename = "bcrm_api_linename";
                public const string ChannelId = "bcrm_api_channelid";
                public const string PictureUrl = "bcrm_api_pictureurl";
            }

            public class Key
            {
                public const string PayloadType = "bcrm_api_token_payload_type";
                public const string Payload = "bcrm_api_token_payload";
                public const string Scope = "bcrm_api_claim_scope";
            }
        }
    }
}
