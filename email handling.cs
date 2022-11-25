using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace CSVParser
{
    /// <summary>
    /// Main class to send email
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Calling function serving as an entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"EmailTask.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                int counter = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (counter > 0)
                    {
                        SendEmail(values[3].Trim(), values[2].Trim(), "Your email here  ", "Your password here ", values[0].Trim(), values[1].Trim().Split('@')[0]);
                    }
                    counter++;
                }
            }
        }
        /// <summary>
        /// Utility function to send email using secure SMTP protocol
        /// </summary>
        /// <param name="htmlString">Email body</param>
        /// <param name="subject">Email subject</param>
        /// <param name="From">Sender Email</param>
        /// <param name="password">Password</param>
        /// <param name="To">To Email</param>
        /// <param name="DisplayName">Send Name</param>
        public static void SendEmail(string htmlString, string subject, string From, string password, string To,string DisplayName)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From, DisplayName, System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = htmlString;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(From, password);
            client.Port = 587;
            client.Host = "smtp.gmail.com";     //we use smtp here because it is most secure connection 
            client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                Console.WriteLine("Error : " + errorMessage);
            }
        }
    }
}
