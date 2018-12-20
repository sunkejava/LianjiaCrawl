namespace LianjiaCrawl
{
    partial class MenuStrip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuStrip));
            this.layeredPanel1 = new LayeredSkin.Controls.LayeredPanel();
            this.Button_About = new LayeredSkin.Controls.LayeredButton();
            this.Button_Set = new LayeredSkin.Controls.LayeredButton();
            this.Button_Exit = new LayeredSkin.Controls.LayeredButton();
            this.layeredPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layeredPanel1
            // 
            this.layeredPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.layeredPanel1.Borders.BottomColor = System.Drawing.Color.Empty;
            this.layeredPanel1.Borders.BottomWidth = 1;
            this.layeredPanel1.Borders.LeftColor = System.Drawing.Color.Empty;
            this.layeredPanel1.Borders.LeftWidth = 1;
            this.layeredPanel1.Borders.RightColor = System.Drawing.Color.Empty;
            this.layeredPanel1.Borders.RightWidth = 1;
            this.layeredPanel1.Borders.TopColor = System.Drawing.Color.Empty;
            this.layeredPanel1.Borders.TopWidth = 1;
            this.layeredPanel1.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("layeredPanel1.Canvas")));
            this.layeredPanel1.Controls.Add(this.Button_Exit);
            this.layeredPanel1.Controls.Add(this.Button_Set);
            this.layeredPanel1.Controls.Add(this.Button_About);
            this.layeredPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layeredPanel1.Location = new System.Drawing.Point(0, 0);
            this.layeredPanel1.Name = "layeredPanel1";
            this.layeredPanel1.Size = new System.Drawing.Size(89, 90);
            this.layeredPanel1.TabIndex = 0;
            // 
            // Button_About
            // 
            this.Button_About.AdaptImage = true;
            this.Button_About.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Button_About.BaseColor = System.Drawing.Color.Transparent;
            this.Button_About.Borders.BottomColor = System.Drawing.Color.Empty;
            this.Button_About.Borders.BottomWidth = 1;
            this.Button_About.Borders.LeftColor = System.Drawing.Color.Empty;
            this.Button_About.Borders.LeftWidth = 1;
            this.Button_About.Borders.RightColor = System.Drawing.Color.Empty;
            this.Button_About.Borders.RightWidth = 1;
            this.Button_About.Borders.TopColor = System.Drawing.Color.Empty;
            this.Button_About.Borders.TopWidth = 1;
            this.Button_About.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("Button_About.Canvas")));
            this.Button_About.ControlState = LayeredSkin.Controls.ControlStates.Normal;
            this.Button_About.HaloColor = System.Drawing.Color.White;
            this.Button_About.HaloSize = 5;
            this.Button_About.HoverImage = null;
            this.Button_About.IsPureColor = false;
            this.Button_About.Location = new System.Drawing.Point(0, 0);
            this.Button_About.Name = "Button_About";
            this.Button_About.NormalImage = null;
            this.Button_About.PressedImage = null;
            this.Button_About.Radius = 10;
            this.Button_About.ShowBorder = true;
            this.Button_About.Size = new System.Drawing.Size(89, 30);
            this.Button_About.TabIndex = 11;
            this.Button_About.Text = "关于";
            this.Button_About.TextLocationOffset = new System.Drawing.Point(0, 0);
            this.Button_About.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.Button_About.TextShowMode = LayeredSkin.TextShowModes.Halo;
            this.Button_About.Click += new System.EventHandler(this.Button_About_Click);
            // 
            // Button_Set
            // 
            this.Button_Set.AdaptImage = true;
            this.Button_Set.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Button_Set.BaseColor = System.Drawing.Color.Transparent;
            this.Button_Set.Borders.BottomColor = System.Drawing.Color.Empty;
            this.Button_Set.Borders.BottomWidth = 1;
            this.Button_Set.Borders.LeftColor = System.Drawing.Color.Empty;
            this.Button_Set.Borders.LeftWidth = 1;
            this.Button_Set.Borders.RightColor = System.Drawing.Color.Empty;
            this.Button_Set.Borders.RightWidth = 1;
            this.Button_Set.Borders.TopColor = System.Drawing.Color.Empty;
            this.Button_Set.Borders.TopWidth = 1;
            this.Button_Set.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("Button_Set.Canvas")));
            this.Button_Set.ControlState = LayeredSkin.Controls.ControlStates.Normal;
            this.Button_Set.HaloColor = System.Drawing.Color.White;
            this.Button_Set.HaloSize = 5;
            this.Button_Set.HoverImage = null;
            this.Button_Set.IsPureColor = false;
            this.Button_Set.Location = new System.Drawing.Point(0, 29);
            this.Button_Set.Name = "Button_Set";
            this.Button_Set.NormalImage = null;
            this.Button_Set.PressedImage = null;
            this.Button_Set.Radius = 10;
            this.Button_Set.ShowBorder = true;
            this.Button_Set.Size = new System.Drawing.Size(89, 30);
            this.Button_Set.TabIndex = 12;
            this.Button_Set.Text = "设置";
            this.Button_Set.TextLocationOffset = new System.Drawing.Point(0, 0);
            this.Button_Set.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.Button_Set.TextShowMode = LayeredSkin.TextShowModes.Halo;
            this.Button_Set.Click += new System.EventHandler(this.Button_Set_Click);
            // 
            // Button_Exit
            // 
            this.Button_Exit.AdaptImage = true;
            this.Button_Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Button_Exit.BaseColor = System.Drawing.Color.Transparent;
            this.Button_Exit.Borders.BottomColor = System.Drawing.Color.Empty;
            this.Button_Exit.Borders.BottomWidth = 1;
            this.Button_Exit.Borders.LeftColor = System.Drawing.Color.Empty;
            this.Button_Exit.Borders.LeftWidth = 1;
            this.Button_Exit.Borders.RightColor = System.Drawing.Color.Empty;
            this.Button_Exit.Borders.RightWidth = 1;
            this.Button_Exit.Borders.TopColor = System.Drawing.Color.Empty;
            this.Button_Exit.Borders.TopWidth = 1;
            this.Button_Exit.Canvas = ((System.Drawing.Bitmap)(resources.GetObject("Button_Exit.Canvas")));
            this.Button_Exit.ControlState = LayeredSkin.Controls.ControlStates.Normal;
            this.Button_Exit.HaloColor = System.Drawing.Color.White;
            this.Button_Exit.HaloSize = 5;
            this.Button_Exit.HoverImage = null;
            this.Button_Exit.IsPureColor = false;
            this.Button_Exit.Location = new System.Drawing.Point(0, 58);
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.NormalImage = null;
            this.Button_Exit.PressedImage = null;
            this.Button_Exit.Radius = 10;
            this.Button_Exit.ShowBorder = true;
            this.Button_Exit.Size = new System.Drawing.Size(89, 30);
            this.Button_Exit.TabIndex = 13;
            this.Button_Exit.Text = "退出";
            this.Button_Exit.TextLocationOffset = new System.Drawing.Point(0, 0);
            this.Button_Exit.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.Button_Exit.TextShowMode = LayeredSkin.TextShowModes.Halo;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // MenuStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LianjiaCrawl.Properties.Resources._471821;
            this.ClientSize = new System.Drawing.Size(89, 90);
            this.Controls.Add(this.layeredPanel1);
            this.DrawIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuStrip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "";
            this.Load += new System.EventHandler(this.MenuStrip_Load);
            this.layeredPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LayeredSkin.Controls.LayeredPanel layeredPanel1;
        private LayeredSkin.Controls.LayeredButton Button_About;
        private LayeredSkin.Controls.LayeredButton Button_Set;
        private LayeredSkin.Controls.LayeredButton Button_Exit;
    }
}