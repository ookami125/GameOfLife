using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class GraphicsPanel : UserControl
    {
        public GraphicsPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
    }
}
