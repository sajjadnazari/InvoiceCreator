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
            label6 = new Label();
            txtCheap = new TextBox();
            groupBox4 = new GroupBox();
            radioModeA = new RadioButton();
            radioModeB = new RadioButton();
            groupBox5 = new GroupBox();
            twoDecimalPointRadio = new RadioButton();
            oneDecimalPointRadio = new RadioButton();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.GreenYellow;
            button1.Location = new Point(31, 253);
            button1.Margin = new Padding(1);
            button1.Name = "button1";
            button1.Size = new Size(127, 42);
            button1.TabIndex = 0;
            button1.Text = "Create Invoice";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(18, 19);
            label1.Margin = new Padding(1, 0, 1, 0);
            label1.Name = "label1";
            label1.Size = new Size(102, 15);
            label1.TabIndex = 1;
            label1.Text = "Products File Path";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(18, 53);
            label2.Margin = new Padding(1, 0, 1, 0);
            label2.Name = "label2";
            label2.Size = new Size(115, 15);
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
            txtProduct.Location = new Point(169, 19);
            txtProduct.Margin = new Padding(1);
            txtProduct.Name = "txtProduct";
            txtProduct.Size = new Size(560, 23);
            txtProduct.TabIndex = 3;
            // 
            // txtTransaction
            // 
            txtTransaction.Enabled = false;
            txtTransaction.Location = new Point(169, 53);
            txtTransaction.Margin = new Padding(1);
            txtTransaction.Name = "txtTransaction";
            txtTransaction.Size = new Size(560, 23);
            txtTransaction.TabIndex = 4;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(29, 325);
            progressBar1.Margin = new Padding(3, 2, 3, 2);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(937, 35);
            progressBar1.TabIndex = 5;
            // 
            // txtInvoiceNumber
            // 
            txtInvoiceNumber.Location = new Point(136, 26);
            txtInvoiceNumber.Margin = new Padding(3, 2, 3, 2);
            txtInvoiceNumber.Name = "txtInvoiceNumber";
            txtInvoiceNumber.Size = new Size(138, 23);
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
            panel1.Location = new Point(29, 16);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(937, 121);
            panel1.TabIndex = 7;
            panel1.Paint += panel1_Paint;
            // 
            // txtSavePath
            // 
            txtSavePath.Enabled = false;
            txtSavePath.Location = new Point(169, 87);
            txtSavePath.Margin = new Padding(3, 2, 3, 2);
            txtSavePath.Name = "txtSavePath";
            txtSavePath.Size = new Size(560, 23);
            txtSavePath.TabIndex = 10;
            // 
            // button4
            // 
            button4.Location = new Point(734, 87);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(83, 22);
            button4.TabIndex = 9;
            button4.Text = "Select";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(18, 90);
            label5.Margin = new Padding(1, 0, 1, 0);
            label5.Name = "label5";
            label5.Size = new Size(124, 15);
            label5.TabIndex = 8;
            label5.Text = "Save file path (output)";
            // 
            // button2
            // 
            button2.Location = new Point(734, 19);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(83, 22);
            button2.TabIndex = 7;
            button2.Text = "open";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(734, 51);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(83, 22);
            button3.TabIndex = 6;
            button3.Text = "open";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // txtDate
            // 
            txtDate.Location = new Point(136, 53);
            txtDate.Margin = new Padding(3, 2, 3, 2);
            txtDate.Name = "txtDate";
            txtDate.Size = new Size(138, 23);
            txtDate.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(13, 53);
            label4.Margin = new Padding(1, 0, 1, 0);
            label4.Name = "label4";
            label4.Size = new Size(72, 15);
            label4.TabIndex = 9;
            label4.Text = "Invoice Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(13, 29);
            label3.Margin = new Padding(1, 0, 1, 0);
            label3.Name = "label3";
            label3.Size = new Size(119, 15);
            label3.TabIndex = 5;
            label3.Text = "Invoice Number Start";
            // 
            // radioPrice
            // 
            radioPrice.AutoSize = true;
            radioPrice.Location = new Point(10, 51);
            radioPrice.Margin = new Padding(3, 2, 3, 2);
            radioPrice.Name = "radioPrice";
            radioPrice.Size = new Size(69, 19);
            radioPrice.TabIndex = 17;
            radioPrice.TabStop = true;
            radioPrice.Text = "Amount";
            radioPrice.UseVisualStyleBackColor = true;
            // 
            // radioInvoice
            // 
            radioInvoice.AutoSize = true;
            radioInvoice.Location = new Point(10, 28);
            radioInvoice.Margin = new Padding(3, 2, 3, 2);
            radioInvoice.Name = "radioInvoice";
            radioInvoice.Size = new Size(108, 19);
            radioInvoice.TabIndex = 16;
            radioInvoice.TabStop = true;
            radioInvoice.Text = "Invoice number";
            radioInvoice.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioInvoice);
            groupBox1.Controls.Add(radioPrice);
            groupBox1.Location = new Point(360, 148);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(124, 88);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Order by";
            // 
            // radioDesc
            // 
            radioDesc.AutoSize = true;
            radioDesc.Location = new Point(15, 54);
            radioDesc.Margin = new Padding(3, 2, 3, 2);
            radioDesc.Name = "radioDesc";
            radioDesc.Size = new Size(87, 19);
            radioDesc.TabIndex = 19;
            radioDesc.TabStop = true;
            radioDesc.Text = "Descending";
            radioDesc.UseVisualStyleBackColor = true;
            // 
            // radioAsc
            // 
            radioAsc.AutoSize = true;
            radioAsc.Location = new Point(15, 25);
            radioAsc.Margin = new Padding(3, 2, 3, 2);
            radioAsc.Name = "radioAsc";
            radioAsc.Size = new Size(81, 19);
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
            groupBox2.Location = new Point(29, 148);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(308, 88);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "Initialize";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioAsc);
            groupBox3.Controls.Add(radioDesc);
            groupBox3.Location = new Point(510, 148);
            groupBox3.Margin = new Padding(3, 2, 3, 2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 2, 3, 2);
            groupBox3.Size = new Size(131, 88);
            groupBox3.TabIndex = 20;
            groupBox3.TabStop = false;
            groupBox3.Text = "Sort type";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(247, 267);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(90, 15);
            label6.TabIndex = 21;
            label6.Text = "Cheap Producs:";
            // 
            // txtCheap
            // 
            txtCheap.BorderStyle = BorderStyle.FixedSingle;
            txtCheap.Location = new Point(346, 267);
            txtCheap.Margin = new Padding(2);
            txtCheap.Multiline = true;
            txtCheap.Name = "txtCheap";
            txtCheap.Size = new Size(616, 29);
            txtCheap.TabIndex = 22;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(radioModeA);
            groupBox4.Controls.Add(radioModeB);
            groupBox4.Location = new Point(662, 148);
            groupBox4.Margin = new Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 2, 3, 2);
            groupBox4.Size = new Size(129, 88);
            groupBox4.TabIndex = 23;
            groupBox4.TabStop = false;
            groupBox4.Text = "Chunk Mode";
            // 
            // radioModeA
            // 
            radioModeA.AutoSize = true;
            radioModeA.Checked = true;
            radioModeA.Location = new Point(17, 25);
            radioModeA.Margin = new Padding(3, 2, 3, 2);
            radioModeA.Name = "radioModeA";
            radioModeA.Size = new Size(67, 19);
            radioModeA.TabIndex = 18;
            radioModeA.TabStop = true;
            radioModeA.Text = "Mode A";
            radioModeA.UseVisualStyleBackColor = true;
            // 
            // radioModeB
            // 
            radioModeB.AutoSize = true;
            radioModeB.Location = new Point(17, 51);
            radioModeB.Margin = new Padding(3, 2, 3, 2);
            radioModeB.Name = "radioModeB";
            radioModeB.Size = new Size(66, 19);
            radioModeB.TabIndex = 19;
            radioModeB.Text = "Mode B";
            radioModeB.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(twoDecimalPointRadio);
            groupBox5.Controls.Add(oneDecimalPointRadio);
            groupBox5.Location = new Point(811, 148);
            groupBox5.Margin = new Padding(3, 2, 3, 2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 2, 3, 2);
            groupBox5.Size = new Size(155, 88);
            groupBox5.TabIndex = 24;
            groupBox5.TabStop = false;
            groupBox5.Text = "Decimal digit";
            // 
            // twoDecimalPointRadio
            // 
            twoDecimalPointRadio.AutoSize = true;
            twoDecimalPointRadio.Checked = true;
            twoDecimalPointRadio.Location = new Point(17, 24);
            twoDecimalPointRadio.Margin = new Padding(3, 2, 3, 2);
            twoDecimalPointRadio.Name = "twoDecimalPointRadio";
            twoDecimalPointRadio.Size = new Size(127, 19);
            twoDecimalPointRadio.TabIndex = 18;
            twoDecimalPointRadio.TabStop = true;
            twoDecimalPointRadio.Text = "Two decimal places";
            twoDecimalPointRadio.UseVisualStyleBackColor = true;
            // 
            // oneDecimalPointRadio
            // 
            oneDecimalPointRadio.AutoSize = true;
            oneDecimalPointRadio.Location = new Point(17, 50);
            oneDecimalPointRadio.Margin = new Padding(3, 2, 3, 2);
            oneDecimalPointRadio.Name = "oneDecimalPointRadio";
            oneDecimalPointRadio.Size = new Size(123, 19);
            oneDecimalPointRadio.TabIndex = 19;
            oneDecimalPointRadio.Text = "One decimal place";
            oneDecimalPointRadio.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSteelBlue;
            ClientSize = new Size(990, 370);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(txtCheap);
            Controls.Add(label6);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(panel1);
            Controls.Add(progressBar1);
            Margin = new Padding(1);
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
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Label label6;
        private TextBox txtCheap;
        private GroupBox groupBox4;
        private RadioButton radioModeA;
        private RadioButton radioModeB;
        private GroupBox groupBox5;
        private RadioButton twoDecimalPointRadio;
        private RadioButton oneDecimalPointRadio;
    }
}
