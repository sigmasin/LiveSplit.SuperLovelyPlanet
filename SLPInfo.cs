using LiveSplit.SuperLovelyPlanet;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
namespace SuperLovelyPlanet {
	public partial class SLPInfo : Form {
		public SLPMemory Memory { get; set; }
		private DateTime lastCheck = DateTime.MinValue;

		[STAThread]
		public static void Main(string[] args) {
			try {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new SLPInfo());
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
		public SLPInfo() {
			try {
				this.DoubleBuffered = true;
				InitializeComponent();
				Text = "Super Lovely Planet " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
				Memory = new SLPMemory();

				Thread t = new Thread(UpdateLoop);
				t.IsBackground = true;
				t.Start();
			} catch (Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}

		private void UpdateLoop() {
			bool lastHooked = false;
			while (true) {
				try {
					bool hooked = Memory.HookProcess();
					if (hooked) {
						UpdateValues();
					}
					if (lastHooked != hooked) {
						lastHooked = hooked;
						this.Invoke((Action)delegate () { lblNote.Visible = !hooked; });
					}
					Thread.Sleep(12);
				} catch { }
			}
		}
		public void UpdateValues() {
			if (this.InvokeRequired) {
				this.Invoke((Action)UpdateValues);
			} else {
				lblLevel.Text = "Level: " + Memory.LevelName() + (Memory.IsLoading() ? " (Loading)" : "");
				lblLevelText.Text = "Time: " + Memory.PlayTime();
			}
		}
	}
}