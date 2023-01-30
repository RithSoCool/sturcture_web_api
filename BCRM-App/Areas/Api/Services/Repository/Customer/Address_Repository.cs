using System;
using System.Linq;
using BCRM_App.Services.RemoteInternal.Repository;
using DuchmillModel = BCRM_App.Models.DBModels.Duchmill;
using BCRM_App.Constants;
using BCRM_App.Areas.Api.Services.Customer;
using System.Collections.Generic;

namespace BCRM_App.Areas.Api.Services.Repository.Customer
{
    public class Address_Repository : Respository_Base<DuchmillModel.CRM_Customer_Address>
    {
        public Address_Repository(DuchmillModel.BCRM_36_Entities dbContext) : base(dbContext)
        {
            TxTimeStamp = DateTime.Now;
        }

        public DuchmillModel.CRM_Customer_Address SyncAddress(AddressModel _address)
        {
            try
            {
                var _addressInfo = Query(it => it.CRM_CustomerId == _address.CRM_CustomerId && it.AddressId == _address.AddressId);

                DuchmillModel.CRM_Customer_Address addressInfo = _addressInfo.FirstOrDefault();

                if (_address.IsDefault == true)
                {
                    var addressUndefaults = Query(it => it.CRM_CustomerId == _address.CRM_CustomerId && it.Addr_Type == _address.AddressType);

                    if (addressUndefaults != null)
                    {
                        foreach (var addrr in addressUndefaults)
                        {
                            addrr.IsDefault = false;
                            Update(addrr);
                        }
                    }
                }

                DuchmillModel.CRM_Customer_Address address = new DuchmillModel.CRM_Customer_Address()
                {
                    CRM_CustomerId = _address.CRM_CustomerId.Value,
                    ProvinceId = _address.ProvinceId,
                    Province = _address.Province,
                    CreatedTime = DateTime.Now,

                    IsDefault = _address.IsDefault.Value,

                    Addr_Type = AppConstants.Customer.Address.Type.Address,
                    Addr_Type_Desc = AppConstants.Customer.Address.Type.GetDesc(AppConstants.Customer.Address.Type.Address),
                    Status = AppConstants.Customer.Address.Status.Active,
                    Label = AppConstants.Customer.Address.Type.GetDesc(AppConstants.Customer.Address.Type.Address),

                    Title = _address.AddressTitle,

                    First_Name = _address.First_Name_Th,
                    Last_Name = _address.Last_Name_Th,
                    Address = _address.Address,
                    SubDistrict = _address.SubDistrict,
                    SubDistrictId = _address.SubDistrictId,
                    District = _address.District,
                    DistrictId = _address.DistrictId,
                    PostalCode = _address.PostalCode,
                    ContactNo = _address.MobileNo,

                    Addr_Label = $"{_address.First_Name_Th} {_address.Last_Name_Th} {_address.Address} {_address.SubDistrict} {_address.District} {_address.Province}  {_address.PostalCode}",
                };

                if (addressInfo == null)
                {
                    var addressRes = Add(address);
                    return addressRes;
                }
                else
                {
                    var addressInfoUpdated = UpdateData(address, addressInfo);
                    var addressRes = Update(addressInfoUpdated);
                    return addressRes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DuchmillModel.CRM_Customer_Address SyncShippingAddress(AddressModel _shippingAddress)
        {
            try
            {
                var _addressInfo = Query(it => it.CRM_CustomerId == _shippingAddress.CRM_CustomerId && it.AddressId == _shippingAddress.AddressId).ToList();

                DuchmillModel.CRM_Customer_Address addressInfo = _addressInfo.FirstOrDefault();

                if (_shippingAddress.IsDefault == true)
                {
                    var addressUndefaults = Query(it => it.CRM_CustomerId == _shippingAddress.CRM_CustomerId && it.Addr_Type == _shippingAddress.AddressType);

                    if (addressUndefaults != null)
                    {
                        foreach (var addrr in addressUndefaults)
                        {
                            addrr.IsDefault = false;
                            Update(addrr);
                        }
                    }
                }

                DuchmillModel.CRM_Customer_Address shippingAddress = new DuchmillModel.CRM_Customer_Address()
                {
                    CRM_CustomerId = _shippingAddress.CRM_CustomerId.Value,
                    ProvinceId = _shippingAddress.ProvinceId,
                    Province = _shippingAddress.Province,
                    CreatedTime = DateTime.Now,

                    IsDefault = _shippingAddress.IsDefault.Value,

                    Title = _shippingAddress.AddressTitle,

                    Addr_Type = AppConstants.Customer.Address.Type.ShippingAddress,
                    Addr_Type_Desc = AppConstants.Customer.Address.Type.GetDesc(AppConstants.Customer.Address.Type.ShippingAddress),
                    Status = AppConstants.Customer.Address.Status.Active,
                    Label = AppConstants.Customer.Address.Type.GetDesc(AppConstants.Customer.Address.Type.ShippingAddress),
                    ContactNo = _shippingAddress.MobileNo,

                    First_Name = _shippingAddress.First_Name_Th,
                    Last_Name = _shippingAddress.Last_Name_Th,
                    Address = _shippingAddress.Address,
                    SubDistrict = _shippingAddress.SubDistrict,
                    SubDistrictId = _shippingAddress.SubDistrictId,
                    District = _shippingAddress.District,
                    DistrictId = _shippingAddress.DistrictId,
                    PostalCode = _shippingAddress.PostalCode,


                    Addr_Label = $"{_shippingAddress.First_Name_Th} {_shippingAddress.Last_Name_Th} {_shippingAddress.Address} {_shippingAddress.SubDistrict} {_shippingAddress.District} {_shippingAddress.Province}  {_shippingAddress.PostalCode}",
                };

                if (addressInfo == null)
                {
                    var addressRes = Add(shippingAddress);
                    return addressRes;
                }
                else
                {
                    var addressInfoUpdated = UpdateData(shippingAddress, addressInfo);
                    var addressRes = Update(addressInfoUpdated);

                    return addressRes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DuchmillModel.CRM_Customer_Address DeleteAddress(int addressId, int customerId)
        {
            try
            {
                var address = Query(it => it.AddressId == addressId && it.CRM_CustomerId == customerId && it.IsDeleted == false).FirstOrDefault();

                if (address == null) throw new Exception("Address not found.");

                address.IsDeleted = true;
                address.UpdatedTime = TxTimeStamp;
                Update(address);

                if (address.IsDefault)
                {
                    var nextAddress = Query(it => it.CRM_CustomerId == customerId && it.Addr_Type == address.Addr_Type && it.IsDeleted == false).FirstOrDefault();
                    if (nextAddress == null) 
                    {
                        address.IsDeleted = false;
                        address.UpdatedTime = TxTimeStamp;
                        Update(address);
                        throw new Exception("Default address can't delete");
                    }

                    nextAddress.IsDefault = true;
                    address.UpdatedTime = TxTimeStamp;
                    Update(address);
                }

                return address;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DuchmillModel.CRM_Customer_Address UpdateData(DuchmillModel.CRM_Customer_Address newAddress, DuchmillModel.CRM_Customer_Address address)
        {
            address.CRM_CustomerId = newAddress.CRM_CustomerId;
            address.ProvinceId = newAddress.ProvinceId;
            address.Province = newAddress.Province;
            address.UpdatedTime = DateTime.Now;

            address.Title = newAddress.Title;

            address.IsDefault = newAddress.IsDefault;
            address.First_Name = newAddress.First_Name;
            address.Last_Name = newAddress.Last_Name;
            address.Address = newAddress.Address;
            address.SubDistrict = newAddress.SubDistrict;
            address.SubDistrictId = newAddress.SubDistrictId;
            address.District = newAddress.District;
            address.DistrictId = newAddress.DistrictId;
            address.PostalCode = newAddress.PostalCode;

            if (!string.IsNullOrEmpty(newAddress.ContactNo)) address.ContactNo = newAddress.ContactNo;

            address.Addr_Label = $"{newAddress.First_Name} {newAddress.Last_Name} {newAddress.Address} {newAddress.SubDistrict} {newAddress.District} {newAddress.Province}  {newAddress.PostalCode}";

            return address;
        }

        public DuchmillModel.CRM_Customer_Address GetAddress(int addressType, int customerId)
        {
            try
            {
                if (AppConstants.Customer.Address.Type.Address == addressType)
                {
                    var address = Query(it => it.CRM_CustomerId == customerId && it.Addr_Type == addressType && !it.IsDeleted).FirstOrDefault();

                    if (address == null) return null;

                    return address;
                }
                else
                {
                    var address = Query(it => it.CRM_CustomerId == customerId && it.Addr_Type == addressType && it.IsDefault == true && !it.IsDeleted).FirstOrDefault();

                    if (address == null) return null;

                    return address;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
