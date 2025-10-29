using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerPopup : Control
{
	private Label PopupName;
	private Label Score;
	
    private Label PlayedAgaist;
    private Player Me;
    private List<Player> PlayerList;

    [Signal]
    public delegate void GiveWinEventHandler(Player winner);

    [Signal]
    public delegate void GiveTieEventHandler(Player winner);

    [Signal]
    public delegate void GiveLossEventHandler(Player loser);

    [Signal]
    public delegate void DropMeEventHandler(Player winner);

    [Signal]
    public delegate void GiveDoubleLossEventHandler(Player loser);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PopupName = (Label)GetNode("Popup/MarginContainer/VBoxContainer/Name");
		Score = (Label)GetNode("Popup/MarginContainer/VBoxContainer/Score");
        PlayedAgaist = (Label)GetNode("Popup/MarginContainer/VBoxContainer/ActualPlayerList");

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
        _on_popup_close_requested();
    }

	private void _on_give_tie_pressed()
	{
        EmitSignal(SignalName.GiveTie, Me);
        _on_popup_close_requested();
    }

	private void _on_give_loss_pressed()
	{
        EmitSignal(SignalName.GiveLoss, Me);
        _on_popup_close_requested();
    }


    private void _on_drop_player_pressed()
	{
        EmitSignal(SignalName.DropMe, Me);
		_on_popup_close_requested();
    }

	private void _on_give_double_loss_pressed()
	{
        EmitSignal(SignalName.GiveDoubleLoss, Me);
        _on_popup_close_requested();
    }		

    //Assign the popip info as needed
    public void assignData(Player p,List<Player> players)
	{
		Me = p;  
		PopupName.Text = p.name;
		Score.Text = "Current Score: " + p.score;
		PlayerList = players;
		//Show each player that this player has played against, exclude themselves and the by which has a id of -1
		foreach (int id in p.getPlayed())
		{
			if(id != p.id && id > -1)
			{
                PlayedAgaist.Text = PlayedAgaist.Text + players[id].name + ", ";
            }
			
		}
	}
}
