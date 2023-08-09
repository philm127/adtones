// ***********************************************************************
// Assembly         : EFMVC.Core
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="EmailHelper.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;

/// <summary>
/// The Common namespace.
/// </summary>

namespace EFMVC.Core.Common
{
    /// <summary>
    /// A static helper class for sending SMTP Mail Messages.
    /// </summary>
    public static class MailHelper
    {
        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body)
        {
            SendMail(from, to, cc, bcc, subject, body, false, MailPriority.Normal);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body)
        {
            var toAddress = new string[1];
            toAddress[0] = to;

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, false, MailPriority.Normal);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    string host, int port)
        {
            SendMail(from, to, cc, bcc, subject, body, false, MailPriority.Normal, host, port);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body,
                                    string host, int port)
        {
            var toAddress = new string[1];
            toAddress[0] = to;

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, true, MailPriority.High, host, port);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    bool isBodyHtml)
        {
            SendMail(from, to, cc, bcc, subject, body, true, MailPriority.High);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body,
                                    bool isBodyHtml)
        {
            var toAddress = new string[1];

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, true, MailPriority.High);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    bool isBodyHtml, string host, int port)
        {
            SendMail(from, to, cc, bcc, subject, body, isBodyHtml, MailPriority.High, host, port);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body,
                                    bool isBodyHtml, string host, int port)
        {
            var toAddress = new string[1];
            toAddress[0] = to;

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, isBodyHtml, MailPriority.High, host,
                     port);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="priority">A System.Net.Mail.MailPriority that contains the priority of this message.</param>
        /// <exception cref="System.ArgumentNullException">
        /// from;from cannot be null.
        /// or
        /// to;to cannot be null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">to;to must contain at least on email address</exception>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    bool isBodyHtml, MailPriority priority)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from", "from cannot be null.");

            if (to == null)
                throw new ArgumentNullException("to", "to cannot be null.");

            if (to.Length == 0)
                throw new ArgumentOutOfRangeException("to", "to must contain at least on email address");

            var client = new SmtpClient();

            var message = new MailMessage {From = new MailAddress(from)};

            foreach (string address in to)
                message.To.Add(address);

            if (cc != null)
                foreach (string address in cc)
                    message.CC.Add(address);

            if (bcc != null)
                foreach (string address in bcc)
                    message.Bcc.Add(address);
            
            message.Subject = subject;
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.Priority = priority;

            client.Send(message);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="priority">A System.Net.Mail.MailPriority that contains the priority of this message.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body,
                                    bool isBodyHtml, MailPriority priority)
        {
            var toAddress = new string[1];
            toAddress[0] = to;

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, isBodyHtml, priority);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="priority">A System.Net.Mail.MailPriority that contains the priority of this message.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        /// <exception cref="System.ArgumentNullException">
        /// host;host cannot be null.
        /// or
        /// from;from cannot be null.
        /// or
        /// to;to cannot be null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// port;port cannot be less than zero.
        /// or
        /// to;to must contain at least on email address
        /// </exception>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    bool isBodyHtml, MailPriority priority, string host, int port)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host", "host cannot be null.");

            if (port < 0)
                throw new ArgumentOutOfRangeException("port", "port cannot be less than zero.");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from", "from cannot be null.");

            if (to == null)
                throw new ArgumentNullException("to", "to cannot be null.");

            if (to.Length == 0)
                throw new ArgumentOutOfRangeException("to", "to must contain at least on email address");

            var client = new SmtpClient(host, port);

            var message = new MailMessage {From = new MailAddress(from)};

            foreach (string address in to)
                message.To.Add(address);

            if (cc != null)
                foreach (string address in cc)
                    message.CC.Add(address);

            if (bcc != null)
                foreach (string address in bcc)
                    message.Bcc.Add(address);

            
            message.Subject = subject;
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.Priority = priority;   
            client.Send(message);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to address for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="priority">A System.Net.Mail.MailPriority that contains the priority of this message.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body,
                                    bool isBodyHtml, MailPriority priority, string host, int port)
        {
            var toAddress = new string[1];
            toAddress[0] = to;

            string[] ccAddress = null;
            if (cc != null)
            {
                ccAddress = new string[1];
                ccAddress[0] = cc;
            }

            string[] bccAddress = null;
            if (bcc != null)
            {
                bccAddress = new string[1];
                bccAddress[0] = bcc;
            }

            SendMail(from, toAddress, ccAddress, bccAddress, subject, body, isBodyHtml, priority, host, port);
        }

        /// <summary>
        /// Sends e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="from">A System.String that contains the from address for this email.</param>
        /// <param name="to">A System.String that contains the to addresses for this email.</param>
        /// <param name="cc">A System.String that contains the to addresses for this email.</param>
        /// <param name="bcc">A System.String that contains the to addresses for this email.</param>
        /// <param name="subject">A System.String that contains the subject line for this email.</param>
        /// <param name="body">A System.String that contains the message body for this email.</param>
        /// <param name="isBodyHtml">A System.Boolean indicating whether the message body is in HTML.</param>
        /// <param name="priority">A System.Net.Mail.MailPriority that contains the priority of this message.</param>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        /// <param name="attachmentPath">An System.String that contains the path to the required attachment.</param>
        /// <exception cref="System.ArgumentNullException">
        /// host;host cannot be null.
        /// or
        /// from;from cannot be null.
        /// or
        /// to;to cannot be null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// port;port cannot be less than zero.
        /// or
        /// to;to must contain at least on email address
        /// </exception>
        public static void SendMail(string from, string[] to, string[] cc, string[] bcc, string subject, string body,
                                    bool isBodyHtml, MailPriority priority, string host, int port,
                                    string attachmentPath)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host", "host cannot be null.");

            if (port < 0)
                throw new ArgumentOutOfRangeException("port", "port cannot be less than zero.");

            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException("from", "from cannot be null.");

            if (to == null)
                throw new ArgumentNullException("to", "to cannot be null.");

            if (to.Length == 0)
                throw new ArgumentOutOfRangeException("to", "to must contain at least on email address");

            var client = new SmtpClient(host, port);

            var message = new MailMessage {From = new MailAddress(from)};

            foreach (string address in to)
                message.To.Add(address);

            if (cc != null)
                foreach (string address in cc)
                    message.CC.Add(address);

            if (bcc != null)
                foreach (string address in bcc)
                    message.Bcc.Add(address);

            message.Subject = subject;
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.Priority = priority;

            var attachment = new Attachment(attachmentPath);
            message.Attachments.Add(attachment);

            client.Send(message);
        }

        /// <summary>
        /// Builds the HTML body.
        /// </summary>
        /// <param name="emailBody">The email body.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>System.String.</returns>
        public static string BuildHtmlBody(string emailBody, ListDictionary replacements)
        {
            if (replacements != null)
                emailBody = replacements.Cast<DictionaryEntry>().Aggregate(emailBody,
                                                                           (current, replacement) =>
                                                                           current.Replace(
                                                                               replacement.Key.ToString(),
                                                                               replacement.Value.ToString()));

            return emailBody;
        }

        /// <summary>
        /// Builds the HTML body.
        /// </summary>
        /// <param name="emailBody">The email body.</param>
        /// <returns>System.String.</returns>
        public static string BuildHtmlBody(string emailBody)
        {
            return BuildHtmlBody(emailBody, null);
        }
    }
}