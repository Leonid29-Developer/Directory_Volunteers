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

            //Установка главной формы
            Panel P = new Panel { Name = Nickname }; P.Click += UserChanged;

            //Вывод данных в список волонтеров
            Table.Controls.Clear(); Table.AutoScroll = false; Table.AutoScroll = true; Table.HorizontalScroll.Visible = false;

            for (int CountUser = 0; CountUser < Users.Count; CountUser++)
            {
                Panel Main = new Panel { Name = Users[CountUser].Nickname, Size = new Size(Table.Width - 6, 120), BorderStyle = BorderStyle.FixedSingle };
                {
                    //Аватарка
                    PictureBox Picture = new PictureBox { Name = Users[CountUser].Nickname, BorderStyle = BorderStyle.Fixed3D, BackgroundImageLayout = ImageLayout.Stretch, Size = new Size(100, 100), Top = 10, Left = 10 };
                    using (MemoryStream MS = new MemoryStream(Users[CountUser].ProfilePicture, 0, Users[CountUser].ProfilePicture.Length)) { MS.Write(Users[CountUser].ProfilePicture, 0, Users[CountUser].ProfilePicture.Length); Picture.BackgroundImage = Image.FromStream(MS, true, true); }

                    //Ник
                    Label Nick = new Label { Name = Users[CountUser].Nickname, Text = Users[CountUser].Nickname, AutoSize = false, Size = new Size(Main.Width - 130, 50), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 18), TextAlign = ContentAlignment.MiddleCenter, Top = 15, Left = 120 };

                    //ФИО
                    Label FIO = new Label { Name = Users[CountUser].Nickname, Text = Users[CountUser].FIO, AutoSize = false, Size = new Size(Main.Width - 130, 30), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 11), TextAlign = ContentAlignment.MiddleCenter, Top = 75, Left = 120 };

                    //Добавление на форму
                    Picture.Click += UserChanged; Main.Controls.Add(Picture); Nick.Click += UserChanged; Main.Controls.Add(Nick);
                    FIO.Click += UserChanged; Main.Controls.Add(FIO); Main.Click += UserChanged; Table.Controls.Add(Main);
                }
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
        }

        private void UserChanged(object sender, EventArgs e) { }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) => System.Windows.Forms.Application.OpenForms["Authorization"].Show();
    }
}