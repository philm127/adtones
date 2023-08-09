using EFMVC.Data;
using EFMVC.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public static class GetProfileMatchOption
    {        
        public static bool IsActiveProfileInfo(int CountryId, int id)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var data = db.ProfileMatchInformation.Where(s => s.CountryId == CountryId && s.Id == id).FirstOrDefault();
            if(data != null)
            {
                return data.IsActive;
            }
            return true;
        }

        public static bool IsActiveProfileInfo(int CountryId, string name)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            var data = db.ProfileMatchInformation.Where(s => s.CountryId == CountryId && s.ProfileName.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (data != null)
            {
                return data.IsActive;
            }
            return true;
        }
    }
}