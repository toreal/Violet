namespace keago0403
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必要的設計工具變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.circle_btn = this.Factory.CreateRibbonButton();
            this.rectangle_btn = this.Factory.CreateRibbonButton();
            this.line_btn = this.Factory.CreateRibbonButton();
            this.Curve = this.Factory.CreateRibbonButton();
            this.Color = this.Factory.CreateRibbonGroup();
            this.red_btn = this.Factory.CreateRibbonButton();
            this.orange_btn = this.Factory.CreateRibbonButton();
            this.yellow_btn = this.Factory.CreateRibbonButton();
            this.green_btn = this.Factory.CreateRibbonButton();
            this.black_btn = this.Factory.CreateRibbonButton();
            this.blue_btn = this.Factory.CreateRibbonButton();
            this.white_btn = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.StrokeThickness = this.Factory.CreateRibbonMenu();
            this.px1 = this.Factory.CreateRibbonButton();
            this.px3 = this.Factory.CreateRibbonButton();
            this.px5 = this.Factory.CreateRibbonButton();
            this.button2 = this.Factory.CreateRibbonButton();
            this.button1 = this.Factory.CreateRibbonButton();
            this.button11 = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.Color.SuspendLayout();
            this.group2.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.Color);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Violet";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.circle_btn);
            this.group1.Items.Add(this.rectangle_btn);
            this.group1.Items.Add(this.line_btn);
            this.group1.Items.Add(this.Curve);
            this.group1.Label = "Shape";
            this.group1.Name = "group1";
            // 
            // circle_btn
            // 
            this.circle_btn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.circle_btn.Image = global::keago0403.Properties.Resources.circle;
            this.circle_btn.Label = "Circle";
            this.circle_btn.Name = "circle_btn";
            this.circle_btn.ShowImage = true;
            this.circle_btn.Tag = "";
            this.circle_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.circle_btn_Click);
            // 
            // rectangle_btn
            // 
            this.rectangle_btn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.rectangle_btn.Image = global::keago0403.Properties.Resources.rectangle;
            this.rectangle_btn.Label = "Rectangle";
            this.rectangle_btn.Name = "rectangle_btn";
            this.rectangle_btn.ShowImage = true;
            this.rectangle_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.rectangle_btn_Click);
            // 
            // line_btn
            // 
            this.line_btn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.line_btn.Image = global::keago0403.Properties.Resources.line;
            this.line_btn.Label = "Line";
            this.line_btn.Name = "line_btn";
            this.line_btn.ShowImage = true;
            this.line_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.line_btn_Click);
            // 
            // Curve
            // 
            this.Curve.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.Curve.Image = global::keago0403.Properties.Resources._123;
            this.Curve.Label = "Curve";
            this.Curve.Name = "Curve";
            this.Curve.ShowImage = true;
            this.Curve.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Curve_Click);
            // 
            // Color
            // 
            this.Color.Items.Add(this.red_btn);
            this.Color.Items.Add(this.orange_btn);
            this.Color.Items.Add(this.yellow_btn);
            this.Color.Items.Add(this.green_btn);
            this.Color.Items.Add(this.black_btn);
            this.Color.Items.Add(this.blue_btn);
            this.Color.Items.Add(this.white_btn);
            this.Color.Label = "Color";
            this.Color.Name = "Color";
            // 
            // red_btn
            // 
            this.red_btn.Image = global::keago0403.Properties.Resources.red;
            this.red_btn.Label = " ";
            this.red_btn.Name = "red_btn";
            this.red_btn.ShowImage = true;
            this.red_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.red_btn_Click);
            // 
            // orange_btn
            // 
            this.orange_btn.Image = global::keago0403.Properties.Resources.orange;
            this.orange_btn.Label = " ";
            this.orange_btn.Name = "orange_btn";
            this.orange_btn.ShowImage = true;
            this.orange_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button7_Click);
            // 
            // yellow_btn
            // 
            this.yellow_btn.Image = global::keago0403.Properties.Resources.yellow1;
            this.yellow_btn.Label = " ";
            this.yellow_btn.Name = "yellow_btn";
            this.yellow_btn.ShowImage = true;
            this.yellow_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.yellow_btn_Click);
            // 
            // green_btn
            // 
            this.green_btn.Image = global::keago0403.Properties.Resources.green;
            this.green_btn.Label = " ";
            this.green_btn.Name = "green_btn";
            this.green_btn.ShowImage = true;
            this.green_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.green_btn_Click);
            // 
            // black_btn
            // 
            this.black_btn.Image = global::keago0403.Properties.Resources.black;
            this.black_btn.Label = " ";
            this.black_btn.Name = "black_btn";
            this.black_btn.ShowImage = true;
            this.black_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.black_btn_Click);
            // 
            // blue_btn
            // 
            this.blue_btn.Image = global::keago0403.Properties.Resources.blue;
            this.blue_btn.Label = " ";
            this.blue_btn.Name = "blue_btn";
            this.blue_btn.ShowImage = true;
            this.blue_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.blue_btn_Click);
            // 
            // white_btn
            // 
            this.white_btn.Image = global::keago0403.Properties.Resources.white1;
            this.white_btn.Label = " ";
            this.white_btn.Name = "white_btn";
            this.white_btn.ShowImage = true;
            this.white_btn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.white_btn_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.StrokeThickness);
            this.group2.Items.Add(this.button2);
            this.group2.Items.Add(this.button1);
            this.group2.Items.Add(this.button11);
            this.group2.Label = "Other";
            this.group2.Name = "group2";
            // 
            // StrokeThickness
            // 
            this.StrokeThickness.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.StrokeThickness.Items.Add(this.px1);
            this.StrokeThickness.Items.Add(this.px3);
            this.StrokeThickness.Items.Add(this.px5);
            this.StrokeThickness.Label = "StrokeThickness";
            this.StrokeThickness.Name = "StrokeThickness";
            this.StrokeThickness.ShowImage = true;
            // 
            // px1
            // 
            this.px1.Image = global::keago0403.Properties.Resources.px1;
            this.px1.Label = "px1";
            this.px1.Name = "px1";
            this.px1.ShowImage = true;
            this.px1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.px1_Click);
            // 
            // px3
            // 
            this.px3.Image = global::keago0403.Properties.Resources.background1;
            this.px3.Label = "px3";
            this.px3.Name = "px3";
            this.px3.ShowImage = true;
            this.px3.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.px3_Click);
            // 
            // px5
            // 
            this.px5.Image = global::keago0403.Properties.Resources.background1;
            this.px5.Label = "px5";
            this.px5.Name = "px5";
            this.px5.ShowImage = true;
            this.px5.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.px5_Click);
            // 
            // button2
            // 
            this.button2.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button2.Image = global::keago0403.Properties.Resources.backpoint;
            this.button2.Label = "Background Point";
            this.button2.Name = "button2";
            this.button2.ShowImage = true;
            this.button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button1.Image = global::keago0403.Properties.Resources.clear1;
            this.button1.Label = " Clear";
            this.button1.Name = "button1";
            this.button1.ShowImage = true;
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // button11
            // 
            this.button11.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button11.Image = global::keago0403.Properties.Resources.text;
            this.button11.Label = "Text";
            this.button11.Name = "button11";
            this.button11.ShowImage = true;
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.Color.ResumeLayout(false);
            this.Color.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton circle_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton rectangle_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton line_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Curve;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Color;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton red_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton orange_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton yellow_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton green_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton black_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton blue_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton white_btn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button11;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu StrokeThickness;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton px1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton px3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton px5;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
