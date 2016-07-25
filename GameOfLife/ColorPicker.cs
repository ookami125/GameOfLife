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
    public partial class ColorPicker : Form
    {
        public Color Color_Cell;
        public Color Color_Grid;
        public Color Color_Background;

        public ColorPicker(Color c1, Color c2, Color c3)
        {
            InitializeComponent();

            Color_Cell = c1;
            Color_Grid = c2;
            Color_Background = c3;

            btn_Cell_Color.BackColor = Color_Cell;
            btn_Grid_Color.BackColor = Color_Grid;
            btn_Back_Color.BackColor = Color_Background;
        }

        private void btn_Cell_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                Color_Cell = cd.Color;
                btn_Cell_Color.BackColor = Color_Cell;
            }
        }

        private void btn_Grid_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Color_Grid = cd.Color;
                btn_Grid_Color.BackColor = Color_Grid;
            }
        }

        private void btn_Back_Color_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Color_Background = cd.Color;
                btn_Back_Color.BackColor = Color_Background;
            }
        }
    }
}
