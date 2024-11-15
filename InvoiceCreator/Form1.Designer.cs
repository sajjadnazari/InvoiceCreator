namespace InvoiceCreator
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
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            openFileDialog1 = new OpenFileDialog();
            txtProduct = new TextBox();
            txtTransaction = new TextBox();
            progressBar1 = new ProgressBar();
            txtInvoiceNumber = new TextBox();
            panel1 = new Panel();
            txtSavePath = new TextBox();
            button4 = new Button();
            label5 = new Label();
            button2 = new Button();
            button3 = new Button();
            txtDate = new TextBox();
            label4 = new Label();
            label3 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            radioPrice = new RadioButton();
            radioInvoice = new RadioButton();
            groupBox1 = new GroupBox();
            radioDesc = new RadioButton();
            radioAsc = new RadioButton();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.GreenYellow;
            button1.Location = new Point(35, 338);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(145, 56);
            button1.TabIndex = 0;
            button1.Text = "Create Invoice";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(21, 26);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(125, 20);
            label1.TabIndex = 1;
            label1.Text = "Products File Path";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(21, 70);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(143, 20);
            label2.TabIndex = 2;
            label2.Text = "Transaction File Path";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtProduct
            // 
            txtProduct.Enabled = false;
            txtProduct.Location = new Point(194, 26);
            txtProduct.Margin = new Padding(2);
            txtProduct.Name = "txtProduct";
            txtProduct.Size = new Size(639, 27);
            txtProduct.TabIndex = 3;
            // 
            // txtTransaction
            // 
            txtTransaction.Enabled = false;
            txtTransaction.Location = new Point(194, 70);
            txtTransaction.Margin = new Padding(2);
            txtTransaction.Name = "txtTransaction";
            txtTransaction.Size = new Size(639, 27);
            txtTransaction.TabIndex = 4;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(34, 434);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1071, 47);
            progressBar1.TabIndex = 5;
            // 
            // txtInvoiceNumber
            // 
            txtInvoiceNumber.Location = new Point(184, 35);
            txtInvoiceNumber.Name = "txtInvoiceNumber";
            txtInvoiceNumber.Size = new Size(157, 27);
            txtInvoiceNumber.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtSavePath);
            panel1.Controls.Add(button4);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(txtProduct);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtTransaction);
            panel1.Location = new Point(34, 21);
            panel1.Name = "panel1";
            panel1.Size = new Size(1071, 161);
            panel1.TabIndex = 7;
            panel1.Paint += panel1_Paint;
            // 
            // txtSavePath
            // 
            txtSavePath.Enabled = false;
            txtSavePath.Location = new Point(193, 116);
            txtSavePath.Name = "txtSavePath";
            txtSavePath.Size = new Size(639, 27);
            txtSavePath.TabIndex = 10;
            // 
            // button4
            // 
            button4.Location = new Point(838, 116);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 9;
            button4.Text = "Select";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(21, 120);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(157, 20);
            label5.TabIndex = 8;
            label5.Text = "Save file path (output)";
            // 
            // button2
            // 
            button2.Location = new Point(838, 26);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 7;
            button2.Text = "open";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(838, 68);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 6;
            button3.Text = "open";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // txtDate
            // 
            txtDate.Location = new Point(184, 70);
            txtDate.Name = "txtDate";
            txtDate.Size = new Size(157, 27);
            txtDate.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(15, 70);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(92, 20);
            label4.TabIndex = 9;
            label4.Text = "Invoice Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(15, 38);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(149, 20);
            label3.TabIndex = 5;
            label3.Text = "Invoice Number Start";
            // 
            // radioPrice
            // 
            radioPrice.AutoSize = true;
            radioPrice.Location = new Point(25, 68);
            radioPrice.Name = "radioPrice";
            radioPrice.Size = new Size(83, 24);
            radioPrice.TabIndex = 17;
            radioPrice.TabStop = true;
            radioPrice.Text = "Amount";
            radioPrice.UseVisualStyleBackColor = true;
            // 
            // radioInvoice
            // 
            radioInvoice.AutoSize = true;
            radioInvoice.Location = new Point(25, 38);
            radioInvoice.Name = "radioInvoice";
            radioInvoice.Size = new Size(132, 24);
            radioInvoice.TabIndex = 16;
            radioInvoice.TabStop = true;
            radioInvoice.Text = "Invoice number";
            radioInvoice.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioInvoice);
            groupBox1.Controls.Add(radioPrice);
            groupBox1.Location = new Point(450, 197);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(193, 117);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Order by";
            // 
            // radioDesc
            // 
            radioDesc.AutoSize = true;
            radioDesc.Location = new Point(33, 70);
            radioDesc.Name = "radioDesc";
            radioDesc.Size = new Size(108, 24);
            radioDesc.TabIndex = 19;
            radioDesc.TabStop = true;
            radioDesc.Text = "Descending";
            radioDesc.UseVisualStyleBackColor = true;
            // 
            // radioAsc
            // 
            radioAsc.AutoSize = true;
            radioAsc.Location = new Point(33, 34);
            radioAsc.Name = "radioAsc";
            radioAsc.Size = new Size(99, 24);
            radioAsc.TabIndex = 18;
            radioAsc.TabStop = true;
            radioAsc.Text = "Ascending";
            radioAsc.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtDate);
            groupBox2.Controls.Add(txtInvoiceNumber);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(34, 197);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(394, 117);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Initialize";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioAsc);
            groupBox3.Controls.Add(radioDesc);
            groupBox3.Location = new Point(665, 197);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(250, 117);
            groupBox3.TabIndex = 20;
            groupBox3.TabStop = false;
            groupBox3.Text = "Sort type";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(1131, 493);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(panel1);
            Controls.Add(progressBar1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Invoice Creator v1.0.01";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private TextBox txtProduct;
        private TextBox txtTransaction;
        private ProgressBar progressBar1;
        private TextBox txtInvoiceNumber;
        private Panel panel1;
        private Label label3;
        private TextBox txtDate;
        private Label label4;
        private Button button3;
        private Button button2;
        private Button button4;
        private Label label5;
        private TextBox txtSavePath;
        private FolderBrowserDialog folderBrowserDialog1;
        private RadioButton radioPrice;
        private RadioButton radioInvoice;
        private GroupBox groupBox1;
        private RadioButton radioAsc;
        private GroupBox groupBox2;
        private RadioButton radioDesc;
        private GroupBox groupBox3;
    }
}
