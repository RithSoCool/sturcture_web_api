using AutoMapper;
using common = BCRM.Common.Models.DBModel;
//using BCRM.Common.Models.DBModel.Wallet;
using BCRM.Common.Services.CRM.Model;
using BCRM_App.Areas.Api.Services.Customer;
using BCRM_App.Areas.Api.Services.Privilege;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM.Common.Services.Privilege;
using static BCRM_App.Areas.Api.Services.Customer.Customer_Internal_Service;
using BCRM_App.Areas.Api.Services.Customer.Models;
using BCRM_App.Models.DBModels.Duchmill;

namespace BCRM_App.Models.MapperConfig
{
    public class ApiModelConfig: Profile
    {
        public ApiModelConfig()
        {
            #region Customer
            CreateMap<RegisterModel, DuchmillModel.BCRM_Customer>().ReverseMap();

            CreateMap<RegisterModel, AddressModel>().ReverseMap();

            CreateMap<AddAddressModel, AddressModel>().ReverseMap();

            CreateMap<EditProfileReq, AddressModel>().ReverseMap();

            CreateMap<BCRM_Dutchmill_Member, MemberFromLastCampaign>()
                    .ForMember(_out => _out.First_Name_Th, _in => _in
                        .MapFrom(it => it.FirstName))
                    .ForMember(_out => _out.Last_Name_Th, _in => _in
                        .MapFrom(it => it.LastName))
                    .ForMember(_out => _out.GenderOld, _in => _in
                        .MapFrom(it => it.Gender))
                    .ForMember(_out => _out.DateOfBirth, _in => _in
                        .MapFrom(it => it.BirthDate))
                    .ForMember(_out => _out.MobileNo, _in => _in
                        .MapFrom(it => it.MobileNo))
                    .ForMember(_out => _out.Address, _in => _in
                        .MapFrom(it => it.Address))
                    .ForMember(_out => _out.Province, _in => _in
                        .MapFrom(it => it.Province))
                    .ForMember(_out => _out.ProvinceId, _in => _in
                        .MapFrom(it => it.ProvinceId))
                    .ForMember(_out => _out.District, _in => _in
                        .MapFrom(it => it.District))
                    .ForMember(_out => _out.DistrictId, _in => _in
                        .MapFrom(it => it.DistrictId))
                    .ForMember(_out => _out.SubDistrict, _in => _in
                        .MapFrom(it => it.SubDistrict))
                    .ForMember(_out => _out.SubDistrictId, _in => _in
                        .MapFrom(it => it.SubDistrictId))
                    .ForMember(_out => _out.PostalCode, _in => _in
                        .MapFrom(it => it.PostalCode))
                .ReverseMap();

            CreateMap<BCRM_Dutchmill_Member_V1, MemberFromLastCampaign>()
                .ReverseMap();

            CreateMap<DuchmillModel.CRM_Customer_Address, CRM_Customer_Address_Resp>()
                 .ForMember(_out => _out.AddressTitle, _in => _in
                    .MapFrom(it => it.Title))
                 .ReverseMap();

            CreateMap<DuchmillModel.CRM_Customer_Address, BCRM.Common.Models.DBModel.CRM.CRM_Customer_Address>().ReverseMap();
            CreateMap<RegisterModel, CRM_Customer_DMG_Info>().ReverseMap();
            #endregion

            #region Wallet
            CreateMap<DuchmillModel.WL_Wallet, common.Wallet.WL_Wallet>().ReverseMap();
            #endregion

            #region Privilege
            CreateMap<DuchmillModel.CRM_Privilege, CRM_Privilege_Resp>().ReverseMap();
            CreateMap<common.Privilege.CRM_Privilege, CRM_Privilege_Resp>().ReverseMap();
            CreateMap<common.Privilege.CRM_Privilege, DuchmillModel.CRM_Privilege>().ReverseMap();

            CreateMap<GetPrivilegies_Resp, GetPrivilegiesWithTier_Resp>()
                .ForMember(_out => _out.Total_Record, _in => _in
                  .MapFrom(it => it.Privilegies_Resp.Total_Record))
                .ForMember(_out => _out.Filtered_Record, _in => _in
                  .MapFrom(it => it.Privilegies_Resp.Filtered_Record))
                .ForMember(_out => _out.Total_Page, _in => _in
                  .MapFrom(it => it.Privilegies_Resp.Total_Page))
                .ForMember(_out => _out.Current_Page, _in => _in
                  .MapFrom(it => it.Privilegies_Resp.Current_Page))
                .ForMember(_out => _out.Privileges, _in => _in
                  .MapFrom(it => it.Privilegies_Resp.Privileges))
                .ReverseMap();

            CreateMap<GetPrivilegeDetails_Resp, GetPrivilegiesDetailsWithTier_Resp>()
                .ReverseMap();

            CreateMap<GetPrivilegeFullDetails_Resp, GetPrivilegiesDetailsWithTier_Resp>()
                .ReverseMap();
            #endregion
        }
    }
}
