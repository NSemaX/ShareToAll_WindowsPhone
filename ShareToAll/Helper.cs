using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareToAll
{
    public class Helper
    {
        public string CalculateShareTime(DateTime time)
        {
            string result = "";

            if (DateTime.Now.Subtract(time).Days >= 365)
                result = (DateTime.Now.Subtract(time).Days / 365).ToString() + " yr";
            else if (DateTime.Now.Subtract(time).Days >= 30 && DateTime.Now.Subtract(time).Days < 365)
                result = (DateTime.Now.Subtract(time).Days / 30).ToString() + " mo";
            else if (DateTime.Now.Subtract(time).Days < 30 && DateTime.Now.Subtract(time).Days >= 7)
                result = (DateTime.Now.Subtract(time).Days / 7).ToString() + " w";
            else if (DateTime.Now.Subtract(time).Days >= 1 && DateTime.Now.Subtract(time).Days < 7)
                result = (DateTime.Now.Subtract(time).Days).ToString() + " d";
            else if (DateTime.Now.Subtract(time).Days < 1)
            {
                if (DateTime.Now.Subtract(time).Hours >= 1)
                    result = (DateTime.Now.Subtract(time).Hours).ToString() + " hr";
                else if (DateTime.Now.Subtract(time).Minutes >= 1 && DateTime.Now.Subtract(time).Hours == 0)
                    result = (DateTime.Now.Subtract(time).Minutes).ToString() + " min";
                else if (DateTime.Now.Subtract(time).Seconds >= 0 && DateTime.Now.Subtract(time).Minutes == 0)
                    result = (DateTime.Now.Subtract(time).Seconds).ToString() + " sec";
                else if (DateTime.Now.Subtract(time).Milliseconds >= 0 && DateTime.Now.Subtract(time).Seconds == 0)
                    result = (DateTime.Now.Subtract(time).Milliseconds).ToString() + " msec";
            }

            return result;
        }

 


        public DateTime SetAccordingToTimeZone(DateTime date)
        {
            DateTime PostLocalDate = new DateTime();

            //istanbul uct+2 mi istanbul uct+ 3 mü ?
            DateTime theDate = new DateTime(DateTime.Now.Year, 4, 1); // april 1st
            DateTime theDate1 = new DateTime(DateTime.Now.Year, 11, 1); // nov 1st
            int istanbuloffset = 0;
            if (date < theDate1 && date > theDate)
                //bool daylight1 = TimeZoneInfo.Utc.IsDaylightSavingTime(date);
                // if (daylight1 == false)
                // istanbuloffset=DateTime.UtcNow.Subtract(date).Hours;
                // else
                istanbuloffset = -3; //daylightsavingtime for istanbul
            else
                istanbuloffset = -2;


            DateTime PostUCTdate = new DateTime();
            PostUCTdate = date.AddHours(istanbuloffset);

            bool daylight = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now);
            int offset = 0;
            if (daylight == false)

                offset = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            else
                offset = TimeZoneInfo.Local.BaseUtcOffset.Hours + 1;

            PostLocalDate = PostUCTdate.AddHours(offset);


            return PostLocalDate;
        }



        //////////////////////////////////////////

    }
}
