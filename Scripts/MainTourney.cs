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
    [Export]
    private Texture2D WinIcon;
    [Export]
    private Texture2D LossIcon;
    [Export]
    private Texture2D DrawIcon;

	private List<Player> playerOverflow;


    private int winPoints = 3;
    private int tiePoints = 1;
    private int losspoints = 0;




    //TODO LIST Code wise only
    //Give way to increase score - cheating immeditetyly i know DONE!
    //Have pairings be done on a bracket by bracket basis DONE!
    //Have a person be able to be knocked down a bracket DONE!
    //Make the by abide by the above behavior PROBABLY WORKS!
    //Fix infinite loop if everyone has been played

    //NOTE
    //REDO ID on a player drop, it will be easier
    //We need to purge the player anyway so this will hopefully prevent things from breaking

    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//Make the player list and set up with initial valus
		playerlist = new List<Player>();
        playerOverflow = new List<Player>();	

        
		VisualPairings = (ItemList)GetNode("PanelContainer/MarginContainer/VScrollBar/HBoxContainer/Pairings");
		



    }

	public void createPlayerList(String[] names)
	{
        
        for (int i = 0; i < names.Count(); i++)
        {
            Player p = new Player();
            p.id = i;
            p.score = 0;
            p.name = names[i];
            p.winloss = "";
            p.opponentID = -1;
            p.listindex = -1;
            //Add itself to played list just in case
            p.addToPlayedList(i);
            playerlist.Add(p);

        }
    }

    private void MainTourney_GiveWin(Player winner)
    {
        if (winner.opponentID != -1) 
        {
            playerlist[winner.id].winloss = "win";
            VisualPairings.SetItemIcon(winner.listindex, WinIcon);
            playerlist[winner.opponentID].winloss = "loss";
            VisualPairings.SetItemIcon(playerlist[winner.opponentID].listindex, LossIcon);
        }
        else
        {
            playerlist[winner.id].winloss = "win";
            VisualPairings.SetItemIcon(winner.listindex, WinIcon);
        }
       
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}
	//Create a list of pairings with each pair being next to each other
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
				playerOverflow.Add(tplayer);
                //Get rid of target player
                copy.RemoveAt(targetplayer);

            }
			else
			{
                //Get rid of target player
                copy.RemoveAt(targetplayer);
                //Find a match
                while (copy.Count > 0)
                {
					
                    int secondTargetPlayer = random.Next(0, copy.Count);
                    if (!tplayer.getPlayed().Contains(copy[secondTargetPlayer].id))
                    {
                        //Found
                        Player tplayer2 = copy[secondTargetPlayer];
                        Player[] temp = { tplayer, tplayer2 };
                        pairings.Add(temp);
                        copy.RemoveAt(secondTargetPlayer);
                        break;
                    }
					//If a match failed add the player to the overflow list
                    else if (copy.Count <= 1)
                    {
                        playerOverflow.Add(tplayer);
                        break;
                    }

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
		VisualPairings.Clear();
        //Increase score if needed
        increasePlayerScore();
        Dictionary<int, List<Player>>  brackets = determineBrackets();
		foreach (List<Player> p in brackets.Values)
		{
			if(playerOverflow.Count > 0)
			{
                foreach (Player players in playerOverflow)
                {
                    p.Add(players);
                }
				playerOverflow.Clear();
            }
            var pairings = decidePairings(p);
            printPairing(pairings);

        }
		//If there are still players in overflow give them the by
		if (playerOverflow.Count > 0) 
		{
			foreach (Player p in playerOverflow)
			{
				int index = VisualPairings.AddItem(p.name, icon);
                p.listindex = index;
                VisualPairings.AddItem("By", icon);

            }
            playerOverflow.Clear();
        }
		GD.Print(VisualPairings.ItemCount);

    }
    //Increase score based on ranking
    //Also reset some variables
    private void increasePlayerScore()
    {
        foreach(var p in playerlist)
        {
            //By default 3 for winning, 1 for a tie, no for losing. Loss is still added in case its not default
            if(p.winloss == "win")
            {
                p.score = p.score + winPoints;
            }
            else if(p.winloss == "tie")
            {
                p.score = p.score + tiePoints;
            }
            else if(p.winloss == "loss")
            {
                p.score = p.score + losspoints;
            }
            p.winloss = "";
            p.opponentID = -1;
            p.listindex = -1;

        }
    }

	private void printPairing(List<Player[]> pairings)
	{
		//Change size as needed
        VisualPairings.FixedColumnWidth = (int)Math.Round((DisplayServer.WindowGetSize().X) / 2.1)-50;
        foreach (Player[] p in pairings)
        {
		    playerlist[p[0].id].listindex = VisualPairings.AddItem(p[0].name, icon);
            playerlist[p[1].id].listindex = VisualPairings.AddItem(p[1].name, icon);

            //Add opponent to playerlist
            playerlist[p[0].id].opponentID = p[1].id;
            playerlist[p[1].id].opponentID = p[0].id;

            playerlist[p[0].id].addToPlayedList(p[1].id);
            playerlist[p[1].id].addToPlayedList(p[0].id);
        }
        
    }
	//Open up a window for that player
	private void _on_pairings_item_activated(int index)
	{
		//TODO change to get player name
		string name = VisualPairings.GetItemText(index);
        foreach (Player p in playerlist)
		{
			if (p.name == name)
			{
				
                var popup = GD.Load<PackedScene>("res://Scenes//PlayerPopup.tscn");
				PlayerPopup playerPopup = (PlayerPopup)popup.Instantiate();
				AddChild(playerPopup);
				playerPopup.assignData(p);
				//Force connect to popup signal
				playerPopup.GiveWin += MainTourney_GiveWin;
                playerPopup.GiveTie += PlayerPopup_GiveTie;
                playerPopup.DropMe += PlayerPopup_DropMe;


                break;
			}
		}

    }

    private void PlayerPopup_DropMe(Player winner)
    {
        //Remove player
        playerlist.Remove(winner);
        //Re give id
        for (int i = 0; i < playerlist.Count; i++) 
        {
            playerlist[i].id = i;
            //Undo pairing
            playerlist[i].removeLastedPlayed();
        }
        //Re pair
        _on_custom_button_pressed();
    }

    private void PlayerPopup_GiveTie(Player winner)
    {
        if (winner.opponentID != -1)
        {
            playerlist[winner.id].winloss = "tie";
            VisualPairings.SetItemIcon(winner.listindex, DrawIcon);
            playerlist[winner.opponentID].winloss = "tie";
            VisualPairings.SetItemIcon(playerlist[winner.opponentID].listindex, DrawIcon);
        }
        else
        {
            playerlist[winner.id].winloss = "tie";
            VisualPairings.SetItemIcon(winner.listindex, DrawIcon);
        }
    }

    private void _on_resized()
	{
        //TODO Figure out why it doesnt want to be exactly half the screen size
		//Check if exists to prevent a exception
		if(VisualPairings != null)
		{
            VisualPairings.FixedColumnWidth = (int)Math.Round((DisplayServer.WindowGetSize().X) / 2.1)-50;
        }
       
    }


}
