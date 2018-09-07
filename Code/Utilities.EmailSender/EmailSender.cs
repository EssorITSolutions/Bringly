using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using Utilities.EmailSender.Domain;

namespace Utilities.EmailSender
{
    public class EmailSender
    {
        static string RemoveIllegalCharactersInPath(string filename)
        {
            return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));
        }

        public static string sendEmail(EmailDomain emailEntity)
        {

            SmtpClient smtp = new SmtpClient(emailEntity.Host, emailEntity.Port);
            MailMessage sendMail = new MailMessage();
            try
            {
                //if (emailEntity == null) throw new ArgumentException("EmailDomain cannot be null", "emailEntity");
                //if (emailEntity.EmailTo == null && emailEntity.ListOfMultpleEmailTo == null) throw new ArgumentException("emailTo cannot be null", "emailEntity");
                if (string.IsNullOrEmpty(emailEntity.EmailTo) && emailEntity.ListOfMultpleEmailTo == null)
                    throw new ArgumentException("emailTo cannot be empty", "emailEntity");


                #region Email To
                if (!string.IsNullOrEmpty(emailEntity.EmailTo))
                {
                    sendMail.To.Add(!string.IsNullOrWhiteSpace(emailEntity.EmailToName)
                        ? new MailAddress(emailEntity.EmailTo, emailEntity.EmailToName.Trim())
                        : new MailAddress(emailEntity.EmailTo));
                }

                if (emailEntity.ListOfMultpleEmailTo != null)
                {
                    foreach (var emailTo in emailEntity.ListOfMultpleEmailTo)
                    {
                        sendMail.To.Add(!string.IsNullOrWhiteSpace(emailTo.Value)
                       ? new MailAddress(emailTo.Key, emailTo.Value.Trim())
                       : new MailAddress(emailTo.Key));
                    }
                }


                #endregion

                #region Email From
                sendMail.From = !string.IsNullOrWhiteSpace(emailEntity.EmailFromName) ? new MailAddress(emailEntity.EmailFrom, emailEntity.EmailFromName.Trim()) : new MailAddress(emailEntity.EmailFrom);
                #endregion

                #region Email CC
                if (!string.IsNullOrWhiteSpace(emailEntity.EmailCc))
                {
                    sendMail.CC.Add(!string.IsNullOrWhiteSpace(emailEntity.EmailCcName)
                        ? new MailAddress(emailEntity.EmailCc, emailEntity.EmailCcName.Trim())
                        : new MailAddress(emailEntity.EmailCc));
                }
                if (emailEntity.ListOfMultipleEmailCc != null)
                {
                    foreach (var emailCc in emailEntity.ListOfMultipleEmailCc)
                    {
                        sendMail.CC.Add(!string.IsNullOrWhiteSpace(emailCc.Value)
                       ? new MailAddress(emailCc.Key, emailCc.Value.Trim())
                       : new MailAddress(emailCc.Key));
                    }
                }
                #endregion

                #region Email BCC
                if (!string.IsNullOrWhiteSpace(emailEntity.EmailBcc))
                {
                    sendMail.Bcc.Add(!string.IsNullOrWhiteSpace(emailEntity.EmailBccName)
                        ? new MailAddress(emailEntity.EmailBcc, emailEntity.EmailBccName.Trim())
                        : new MailAddress(emailEntity.EmailBcc));
                }
                #endregion

                #region Reply
                if (!string.IsNullOrWhiteSpace(emailEntity.EmailReply))
                {
                    sendMail.ReplyToList.Add(!string.IsNullOrWhiteSpace(emailEntity.EmailReplyName)
                       ? new MailAddress(emailEntity.EmailReply, emailEntity.EmailReplyName.Trim())
                       : new MailAddress(emailEntity.EmailReply));
                }
                #endregion

                if (!string.IsNullOrEmpty(emailEntity.EmailSubject) && !string.IsNullOrWhiteSpace(emailEntity.EmailSubject))
                {
                    sendMail.Subject = emailEntity.EmailSubject.Replace('\r', ' ').Replace('\n', ' ');
                }
                sendMail.Body = emailEntity.EmailBody;

                sendMail.IsBodyHtml = !emailEntity.IsBodyHtml.HasValue || Convert.ToBoolean(emailEntity.IsBodyHtml);

                if (!emailEntity.MailPriority.HasValue)
                {
                    sendMail.Priority = MailPriority.Normal;
                }
                else if (emailEntity.MailPriority == EmailMailPriority.High)
                {
                    sendMail.Priority = MailPriority.High;
                }
                else if (emailEntity.MailPriority == EmailMailPriority.Low)
                {
                    sendMail.Priority = MailPriority.Low;
                }
                else if (emailEntity.MailPriority == EmailMailPriority.Normal)
                {
                    sendMail.Priority = MailPriority.Normal;
                }

                #region Adding Attachments
                if (emailEntity.Attachments != null && emailEntity.Attachments.Count > 0)
                {
                    Attachment attachment;
                    foreach (DictionaryEntry entry in emailEntity.Attachments)
                    {
                        string fullPath = entry.Value.ToString();
                        string fileName = RemoveIllegalCharactersInPath(entry.Key.ToString());
                        if (File.Exists(fullPath))
                        {
                            attachment = new Attachment(fullPath, MediaTypeNames.Application.Octet);

                            ContentDisposition disposition = attachment.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(fullPath);
                            disposition.ModificationDate = File.GetLastWriteTime(fullPath);
                            disposition.ReadDate = File.GetLastAccessTime(fullPath);
                            disposition.FileName = Path.GetFileName(fileName);
                            disposition.Size = new FileInfo(fullPath).Length;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            sendMail.Attachments.Add(attachment);
                        }
                    }
                }
                if (emailEntity.ListOfAttachments != null && emailEntity.ListOfAttachments.Count > 0)
                {
                    foreach (var item in emailEntity.ListOfAttachments)
                    {
                        string fullPath = item.Value;
                        string fileName = RemoveIllegalCharactersInPath(item.Key.ToString(CultureInfo.InvariantCulture));
                        if (File.Exists(fullPath))
                        {
                            Attachment attachment = new Attachment(fullPath, MediaTypeNames.Application.Octet);

                            ContentDisposition disposition = attachment.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(fullPath);
                            disposition.ModificationDate = File.GetLastWriteTime(fullPath);
                            disposition.ReadDate = File.GetLastAccessTime(fullPath);
                            disposition.FileName = Path.GetFileName(fileName);
                            disposition.Size = new FileInfo(fullPath).Length;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            sendMail.Attachments.Add(attachment);
                        }
                    }
                }
                #endregion

                #region Adding Network Credential
                if (emailEntity.HasNetworkCredential)
                {
                    smtp.Credentials = new System.Net.NetworkCredential(emailEntity.NetworkCredentialUserName, emailEntity.NetworkCredentialPassword);
                    smtp.EnableSsl = emailEntity.EnableSsl;
                }
                #endregion

                smtp.Port = emailEntity.Port;
                smtp.Host = emailEntity.Host;
                //smtp.Timeout = 1000000000;
                if (!emailEntity.CanSendEmail)
                {
                    emailEntity.EmailNotSentError = "Email not send as property 'CanSendEmail' is set to false.";
                    emailEntity.IsEmailSent = false;
                    return emailEntity.EmailNotSentError;
                }
                else
                {
                    smtp.Send(sendMail);
                }
                emailEntity.IsEmailSent = true;
                return "";
            }
            catch (Exception ex)
            {
                emailEntity.IsEmailSent = false;
                return emailEntity.EmailNotSentError = Convert.ToString(ex.Message) + " \n " + Convert.ToString(ex.InnerException) + "\n stack trace: " + ex.StackTrace;
            }
            finally
            {
                if (smtp != null)
                {
                    smtp.Dispose();
                }
                if (sendMail != null)
                {
                    sendMail.Dispose();
                }
            }
        }
    }
}
