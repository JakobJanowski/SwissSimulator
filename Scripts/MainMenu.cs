using Godot;
using System;

public partial class MainMenu : Control
{
	public Node mainscene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        mainscene = ResourceLoader.Load<PackedScene>("res://scenes/MainTourney.tscn").Instantiate();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_custom_button_pressed()
	{
        GetTree().Root.AddChild(mainscene);
		Hide();
    }
}
