﻿using System.IO;
using System.Linq;
using Tools;

[Tool( "Tools Manager", "hardware", "Manages your tools." )]
public class ToolsManager : BaseWindow
{
	public ToolsManager()
	{
		Size = new Vector2( 600, 400 );
		MinimumSize = Size;
		WindowTitle = "Tools Manager";

		SetWindowIcon( "hardware" );

		WriteDummyManifest();

		CreateUI();
		Show();
	}

	private void WriteDummyManifest()
	{
		// If we don't have our own manifest, then we need to create
		// one and (later) force an update.

		var project = Utility.Projects.GetAll().First( x => x.Config.Ident == "tools_manager" );
		var folder = project.GetRootPath();

		var manifestPath = System.IO.Path.Combine( folder, "tm-manifest.json" );

		if ( File.Exists( manifestPath ) )
			return;

		// Create tools manifest
		var manifest = new Manifest();
		manifest.ReleaseName = "None";
		manifest.ReleaseVersion = "0";
		manifest.ReleaseDescription = "Invalid release";

		manifest.Repo = "xezno/tools-manager";
		manifest.Description = "Manages your tools.";
		manifest.AutoUpdate = true;

		System.IO.File.WriteAllText( manifestPath, manifest.ToJson() );
	}

	public void CreateUI()
	{
		SetLayout( LayoutMode.LeftToRight );
		var layout = Layout;

		var toolsList = layout.Add( new NavigationView( this ) );

		foreach ( var project in Utility.Projects.GetAll() )
		{
			var config = project.Config;

			if ( config.PackageType == Sandbox.Package.Type.Tool )
			{
				var option = toolsList.AddPage( config.Title, "hardware", new Page( project ) );
				option.OnPaintOverride = () => PaintPageOption( option );
			}
		}

		var footer = toolsList.MenuBottom.AddRow();
		footer.Spacing = 4;

		var add = footer.Add( new Button.Primary( "Add Tool...", "add" ), 1 );
		add.Clicked = () => AddToolDialog.Open();

		var update = footer.Add( new Button( null, "download" ) );
		update.Clicked = () => ToolUpdateNotice.Open( 4 );
	}

	private bool PaintPageOption( NavigationView.Option option )
	{
		var fg = Theme.White.WithAlpha( 0.5f );

		if ( option.IsSelected )
		{
			fg = Theme.White;
		}

		Paint.ClearPen();
		Paint.SetBrush( Theme.WidgetBackground.WithAlpha( 0.0f ) );

		if ( Paint.HasMouseOver )
		{
			fg = Theme.White.WithAlpha( 0.8f );
		}

		Paint.TextAntialiasing = true;
		Paint.Antialiasing = true;

		Paint.DrawRect( option.LocalRect.Shrink( 0 ) );

		var inner = option.LocalRect.Shrink( 8, 0, 0, 0 );

		Paint.SetPen( fg.WithAlphaMultiplied( 0.8f ) );
		Paint.SetFont( "Poppins", 8, 440 );

		Paint.DrawText( inner, option.Title, TextFlag.LeftCenter );

		var iconRect = inner;
		iconRect.Left = inner.Right - 32;
		iconRect.Top -= 2;

		if ( option.IsSelected )
		{
			Paint.SetPen( fg );
			Paint.DrawIcon( iconRect, "update", 14, TextFlag.Center );

			inner.Left += iconRect.Width + 4;
		}

		return true;
	}
}
