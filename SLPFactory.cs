using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;
namespace LiveSplit.SuperLovelyPlanet {
	public class SLPFactory : IComponentFactory {
		public string ComponentName { get { return "Super Lovely Planet Autosplitter v" + this.Version.ToString(); } }
		public string Description { get { return "Autosplitter for Super Lovely Planet"; } }
		public ComponentCategory Category { get { return ComponentCategory.Control; } }
		public IComponent Create(LiveSplitState state) { return new SLPComponent(state); }
		public string UpdateName { get { return this.ComponentName; } }
		public string UpdateURL { get { return "https://raw.githubusercontent.com/sigmasin/LiveSplit.SuperLovelyPlanet/master/"; } }
		public string XMLURL { get { return this.UpdateURL + "Components/Updates.xml"; } }
		public Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }
	}
}