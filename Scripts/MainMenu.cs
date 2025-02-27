using Godot;
using System;

public partial class MainMenu : Control
{
	public Node mainscene;
    private LineEdit winEdit;
    private LineEdit tieEdit;
    private LineEdit lossEdit;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        mainscene = GD.Load<PackedScene>("res://Scenes/MainTourney.tscn").Instantiate();
        winEdit = GetNode<LineEdit>("PanelContainer/MarginContainer/HBoxContainer/Wininput");
        tieEdit = GetNode<LineEdit>("PanelContainer/MarginContainer/HBoxContainer/Tie Input");
        lossEdit = GetNode<LineEdit>("PanelContainer/MarginContainer/HBoxContainer/Loss Input");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_custom_button_pressed()
	{
        var popup = GD.Load<PackedScene>("res://Scenes//TourneyPopup.tscn");
        TourneyPopup playerPopup = (TourneyPopup)popup.Instantiate();
        AddChild(playerPopup);
        playerPopup.HideMainMenu += PlayerPopup_HideMainMenu;
        //GetTree().Root.AddChild(mainscene);
        //Hide();
        int winpoints = 3;
        int tiepoints = 1;
        int losspoints = 0;
        if (winEdit.Text.Length > 0) 
        {
            try
            {
                winpoints = winEdit.Text.ToInt();
            }
            catch 
            { 
            }
        }
        if (tieEdit.Text.Length > 0)
        {
            try
            {
                tiepoints = tieEdit.Text.ToInt();
            }
            catch
            {
            }
        }
        if (lossEdit.Text.Length > 0)
        {
            try
            {
                losspoints = lossEdit.Text.ToInt();
            }
            catch
            {
            }
        }
        playerPopup.assignCustomScores(winpoints, tiepoints, losspoints);   
    }

    private void PlayerPopup_HideMainMenu()
    {
        Hide();
    }
}

