using Godot;
using System;
using System.Linq;

public partial class TourneyPopup : Control
{
	private TextEdit participents;
    public PackedScene mainscene;

    private int winpoints;
    private int tiepoints;
    private int losspoints;

    [Signal]
    public delegate void HideMainMenuEventHandler();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		participents = (TextEdit)GetNode("Popup/MarginContainer/VBoxContainer/Participents");
        mainscene = GD.Load<PackedScene>("res://Scenes/MainTourney.tscn");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_popup_close_requested()
	{
		this.QueueFree();
	}

	public void assignCustomScores(int w1,int t1,int l1)
	{
		winpoints = w1;
		tiepoints = t1;
		losspoints = l1;
	}

	private void _on_button_pressed()
	{
		string fullparticipents = participents.Text;
		string[] participentList = fullparticipents.Split("\n");
		

		MainTourney loadmainscen = (MainTourney)mainscene.Instantiate();
        GetTree().Root.AddChild(loadmainscen);
		loadmainscen.createPlayerList(participentList);
		EmitSignal(SignalName.HideMainMenu);
		loadmainscen.assignWinLossDrawPoints(winpoints,tiepoints,losspoints);
		QueueFree();
    }
}
