﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{

    public partial class Form1 : Form
    {
        bool showNeighborCount;
        bool showGrid;
        bool showHud;
        bool wrap;
        int width;
        int height;
        bool[,] grid1;
        bool[,] grid2;

        SolidBrush Cell_Color;
        Pen Grid_Color;
        SolidBrush Back_Color;
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;

            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            Cell_Color = new SolidBrush(Properties.Settings.Default.CellColor);
            Grid_Color = new Pen(Properties.Settings.Default.GridColor);
            Back_Color = new SolidBrush(Properties.Settings.Default.BackColor);

            showNeighborCount = Properties.Settings.Default.showNeighborCount;
            showGrid = Properties.Settings.Default.showGrid;
            showHud = Properties.Settings.Default.showHud;

            wrap = Properties.Settings.Default.Wrap;
            width = Properties.Settings.Default.Width;
            height = Properties.Settings.Default.Height;

            grid1 = new bool[width, height];
            grid2 = new bool[width, height];

            timer.Interval = Properties.Settings.Default.TimerInterval;
        }


        float boxWidth = 0;
        float boxHeight = 0;

        int generation = 0;

        Box Selection = null;
        Pen select_pen = new Pen(Color.Red, 3);
        Pen paste_pen = new Pen(Color.Blue, 3);
        bool pasting = false;
        bool[,] clipboard = null;

        Font f;
        private void graphicsPanel1_Load(object sender, EventArgs e)
        {
            boxWidth = (float)graphicsPanel1.Width / (float)width;
            boxHeight = (float)graphicsPanel1.Height / (float)height;

            timer.Tick += new EventHandler(TimerUpdate);

            f = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
        }


        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            int count = 0;
            Graphics g = e.Graphics;
            g.FillRectangle(Back_Color, 0, 0, graphicsPanel1.Width - 1, graphicsPanel1.Height - 1);
            g.DrawRectangle(Grid_Color, 0, 0, graphicsPanel1.Width-1, graphicsPanel1.Height-1);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (grid1[x, y])
                    {
                        count++;
                        g.FillRectangle(Cell_Color, x * boxWidth, y * boxHeight, boxWidth, boxHeight);
                    }
                    else
                    {
                        if(showGrid)
                            g.DrawRectangle(Grid_Color, x * boxWidth, y * boxHeight, boxWidth, boxHeight);

                    }
                    if (showNeighborCount)
                        g.DrawString($"{getNeighbors(grid1, x, y)}", f, Brushes.Red, x * boxWidth, y * boxHeight);
                }
            if (Selection != null)
            {
                Rectangle sel = Selection.rec;
                g.DrawRectangle(pasting ? paste_pen : select_pen, sel.X * boxWidth, sel.Y * boxHeight, sel.Width * boxWidth, sel.Height * boxHeight);
            }
            if (clipboard != null && pasting)
            {
                Rectangle sel = Selection.rec;
                for (int x = 0; x < clipboard.GetLength(0); x++)
                    for (int y = 0; y < clipboard.GetLength(1); y++)
                        if (clipboard[x, y])
                            g.FillRectangle(Brushes.Blue, (sel.X + x) * boxWidth, (sel.Y + y) * boxHeight, boxWidth, boxHeight);
            }
            if(showHud)
                g.DrawString($"Generation: {generation}\nAlive Cells: {count}", f, Brushes.Red, 16, 16);
            
        }

        private void TimerUpdate(Object myObject, EventArgs myEventArgs)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int count = getNeighbors(grid1, x, y);
                    grid2[x, y] = (count == 3 || (count == 2) && grid1[x,y]);
                }
            }
            generation++;
            bool[,] temp = grid1;
            grid1 = grid2;
            grid2 = temp;
            graphicsPanel1.Invalidate();
        }

        private int getNeighbors(bool[,] grid, int x, int y)
        {
            return get(grid, x-1, y - 1) + get(grid, x - 1, y) + get(grid, x - 1, y + 1) + get(grid, x, y - 1) +
                get(grid, x, y+1) + get(grid, x + 1, y-1) + get(grid, x + 1, y) + get(grid, x + 1, y+1);
        }

        private int get(bool[,] grid, int x, int y)
        {
            if (wrap)
                return grid[(x+width)%width, (y+height)%height] ? 1 : 0;
            if (x < 0 || x >= width|| y < 0 || y >= height)
                return 0;
            return grid[x, y]?1:0;
        }

        Image play = LoadImage("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAABMSURBVDhPtYxBDgAgDML2 / 09rcCZjtyGxhGMbP1j3z6wzI5IBI1IB7CHCek6MsFoTIqz1DSOs9FmBoQxYywkyYFWWgSUDSwaWLBKxARiAb5H143r4AAAAAElFTkSuQmCC");
        Image stop = LoadImage("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAABcSURBVDhPzYuxDcAwDMP8 / 9NpBaKDEA7pFgJcbGquYv10Y73XI9MyaTQ20zJpNDbTMmk0NtMyaTQ20zJpNDbTMmk0NtMyaTQ20zJpNDbTMmk0NtMyab7HqVcw8wCPlfMN3SzxigAAAABJRU5ErkJggg == ");

        public static Image LoadImage(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
                image = Image.FromStream(ms);
            return image;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
            updatePlayButton();
        }

        private void updatePlayButton()
        {
            if (timer.Enabled)
                toolStripButton8.Image = stop;
            else
                toolStripButton8.Image = play;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            TimerUpdate(null, null);
        }

        bool firstpress = false;
        bool firstOn = false;

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (int)(e.X / boxWidth);
            int y = (int)(e.Y / boxHeight);
            if (!pasting)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!firstpress)
                    {
                        firstpress = true;
                        firstOn = grid1[x, y];
                    }
                    if (grid1[x, y] == firstOn)
                    {
                        grid1[x, y] = !firstOn;
                        graphicsPanel1.Invalidate();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (!firstpress)
                    {
                        firstpress = true;
                        //clipboard = null;
                        Selection = null;
                    }
                    else
                    {
                        if(Selection.Width>0 || Selection.Height>0)
                            clipboard = new bool[Selection.Width, Selection.Height];
                    }
                    graphicsPanel1.Invalidate();
                }
                firstpress = false;
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int _x = 0; _x < clipboard.GetLength(0); _x++)
                        for (int _y = 0; _y < clipboard.GetLength(1); _y++)
                            if (!(_x + Selection.P1.X >= width || _x + Selection.P1.X < 0 || _y + Selection.P1.Y >= height || _y + Selection.P1.Y < 0))
                                grid1[_x + Selection.P1.X, _y + Selection.P1.Y] = clipboard[_x, _y];
                    pasting = false;
                    Selection = null;
                    graphicsPanel1.Invalidate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    Selection.P1.X = x;
                    Selection.P1.Y = y;
                    Selection.P2.X = x + clipboard.GetLength(0)-1;
                    Selection.P2.Y = y + clipboard.GetLength(1)-1;
                    graphicsPanel1.Invalidate();
                }
            }
        }

        private void graphicsPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = (int)(e.X / boxWidth);
            int y = (int)(e.Y / boxHeight);
            if (x >= width || x < 0 || y >= height || y < 0)
                return;
            if (e.Button == MouseButtons.Left)
            {
                if (!pasting)
                {
                    if (!firstpress)
                    {
                        firstpress = true;
                        firstOn = grid1[x, y];
                    }
                    if (grid1[x, y] == firstOn)
                    {
                        grid1[x, y] = !firstOn;
                        graphicsPanel1.Invalidate();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (pasting)
                {
                    Selection.P1.X = x;
                    Selection.P1.Y = y;
                    Selection.P2.X = x + clipboard.GetLength(0) - 1;
                    Selection.P2.Y = y + clipboard.GetLength(1) - 1;
                }
                else
                {
                    if (!firstpress || Selection == null)
                    {
                        firstpress = true;
                        Selection = new Box(x, y, 0, 0);
                    }
                    Selection.P2.X = x;
                    Selection.P2.Y = y;
                }
                graphicsPanel1.Invalidate();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timer.Stop();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid1[x, y] = grid2[x,y] = false;
            graphicsPanel1.Invalidate();
            updatePlayButton();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (Selection != null)
            {
                Rectangle rec = Selection.rec;
                clipboard = new bool[Selection.Width, Selection.Height];
                for (int x = 0; x < clipboard.GetLength(0); x++)
                    for (int y = 0; y < clipboard.GetLength(1); y++)
                        clipboard[x, y] = grid1[x + rec.X, y + rec.Y];
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (clipboard != null)
            {
                if (Selection == null)
                    Selection = new Box(0, 0, clipboard.GetLength(0)-1, clipboard.GetLength(1)-1);
                pasting = true;
            }
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (Selection != null)
            {
                Rectangle rec = Selection.rec;
                clipboard = new bool[Selection.Width, Selection.Height];
                for (int x = 0; x < clipboard.GetLength(0); x++)
                    for (int y = 0; y < clipboard.GetLength(1); y++)
                    {
                        clipboard[x, y] = grid1[x + rec.X, y + rec.Y];
                        grid1[x + rec.X, y + rec.Y] = false;
                    }
            }
            graphicsPanel1.Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "Cells File|*.cells";
            FileDialog.Title = "Saving a cells file";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                List<char[]> lines = new List<char[]>();
                foreach (string line in File.ReadLines(FileDialog.FileName))
                {
                    if(!line.StartsWith("!"))
                        lines.Add(line.ToCharArray());
                }
                char[][] char_array = lines.ToArray();

                SetGridSize(char_array[0].Length, char_array.Length);
                for (int i = 0; i < grid1.GetLength(1); i++)
                    for (int j = 0; j < grid1.GetLength(0); j++)
                        grid1[j, i] = grid2[j, i] = (char_array[i][j] == 'O');
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Cells File|*.cells";
            saveFileDialog1.Title = "Saving a cells file";
            saveFileDialog1.ShowDialog();

            using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName))
            {
                file.Write("!" + grid1.GetLength(0) + " " + grid1.GetLength(1) + "\n");
                for (int i = 0; i < grid1.GetLength(1); i++)
                {
                    for (int j = 0; j < grid1.GetLength(0); j++)
                        file.Write(grid1[i, j] ? "O" : ".");
                    file.Write("\n");
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid1[x, y] = grid2[x, y] = false;
            graphicsPanel1.Invalidate();
            updatePlayButton();
        }

        private void generateRandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    grid1[x, y] = grid2[x, y] = (rand.NextDouble()>0.5);
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Resize(object sender, EventArgs e)
        {
            boxWidth = (float)graphicsPanel1.Width / (float)width;
            boxHeight = (float)graphicsPanel1.Height / (float)height;
            graphicsPanel1.Invalidate();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "Cells File|*.cells";
            FileDialog.Title = "Saving a cells file";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                List<char[]> lines = new List<char[]>();
                foreach (string line in File.ReadLines(FileDialog.FileName))
                {
                    if (!line.StartsWith("!"))
                        lines.Add(line.ToCharArray());
                }
                char[][] char_array = lines.ToArray();

                clipboard = new bool[char_array[0].Length, char_array.Length];
                for (int i = 0; i < clipboard.GetLength(1); i++)
                    for (int j = 0; j < clipboard.GetLength(0); j++)
                        clipboard[j, i] = (char_array[i][j] == 'O');
                Selection = new Box(0, 0, clipboard.GetLength(0) - 1, clipboard.GetLength(1) - 1);
                pasting = true;

                graphicsPanel1.Invalidate();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Cells File|*.cells";
            saveFileDialog1.Title = "Saving a cells file";
            saveFileDialog1.ShowDialog();

            using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName))
            {
                file.Write("!" + clipboard.GetLength(0) + " " + clipboard.GetLength(1) + "\n");
                for (int i = 0; i < clipboard.GetLength(1); i++)
                {
                    for (int j = 0; j < clipboard.GetLength(0); j++)
                        file.Write(clipboard[j, i] ? "O" : ".");
                    file.Write("\n");
                }
            }
        }

        private void updateSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateSpeed us = new UpdateSpeed(timer.Interval);
            us.ShowDialog();
            timer.Interval = us.getSpeed();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Cells File|*.cells";
            saveFileDialog1.Title = "Saving a cells file";
            saveFileDialog1.ShowDialog();

            using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName))
            {
                file.Write("!" + grid1.GetLength(0) + " " + grid1.GetLength(1) + "\n");
                for (int i = 0; i < grid1.GetLength(1); i++)
                {
                    for (int j = 0; j < grid1.GetLength(0); j++)
                        file.Write(grid1[i, j]?"O":".");
                    file.Write("\n");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "Cells File|*.cells";
            FileDialog.Title = "Saving a cells file";
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                List<char[]> lines = new List<char[]>();
                foreach (string line in File.ReadLines(FileDialog.FileName))
                {
                    if (!line.StartsWith("!"))
                        lines.Add(line.ToCharArray());
                }
                char[][] char_array = lines.ToArray();

                SetGridSize(char_array[0].Length, char_array.Length);
                for (int i = 0; i < grid1.GetLength(1); i++)
                    for (int j = 0; j < grid1.GetLength(0); j++)
                        grid1[j, i] = grid2[j, i] = (char_array[i][j] == 'O');
            }
        }

        private void SetGridSize(int a, int b)
        {
            width = a;
            height = b;
            grid1 = new bool[a, b];
            grid2 = new bool[a, b];

            boxWidth = (float)graphicsPanel1.Width / (float)width;
            boxHeight = (float)graphicsPanel1.Height / (float)height;

            graphicsPanel1.Invalidate();
        }

        private void setSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 ss = new Form2(width, height);
            ss.ShowDialog();

            SetGridSize(ss.getWidth(), ss.getHeight());

            graphicsPanel1.Invalidate();
        }

        private void toggleNeighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNeighborCount = !showNeighborCount;
            graphicsPanel1.Invalidate();
        }

        private void toggleGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showGrid = !showGrid;
            graphicsPanel1.Invalidate();
        }

        private void toggleHudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showHud = !showHud;
            graphicsPanel1.Invalidate();
        }

        private void zeroEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wrap = false;
        }

        private void wrappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wrap = true;
        }

        private void colorSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorPicker cp = new ColorPicker(Cell_Color.Color, Grid_Color.Color, Back_Color.Color);
            cp.ShowDialog();
            Cell_Color = new SolidBrush(cp.Color_Cell);
            Grid_Color = new Pen(cp.Color_Grid);
            Back_Color = new SolidBrush(cp.Color_Background);

            graphicsPanel1.Invalidate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.CellColor = Cell_Color.Color;
            Properties.Settings.Default.GridColor = Grid_Color.Color;
            Properties.Settings.Default.BackColor = Back_Color.Color;

            Properties.Settings.Default.showGrid = showGrid;
            Properties.Settings.Default.showHud = showHud;
            Properties.Settings.Default.showNeighborCount = showNeighborCount;

            Properties.Settings.Default.Wrap = wrap;
            Properties.Settings.Default.Width = width;
            Properties.Settings.Default.Height = height;

            Properties.Settings.Default.TimerInterval = timer.Interval;

            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();

            Cell_Color = new SolidBrush(Properties.Settings.Default.CellColor);
            Grid_Color = new Pen(Properties.Settings.Default.GridColor);
            Back_Color = new SolidBrush(Properties.Settings.Default.BackColor);

            showNeighborCount = Properties.Settings.Default.showNeighborCount;
            showGrid = Properties.Settings.Default.showGrid;
            showHud = Properties.Settings.Default.showHud;

            wrap = Properties.Settings.Default.Wrap;
            width = Properties.Settings.Default.Width;
            height = Properties.Settings.Default.Height;

            timer.Interval = Properties.Settings.Default.TimerInterval;

            graphicsPanel1.Invalidate();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            Cell_Color = new SolidBrush(Properties.Settings.Default.CellColor);
            Grid_Color = new Pen(Properties.Settings.Default.GridColor);
            Back_Color = new SolidBrush(Properties.Settings.Default.BackColor);

            showNeighborCount = Properties.Settings.Default.showNeighborCount;
            showGrid = Properties.Settings.Default.showGrid;
            showHud = Properties.Settings.Default.showHud;

            wrap = Properties.Settings.Default.Wrap;
            width = Properties.Settings.Default.Width;
            height = Properties.Settings.Default.Height;

            timer.Interval = Properties.Settings.Default.TimerInterval;
            graphicsPanel1.Invalidate();
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string file = files[0];

                List<char[]> lines = new List<char[]>();
                foreach (string line in File.ReadLines(file))
                {
                    if (!line.StartsWith("!"))
                        lines.Add(line.ToCharArray());
                }
                char[][] char_array = lines.ToArray();

                clipboard = new bool[char_array[0].Length, char_array.Length];
                for (int i = 0; i < clipboard.GetLength(1); i++)
                    for (int j = 0; j < clipboard.GetLength(0); j++)
                        clipboard[j, i] = (char_array[i][j] == 'O');

                Point screenCoords = Cursor.Position;
                Point crc = this.graphicsPanel1.PointToClient(screenCoords);
                Selection = new Box((int)(crc.X/boxWidth), (int)(crc.Y/boxHeight), clipboard.GetLength(0) - 1, clipboard.GetLength(1) - 1);
                pasting = true;

                graphicsPanel1.Invalidate();
            }
            
        }
    }

    public class Box
    {
        public Point P1;
        public Point P2;

        public int Width
        {
            get
            {
                return Math.Abs(P1.X - P2.X) + 1;
            }
        }

        public int Height
        {
            get
            {
                return Math.Abs(P1.Y - P2.Y) + 1;
            }
        }

        public Rectangle rec
        {
            get
            {
               return new Rectangle(Math.Min(P1.X, P2.X),
               Math.Min(P1.Y, P2.Y),
               Math.Abs(P1.X - P2.X)+1,
               Math.Abs(P1.Y - P2.Y)+1);
            }
        }

        public Box(int x, int y, int width, int height)
        {
            P1 = new Point(x, y);
            P2 = new Point(x+width, y+height);
        }
    }
}
