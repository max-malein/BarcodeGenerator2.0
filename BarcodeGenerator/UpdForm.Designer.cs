namespace BarcodeGenerator
{
    partial class UpdForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.number = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.date = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.orderNumber = new System.Windows.Forms.TextBox();
            this.saveUPD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер";
            // 
            // number
            // 
            this.number.Location = new System.Drawing.Point(59, 22);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(70, 20);
            this.number.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "от";
            // 
            // date
            // 
            this.date.Location = new System.Drawing.Point(159, 22);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(134, 20);
            this.date.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Номер заказа";
            // 
            // orderNumber
            // 
            this.orderNumber.Location = new System.Drawing.Point(98, 67);
            this.orderNumber.Name = "orderNumber";
            this.orderNumber.Size = new System.Drawing.Size(195, 20);
            this.orderNumber.TabIndex = 1;
            // 
            // saveUPD
            // 
            this.saveUPD.Location = new System.Drawing.Point(15, 120);
            this.saveUPD.Name = "saveUPD";
            this.saveUPD.Size = new System.Drawing.Size(278, 35);
            this.saveUPD.TabIndex = 3;
            this.saveUPD.Text = "Сохранить УПД";
            this.saveUPD.UseVisualStyleBackColor = true;
            this.saveUPD.Click += new System.EventHandler(this.saveUPD_Click);
            // 
            // UpdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 187);
            this.Controls.Add(this.saveUPD);
            this.Controls.Add(this.date);
            this.Controls.Add(this.orderNumber);
            this.Controls.Add(this.number);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UpdForm";
            this.Text = "UpdForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox number;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox orderNumber;
        private System.Windows.Forms.Button saveUPD;
    }
}