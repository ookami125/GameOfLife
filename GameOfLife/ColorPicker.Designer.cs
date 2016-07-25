namespace GameOfLife
{
    partial class ColorPicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Cell_Color = new System.Windows.Forms.Button();
            this.btn_Grid_Color = new System.Windows.Forms.Button();
            this.btn_Back_Color = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Cell_Color
            // 
            this.btn_Cell_Color.Location = new System.Drawing.Point(13, 13);
            this.btn_Cell_Color.Name = "btn_Cell_Color";
            this.btn_Cell_Color.Size = new System.Drawing.Size(23, 23);
            this.btn_Cell_Color.TabIndex = 0;
            this.btn_Cell_Color.UseVisualStyleBackColor = true;
            this.btn_Cell_Color.Click += new System.EventHandler(this.btn_Cell_Color_Click);
            // 
            // btn_Grid_Color
            // 
            this.btn_Grid_Color.Location = new System.Drawing.Point(13, 43);
            this.btn_Grid_Color.Name = "btn_Grid_Color";
            this.btn_Grid_Color.Size = new System.Drawing.Size(23, 23);
            this.btn_Grid_Color.TabIndex = 1;
            this.btn_Grid_Color.UseVisualStyleBackColor = true;
            this.btn_Grid_Color.Click += new System.EventHandler(this.btn_Grid_Color_Click);
            // 
            // btn_Back_Color
            // 
            this.btn_Back_Color.Location = new System.Drawing.Point(13, 73);
            this.btn_Back_Color.Name = "btn_Back_Color";
            this.btn_Back_Color.Size = new System.Drawing.Size(23, 23);
            this.btn_Back_Color.TabIndex = 2;
            this.btn_Back_Color.UseVisualStyleBackColor = true;
            this.btn_Back_Color.Click += new System.EventHandler(this.btn_Back_Color_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cell Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Grid Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Background Color";
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 107);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Back_Color);
            this.Controls.Add(this.btn_Grid_Color);
            this.Controls.Add(this.btn_Cell_Color);
            this.Name = "ColorPicker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Cell_Color;
        private System.Windows.Forms.Button btn_Grid_Color;
        private System.Windows.Forms.Button btn_Back_Color;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}