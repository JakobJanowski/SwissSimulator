using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class StandingsPopup : Control
{
    private List<Player> playerlist;
	private ItemList standings;

    [Export]
    private Texture2D icon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		standings = GetNode<ItemList>("Window/MarginContainer/ScrollContainer/VBoxContainer/Standings");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void assignPlayerList(List<Player> playerlist) 
	{
		this.playerlist = playerlist;
		createStandings();
	}

	private void createStandings()
	{
        List<Player> SortedList = playerlist.OrderByDescending(o => o.score).ToList();
		foreach (Player player in SortedList) 
		{
			standings.AddItem(player.name + ", Score: " + player.score, icon);
		}
    }

	private void _on_window_close_requested()
	{
		this.QueueFree();
	}
}
