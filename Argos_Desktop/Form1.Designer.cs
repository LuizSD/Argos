namespace Argos_Desktop
{
    partial class Form1
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
            this.buttonApagador1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelResponse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonApagador1
            // 
            this.buttonApagador1.Location = new System.Drawing.Point(12, 12);
            this.buttonApagador1.Name = "buttonApagador1";
            this.buttonApagador1.Size = new System.Drawing.Size(75, 23);
            this.buttonApagador1.TabIndex = 0;
            this.buttonApagador1.Text = "Executar";
            this.buttonApagador1.UseVisualStyleBackColor = true;
            this.buttonApagador1.Click += new System.EventHandler(this.buttonApagador1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // labelResponse
            // 
            this.labelResponse.AutoSize = true;
            this.labelResponse.Location = new System.Drawing.Point(109, 17);
            this.labelResponse.Name = "labelResponse";
            this.labelResponse.Size = new System.Drawing.Size(13, 13);
            this.labelResponse.TabIndex = 2;
            this.labelResponse.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 97);
            this.Controls.Add(this.labelResponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonApagador1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arduino";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApagador1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelResponse;
    }
}

