// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Email.EmailSender
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yetkilim.Global.Configuration;
using Yetkilim.Global.Model;

namespace Yetkilim.Infrastructure.Email
{
  public class EmailSender : IEmailSender
  {
    private readonly ILogger<EmailSender> _logger;
    private readonly SmtpOptions _options;

    public EmailSender(ILogger<EmailSender> logger, IOptions<ConfigurationModel> options)
    {
      this._logger = logger;
      this._options = options.Value.Smtp;
    }

    public async Task<Result> Send(
      IEnumerable<string> toAddresses,
      string subject,
      string body)
    {
      try
      {
        SmtpClient client = new SmtpClient(this._options.Host, this._options.Port)
        {
          UseDefaultCredentials = false,
          EnableSsl = this._options.EnableSsl,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          Credentials = (ICredentialsByHost) new NetworkCredential(this._options.Username, this._options.Password)
        };
        MailMessage mailMessage = new MailMessage()
        {
          Subject = subject,
          Body = body,
          IsBodyHtml = true,
          From = new MailAddress(this._options.Username, this._options.SenderName)
        };

                toAddresses = toAddresses.Where(x => isEmail(x)).Select(x => x.Trim());


                foreach (string toAddress in toAddresses)
        {
          string to = toAddress;
          mailMessage.To.Add(to);
          to = (string) null;
        }



                await client.SendMailAsync(mailMessage);
        this._logger.LogInformation("Mail gönderildi", (object) toAddresses, (object) subject);
        return Result.Success("Mail gönderildi!");
      }
      catch (Exception ex)
      {
        this._logger.LogError("Mail gönderilirken hata oluştu!", (object) ex);
        return Result.Fail("Mail gönderilemedi!", (Exception) null, (string) null);
      }
    }


         bool isEmail(string emailString)
        {
            return Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }


  }
}
