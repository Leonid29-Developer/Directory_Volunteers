using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DirectoryOfVolunteers
{
    public partial class Authorization : Form
    {
        public Authorization() => InitializeComponent();

        // Строка подключения
        public static string ConnectString = "Data Source=PC-LEONID29\\SQLEXPRESS;Integrated Security=True";

        private void Button_Input_Click(object sender, EventArgs e)
        {
            bool T = true;
            if (TB_Login.Text != "")
            {
                string Nickname = null, Password = null;
                using (SqlConnection SQL_Connection = new SqlConnection(ConnectString))
                {
                    SQL_Connection.Open();
                    string Request = $"EXEC [DirectoryOfVolunteers].[dbo].[Authorization] @Login = '{TB_Login.Text}';"; // SQL-запрос
                    SqlCommand Command = new SqlCommand(Request, SQL_Connection); SqlDataReader Reader = Command.ExecuteReader();
                    while (Reader.Read()) { Nickname = (string)Reader.GetValue(0); Password = (string)Reader.GetValue(1); }
                    SQL_Connection.Close();
                }

                if (TB_Password.Text == Password) { Main.Nickname = Nickname; Hide(); new Main().ShowDialog(); } else T = false;
            }
            else T = false; if (T == false) MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Кнопки быстрой авторизации
        private void pictureBox1_Click(object sender, EventArgs e) { TB_Login.Text = "Mariscarl"; TB_Password.Text = "sMgYIIwQ"; }
    }
}