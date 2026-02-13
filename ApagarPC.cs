using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ApagarPC
{
    public class MainForm : Form
    {
        // Controls
        private TabControl tabControl;
        private TabPage tabTime;
        private TabPage tabTimer;
        
        // Time Mode Controls
        private DateTimePicker timePicker;
        private Label lblTimeInstruction;
        
        // Timer Mode Controls
        private NumericUpDown numHours;
        private NumericUpDown numMinutes;
        private Label lblHours;
        private Label lblMinutes;
        private Label lblSep1;
        private Label lblSep2;
        
        // Shared Controls
        private Button btnSchedule;
        private Button btnCancel;
        private Label lblStatus;
        private Timer appTimer;
        
        // Logic Variables
        private DateTime scheduledTime;
        private bool isScheduled = false;

        // Fonts
        private Font fontLarge = new Font("Segoe UI", 24F, FontStyle.Regular);
        private Font fontMedium = new Font("Segoe UI", 14F, FontStyle.Regular);
        private Font fontSmall = new Font("Segoe UI", 10F, FontStyle.Regular);
        private Font fontBold = new Font("Segoe UI", 12F, FontStyle.Bold);

        public MainForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.Text = "ApagarPC";
            this.Size = new Size(450, 400); // Increased size
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Icon = SystemIcons.Application;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tab Control
            tabControl = new TabControl();
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(415, 220); // Increased size
            tabControl.Font = fontSmall;
            tabControl.ItemSize = new Size(100, 30);
            
            // Tab 1: Exact Time
            tabTime = new TabPage("Hora Exacta");
            tabTime.UseVisualStyleBackColor = true;
            
            lblTimeInstruction = new Label();
            lblTimeInstruction.Text = "Apagar a las:";
            lblTimeInstruction.AutoSize = false;
            lblTimeInstruction.TextAlign = ContentAlignment.MiddleCenter;
            lblTimeInstruction.Dock = DockStyle.Top;
            lblTimeInstruction.Height = 50;
            lblTimeInstruction.Font = fontMedium;
            
            timePicker = new DateTimePicker();
            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.ShowUpDown = true;
            timePicker.Font = fontLarge; // Larger font
            timePicker.Width = 200;
            timePicker.Location = new Point((415 - 200) / 2 - 5, 70); // Centered
            
            tabTime.Controls.Add(timePicker);
            tabTime.Controls.Add(lblTimeInstruction);
            
            // Tab 2: Timer (Countdown)
            tabTimer = new TabPage("Temporizador");
            tabTimer.UseVisualStyleBackColor = true;
            
            // Panel for centering timer controls
            Panel pnlTimer = new Panel();
            pnlTimer.Size = new Size(300, 100);
            pnlTimer.Location = new Point((415 - 300) / 2, 50);
            // pnlTimer.BackColor = Color.Red; // Debug

            numHours = new NumericUpDown() { Font = fontLarge, Width = 80, Maximum = 23, Minimum = 0, TextAlign = HorizontalAlignment.Center };
            numHours.Location = new Point(10, 20);
            
            lblSep1 = new Label() { Text = "H", Font = fontMedium, AutoSize = true,  Location = new Point(95, 35) };

            numMinutes = new NumericUpDown() { Font = fontLarge, Width = 80, Maximum = 59, Minimum = 0, TextAlign = HorizontalAlignment.Center };
            numMinutes.Location = new Point(130, 20);

            lblSep2 = new Label() { Text = "M", Font = fontMedium, AutoSize = true, Location = new Point(215, 35) };

            pnlTimer.Controls.Add(numHours);
            pnlTimer.Controls.Add(lblSep1);
            pnlTimer.Controls.Add(numMinutes);
            pnlTimer.Controls.Add(lblSep2);
            
            Label lblTimerInstruction = new Label();
            lblTimerInstruction.Text = "Apagar en:";
            lblTimerInstruction.AutoSize = false;
            lblTimerInstruction.TextAlign = ContentAlignment.MiddleCenter;
            lblTimerInstruction.Dock = DockStyle.Top;
            lblTimerInstruction.Height = 40;
            lblTimerInstruction.Font = fontMedium;

            tabTimer.Controls.Add(pnlTimer);
            tabTimer.Controls.Add(lblTimerInstruction);

            tabControl.Controls.Add(tabTime);
            tabControl.Controls.Add(tabTimer);

            // Buttons
            btnSchedule = new Button();
            btnSchedule.Text = "PROGRAMAR APAGADO";
            btnSchedule.Font = fontBold;
            btnSchedule.Location = new Point(10, 240);
            btnSchedule.Size = new Size(415, 50);
            btnSchedule.Cursor = Cursors.Hand;
            btnSchedule.Click += BtnSchedule_Click;
            
            btnCancel = new Button();
            btnCancel.Text = "CANCELAR";
            btnCancel.Font = fontSmall;
            btnCancel.Location = new Point(10, 300);
            btnCancel.Size = new Size(415, 35);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Enabled = false;
            btnCancel.Click += BtnCancel_Click;

            // Status Label
            lblStatus = new Label();
            lblStatus.Text = "Estado: Inactivo";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 30;
            lblStatus.Font = fontSmall;

            // App Timer
            appTimer = new Timer();
            appTimer.Interval = 1000;
            appTimer.Tick += AppTimer_Tick;

            // Add Controls
            this.Controls.Add(tabControl);
            this.Controls.Add(btnSchedule);
            this.Controls.Add(btnCancel);
            this.Controls.Add(lblStatus);
        }

        private void ApplyTheme()
        {
            Color darkBg = Color.FromArgb(32, 32, 32);
            Color slightlyLighterBg = Color.FromArgb(45, 45, 48);
            Color lightText = Color.WhiteSmoke;
            Color accentColor = Color.FromArgb(0, 120, 215);
            Color cancelColor = Color.FromArgb(200, 50, 50);

            this.BackColor = darkBg;
            this.ForeColor = lightText;

            tabControl.BackColor = darkBg;
            
            // Apply to tabs
            foreach(TabPage page in tabControl.TabPages)
            {
                page.BackColor = slightlyLighterBg;
                page.ForeColor = lightText;
            }

            // Button Styles
            btnSchedule.BackColor = accentColor;
            btnSchedule.ForeColor = Color.White;
            btnSchedule.FlatStyle = FlatStyle.Flat;
            btnSchedule.FlatAppearance.BorderSize = 0;

            btnCancel.BackColor = cancelColor;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            
            lblStatus.ForeColor = Color.Gold;
        }

        private void BtnSchedule_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabTime)
            {
                scheduledTime = timePicker.Value;
                if (scheduledTime < DateTime.Now)
                {
                    scheduledTime = scheduledTime.AddDays(1);
                }
            }
            else
            {
                int hours = (int)numHours.Value;
                int minutes = (int)numMinutes.Value;
                
                if (hours == 0 && minutes == 0)
                {
                    MessageBox.Show("Por favor establece un tiempo mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                scheduledTime = DateTime.Now.AddHours(hours).AddMinutes(minutes);
            }

            StartSchedule();
        }

        private void StartSchedule()
        {
            isScheduled = true;
            UpdateUIState(true);
            appTimer.Start();
            UpdateStatus();
        }

        private void StopSchedule()
        {
            isScheduled = false;
            UpdateUIState(false);
            appTimer.Stop();
            lblStatus.Text = "Estado: Inactivo";
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            StopSchedule();
        }

        private void UpdateUIState(bool running)
        {
            btnSchedule.Enabled = !running;
            btnCancel.Enabled = running;
            tabControl.Enabled = !running;
            
            if(running)
            {
                btnSchedule.BackColor = Color.Gray;
                btnCancel.BackColor = Color.FromArgb(200, 50, 50);
            }
            else
            {
                btnSchedule.BackColor = Color.FromArgb(0, 120, 215);
                btnCancel.BackColor = Color.Gray;
            }
        }

        private void UpdateStatus()
        {
            TimeSpan remaining = scheduledTime - DateTime.Now;
            if (remaining.TotalSeconds <= 0)
            {
                lblStatus.Text = "Apagando ahora...";
            }
            else
            {
                lblStatus.Text = string.Format("Apagado en: {0:D2}:{1:D2}:{2:D2}", 
                    remaining.Hours, remaining.Minutes, remaining.Seconds);
            }
        }

        private void AppTimer_Tick(object sender, EventArgs e)
        {
            if (!isScheduled) return;

            if (DateTime.Now >= scheduledTime)
            {
                PerformShutdown();
            }
            else
            {
                UpdateStatus();
            }
        }

        private void PerformShutdown()
        {
            appTimer.Stop();
            try
            {
                Process.Start(new ProcessStartInfo("shutdown", "/s /t 0") { CreateNoWindow = true, UseShellExecute = false });
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar apagar: " + ex.Message);
                StopSchedule();
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
