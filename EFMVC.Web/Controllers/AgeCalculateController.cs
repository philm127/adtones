using EFMVC.Data;
using EFMVC.ProvisioningModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    public class AgeCalculateController : Controller
    {
        // GET: AgeCalculate
        public ActionResult Index()
        {
            using (var SQLServerEntities = new EFMVCDataContex())
            {
                // Add AGE_DEMOGRAPHICS matching criteria in SP 
                var UserMatchList = SQLServerEntities.UserMatch.Where(s => s.DOB != null).ToList();
                if(UserMatchList.Count() > 0)
                {
                    foreach(var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                       // DateTime now = new DateTime(2017, 11, 02);
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList2 = SQLServerEntities.UserMatch2.Where(s => s.DOB != null).ToList();
                if (UserMatchList2.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList3 = SQLServerEntities.UserMatch3.Where(s => s.DOB != null).ToList();
                if (UserMatchList3.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList4 = SQLServerEntities.UserMatch4.Where(s => s.DOB != null).ToList();
                if (UserMatchList4.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList5 = SQLServerEntities.UserMatch5.Where(s => s.DOB != null).ToList();
                if (UserMatchList5.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList6 = SQLServerEntities.UserMatch6.Where(s => s.DOB != null).ToList();
                if (UserMatchList6.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList7 = SQLServerEntities.UserMatch7.Where(s => s.DOB != null).ToList();
                if (UserMatchList7.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }


                var UserMatchList8 = SQLServerEntities.UserMatch8.Where(s => s.DOB != null).ToList();
                if (UserMatchList8.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }


                var UserMatchList9 = SQLServerEntities.UserMatch9.Where(s => s.DOB != null).ToList();
                if (UserMatchList9.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                var UserMatchList10 = SQLServerEntities.UserMatch10.Where(s => s.DOB != null).ToList();
                if (UserMatchList10.Count() > 0)
                {
                    foreach (var item in UserMatchList)
                    {
                        DateTime now = DateTime.Today;
                        int age = now.Year - item.DOB.Value.Year;
                        if (now < item.DOB.Value.AddYears(age))
                            age--;
                        if (age < 18)
                            item.Age_Demographics = "A";
                        else if (age >= 18 && age <= 24)
                            item.Age_Demographics = "B";
                        else if (age >= 25 && age <= 34)
                            item.Age_Demographics = "C";
                        else if (age >= 35 && age <= 44)
                            item.Age_Demographics = "D";
                        else if (age >= 45 && age <= 54)
                            item.Age_Demographics = "E";
                        else if (age >= 55 && age <= 64)
                            item.Age_Demographics = "F";
                        else if (age >= 65)
                            item.Age_Demographics = "G";
                        else
                            item.Age_Demographics = "H";
                        SQLServerEntities.SaveChanges();
                    }
                }

                ViewBag.Window = true;

            }

            return View();
        }
        public ActionResult ImportCsv()
        {
            string yourDirectoryName = Server.MapPath("~/UserFile/Vodafone/Play/Live");
            string[] files = Directory.GetFiles(yourDirectoryName, "*.csv");
            for(int i=1;i < files.Length;i++)
            {
                StreamReader sr = new StreamReader(files[i]);

                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    try
                    {
                        string[] rowsArray = line.Split(';');
                        string FirstValue = rowsArray[0];
                        if (FirstValue.ToLower() != "header" || FirstValue.ToLower() != "trailer")
                        {

                        }

                        //line = string.Empty;
                        //string account = rowsArray[0];
                        //string relationID = rowsArray[1];                       
                        //line = account + ";" + "0" + ";" + "0" + ";" + relationID;
                        
                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg);
                    }
                   
                }
                sr.Close();
            }
            //StreamReader sr = new StreamReader(fileName);
            //StreamWriter sw = new StreamWriter(outFileName);
            //while (sr.Peek() >= 0)
            //{
            //    string line = sr.ReadLine();
            //    try
            //    {
            //        string[] rowsArray = line.Split(';');
            //        line = string.Empty;
            //        string account = rowsArray[0];
            //        string relationID = rowsArray[1];
            //        string resultIBAN = client.BBANtoIBAN(account);
            //        string resultBIC = client.BBANtoBIC(account);
            //        if (resultIBAN != string.Empty && resultBIC != string.Empty)
            //        {
            //            line = account + ";" + resultIBAN + ";" + resultBIC + ";" + relationID;
            //        }

            //        else
            //        {
            //            line = account + ";" + "0" + ";" + "0" + ";" + relationID;
            //        }
            //    }
            //    catch (Exception msg)
            //    {
            //        Console.WriteLine(msg);
            //    }
            //    sw.WriteLine(line);

            //}
            //sr.Close();
            //sw.Close();
            return View();
        }
    }
}