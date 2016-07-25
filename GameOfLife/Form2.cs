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
    public partial class Form2 : Form
    {
        public Form2(int width, int height)
        {
            InitializeComponent();
            numericUpDown1.Value = width;
            numericUpDown2.Value = height;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int getWidth()
        {
            return (int)numericUpDown1.Value;
        }

        public int getHeight()
        {
            return (int)numericUpDown2.Value;
        }
    }
}
