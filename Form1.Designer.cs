
using System;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.txtArquivoTexto = new System.Windows.Forms.TextBox();
            this.TxtTexto = new System.Windows.Forms.TextBox();
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            this.listData = new System.Windows.Forms.DataGridView();
            this.TxtLines = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnGravarCSV = new System.Windows.Forms.Button();
            this.btnAdicionarFichiro = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listData)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // btnProcurar
            // 
            this.btnProcurar.Location = new System.Drawing.Point(549, 39);
            this.btnProcurar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Size = new System.Drawing.Size(171, 31);
            this.btnProcurar.TabIndex = 1;
            this.btnProcurar.Text = "Procurar";
            this.btnProcurar.UseVisualStyleBackColor = true;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // txtArquivoTexto
            // 
            this.txtArquivoTexto.Location = new System.Drawing.Point(14, 40);
            this.txtArquivoTexto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtArquivoTexto.Name = "txtArquivoTexto";
            this.txtArquivoTexto.Size = new System.Drawing.Size(527, 27);
            this.txtArquivoTexto.TabIndex = 3;
            // 
            // TxtTexto
            // 
            this.TxtTexto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtTexto.Location = new System.Drawing.Point(3, 3);
            this.TxtTexto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtTexto.Multiline = true;
            this.TxtTexto.Name = "TxtTexto";
            this.TxtTexto.Size = new System.Drawing.Size(1429, 523);
            this.TxtTexto.TabIndex = 4;
            // 
            // ofd1
            // 
            this.ofd1.FileName = "openFileDialog1";
            // 
            // listData
            // 
            this.listData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.listData.Location = new System.Drawing.Point(3, 3);
            this.listData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listData.MultiSelect = false;
            this.listData.Name = "listData";
            this.listData.ReadOnly = true;
            this.listData.RowHeadersWidth = 51;
            this.listData.RowTemplate.Height = 25;
            this.listData.Size = new System.Drawing.Size(1429, 523);
            this.listData.TabIndex = 5;
            // 
            // TxtLines
            // 
            this.TxtLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TxtLines.Location = new System.Drawing.Point(3, 3);
            this.TxtLines.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtLines.Multiline = true;
            this.TxtLines.Name = "TxtLines";
            this.TxtLines.Size = new System.Drawing.Size(1429, 523);
            this.TxtLines.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 685);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(97, 678);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(125, 27);
            this.txtTotal.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(24, 93);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1443, 562);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listData);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1435, 529);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grelha Items";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TxtLines);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1435, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Linhas texto descodificadas";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TxtTexto);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1435, 529);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Ficheiro Pdf->Txt";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnGravarCSV
            // 
            this.btnGravarCSV.Location = new System.Drawing.Point(737, 40);
            this.btnGravarCSV.Name = "btnGravarCSV";
            this.btnGravarCSV.Size = new System.Drawing.Size(157, 29);
            this.btnGravarCSV.TabIndex = 10;
            this.btnGravarCSV.Text = "Gravar Ficheiro CSV";
            this.btnGravarCSV.UseVisualStyleBackColor = true;
            this.btnGravarCSV.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAdicionarFichiro
            // 
            this.btnAdicionarFichiro.Location = new System.Drawing.Point(900, 41);
            this.btnAdicionarFichiro.Name = "btnAdicionarFichiro";
            this.btnAdicionarFichiro.Size = new System.Drawing.Size(206, 29);
            this.btnAdicionarFichiro.TabIndex = 11;
            this.btnAdicionarFichiro.Text = "Adicionar Ficheiro";
            this.btnAdicionarFichiro.UseVisualStyleBackColor = true;
            this.btnAdicionarFichiro.Click += new System.EventHandler(this.btnAdicionarFichiro_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1522, 871);
            this.Controls.Add(this.btnAdicionarFichiro);
            this.Controls.Add(this.btnGravarCSV);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtArquivoTexto);
            this.Controls.Add(this.btnProcurar);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listData)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcurar;       
        private System.Windows.Forms.TextBox txtArquivoTexto;
        private System.Windows.Forms.TextBox TxtTexto;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.DataGridView listData;
        private System.Windows.Forms.TextBox TxtLines;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGravarCSV;
        private System.Windows.Forms.Button btnAdicionarFichiro;
    }
}

