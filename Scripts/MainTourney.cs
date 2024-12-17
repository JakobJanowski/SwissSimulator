using Godot;
using System;
using System.Collections.Generic;

public partial class MainTourney : Control
{
	private int Players = 32;
	private List<Player> playerlist;
	private ItemList VisualPairings;
	[Export]
	private Texture2D icon;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Make the player list and set up with initial valus
		playerlist = new List<Player>();
		for (int i = 0; i < Players; i++) 
		{
			Player p = new Player();
			p.id = i;
			p.score = 0;
			playerlist.Add(p);
			p.name = i.ToString();
			//Add itself to played list just in case
			p.addToPlayedList(i);

		}
		VisualPairings = (ItemList)GetNode("PanelContainer/VScrollBar/HBoxContainer/Pairings");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private List<Player[]> decidePairings(List<Player> bracket)
	{
        Random random = new Random();
        //For each score bracket
        var copy = bracket;
		List<Player[]> pairings = new List<Player[]>();
		while (copy.Count > 0)
		{
			//Get a random player in the bracket
            int targetplayer = random.Next(0, copy.Count);
            Player tplayer = copy[targetplayer];
			//If theres one left give the by
            if (copy.Count == 1)
			{
                Player by = new Player();
                by.id = -1;
                Player[] temp = { tplayer, by };
                pairings.Add(temp);
            }
			//Get rid of target player
			copy.RemoveAt(targetplayer);
			//Find a match
			while (copy.Count > 0) 
			{
                int secondTargetPlayer = random.Next(0, copy.Count);
				if (!tplayer.getPlayed().Contains(secondTargetPlayer))
				{
					//Found
					Player tplayer2 = copy[secondTargetPlayer];
					Player[] temp = { tplayer, tplayer2 };
					pairings.Add(temp);
					copy.RemoveAt(secondTargetPlayer);
					break;
                }
				else if (copy.Count <= 1) 
				{
                    break;
				}

            }
			
		}

		return pairings;



	}

	private void _on_custom_button_pressed()
	{
		var pairings = decidePairings(playerlist);
		printPairing(pairings);

    }

	private void printPairing(List<Player[]> pairings)
	{

        foreach (Player[] p in pairings)
        {
			VisualPairings.AddItem(p[0].id.ToString(), icon);
            VisualPairings.AddItem(p[1].id.ToString(), icon);
        }
        
    }

}
