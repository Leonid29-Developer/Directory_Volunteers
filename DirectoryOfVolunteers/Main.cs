using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace DirectoryOfVolunteers
{
    public partial class Main : Form
    {
        public Main() => InitializeComponent();

        private List<UserData> Users = new List<UserData>();

        public static string Nickname { get; set; }

        private void Main_Load(object sender, EventArgs e)
        {
            //Загрузка данных
            using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
            {
                SQL_Connection.Open();
                string Request = $"EXEC [DirectoryOfVolunteers].[dbo].[UserData_ALL]"; // SQL-запрос
                SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read()) Users.Add(new UserData((string)Reader.GetValue(0), (byte[])Reader.GetValue(1), (string)Reader.GetValue(2)));
                SQL_Connection.Close();
            }

            ////Сохранение изображения
            //string filename = @"E:\314.jpg"; byte[] imageData;
            //using (FileStream fs = new FileStream(filename, FileMode.Open)) { imageData = new byte[fs.Length]; fs.Read(imageData, 0, imageData.Length); }

            //using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
            //{
            //    SQL_Connection.Open(); SqlCommand SQL_Command = SQL_Connection.CreateCommand();
            //    string Request = $"UPDATE [DirectoryOfVolunteers].[dbo].[Images] SET [Image] = @ImageData WHERE [ID] = 'I1';"; // SQL-запрос
            //    SQL_Command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000); SQL_Command.Parameters["@ImageData"].Value = imageData;
            //    SQL_Command.CommandText = Request; SQL_Command.ExecuteNonQuery(); SQL_Connection.Close();
            //}

            //for (int i = 0; i < Users.Count; i++) using (MemoryStream ms = new MemoryStream(Users[i].ProfilePicture, 0, Users[i].ProfilePicture.Length))
            //    { ms.Write(Users[i].ProfilePicture, 0, Users[i].ProfilePicture.Length); splitContainer1.Panel2.Controls[$"pictureBox{i + 1}"].BackgroundImage = Image.FromStream(ms, true, true); }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) => System.Windows.Forms.Application.OpenForms["Authorization"].Show();

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}