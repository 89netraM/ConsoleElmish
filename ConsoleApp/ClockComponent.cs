using ConsoleElmish;
using System;
using System.Timers;

namespace ConsoleApp
{
	public class ClockComponent : Component<ClockProperties, ClockState>
	{
		public ClockComponent() : base(new ClockState(DateTime.Now))
		{
			Timer timer = new Timer(1000.0d);
			timer.AutoReset = true;
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			State = new ClockState(e.SignalTime);
		}

		public override void Render(IConsole console, uint height, uint width)
		{
			console.Draw(0, 0, height, width, new BorderComponent<TextProperties, TextState>(new TextComponent(State.Time.ToString("HH:mm:ss"))));
		}
	}

	public readonly struct ClockProperties { }
	public readonly struct ClockState
	{
		public DateTime Time { get; }

		public ClockState(DateTime time)
		{
			Time = time;
		}
	}
}