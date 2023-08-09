using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.CountryConnectionString
{
    public class ConnectionString
    {
        public static string GetConnectionString(int? CountryID)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            string Conn = null;
            //if (CountryID == 9)
            //{
            //    Conn = ConfigurationManager.AppSettings["KenyaConnectionString"];
            //}

            var CountryConnectionString = db.CountryConnectionString.Where(s => s.CountryId == CountryID);
            if (CountryConnectionString.Count() > 0)
                Conn = CountryConnectionString.FirstOrDefault().ConnectionString;

            return Conn;
        }

        public static List<string> GetConnectionStringByOperatorId(int? OperatorId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            List<string> Conn = null;
          
            var operatorConnectionString = db.CountryConnectionString.Where(s => s.OperatorId == OperatorId);
            if (operatorConnectionString.Count() > 0)
                Conn = operatorConnectionString.Select(s=>s.ConnectionString).ToList();

            return Conn;
        }

        public static List<string> GetConnectionStringByCountryId(int? CountryId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            List<string> Conn = null;

            var countryConnectionString = db.CountryConnectionString.Where(s => s.CountryId == CountryId);
            if (countryConnectionString.Count() > 0)
                Conn = countryConnectionString.Select(s => s.ConnectionString).ToList();

            return Conn;
        }

        public static List<string> GetAllConnectionString()
        {

            EFMVCDataContex db = new EFMVCDataContex();
            List<string> Conn = null;
            var countryConnectionString = db.CountryConnectionString.ToList();
            if (countryConnectionString.Count() > 0)
                Conn = countryConnectionString.Select(s => s.ConnectionString).ToList();

            return Conn;

        }

        public static string GetSingleConnectionStringByOperatorId(int? OperatorId)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            string Conn = null;

            var operatorConnectionString = db.CountryConnectionString.Where(s => s.OperatorId == OperatorId);
            if (operatorConnectionString.Count() > 0)
                Conn = operatorConnectionString.FirstOrDefault().ConnectionString;

            return Conn;
        }
    }
}
