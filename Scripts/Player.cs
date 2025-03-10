using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node2D
{
	//Id to indentiy in list
	public int id { get; set; }
	public int score { get; set; }
    private List<int> played = new List<int>();
	//Player name used to identify
	public string name { get; set; }
	//Used to increase score
	public string winloss { get; set; }
	//Id of opponent 
	public int opponentID { get; set; }
	//To keep track of place in itemlist
	public int listindex {  get; set; }	


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

	public void removeLastedPlayed()
	{
		played.RemoveAt(played.Count-1);
	}

	public List<int> getPlayed() { return played; }
	//Remove the first instance in played and set itself to it, used in case of a dropped player
	public void resetFirstPlayed()
	{
		played.RemoveAt(0);
		played.Insert(0, id);
	}
	
}
