using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Configuration;

public class EmailUtl
{
   private static string EMAIL_ID = "aptivephysiotherapy-no-reply@hotmail.com";
    private static string EMAIL_PW = "Aptivefyp2017";


   // Using Microsoft's LIVE
   private static string HOST = "smtp.live.com";
   private static int PORT = 25;

   public static bool SendEmail(string recipient,
                                string subject, string msg,
                            out string error)
   {
      SmtpClient client = new SmtpClient(HOST, PORT);
      client.EnableSsl = true;
      client.Timeout = 10000;
      client.Credentials = new System.Net.NetworkCredential(EMAIL_ID, EMAIL_PW);

      MailMessage mm = new MailMessage(EMAIL_ID, recipient, subject, msg);
      mm.IsBodyHtml = true;
      bool success = true;
      error = "";
      try
      {
         client.Send(mm);
      }
      catch (Exception e)
      {
         error = e.Message;
         success = false;
      }
      return success;
   }

}
