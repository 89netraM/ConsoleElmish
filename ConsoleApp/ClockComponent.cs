using ConsoleElmish;
using Buffer = ConsoleElmish.Buffer;
using System;
using System.Timers;
using ConsoleElmish.Common;

namespace ConsoleApp
{
	public class ClockComponent : Component<ClockState>
	{
		private readonly Timer timer;

		public ClockComponent() : base(new ClockState(DateTime.Now, TextComponent.Alignment.Center))
		{
			timer = new Timer(1000.0d);
			timer.AutoReset = true;
			timer.Elapsed += Timer_Elapsed;
			timer.Start();

			Input.KeyDown += Input_KeyDown;
			Input.Start();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			State = new ClockState(e.SignalTime, State.TextAlignment);
		}

		private void Input_KeyDown(ConsoleKeyInfo info)
		{
			if (info.Key == ConsoleKey.Spacebar)
			{
				State = new ClockState(
					State.Time,
					State.TextAlignment switch
					{
						TextComponent.Alignment.Left => TextComponent.Alignment.Center,
						TextComponent.Alignment.Center => TextComponent.Alignment.Right,
						TextComponent.Alignment.Right => TextComponent.Alignment.Left,
						_ => throw new NotImplementedException(),
					}
				);
			}
		}

		public override Buffer Render(uint height, uint width)
		{
			return new Buffer
			{
				{
					new Area(0, 0, height, width),
					new TextComponent(
						State.Time.ToString("HH:mm:ss"),
						State.TextAlignment
					).WithColors(foreground: ConsoleColor.Red)
				}
			};
		}

		public override void Dispose()
		{
			timer.Stop();
			timer.Elapsed -= Timer_Elapsed;
			timer.Dispose();

			Input.KeyDown -= Input_KeyDown;
		}
	}

	public readonly struct ClockState
	{
		public DateTime Time { get; }
		public TextComponent.Alignment TextAlignment { get; }

		public ClockState(DateTime time, TextComponent.Alignment textAlignment)
		{
			Time = time;
			TextAlignment = textAlignment;
		}
	}
}