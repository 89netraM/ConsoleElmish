using ConsoleElmish;
using Buffer = ConsoleElmish.Buffer;
using System;
using System.Timers;

namespace ConsoleApp
{
	public class ClockComponent : Component<ClockState>
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

		public override Buffer Render(uint height, uint width)
		{
			return new Buffer
			{
				{
					new Area(0, 0, height, width),
					new BorderComponent(
						new TextComponent(
							State.Time.ToString("HH:mm:ss"),
							true
						)
					)
				}
			};
		}
	}

	public readonly struct ClockState
	{
		public DateTime Time { get; }

		public ClockState(DateTime time)
		{
			Time = time;
		}
	}
}