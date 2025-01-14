using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node2D
{
	public int id { get; set; }
	public int score { get; set; }
    private List<int> played = new List<int>();
	public string name { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void addToPlayedList(int opponentId) 
	{ 
		played.Add(opponentId);
	}

	public List<int> getPlayed() { return played; }
	
}
