using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Globalization;
using System.IO;

namespace fft_mobileapp.Controllers
{
    [MobileAppController]
    public class ExpiryDateController : ApiController
    {
        static string[] bestby_phrases = { "best by", "expiry", "best if used by", "exp", "expiration", "sell by", "use by", "consume by", "bbn" };
        static string[] date_abb = { "jan", "january", "feb", "february", "march", "mar", "april", "apr", "april", "may", "june", "jun", "july", "jul", "sept", "sep", "september", "oct", "october", "nov", "november", "dec", "december" };

        public DateTime Get(Stream imageFileStream)
        {
            System.Configuration.Configuration rootWebConfig1 =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            VisionServiceClient VisionServiceClient = new VisionServiceClient(rootWebConfig1.AppSettings.Settings["ProjectOxfordVisionSubscriptionKey"].Value);
            OcrResults analysisResult = VisionServiceClient.RecognizeTextAsync(imageFileStream, "en").Result;
            var expiryDate = GetExpiryDate(analysisResult);

            return expiryDate;
        }

        private static DateTime GetExpiryDate(OcrResults rawresults)
        {
            DateTime dt = DateTime.Now.Date.AddDays(-20);
            foreach (var res in rawresults.Regions)
            {
                foreach (var line in res.Lines)
                {
                    var sentence = "";
                    int num_of_ints = 0;
                    List<string> date_ints = new List<string>();

                    foreach (var word in line.Words)
                    {
                        sentence += word.Text.ToLower() + " ";
                        int n;
                        bool isInt = int.TryParse(word.Text, out n);
                        if (isInt)
                        {
                            num_of_ints++;
                            date_ints.Add(n.ToString());
                        }
                    }

                    //Checking to see if the 'Best By' phrase is contained in the sentence
                    foreach (var bestby in bestby_phrases)
                    {
                        if (sentence.Contains(bestby))
                        {
                            dt = FetchDate(date_ints, line);
                            if (dt >= DateTime.Now.Date)
                            {
                                return dt;
                            }
                            break;
                        }
                        else
                        {
                            //This sentence does not contain date
                        }
                    }

                    //Case when the date is not on the same line as 'Best by' or 'best by' does not exist
                    if (dt <= DateTime.Now.Date.AddDays(-20))
                    {
                        dt = FetchDate(date_ints, line);
                        if (dt >= DateTime.Now.Date)
                        {
                            return dt;
                        }
                    }
                }
            }
            return DateTime.MinValue;
        }


        public static DateTime FetchDate(List<string> date_ints, Line line)
        {
            int month_num = -12;
            string date_num = "";

            foreach (var word in line.Words)
            {
                //Checking to see if there 
                try
                {
                    month_num = DateTime.ParseExact(word.Text.Substring(0, 3), "MMM", CultureInfo.CurrentCulture).Month;

                }
                catch (Exception e)
                {
                    try
                    {
                        month_num = DateTime.ParseExact(word.Text.Substring(0, 3), "MMMM", CultureInfo.CurrentCulture).Month;
                    }
                    catch (Exception e1)
                    {
                        //Do Nothing
                    }

                }
                if (month_num > 0 && month_num < 13)
                {
                    if (word.Text.Length > 3)
                    {
                        bool isDate = word.Text.Any(c => char.IsDigit(c));
                        if (isDate)
                        {
                            date_num = word.Text.Substring(word.Text.IndexOfAny("0123456789".ToCharArray()), word.Text.Length - word.Text.IndexOfAny("0123456789".ToCharArray()));
                            date_ints.Add(date_num);
                        }
                        else
                        {
                            Console.WriteLine("No Date found.");
                        }
                    }
                    break;
                }
            }

            if (month_num > 0 && month_num < 13)
            {
                if (date_ints.Count == 1)
                {
                    if (date_ints.ElementAt(0).Length >= 4) //Format Mar 201603
                    {
                        if (date_ints.ElementAt(0).Length == 4)
                        {
                            string part1 = date_ints.ElementAt(0).Substring(0, 2);
                            string part2 = date_ints.ElementAt(0).Substring(2, 2);
                            var date = Convert.ToDateTime(month_num + "/" + part1 + "/" + part2);
                            return date;

                        }
                        else if (date_ints.ElementAt(0).Length == 5)
                        {
                            string part1 = date_ints.ElementAt(0).Substring(0, 1);
                            string part2 = date_ints.ElementAt(0).Substring(1, 4);
                            var date = Convert.ToDateTime(month_num + "/" + part1 + "/" + part2);

                            return date;
                        }
                        else if (date_ints.ElementAt(0).Length == 6)
                        {
                            string part1 = date_ints.ElementAt(0).Substring(0, 2);
                            string part2 = date_ints.ElementAt(0).Substring(2, 4);
                            var date = Convert.ToDateTime(month_num + "/" + part1 + "/" + part2);

                            return date;
                        }
                    }
                    else
                    {
                        //The date is probably on the next line
                        return DateTime.MinValue.Date;
                    }
                }
                else if (date_ints.Count == 2) //Format: Mar 14 2017, 14 Mar 2017, 2017 Mar 14, Mar 14, 17, etc.
                {
                    var part1 = date_ints.ElementAt(0);
                    var part2 = date_ints.ElementAt(1);
                    if (part1.Length == part2.Length && part2.Length == 2)
                    {
                        var date = Convert.ToDateTime(month_num + "/" + part1 + "/" + part2);

                        return date;
                    }
                    else
                    {
                        if (part1.Length > part2.Length)
                        {
                            var date = Convert.ToDateTime(month_num + "/" + part2 + "/" + part1);
                            return date;
                        }
                        else if (part1.Length < part2.Length)
                        {
                            var date = Convert.ToDateTime(month_num + "/" + part2 + "/" + part1);
                            return date;
                        }
                        else
                            return DateTime.MinValue.Date;
                    }
                }
                else if (date_ints.Count == 0)
                {
                    Console.Error.WriteLine("No date found on this line");
                    return DateTime.MinValue.Date;
                }
            }
            else
            {
                //TODO: Use case when the date is only made up of numbers. //Format: 03/14/16 , 03/14/2016 , 031416 , 03142016 , 03.14.16
                if (date_ints.Count == 0)
                {
                    //Use case: BB20160605
                }
                else if (date_ints.Count == 1)
                {
                    //use case: BB 20160605
                }
                else if (date_ints.Count == 3)
                {
                    string year = "";
                    int index_of_year = 0;
                    foreach (var dtin in date_ints)
                    {
                        if (dtin.Length == 4)
                        {
                            year = dtin;
                            break;
                        }
                        index_of_year++;
                    }
                    if (year.Equals(""))
                    {
                        if (date_ints.ElementAt(0).Length == date_ints.ElementAt(1).Length && date_ints.ElementAt(1).Length == date_ints.ElementAt(2).Length)
                        {
                            var date = Convert.ToDateTime(date_ints.ElementAt(0) + "/" + date_ints.ElementAt(1) + "/" + date_ints.ElementAt(2));
                            return date;
                        }
                        else
                        {
                            Console.Error.WriteLine("Something is wrong with the date");
                            //Something is wrong with the date
                        }
                    }
                    else
                    {

                        if (index_of_year == 0)    //Format: yyyy mm dd
                        {
                            var date = Convert.ToDateTime(date_ints.ElementAt(1) + "/" + date_ints.ElementAt(0) + "/" + year);
                            return date;
                        }
                        else if (index_of_year == 2)  //Format: mm dd yyyy
                        {
                            var date = Convert.ToDateTime(date_ints.ElementAt(0) + "/" + date_ints.ElementAt(1) + "/" + year);
                            return date;
                        }
                        else
                        {
                            Console.Error.WriteLine("Something is wrong with the date ; Year length is 4");
                            //Something is wrong with the date
                        }

                    }
                }
            }

            return DateTime.MinValue.Date;
        }
    }
}
