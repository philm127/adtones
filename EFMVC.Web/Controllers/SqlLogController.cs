using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace EFMVC.Web.Controllers
{
    public class SqlLogController : Controller
    {
        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICountryRepository _CountryRepository;

        public SqlLogController(ICampaignProfileRepository profileRepository, ICountryRepository CountryRepository)
        {
            _profileRepository = profileRepository;
            _CountryRepository = CountryRepository;
        }
        // GET: SqlLog
        public ActionResult Index()
        {

            //var CountryServer = ConfigurationManager.AppSettings["KenyaConnectionString"];

            //var GetCompaign = _profileRepository.GetAll();

            //EFMVCDataContex db = new EFMVCDataContex(CountryServer);

            //var s = _CountryRepository.GetAll();

            //var s2 = db.Country.ToList();

            try
            {
                EFMVCDataContex db = new EFMVCDataContex();
                var advert = db.Adverts.Where(s => s.AdvertId == 1319).FirstOrDefault();
                var mediaFile = advert.MediaFileLocation;
                var host = "172.29.128.102";
                var port = 22;
                var username = "adds";
                var password = "so1xo5zaeth0Uthie";
                var localRoot = System.Web.HttpContext.Current.Server.MapPath("~/Media");
                var ftpRoot = "/usr/local/arthar/adds";

                using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        //var SourceFile = localRoot + @"\" + advert.UserId + @"\" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                        var SourceFile = @"D:\AdtoneLatest\Adtone\Media\1552324\2019520952.wav";
                        var DestinationFile = ftpRoot + "/" + System.IO.Path.GetFileName(advert.MediaFileLocation);
                        var filestream = new FileStream(SourceFile, FileMode.Open);
                        client.UploadFile(filestream, DestinationFile);
                        ViewBag.Success = "Success";
                        return View();
                    }
                }
             //   var test = @"D:\AdtoneLatest\Adtone\Media\1552324\2019520952.wav";
                ViewBag.Success = "Not Connected";
            }
            catch(Exception ex)
            {
                ViewBag.Success = ex.Message.ToString();
            }
            return View();


        }



        //private static string GetConnectionStringFromWebConfigByName(string name)
        //{
        //    return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        //}
    }

  
}