using Godot;
using System;

public partial class PlayerPopup : Control
{
	private Label PopupName;
	private Label Score;
	private Player Me;

    [Signal]
    public delegate void GiveWinEventHandler(Player winner);

    [Signal]
    public delegate void GiveTieEventHandler(Player winner);

    [Signal]
    public delegate void DropMeEventHandler(Player winner);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PopupName = (Label)GetNode("Popup/MarginContainer/VBoxContainer/Name");
		Score = (Label)GetNode("Popup/MarginContainer/VBoxContainer/Score");
		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void _on_popup_close_requested()
	{
		this.QueueFree();
	}
	//Give the round win to this player
	//Emit signal back to main touney about who won and let it figure it out
	private void _on_give_win_pressed()
	{
		EmitSignal(SignalName.GiveWin,Me);
	}

	private void _on_give_tie_pressed()
	{
        EmitSignal(SignalName.GiveTie, Me);
    }

	private void _on_drop_player_pressed()
	{
        EmitSignal(SignalName.DropMe, Me);
		_on_popup_close_requested();
    }

    //Assign the popip info as needed
    public void assignData(Player p)
	{
		Me = p;  
		PopupName.Text = p.name;
		Score.Text = "Current Score: " + p.score; 
	}
}
