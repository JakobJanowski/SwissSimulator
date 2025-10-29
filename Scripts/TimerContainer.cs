using Godot;
using System;

public partial class TimerContainer : HBoxContainer
{
	Timer Timer;
	LineEdit timerValue;
	Label timerLabel;
	bool running = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer = (Timer)GetNode("Timer");
		timerValue = (LineEdit)GetNode("VBoxContainer/HBoxContainer/LineEdit");
		timerLabel = (Label)GetNode("VBoxContainer/TimerLabel");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timerLabel.Text = "";
		//Get minutes
        double timeleft = Timer.TimeLeft;
		double min = Math.Floor(timeleft/60);
        double sec = Math.Floor(timeleft - min * 60);

        //Hours
        if (min > 60)
		{
			//Get hours
			double hours = Math.Floor(min/60);
			//Display Hours
			if(hours < 10)
			{
                timerLabel.Text = timerLabel.Text + "0";
            }
            timerLabel.Text = timerLabel.Text + hours.ToString()+":";
            //Update min
            min = min - (hours * 60);
		}
		//If minutes are less than 10 add a 0
		if(min < 10)
		{
			timerLabel.Text = timerLabel.Text+"0";
		}
		//Then do seconds and do same
		timerLabel.Text =timerLabel.Text+min.ToString()+":";
        if (sec < 10)
        {
            timerLabel.Text = timerLabel.Text+"0";
        }
        timerLabel.Text = timerLabel.Text + sec.ToString();


    }

	private void _on_start_stop_button_pressed()
	{
   
        //If timer is running stop the timer
        if (running)
		{
			Timer.Paused = true;
			running = false;
		}
		//Otherwise if theres time left restart it, otherwise start a new timer
		else
		{
           
            if (Timer.TimeLeft > 0)
			{
				Timer.Paused = false;
				running = true;
			}
			else
			{
				try
				{
					
					double time = double.Parse(timerValue.Text);
					time = time * 60;
					Timer.Start(time);
					running = true;
				}
				catch (Exception)
				{
					//Do Nothing
				}
				
			}
		}
	}

	private void _on_reset_button_pressed()
	{
		Timer.Stop();
		running = false;
		Timer.Paused = false;
		
	}

	private void _on_timer_timeout()
    {
		_on_reset_button_pressed();
	}
}
