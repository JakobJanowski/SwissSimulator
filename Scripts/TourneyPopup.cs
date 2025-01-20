using Godot;
using System;
using System.Linq;

public partial class TourneyPopup : Control
{
	private TextEdit participents;
    public PackedScene mainscene;

    [Signal]
    public delegate void HideMainMenuEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		participents = (TextEdit)GetNode("Popup/MarginContainer/VBoxContainer/Participents");
        mainscene = ResourceLoader.Load<PackedScene>("res://scenes/MainTourney.tscn");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_popup_close_requested()
	{
		this.QueueFree();
	}

	private void _on_button_pressed()
	{
		string fullparticipents = participents.Text;
		string[] participentList = fullparticipents.Split("\n");
		

		MainTourney loadmainscen = (MainTourney)mainscene.Instantiate();
        GetTree().Root.AddChild(loadmainscen);
		loadmainscen.createPlayerList(participentList);
		EmitSignal(SignalName.HideMainMenu);
		QueueFree();
    }
}
