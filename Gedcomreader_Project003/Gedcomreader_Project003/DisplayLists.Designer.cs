namespace Gedcomreader_Project003
{
    partial class DisplayLists
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
            this.Individuals = new System.Windows.Forms.ListBox();
            this.Families = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Individuals
            // 
            this.Individuals.FormattingEnabled = true;
            this.Individuals.Location = new System.Drawing.Point(32, 12);
            this.Individuals.Name = "Individuals";
            this.Individuals.Size = new System.Drawing.Size(792, 186);
            this.Individuals.TabIndex = 0;
            // 
            // Families
            // 
            this.Families.FormattingEnabled = true;
            this.Families.IntegralHeight = false;
            this.Families.Location = new System.Drawing.Point(32, 204);
            this.Families.Name = "Families";
            this.Families.Size = new System.Drawing.Size(792, 186);
            this.Families.TabIndex = 1;
            // 
            // DisplayLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 424);
            this.Controls.Add(this.Families);
            this.Controls.Add(this.Individuals);
            this.Name = "DisplayLists";
            this.Text = "DisplayLists";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Individuals;
        private System.Windows.Forms.ListBox Families;
    }
}