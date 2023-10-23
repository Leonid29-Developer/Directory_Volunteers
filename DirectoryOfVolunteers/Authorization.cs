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
                string SQL = $"SELECT [Nickname],[Password] FROM [DirectoryOfVolunteers].[dbo].[DataForAccess] WHERE [Login] = '{TB_Login.Text}';"; // SQL-запрос
                SqlDataAdapter data = new SqlDataAdapter(SQL, ConnectString); DataSet Set = new DataSet(); data.Fill(Set, "[]"); DATA.DataSource = Set.Tables["[]"].DefaultView; DATA.DataSource = Set.Tables["[]"].DefaultView;
                if (DATA.RowCount - 1 > 0) if (TB_Password.Text == DATA.Rows[0].Cells[1].Value.ToString()) { Main.Nickname = DATA.Rows[0].Cells[0].Value.ToString(); Hide(); new Main().ShowDialog(); } else T = false;
            }
            else T = false; if (T == false) MessageBox.Show("Неверные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Кнопки быстрой авторизации
        private void pictureBox1_Click(object sender, EventArgs e) { TB_Login.Text = "Mariscarl"; TB_Password.Text = "sMgYIIwQ"; }
    }
}