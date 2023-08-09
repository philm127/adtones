using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class DateTimeFormat
    {
        public string Format(DateTime? Data)
        {
            String FormatedDateTime = "";
            if (Data != null)
            {
                string CreatedDate = Convert.ToString(Data);
                string[] Datetime = CreatedDate.Split(' ');
                string[] Time = Datetime[1].Split(':');
                TimeSpan ts = new TimeSpan(Convert.ToInt32(Time[0]), Convert.ToInt32(Time[1]), Convert.ToInt32(Time[2]));
                Data = null;
                DateTime Date = Convert.ToDateTime(Datetime[0]);
                DateTime dt = Date.Add(ts);
                FormatedDateTime = String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dt);
            }
            else
            {
                FormatedDateTime = Data.Value.ToString();
            }

            return FormatedDateTime;
        }
    }
}