using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainTourney : Control
{
	private int Players = 32;
	private List<Player> playerlist;
	private ItemList VisualPairings;
	[Export]
	private Texture2D icon;


    


    //TODO LIST Code wise only
    //Give way to increase score - cheating immeditetyly i know
    //Have pairings be done on a bracket by bracket basis
    //Have a person be able to be knocked down a bracket
    //Make the by abide by the above behavior

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
			
			p.name = i.ToString();
			//Add itself to played list just in case
			p.addToPlayedList(i);
            playerlist.Add(p);

        }
		VisualPairings = (ItemList)GetNode("PanelContainer/VScrollBar/HBoxContainer/Pairings");

		
        
    }

    private void MainTourney_GiveWin(Player winner)
    {
		GD.Print("Winner is you " + winner.name);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}

	private List<Player[]> decidePairings(List<Player> bracket)
	{
        Random random = new Random();
        //For each score bracket
        var copy = bracket.ToList();
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
	//Sorts each player into their individual brackets
	private Dictionary<int,List<Player>> determineBrackets()
	{
        Dictionary<int, List<Player>> initbrackets = new Dictionary<int, List<Player>>();
		foreach (Player p in playerlist){
			if (initbrackets.ContainsKey(p.score))
			{
				initbrackets[p.score].Add(p);
			}
			else
			{
				List<Player> temp = new List<Player>();
				temp.Add(p);
				initbrackets.Add(p.score, temp);
			}
		}
		foreach (var x in initbrackets) 
		{
			//GD.Print("Key " + x.Key);
			//foreach(var y in x.Value) { GD.Print(y.id); }
		}
		return initbrackets;
	}

	private void _on_custom_button_pressed()
	{
		determineBrackets();

        var pairings = decidePairings(playerlist);
        GD.Print(playerlist.Count);
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
	//Open up a window for that player
	private void _on_pairings_item_activated(int index)
	{
		//TODO change to get player name
		string name = VisualPairings.GetItemText(index);
	
		foreach (Player p in playerlist)
		{
			GD.Print(p.name);
			if (p.name == name)
			{
				
                var popup = GD.Load<PackedScene>("res://Scenes//PlayerPopup.tscn");
				PlayerPopup playerPopup = (PlayerPopup)popup.Instantiate();
				AddChild(playerPopup);
				playerPopup.assignData(p);
				//Force connect to popup signal
				playerPopup.GiveWin += MainTourney_GiveWin;


                break;
			}
		}

    }

}
