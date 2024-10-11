using AntdUI;
using Notepad.Properties;

namespace Notepad
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            WindowBar = new PageHeader();
            LiteButton = new AntdUI.Button();
            SearchInput = new Input();
            OperationPanel = new AntdUI.Panel();
            ShowButton = new AntdUI.Button();
            SortButton = new AntdUI.Button();
            AddButton = new AntdUI.Button();
            SearchButton = new AntdUI.Button();
            FlowPanel = new FlowPanel();
            TooltipComponent = new TooltipComponent();
            WindowBar.SuspendLayout();
            OperationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // WindowBar
            // 
            WindowBar.Controls.Add(LiteButton);
            WindowBar.DividerShow = true;
            WindowBar.Dock = DockStyle.Top;
            WindowBar.Icon = Resources.icon;
            WindowBar.Location = new Point(0, 0);
            WindowBar.Name = "WindowBar";
            WindowBar.ShowButton = true;
            WindowBar.Size = new Size(936, 35);
            WindowBar.SubText = "";
            WindowBar.TabIndex = 0;
            WindowBar.TabStop = false;
            // 
            // LiteButton
            // 
            LiteButton.Dock = DockStyle.Right;
            LiteButton.Ghost = true;
            LiteButton.IconSize = new Size(24, 24);
            LiteButton.Location = new Point(744, 0);
            LiteButton.Name = "LiteButton";
            LiteButton.Radius = 0;
            LiteButton.Size = new Size(48, 35);
            LiteButton.TabIndex = 0;
            LiteButton.TabStop = false;
            LiteButton.Tag = false;
            LiteButton.WaveSize = 0;
            LiteButton.Click += LiteButton_Click;
            // 
            // SearchInput
            // 
            SearchInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SearchInput.CaretSpeed = 700;
            SearchInput.JoinRight = true;
            SearchInput.Location = new Point(716, 10);
            SearchInput.Name = "SearchInput";
            SearchInput.PlaceholderText = "搜索";
            SearchInput.Radius = 3;
            SearchInput.Size = new Size(172, 34);
            SearchInput.SuffixText = "";
            SearchInput.TabIndex = 0;
            SearchInput.TabStop = false;
            SearchInput.WaveSize = 0;
            SearchInput.KeyDown += SearchInput_KeyDown;
            // 
            // OperationPanel
            // 
            OperationPanel.BackColor = Color.White;
            OperationPanel.Controls.Add(ShowButton);
            OperationPanel.Controls.Add(SortButton);
            OperationPanel.Controls.Add(AddButton);
            OperationPanel.Controls.Add(SearchButton);
            OperationPanel.Controls.Add(SearchInput);
            OperationPanel.Dock = DockStyle.Top;
            OperationPanel.Location = new Point(0, 35);
            OperationPanel.Name = "OperationPanel";
            OperationPanel.Radius = 0;
            OperationPanel.Size = new Size(936, 50);
            OperationPanel.TabIndex = 3;
            OperationPanel.TabStop = false;
            // 
            // ShowButton
            // 
            ShowButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ShowButton.ForeColor = Color.FromArgb(22, 119, 255);
            ShowButton.Ghost = true;
            ShowButton.IconSize = new Size(24, 24);
            ShowButton.IconSvg = resources.GetString("ShowButton.IconSvg");
            ShowButton.Location = new Point(530, 6);
            ShowButton.Name = "ShowButton";
            ShowButton.Radius = 3;
            ShowButton.Size = new Size(42, 42);
            ShowButton.TabIndex = 3;
            ShowButton.TabStop = false;
            ShowButton.Tag = false;
            TooltipComponent.SetTip(ShowButton, "显示方式");
            ShowButton.Click += ShowButton_Click;
            // 
            // SortButton
            // 
            SortButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SortButton.ForeColor = Color.FromArgb(22, 119, 255);
            SortButton.Ghost = true;
            SortButton.IconSize = new Size(20, 20);
            SortButton.IconSvg = resources.GetString("SortButton.IconSvg");
            SortButton.Location = new Point(578, 6);
            SortButton.Name = "SortButton";
            SortButton.Radius = 3;
            SortButton.Size = new Size(42, 42);
            SortButton.TabIndex = 3;
            SortButton.TabStop = false;
            SortButton.Tag = 0;
            SortButton.Click += SortButton_Click;
            SortButton.MouseEnter += SortButton_MouseEnter;
            // 
            // AddButton
            // 
            AddButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddButton.Enabled = false;
            AddButton.ForeColor = Color.FromArgb(22, 119, 255);
            AddButton.Ghost = true;
            AddButton.IconSize = new Size(32, 32);
            AddButton.IconSvg = resources.GetString("AddButton.IconSvg");
            AddButton.Location = new Point(628, 6);
            AddButton.Name = "AddButton";
            AddButton.Radius = 0;
            AddButton.Size = new Size(42, 42);
            AddButton.TabIndex = 2;
            AddButton.TabStop = false;
            TooltipComponent.SetTip(AddButton, "添加");
            AddButton.Click += AddButton_Click;
            // 
            // SearchButton
            // 
            SearchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SearchButton.IconSvg = resources.GetString("SearchButton.IconSvg");
            SearchButton.JoinLeft = true;
            SearchButton.Location = new Point(888, 10);
            SearchButton.Name = "SearchButton";
            SearchButton.Radius = 3;
            SearchButton.Size = new Size(40, 34);
            SearchButton.TabIndex = 1;
            SearchButton.TabStop = false;
            SearchButton.Type = TTypeMini.Primary;
            SearchButton.WaveSize = 0;
            SearchButton.Click += SearchButton_Click;
            // 
            // FlowPanel
            // 
            FlowPanel.AutoScroll = true;
            FlowPanel.Dock = DockStyle.Fill;
            FlowPanel.Location = new Point(0, 85);
            FlowPanel.Name = "FlowPanel";
            FlowPanel.Size = new Size(936, 454);
            FlowPanel.TabIndex = 4;
            FlowPanel.TabStop = false;
            // 
            // TooltipComponent
            // 
            TooltipComponent.ArrowSize = 5;
            TooltipComponent.Radius = 3;
            // 
            // MainForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(936, 539);
            Controls.Add(FlowPanel);
            Controls.Add(OperationPanel);
            Controls.Add(WindowBar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(326, 539);
            Name = "MainForm";
            Opacity = 0D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Notepad";
            WindowBar.ResumeLayout(false);
            OperationPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.PageHeader WindowBar;
        private AntdUI.Input SearchInput;
        private AntdUI.Panel OperationPanel;
        private AntdUI.Button SearchButton;
        private AntdUI.FlowPanel FlowPanel;
        private AntdUI.Button AddButton;
        private AntdUI.Button SortButton;
        private AntdUI.Button ShowButton;
        private TooltipComponent TooltipComponent;
        private AntdUI.Button LiteButton;
    }
}
