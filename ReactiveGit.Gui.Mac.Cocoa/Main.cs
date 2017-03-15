using AppKit;

namespace ReactiveGit.Gui.Mac.Cocoa
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			NSApplication.Init();
			NSApplication.Main(args);
		}
	}
}
