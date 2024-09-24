using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace DEDS
{
    public class EmailHelper
    {
        private string _mailServer = "";

        private int _mailPort = 0;

        private string _account = "";

        private string _password = "";

        private string _mailFrom = "";

        private string _mailFromName = "";

        private string _subject = "";

        private bool _isBodyHtml = true;

        private string _body = "";

        private string _errorMessage;

        private bool _isSendEmail;

        private bool _enableSSL = false;

        private List<MailAddress> _toMailList;

        private List<MailAddress> _ccMailList;

        private List<MailAddress> _bccMailList;

        private List<string> _attachmentList;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool EnableSSL
        {
            get
            {
                return _enableSSL;
            }
            set
            {
                _enableSSL = value;
            }
        }

        public bool IsSendEmail
        {
            get
            {
                return _isSendEmail;
            }
            set
            {
                _isSendEmail = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        public int MailPort
        {
            get
            {
                return _mailPort;
            }
            set
            {
                _mailPort = value;
            }
        }

        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string MailServer
        {
            get
            {
                return _mailServer;
            }
            set
            {
                _mailServer = value;
            }
        }

        public string MailFrom
        {
            get
            {
                return _mailFrom;
            }
            set
            {
                _mailFrom = value;
            }
        }

        public string MailFromName
        {
            get
            {
                return _mailFromName;
            }
            set
            {
                _mailFromName = value;
            }
        }

        //view丟至後端(多筆 ','區別) 收件者
        public string ToMails
        {
            get; set;
        }

        public List<MailAddress> ToMailList
        {
            get
            {
                return _toMailList;
            }
            set
            {
                _toMailList = value;
            }
        }

        public List<MailAddress> CCMailList
        {
            get
            {
                return _ccMailList;
            }
            set
            {
                _ccMailList = value;
            }
        }

        //view丟至後端(多筆 ','區別) 密件副本
        public string BCCMails
        {
            get; set;
        }

        public List<MailAddress> BCCMailList
        {
            get
            {
                return _bccMailList;
            }
            set
            {
                _bccMailList = value;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public bool IsBodyHtml
        {
            get
            {
                return _isBodyHtml;
            }
            set
            {
                _isBodyHtml = value;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        public List<string> AttachmentList
        {
            get
            {
                return _attachmentList;
            }
            set
            {
                _attachmentList = value;
            }
        }

        public EmailHelper()
        {
            _mailServer = "";
            _mailPort = 0;
            _account = "";
            _password = "";
            _mailFrom = "";
            _mailFromName = "";
            _subject = "";
            _isBodyHtml = true;
            _body = "";
            _errorMessage = "";
            _isSendEmail = true;
            _enableSSL = false;
            _toMailList = new List<MailAddress>();
            _ccMailList = new List<MailAddress>();
            _bccMailList = new List<MailAddress>();
            _attachmentList = new List<string>();
        }

        public bool SendBySmtp()
        {
            bool result = false;
            try
            {
                //測試連線
                if (!CommonFunc.TestMailServerSmtpIp(_mailServer, _mailPort))
                {
                    return result;
                }

                //https處理 REF: https://stackoverflow.com/a/39534068/288936
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                    SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                _errorMessage = string.Empty;
                SmtpClient smtpClient = new SmtpClient();
                MailMessage mailMessage = new MailMessage();
                if (_mailServer == string.Empty)
                {
                    _errorMessage = "未設定mail server";
                    return false;
                }

                if (_mailPort == 0)
                {
                    _errorMessage = "未設定mail server port";
                    return false;
                }

                smtpClient.Host = _mailServer;
                smtpClient.EnableSsl = _enableSSL;
                smtpClient.Port = _mailPort;
                if (_account != string.Empty || _password != string.Empty)
                {
                    smtpClient.Credentials = new NetworkCredential(_account, _password);
                }

                mailMessage.From = new System.Net.Mail.MailAddress(_mailFrom, _mailFromName);
                foreach (MailAddress toMail in _toMailList)
                {
                    if (toMail.DisplayName == "")
                    {
                        mailMessage.To.Add(toMail.Address);
                    }
                    else
                    {
                        mailMessage.To.Add(new System.Net.Mail.MailAddress(toMail.Address, toMail.DisplayName));
                    }
                }

                foreach (MailAddress ccMail in _ccMailList)
                {
                    if (ccMail.DisplayName == "")
                    {
                        mailMessage.CC.Add(ccMail.Address);
                    }
                    else
                    {
                        mailMessage.CC.Add(new System.Net.Mail.MailAddress(ccMail.Address, ccMail.DisplayName));
                    }
                }

                foreach (MailAddress bccMail in _bccMailList)
                {
                    if (bccMail.DisplayName == "")
                    {
                        mailMessage.Bcc.Add(bccMail.Address);
                    }
                    else
                    {
                        mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(bccMail.Address, bccMail.DisplayName));
                    }
                }

                mailMessage.Subject = _subject;
                mailMessage.IsBodyHtml = _isBodyHtml;
                mailMessage.Body = _body;
                if (_attachmentList.Count > 0)
                {
                    foreach (string attachment in _attachmentList)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }
                }

                if (_isSendEmail)
                {
                    smtpClient.Send(mailMessage);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Log.For(null).Error(ex.Message);
                Logger.Log.For(null).Error(ex.StackTrace);
            }

            return result;
        }

        public void AddTo(string address, string displayName)
        {
            if (address != "")
            {
                _toMailList.Add(new MailAddress(address, displayName));
            }
        }

        public void AddCC(string address, string displayName)
        {
            if (address != "")
            {
                _ccMailList.Add(new MailAddress(address, displayName));
            }
        }

        public void AddBCC(string address, string displayName)
        {
            if (address != "")
            {
                _bccMailList.Add(new MailAddress(address, displayName));
            }
        }
    }

    public class MailParam
    {
        public virtual void iniParam()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/FileDatas/json")), "TestMailParam.json")))
            {
                string text = sr.ReadToEnd().Replace("\r\n", "");
                TestMailParam obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TestMailParam>(text);

                this.MailFrom = obj.MailFrom;
                this.MailFromName = obj.MailFromName;
                this.Account = obj.Account;
                this.Password = obj.Password;
                this.MailServer = obj.MailServer;
                this.MailPort = obj.MailPort;
                this.EnableSSL = obj.EnableSSL;
            }
        }

        public string MailFrom { get; set; }
        public string MailFromName { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public bool EnableSSL { get; set; }
    }

    public class TestMailParam : MailParam
    {
        public override void iniParam()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/Data")), "TestMailParam.json")))
            {
                string text = sr.ReadToEnd().Replace("\r\n", "");
                TestMailParam obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TestMailParam>(text);

                this.ToMails = obj.ToMails;
                this.BCCMails = obj.BCCMails;
                this.MailFrom = obj.MailFrom;
                this.MailFromName = obj.MailFromName;
                this.Account = obj.Account;
                this.Password = obj.Password;
                this.MailServer = obj.MailServer;
                this.MailPort = obj.MailPort;
                this.EnableSSL = obj.EnableSSL;
            }
        }

        public string ToMails { get; set; }
        public string BCCMails { get; set; }
    }
}