namespace CryptocurrencyRateParserWfApp.Forms
{
    partial class MainForm
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
            this.symbolsPairsComboBox = new System.Windows.Forms.ComboBox();
            this.bybitRateLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.kucoinRateLabel = new System.Windows.Forms.Label();
            this.binanceRateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // symbolsPairsComboBox
            // 
            this.symbolsPairsComboBox.FormattingEnabled = true;
            this.symbolsPairsComboBox.Location = new System.Drawing.Point(294, 37);
            this.symbolsPairsComboBox.Name = "symbolsPairsComboBox";
            this.symbolsPairsComboBox.Size = new System.Drawing.Size(203, 23);
            this.symbolsPairsComboBox.TabIndex = 1;
            // 
            // bybitRateLabel
            // 
            this.bybitRateLabel.AutoSize = true;
            this.bybitRateLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bybitRateLabel.Location = new System.Drawing.Point(216, 153);
            this.bybitRateLabel.Name = "bybitRateLabel";
            this.bybitRateLabel.Size = new System.Drawing.Size(79, 21);
            this.bybitRateLabel.TabIndex = 2;
            this.bybitRateLabel.Text = "Загрузка..";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Kucoin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Binance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Bybit";
            // 
            // kucoinRateLabel
            // 
            this.kucoinRateLabel.AutoSize = true;
            this.kucoinRateLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.kucoinRateLabel.Location = new System.Drawing.Point(216, 223);
            this.kucoinRateLabel.Name = "kucoinRateLabel";
            this.kucoinRateLabel.Size = new System.Drawing.Size(79, 21);
            this.kucoinRateLabel.TabIndex = 6;
            this.kucoinRateLabel.Text = "Загрузка..";
            // 
            // binanceRateLabel
            // 
            this.binanceRateLabel.AutoSize = true;
            this.binanceRateLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.binanceRateLabel.Location = new System.Drawing.Point(216, 305);
            this.binanceRateLabel.Name = "binanceRateLabel";
            this.binanceRateLabel.Size = new System.Drawing.Size(79, 21);
            this.binanceRateLabel.TabIndex = 7;
            this.binanceRateLabel.Text = "Загрузка..";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.binanceRateLabel);
            this.Controls.Add(this.kucoinRateLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bybitRateLabel);
            this.Controls.Add(this.symbolsPairsComboBox);
            this.Name = "MainForm";
            this.Text = "Парсер текущих курсов криптовалюты";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox symbolsPairsComboBox;
        private Label bybitRateLabel;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label kucoinRateLabel;
        private Label binanceRateLabel;
    }
}