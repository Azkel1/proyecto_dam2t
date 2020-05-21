using Newtonsoft.Json;
using ProyectoFinal_DI_AlexisSantana.model;
using System;
using System.Net.Mail;
using System.Windows;

namespace ProyectoFinal_DI_AlexisSantana.view
{
    public partial class MailWindow : Window
    {
        public MailWindow()
        {
            InitializeComponent();
        }

        //Botón para enviar un correo con los datos introducidos
        public void SendMail(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Mail_Send_To.Text.Trim()))
            {
                UIGlobal.MainWindow.ShowMessage("Introduzca uno o varios destinatario", "error");
            }
            else
            {
                MailConfig mailData = JsonConvert.DeserializeObject<MailConfig>(System.IO.File.ReadAllText(@"mailconfig.json"));
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient(mailData.smtp_server);

                mail.From = new MailAddress(mailData.username);
                mail.To.Add(Mail_Send_To.Text);
                mail.Subject = Mail_Send_Subject.Text;
                mail.Body = Mail_Send_Content.Text;

                client.UseDefaultCredentials = false;
                client.Port = mailData.smtp_port;
                client.Credentials = new System.Net.NetworkCredential(mailData.username, mailData.password);
                client.EnableSsl = true;

                client.Send(mail);
                MessageBox.Show("Correo enviado");
            }
        }
    }

    class MailConfig
    {
        public string username, password, smtp_server;
        public int smtp_port;

        public MailConfig(string username, string password, string smtp_server, int smtp_port)
        {
            this.username = username;
            this.password = password;
            this.smtp_server = smtp_server;
            this.smtp_port = smtp_port;
        } 
    }

    class MailData
    {

    }
}
