namespace QuestionModelGenerator
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
            this.label1 = new System.Windows.Forms.Label();
            this.inputValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.outputValue = new System.Windows.Forms.TextBox();
            this.Generate = new System.Windows.Forms.Button();
            this.generateCSHTML = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Value";
            // 
            // inputValue
            // 
            this.inputValue.Location = new System.Drawing.Point(54, 10);
            this.inputValue.Name = "inputValue";
            this.inputValue.Size = new System.Drawing.Size(735, 20);
            this.inputValue.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output";
            // 
            // outputValue
            // 
            this.outputValue.Location = new System.Drawing.Point(54, 46);
            this.outputValue.Multiline = true;
            this.outputValue.Name = "outputValue";
            this.outputValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputValue.Size = new System.Drawing.Size(735, 643);
            this.outputValue.TabIndex = 3;
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(705, 699);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(84, 23);
            this.Generate.TabIndex = 4;
            this.Generate.Text = "Generate CS";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // generateCSHTML
            // 
            this.generateCSHTML.Location = new System.Drawing.Point(592, 699);
            this.generateCSHTML.Name = "generateCSHTML";
            this.generateCSHTML.Size = new System.Drawing.Size(107, 23);
            this.generateCSHTML.TabIndex = 5;
            this.generateCSHTML.Text = "Generate CSHTML";
            this.generateCSHTML.UseVisualStyleBackColor = true;
            this.generateCSHTML.Click += new System.EventHandler(this.generateCSHTML_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 729);
            this.Controls.Add(this.generateCSHTML);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.outputValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputValue);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputValue;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.Button generateCSHTML;
    }
}

