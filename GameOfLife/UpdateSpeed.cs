using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class UpdateSpeed : Form
    {
        public UpdateSpeed(int interval)
        {
            InitializeComponent();
            numericUpDown1.Value = interval;
        }

        public int getSpeed()
        {
            return (int)numericUpDown1.Value;
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
