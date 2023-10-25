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
        public static string Nickname { get; set; }

        private List<UserData> Users = new List<UserData>(); private bool Clubs_Sections = false; private bool Eventis = false; private int ShowUserID = -1;

        private void Main_Load(object sender, EventArgs e)
        {
            //Загрузка данных
            using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
            {
                SQL_Connection.Open();
                string Request = $"EXEC [DirectoryOfVolunteers].[dbo].[UserData_ALL]"; // SQL-запрос
                SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read()) Users.Add(new UserData((string)Reader.GetValue(0), (byte[])Reader.GetValue(1), (string)Reader.GetValue(2), (string)Reader.GetValue(3), (string)Reader.GetValue(4), (string)Reader.GetValue(5), (string)Reader.GetValue(6)));
                SQL_Connection.Close();
            }

            //Установка главной формы
            Panel P = new Panel { Name = Nickname }; P.Click += UserChanged;

            //Генерация списка волонтеров
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

        private void UserChanged(object sender, EventArgs e)
        {
            SplitContainer.Panel2.Controls.Clear(); var Element_Panel = new Panel(); var Element_PictureBox = new PictureBox(); var Element_Label = new Label();

            switch (sender.GetType().ToString())
            {
                case "System.Windows.Forms.Panel": Element_Panel = (Panel)sender; break;
                case "System.Windows.Forms.PictureBox": Element_PictureBox = (PictureBox)sender; break;
                case "System.Windows.Forms.Label": Element_Label = (Label)sender; break;
            }

            //Создание элементов
            {
                if (Element_Panel.Name != "" | Element_PictureBox.Name != "" | Element_Label.Name != "") for (int i = 0; i < Users.Count; i++)
                    {
                        if (Element_Panel != null) if (Users[i].Nickname == Element_Panel.Name) ShowUserID = i;
                        if (Element_PictureBox != null) if (Users[i].Nickname == Element_PictureBox.Name) ShowUserID = i;
                        if (Element_Label != null) if (Users[i].Nickname == Element_Label.Name) ShowUserID = i;
                    }

                //Аватарка
                PictureBox Picture = new PictureBox { BorderStyle = BorderStyle.Fixed3D, BackgroundImageLayout = ImageLayout.Stretch, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2), (int)((float)SplitContainer.Panel2.Width / 2)), Top = 15, Left = 15 };
                using (MemoryStream MS = new MemoryStream(Users[ShowUserID].ProfilePicture, 0, Users[ShowUserID].ProfilePicture.Length)) { MS.Write(Users[ShowUserID].ProfilePicture, 0, Users[ShowUserID].ProfilePicture.Length); Picture.BackgroundImage = Image.FromStream(MS, true, true); }

                //Ник
                Label Nick = new Label { Text = Users[ShowUserID].Nickname, AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2 - 35), 50), Font = new Font("Times New Roman", 18, FontStyle.Underline), TextAlign = ContentAlignment.MiddleCenter, Top = 25, Left = (int)((float)SplitContainer.Panel2.Width / 2) + 25 };

                //ФИО
                Label Heading_FIO = new Label { Text = "Фамилия Имя Отчество", AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2 - 45), 20), Font = new Font("Times New Roman", 10), ForeColor = Color.DarkOrange, TextAlign = ContentAlignment.MiddleLeft, Top = 90, Left = (int)((float)SplitContainer.Panel2.Width / 2) + 30 };
                Label FIO = new Label { Text = Users[ShowUserID].FIO, AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2 - 45), 30), Font = new Font("Times New Roman", 11), TextAlign = ContentAlignment.MiddleLeft, Top = 110, Left = (int)((float)SplitContainer.Panel2.Width / 2) + 30 };

                //Должность
                Label Heading_Position = new Label { Text = "Должность", AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2 - 45), 20), Font = new Font("Times New Roman", 10), ForeColor = Color.DarkOrange, TextAlign = ContentAlignment.MiddleLeft, Top = 150, Left = (int)((float)SplitContainer.Panel2.Width / 2) + 30 };
                Label Position = new Label { Text = Users[ShowUserID].Position, AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 2 - 45), 30), Font = new Font("Times New Roman", 11), TextAlign = ContentAlignment.MiddleLeft, Top = 170, Left = (int)((float)SplitContainer.Panel2.Width / 2) + 30 };

                //Клубы и Секции
                int Start1PositionY = 0;
                if (Clubs_Sections == true)
                {
                    if (Users[ShowUserID].Clubs_Sections == "")
                    {
                        Panel Head = new Panel { Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 100), BorderStyle = BorderStyle.FixedSingle, Top = (int)((float)SplitContainer.Panel2.Width / 2) + 40, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                        {
                            Label Heading_ClubsSections = new Label { Text = "Клубы и Секции\n▲", AutoSize = false, Size = new Size(Head.Width - 4, 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 1, Left = 1 };
                            Heading_ClubsSections.Click += ClubsSections_ShowOrHide; Head.Controls.Add(Heading_ClubsSections);

                            Label None = new Label { Text = "Отсутствует", AutoSize = false, Size = new Size(Head.Width - 4, 51), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 46, Left = 1 };
                            Head.Controls.Add(None); SplitContainer.Panel2.Controls.Add(Head); Start1PositionY = Head.Top + Head.Size.Height;
                        }
                    }
                    else
                    {
                        List<string> CS = new List<string>(); if (Users[ShowUserID].Clubs_Sections.Contains(" ")) { string[] D = Users[ShowUserID].Clubs_Sections.Split(' '); foreach (string User in D) CS.Add(User); } else CS.Add(Users[ShowUserID].Clubs_Sections);

                        Panel Head = new Panel { Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 54 + 47 * (int)Math.Ceiling((double)CS.Count / 2)), BorderStyle = BorderStyle.FixedSingle, Top = (int)((float)SplitContainer.Panel2.Width / 2) + 40, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                        {
                            Label Heading_ClubsSections = new Label { Text = "Клубы и Секции\n▲", AutoSize = false, Size = new Size(Head.Width - 4, 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 1, Left = 1 };
                            Heading_ClubsSections.Click += ClubsSections_ShowOrHide; Head.Controls.Add(Heading_ClubsSections); SplitContainer.Panel2.Controls.Add(Head); Start1PositionY = Head.Top + Head.Size.Height;

                            TableLayoutPanel Tab = new TableLayoutPanel { Size = new Size(Head.Width - 4, Head.Height - 49), ColumnCount = 2, CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble, BorderStyle = BorderStyle.FixedSingle, Top = 46, Left = 1 }; Head.Controls.Add(Tab);
                            {
                                foreach (string ClubsSections_ID in CS)
                                {
                                    //Загрузка данных
                                    using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
                                    {
                                        SQL_Connection.Open();
                                        string Request = $"EXEC [DirectoryOfVolunteers].[dbo].[Clubs and Sections_ID] @CS_ID = '{ClubsSections_ID}'"; // SQL-запрос
                                        SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                                        while (Reader.Read())
                                        {
                                            Label ClubsSections = new Label { Margin = new Padding(0, 0, 0, 0), BorderStyle = BorderStyle.FixedSingle, Text = (string)Reader.GetValue(0), AutoSize = false, Size = new Size((int)(float)(Tab.Width / 2) - 5, 44), Font = new Font("Times New Roman", 11), ForeColor = Color.DarkOrchid, TextAlign = ContentAlignment.MiddleCenter };
                                            Tab.Controls.Add(ClubsSections);
                                        }
                                        SQL_Connection.Close();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Label Heading_ClubsSections = new Label { Text = "Клубы и Секции\n▼", AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = (int)((float)SplitContainer.Panel2.Width / 2) + 30, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                    Heading_ClubsSections.Click += ClubsSections_ShowOrHide; SplitContainer.Panel2.Controls.Add(Heading_ClubsSections); Start1PositionY = Heading_ClubsSections.Top + Heading_ClubsSections.Size.Height;
                }

                //Мероприятия
                int Start2PositionY = 0;
                if (Eventis == true)
                {
                    List<string> Eventi = new List<string>();

                    //Загрузка данных
                    using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
                    {
                        SQL_Connection.Open();
                        string Request = $"EXEC [DirectoryOfVolunteers].[dbo].[Events_User] @Nickname = '{Users[ShowUserID].Nickname}'"; // SQL-запрос
                        SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                        while (Reader.Read()) Eventi.Add(Reader.GetString(0)); SQL_Connection.Close();
                    }

                    int Count_Rows = 2; if (Eventi.Count <= 4) Count_Rows = (int)Math.Ceiling((double)Eventi.Count / 2);


                    if (Count_Rows == 0)
                    {
                        Panel Head = new Panel { Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 100), BorderStyle = BorderStyle.FixedSingle, Top = Start1PositionY + 20, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                        {
                            Label Heading_Events = new Label { Text = "Мероприятия\n▲", AutoSize = false, Size = new Size(Head.Width - 4, 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 1, Left = 1 };
                            Heading_Events.Click += ClubsSections_ShowOrHide; Head.Controls.Add(Heading_Events);

                            Label None = new Label { Text = "Отсутствует", AutoSize = false, Size = new Size(Head.Width - 4, 51), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 46, Left = 1 };
                            Head.Controls.Add(None); SplitContainer.Panel2.Controls.Add(Head); Start2PositionY = Head.Top + Head.Size.Height;
                        }
                    }
                    else
                    {
                        Panel Head = new Panel { Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 54 + 47 * Count_Rows), BorderStyle = BorderStyle.FixedSingle, Top = Start1PositionY + 20, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                        {
                            Label Heading_Events = new Label { Text = "Мероприятия\n▲", AutoSize = false, Size = new Size(Head.Width - 4, 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = 1, Left = 1 };
                            Heading_Events.Click += Events_ShowOrHide; Head.Controls.Add(Heading_Events); SplitContainer.Panel2.Controls.Add(Head); Start2PositionY = Head.Top + Head.Size.Height;

                            TableLayoutPanel Tab = new TableLayoutPanel { Size = new Size(Head.Width - 4, Head.Height - 49), ColumnCount = 2, AutoScroll = true, CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble, BorderStyle = BorderStyle.FixedSingle, Top = 46, Left = 1 }; Head.Controls.Add(Tab);
                            {
                                foreach (string ClubsSections_ID in Eventi)
                                {
                                    Label Event = new Label { Margin = new Padding(0, 0, 0, 0), BorderStyle = BorderStyle.FixedSingle, Text = ClubsSections_ID, AutoSize = false, Size = new Size((int)(float)(Tab.Width / 2) - 5, 44), Font = new Font("Times New Roman", 11), ForeColor = Color.DarkOrchid, TextAlign = ContentAlignment.MiddleCenter };
                                    Tab.Controls.Add(Event);
                                }
                                Tab.HorizontalScroll.Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    Label Heading_Events = new Label { Text = "Мероприятия\n▼", AutoSize = false, Size = new Size((int)((float)SplitContainer.Panel2.Width / 4 * 3), 44), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Times New Roman", 12), TextAlign = ContentAlignment.MiddleCenter, Top = Start1PositionY + 20, Left = (int)((float)SplitContainer.Panel2.Width / 8) };
                    Heading_Events.Click += Events_ShowOrHide; SplitContainer.Panel2.Controls.Add(Heading_Events); Start2PositionY = Heading_Events.Top + Heading_Events.Size.Height;
                }

                //Контакты
                Panel Contacts = new Panel { Size = new Size((int)(SplitContainer.Panel2.Width * 0.9), 40), Top = Start2PositionY + 20, Left = (int)(SplitContainer.Panel2.Width * 0.05) };
                {
                    Label Heading_Contact_Phone = new Label { Text = "Телефон:", AutoSize = false, Size = new Size(68, 20), Font = new Font("Times New Roman", 11), ForeColor = Color.DarkOrange, TextAlign = ContentAlignment.MiddleCenter, Top = 10, Left = 1 };
                    Label Contact_Phone = new Label { Text = Users[ShowUserID].Phone, AutoSize = false, Size = new Size(124, 20), Font = new Font("Times New Roman", 11), TextAlign = ContentAlignment.MiddleLeft, Top = 10, Left = 69 };
                    Label Heading_Contact_Email = new Label { Text = "Email:", AutoSize = false, Size = new Size(50, 20), Font = new Font("Times New Roman", 11), ForeColor = Color.DarkOrange, TextAlign = ContentAlignment.MiddleCenter, Top = 10, Left = 202 };
                    Label Contact_Email = new Label { Text = Users[ShowUserID].Email, AutoSize = false, Size = new Size(225, 20), Font = new Font("Times New Roman", 11), TextAlign = ContentAlignment.MiddleLeft, AutoEllipsis = true, Top = 10, Left = 252 };
                    Contacts.Controls.Add(Heading_Contact_Phone); Contacts.Controls.Add(Contact_Phone); Contacts.Controls.Add(Heading_Contact_Email); Contacts.Controls.Add(Contact_Email);
                }

                SplitContainer.Panel2.Controls.Add(Picture); SplitContainer.Panel2.Controls.Add(Nick); SplitContainer.Panel2.Controls.Add(Heading_FIO); SplitContainer.Panel2.Controls.Add(FIO);
                SplitContainer.Panel2.Controls.Add(Heading_Position); SplitContainer.Panel2.Controls.Add(Position); SplitContainer.Panel2.Controls.Add(Contacts);
            }
        }

        private void ClubsSections_ShowOrHide(object sender, EventArgs e)
        { if (Clubs_Sections == true) Clubs_Sections = false; else Clubs_Sections = true; UserChanged(sender, e); }

        private void Events_ShowOrHide(object sender, EventArgs e)
        { if (Eventis == true) Eventis = false; else Eventis = true; UserChanged(sender, e); }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) => System.Windows.Forms.Application.OpenForms["Authorization"].Show();
    }
}