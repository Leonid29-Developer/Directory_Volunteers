using System;
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

        private void Main_Load(object sender, EventArgs e)
        {
            //Сохранение изображения
            string filename = @"E:\314.jpg"; byte[] imageData;
            using (FileStream fs = new FileStream(filename, FileMode.Open)) { imageData = new byte[fs.Length]; fs.Read(imageData, 0, imageData.Length); }

            using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
            {
                SQL_Connection.Open(); SqlCommand SQL_Command = SQL_Connection.CreateCommand();
                string Request = $"UPDATE [DirectoryOfVolunteers].[dbo].[Images] SET [Image] = @ImageData WHERE [ID] = 'I1';"; // SQL-запрос
                SQL_Command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000); SQL_Command.Parameters["@ImageData"].Value = imageData;
                SQL_Command.CommandText = Request; SQL_Command.ExecuteNonQuery(); SQL_Connection.Close();
            }

            //Использование без DataGridView
            //DataTable matcher_query = new DataTable(); SqlDataAdapter da = new SqlDataAdapter(cmd); da.Fill(dataTable);

            //Загрузка изображения
            byte[] Im = null;
            using (SqlConnection SQL_Connection = new SqlConnection(Authorization.ConnectString))
            {
                SQL_Connection.Open();
                string Request = $"SELECT [Image] FROM [DirectoryOfVolunteers].[dbo].[Images] WHERE [ID] = 'I1';"; // SQL-запрос
                SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                while (Reader.Read()) { Im = (byte[])Reader.GetValue(0); }
                SQL_Connection.Close();
            }

            Image newImage;
            using (MemoryStream ms = new MemoryStream(Im, 0, Im.Length)) { ms.Write(Im, 0, Im.Length); newImage = Image.FromStream(ms, true, true); }
            pictureBox1.BackgroundImage = newImage;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) => System.Windows.Forms.Application.OpenForms["Authorization"].Show();

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}