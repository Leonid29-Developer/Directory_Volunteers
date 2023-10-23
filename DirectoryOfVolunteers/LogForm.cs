using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryOfVolunteers
{
    public partial class LogForm : Form
    {
        public LogForm() => InitializeComponent();

        public DataGridView DA;

        private void LogForm_Load(object sender, EventArgs e) => DA = dataGridView1;
    }
}
