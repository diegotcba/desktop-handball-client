namespace HandballCliente
{
    partial class Posicion
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudPoints = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudLost = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudGoalDiff = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudGoalsFor = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudGoalsAgainst = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudDrawn = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudPlayed = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudWon = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTeam = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalsFor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalsAgainst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDrawn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(205, 163);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(124, 163);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 9;
            this.btnAceptar.Text = "&Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudPoints);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.nudLost);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.nudGoalDiff);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.nudGoalsFor);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.nudGoalsAgainst);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nudDrawn);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudPlayed);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudWon);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTeam);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 137);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // nudPoints
            // 
            this.nudPoints.Location = new System.Drawing.Point(55, 50);
            this.nudPoints.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudPoints.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPoints.Name = "nudPoints";
            this.nudPoints.Size = new System.Drawing.Size(39, 20);
            this.nudPoints.TabIndex = 1;
            this.nudPoints.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Pts.:";
            // 
            // nudLost
            // 
            this.nudLost.Location = new System.Drawing.Point(328, 79);
            this.nudLost.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudLost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLost.Name = "nudLost";
            this.nudLost.Size = new System.Drawing.Size(39, 20);
            this.nudLost.TabIndex = 5;
            this.nudLost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLost.ValueChanged += new System.EventHandler(this.nudLost_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(292, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "PP:";
            // 
            // nudGoalDiff
            // 
            this.nudGoalDiff.Location = new System.Drawing.Point(237, 105);
            this.nudGoalDiff.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudGoalDiff.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGoalDiff.Name = "nudGoalDiff";
            this.nudGoalDiff.Size = new System.Drawing.Size(39, 20);
            this.nudGoalDiff.TabIndex = 8;
            this.nudGoalDiff.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Dif:";
            // 
            // nudGoalsFor
            // 
            this.nudGoalsFor.Location = new System.Drawing.Point(55, 105);
            this.nudGoalsFor.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudGoalsFor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGoalsFor.Name = "nudGoalsFor";
            this.nudGoalsFor.Size = new System.Drawing.Size(41, 20);
            this.nudGoalsFor.TabIndex = 6;
            this.nudGoalsFor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGoalsFor.ValueChanged += new System.EventHandler(this.nudGoalsFor_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "GF:";
            // 
            // nudGoalsAgainst
            // 
            this.nudGoalsAgainst.Location = new System.Drawing.Point(147, 105);
            this.nudGoalsAgainst.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudGoalsAgainst.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGoalsAgainst.Name = "nudGoalsAgainst";
            this.nudGoalsAgainst.Size = new System.Drawing.Size(39, 20);
            this.nudGoalsAgainst.TabIndex = 7;
            this.nudGoalsAgainst.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGoalsAgainst.ValueChanged += new System.EventHandler(this.nudGoalsAgainst_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(111, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "GC:";
            // 
            // nudDrawn
            // 
            this.nudDrawn.Location = new System.Drawing.Point(237, 79);
            this.nudDrawn.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudDrawn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDrawn.Name = "nudDrawn";
            this.nudDrawn.Size = new System.Drawing.Size(39, 20);
            this.nudDrawn.TabIndex = 4;
            this.nudDrawn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDrawn.ValueChanged += new System.EventHandler(this.nudDrawn_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(201, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "PE:";
            // 
            // nudPlayed
            // 
            this.nudPlayed.Location = new System.Drawing.Point(55, 79);
            this.nudPlayed.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudPlayed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPlayed.Name = "nudPlayed";
            this.nudPlayed.Size = new System.Drawing.Size(41, 20);
            this.nudPlayed.TabIndex = 2;
            this.nudPlayed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "PJ:";
            // 
            // nudWon
            // 
            this.nudWon.Location = new System.Drawing.Point(147, 79);
            this.nudWon.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudWon.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWon.Name = "nudWon";
            this.nudWon.Size = new System.Drawing.Size(39, 20);
            this.nudWon.TabIndex = 3;
            this.nudWon.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWon.ValueChanged += new System.EventHandler(this.nudWon_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "PG:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Equipo:";
            // 
            // txtTeam
            // 
            this.txtTeam.Location = new System.Drawing.Point(55, 22);
            this.txtTeam.Name = "txtTeam";
            this.txtTeam.Size = new System.Drawing.Size(312, 20);
            this.txtTeam.TabIndex = 0;
            // 
            // Posicion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 200);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Posicion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Posicion";
            this.Load += new System.EventHandler(this.Posicion_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalsFor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGoalsAgainst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDrawn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlayed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTeam;
        private System.Windows.Forms.NumericUpDown nudPlayed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudWon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDrawn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudGoalDiff;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudGoalsFor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudGoalsAgainst;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudPoints;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudLost;
        private System.Windows.Forms.Label label10;
    }
}