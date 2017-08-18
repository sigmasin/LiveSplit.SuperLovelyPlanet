using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace LiveSplit.SuperLovelyPlanet {
	public class SLPComponent : IComponent {
		public TimerModel Model { get; set; }
		public string ComponentName { get { return "Super Lovely Planet Autosplitter"; } }
		public IDictionary<string, Action> ContextMenuControls { get { return null; } }
		internal static string[] keys = { "CurrentSplit", "LevelName" };
		private static string LOGFILE = "_SLP.log";
		private SLPMemory mem;
		private int currentSplit = -1, lastLogCheck = 0, state = 0;
		private bool hasLog = false;
		private Dictionary<string, string> currentValues = new Dictionary<string, string>();
        private bool wasCompleted;
        private bool levelTransition;
		private string lastLevelName;

		public SLPComponent(LiveSplitState state) {
			mem = new SLPMemory();

			Model = new TimerModel() { CurrentState = state };
			Model.InitializeGameTime();
			Model.CurrentState.IsGameTimePaused = true;
			state.OnReset += OnReset;
			state.OnPause += OnPause;
			state.OnResume += OnResume;
			state.OnStart += OnStart;
			state.OnSplit += OnSplit;
			state.OnUndoSplit += OnUndoSplit;
			state.OnSkipSplit += OnSkipSplit;

			foreach (string key in keys) {
				currentValues[key] = "";
			}

            levelTransition = false;
		}

		public void GetValues() {
			if (!mem.HookProcess()) { return; }

			if (Model != null) {
				HandleSplits();
			}

			LogValues();
		}
		private void HandleSplits() {
			bool shouldSplit = false;
			string levelName = mem.LevelName();
			bool isLoading = mem.IsLoading();
            bool isCompleted = mem.LevelCompleted();

			if (currentSplit == -1) {
				switch (state) {
					case 0:
						if (levelName == "air_intro" && isLoading) {
							state = 1;
						}
						break;
					case 1:
						shouldSplit = !isLoading;
						break;
				}
			} else if (Model.CurrentState.CurrentPhase == TimerPhase.Running) {
				switch (state) {
					case 0:
                        if (isCompleted && !wasCompleted)
                        {
                            state = 1;
                            if (levelName == "hill_end") {
                                state = 2;
                            }
                        }
                        else if (isLoading) {
                            levelTransition = false;
                        }
                        break;
                    case 1:
                        if (lastLevelName != levelName) {
                            state = 2;
                            levelTransition = true;
                        }
                        break;
                    case 2:
						shouldSplit = true;
						break;
				}

				Model.CurrentState.IsGameTimePaused = isLoading || levelTransition;
			}

            wasCompleted = isCompleted;
			lastLevelName = levelName;
			HandleSplit(shouldSplit);
		}
		private void HandleSplit(bool shouldSplit, bool shouldReset = false) {
			if (shouldReset) {
				if (currentSplit >= 0) {
					Model.Reset();
				}
			} else if (shouldSplit) {
				if (currentSplit < 0) {
					Model.Start();
				} else {
					Model.Split();
				}
			}
		}
		private void LogValues() {
			if (lastLogCheck == 0) {
				hasLog = File.Exists(LOGFILE);
				lastLogCheck = 300;
			}
			lastLogCheck--;

			if (hasLog || !Console.IsOutputRedirected) {
				string prev = "", curr = "";
				foreach (string key in keys) {
					prev = currentValues[key];

					switch (key) {
						case "CurrentSplit": curr = currentSplit.ToString(); break;
						case "LevelName": curr = mem.LevelName().ToString(); break;
						default: curr = ""; break;
					}

					if (!prev.Equals(curr)) {
						WriteLogWithTime(key + ": ".PadRight(16 - key.Length, ' ') + prev.PadLeft(25, ' ') + " -> " + curr);

						currentValues[key] = curr;
					}
				}
			}
		}
		private void WriteLog(string data) {
			if (hasLog || !Console.IsOutputRedirected) {
				if (!Console.IsOutputRedirected) {
					Console.WriteLine(data);
				}
				if (hasLog) {
					using (StreamWriter wr = new StreamWriter(LOGFILE, true)) {
						wr.WriteLine(data);
					}
				}
			}
		}
		private void WriteLogWithTime(string data) {
			WriteLog(DateTime.Now.ToString(@"HH\:mm\:ss.fff") + (Model != null && Model.CurrentState.CurrentTime.RealTime.HasValue ? " | " + Model.CurrentState.CurrentTime.RealTime.Value.ToString("G").Substring(3, 11) : "") + ": " + data);
		}
		public void Update(IInvalidator invalidator, LiveSplitState lvstate, float width, float height, LayoutMode mode) {
			GetValues();
		}

		public void OnReset(object sender, TimerPhase e) {
			currentSplit = -1;
			state = 0;
			Model.CurrentState.IsGameTimePaused = true;
			WriteLog("---------Reset----------------------------------");
		}
		public void OnResume(object sender, EventArgs e) {
			WriteLog("---------Resumed--------------------------------");
		}
		public void OnPause(object sender, EventArgs e) {
			WriteLog("---------Paused---------------------------------");
		}
		public void OnStart(object sender, EventArgs e) {
			currentSplit = 0;
			state = 0;
			Model.CurrentState.IsGameTimePaused = false;
			WriteLog("---------New Game-------------------------------");
		}
		public void OnUndoSplit(object sender, EventArgs e) {
			currentSplit--;
			state = 0;
		}
		public void OnSkipSplit(object sender, EventArgs e) {
			currentSplit++;
			state = 0;
		}
		public void OnSplit(object sender, EventArgs e) {
			currentSplit++;
			state = 0;
		}
		public Control GetSettingsControl(LayoutMode mode) { return null; }
		public void SetSettings(XmlNode document) { }
		public XmlNode GetSettings(XmlDocument document) { return document.CreateElement("Settings"); }
		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }
		public float HorizontalWidth { get { return 0; } }
		public float MinimumHeight { get { return 0; } }
		public float MinimumWidth { get { return 0; } }
		public float PaddingBottom { get { return 0; } }
		public float PaddingLeft { get { return 0; } }
		public float PaddingRight { get { return 0; } }
		public float PaddingTop { get { return 0; } }
		public float VerticalHeight { get { return 0; } }
		public void Dispose() { }
	}
}