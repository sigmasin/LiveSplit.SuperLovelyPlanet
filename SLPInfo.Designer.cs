namespace SuperLovelyPlanet {
	partial class SLPInfo {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lblNote = new System.Windows.Forms.Label();
			this.lblLevel = new System.Windows.Forms.Label();
			this.lblLevelText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblNote
			// 
			this.lblNote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblNote.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNote.Location = new System.Drawing.Point(0, 0);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(433, 76);
			this.lblNote.TabIndex = 3;
			this.lblNote.Text = "Not available";
			this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblLevel
			// 
			this.lblLevel.AutoSize = true;
			this.lblLevel.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLevel.Location = new System.Drawing.Point(12, 9);
			this.lblLevel.Name = "lblLevel";
			this.lblLevel.Size = new System.Drawing.Size(87, 21);
			this.lblLevel.TabIndex = 0;
			this.lblLevel.Text = "Level: ";
			// 
			// lblLevelText
			// 
			this.lblLevelText.AutoSize = true;
			this.lblLevelText.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLevelText.Location = new System.Drawing.Point(23, 33);
			this.lblLevelText.Name = "lblLevelText";
			this.lblLevelText.Size = new System.Drawing.Size(76, 21);
			this.lblLevelText.TabIndex = 4;
			this.lblLevelText.Text = "Time: ";
			// 
			// SLPInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(433, 76);
			this.Controls.Add(this.lblNote);
			this.Controls.Add(this.lblLevelText);
			this.Controls.Add(this.lblLevel);
			this.Name = "SLPInfo";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblNote;
		private System.Windows.Forms.Label lblLevel;
		private System.Windows.Forms.Label lblLevelText;
	}
}