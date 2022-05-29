using System.Diagnostics;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;

namespace UpTimeMonitor
{
    public class UpTimeMonitor
    {
        public void runCheck()
        {
            var db = new UptimeMonitorContext();
            var sites = db.Websites;
            foreach (var site in sites)
            {
                CheckSite(site.Url);
            }

        }

        public SiteResponse CheckSite(string? url)
        {
            var result = new SiteResponse();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var client = new HttpClient();


            try
            {
                var checkingResponse = client.GetAsync(url).Result;

                result.status = checkingResponse.IsSuccessStatusCode &&
                                checkingResponse.StatusCode == HttpStatusCode.OK;
                
            }
            catch
            {
                result.status = false;
                // offline
            }

            
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            result.responseTime = elapsed;


            if (result.status)
            {
                // archive record
                recordToDb(result);
            }
            else
            {
                // archive record
                recordToDb(result);


                // send email
                SendEmail();

            }


            return result;
        }

        public void recordToDb(SiteResponse response)
        {
            var db = new UptimeMonitorContext();
            var newRecord = new Uptime
            {
                Time = DateTime.Now,
                Status = response.status,
                ResponseTime = (int)response.responseTime,
                Websiteid = 1
            };
            db.Uptimes.Add(newRecord);
            db.SaveChanges();
        }

        public void SendEmail()
        {
            //Implement Your Own Method
        }

        public async Task<string> SendEmail(string serverAddress,int serverPort, string user,string password,bool isSecure,
            string toAddress,string websiteURL)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("FromName", "FromAddress@Email.com")); //Change!!
                message.To.Add(new MailboxAddress("", toAddress));
                message.Subject = "Website is Down!";
                message.ReplyTo.Add(new MailboxAddress("FromName", "ReplyTo"));  //Change!!

                //Change!!
                var builder = new BodyBuilder
                {
                    TextBody = websiteURL + " is Down!",
                    HtmlBody = "<p>" + websiteURL + " is Down!</p>"
                };

                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(serverAddress, serverPort, isSecure);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(user, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                return e.Message;
            }


            return "OK";
        }


        public class SiteResponse
        {
            public bool status { get; set; }
            public long responseTime { get; set; }
        }


    }
}
