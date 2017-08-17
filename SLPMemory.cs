using System;
using System.Diagnostics;
namespace LiveSplit.SuperLovelyPlanet {
	public partial class SLPMemory {
        private static ProgramPointer LEVEL = new ProgramPointer(true, new ProgramSignature(PointerVersion.V1, "558BEC83EC088B05????????83EC086A0050E8????????83C41085C0740E8B05|32"));
        private static ProgramPointer LEVELLOADER = new ProgramPointer(true, new ProgramSignature(PointerVersion.V1, "0FB6471085C075398B05????????83EC0868????????50E8????????83C41085C074158B05|10"));

        public Process Program { get; set; }
		public bool IsHooked { get; set; } = false;
		private DateTime lastHooked = DateTime.MinValue;

		public int EnemyCount() {
			return LEVEL.Read<int>(Program, 0x0, 0x74);
		}
		public bool IsLoading() {
            //Level.instance == null || !Level.instace.started
			return LEVEL.Read<int>(Program, 0x0) == 0 || !LEVEL.Read<bool>(Program, 0x0, 0xC1);
		}
        public bool LevelCompleted() {
            return LEVEL.Read<bool>(Program, 0x0, 0xC2);
        }
		public string PlayTime() {
			//Level.rawPlayTime
			return string.Format("{0:0.00}", LEVEL.Read<float>(Program, 0xc));
		}
		public string LevelName() {
			return LEVELLOADER.Read(Program);
		}

		public bool HookProcess() {
			if ((Program == null || Program.HasExited) && DateTime.Now > lastHooked.AddSeconds(1)) {
				lastHooked = DateTime.Now;
				Process[] processes = Process.GetProcessesByName("SuperLovelyPlanet");
				Program = processes.Length == 0 ? null : processes[0];
			}

			IsHooked = Program != null && !Program.HasExited;

			return IsHooked;
		}
		public void Dispose() {
			if (Program != null) {
				Program.Dispose();
			}
		}
	}
	public enum PointerVersion {
		V1
	}
	public class ProgramSignature {
		public PointerVersion Version { get; set; }
		public string Signature { get; set; }
		public ProgramSignature(PointerVersion version, string signature) {
			Version = version;
			Signature = signature;
		}
		public override string ToString() {
			return Version.ToString() + " - " + Signature;
		}
	}
	public class ProgramPointer {
		private int lastID;
		private DateTime lastTry;
		private ProgramSignature[] signatures;
		private int[] offsets;
		public IntPtr Pointer { get; private set; }
		public PointerVersion Version { get; private set; }
		public bool AutoDeref { get; private set; }

		public ProgramPointer(bool autoDeref, params ProgramSignature[] signatures) {
			AutoDeref = autoDeref;
			this.signatures = signatures;
			lastID = -1;
			lastTry = DateTime.MinValue;
		}
		public ProgramPointer(bool autoDeref, params int[] offsets) {
			AutoDeref = autoDeref;
			this.offsets = offsets;
			lastID = -1;
			lastTry = DateTime.MinValue;
		}

		public T Read<T>(Process program, params int[] offsets) where T : struct {
			GetPointer(program);
			return program.Read<T>(Pointer, offsets);
		}
		public string Read(Process program, params int[] offsets) {
			GetPointer(program);
			IntPtr ptr = (IntPtr)program.Read<uint>(Pointer, offsets);
			return program.Read(ptr);
		}
		public void Write<T>(Process program, T value, params int[] offsets) where T : struct {
			GetPointer(program);
			program.Write<T>(Pointer, value, offsets);
		}
		public void Write(Process program, byte[] value, params int[] offsets) {
			GetPointer(program);
			program.Write(Pointer, value, offsets);
		}
		private void GetPointer(Process program) {
			if ((program?.HasExited).GetValueOrDefault(true)) {
				Pointer = IntPtr.Zero;
				lastID = -1;
				return;
			} else if (program.Id != lastID) {
				Pointer = IntPtr.Zero;
				lastID = program.Id;
			}

			if (Pointer == IntPtr.Zero && DateTime.Now > lastTry.AddSeconds(1)) {
				lastTry = DateTime.Now;

				Pointer = GetVersionedFunctionPointer(program);
				if (Pointer != IntPtr.Zero) {
					if (AutoDeref) {
						Pointer = (IntPtr)program.Read<uint>(Pointer);
					}
				}
			}
		}
		private IntPtr GetVersionedFunctionPointer(Process program) {
			if (signatures != null) {
				for (int i = 0; i < signatures.Length; i++) {
					ProgramSignature signature = signatures[i];

					IntPtr ptr = program.FindSignatures(signature.Signature)[0];
					if (ptr != IntPtr.Zero) {
						Version = signature.Version;
						return ptr;
					}
				}
			} else {
				IntPtr ptr = (IntPtr)program.Read<uint>(program.MainModule.BaseAddress, offsets);
				if (ptr != IntPtr.Zero) {
					return ptr;
				}
			}

			return IntPtr.Zero;
		}
	}
}