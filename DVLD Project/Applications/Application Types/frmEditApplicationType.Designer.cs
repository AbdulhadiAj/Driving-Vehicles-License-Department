namespace DVLD_Project.Applications.Applications_Types
{
    partial class frmEditApplicationType
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblApplicationTypeID = new System.Windows.Forms.Label();
            this.tbApplicationTypeTitle = new System.Windows.Forms.TextBox();
            this.btnSavePerson = new System.Windows.Forms.Button();
            this.tbApplicationTypeFees = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(388, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "Application Type ID: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "Application Type Title: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Application Type Fees: ";
            // 
            // lblApplicationTypeID
            // 
            this.lblApplicationTypeID.AutoSize = true;
            this.lblApplicationTypeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationTypeID.Location = new System.Drawing.Point(185, 53);
            this.lblApplicationTypeID.Name = "lblApplicationTypeID";
            this.lblApplicationTypeID.Size = new System.Drawing.Size(36, 20);
            this.lblApplicationTypeID.TabIndex = 21;
            this.lblApplicationTypeID.Text = "???";
            // 
            // tbApplicationTypeTitle
            // 
            this.tbApplicationTypeTitle.Location = new System.Drawing.Point(204, 98);
            this.tbApplicationTypeTitle.Name = "tbApplicationTypeTitle";
            this.tbApplicationTypeTitle.Size = new System.Drawing.Size(150, 20);
            this.tbApplicationTypeTitle.TabIndex = 22;
            this.tbApplicationTypeTitle.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // btnSavePerson
            // 
            this.btnSavePerson.BackColor = System.Drawing.Color.Green;
            this.btnSavePerson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePerson.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSavePerson.Location = new System.Drawing.Point(180, 192);
            this.btnSavePerson.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSavePerson.Name = "btnSavePerson";
            this.btnSavePerson.Size = new System.Drawing.Size(67, 38);
            this.btnSavePerson.TabIndex = 24;
            this.btnSavePerson.Text = "Save";
            this.btnSavePerson.UseVisualStyleBackColor = false;
            this.btnSavePerson.Click += new System.EventHandler(this.btnSavePerson_Click);
            // 
            // tbApplicationTypeFees
            // 
            this.tbApplicationTypeFees.Location = new System.Drawing.Point(204, 143);
            this.tbApplicationTypeFees.Name = "tbApplicationTypeFees";
            this.tbApplicationTypeFees.Size = new System.Drawing.Size(150, 20);
            this.tbApplicationTypeFees.TabIndex = 23;
            this.tbApplicationTypeFees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbApplicationTypeFees_KeyPress);
            this.tbApplicationTypeFees.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmEditApplicationType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 244);
            this.Controls.Add(this.btnSavePerson);
            this.Controls.Add(this.tbApplicationTypeFees);
            this.Controls.Add(this.tbApplicationTypeTitle);
            this.Controls.Add(this.lblApplicationTypeID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmEditApplicationType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmEditApplicationType";
            this.Load += new System.EventHandler(this.frmEditApplicationType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblApplicationTypeID;
        private System.Windows.Forms.TextBox tbApplicationTypeTitle;
        private System.Windows.Forms.Button btnSavePerson;
        private System.Windows.Forms.TextBox tbApplicationTypeFees;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}