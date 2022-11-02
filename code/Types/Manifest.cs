﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tools;

public class Manifest
{
	[JsonPropertyName( "repo" )]
	public string Repo { get; set; }

	[JsonPropertyName( "description" )]
	public string Description { get; set; }

	[JsonPropertyName( "release_version" )]
	public string ReleaseVersion { get; set; }

	[JsonPropertyName( "release_name" )]
	public string ReleaseName { get; set; }

	[JsonPropertyName( "release_description" )]
	public string ReleaseDescription { get; set; }

	[JsonPropertyName( "auto_update" )]
	public bool AutoUpdate { get; set; }

	public string ToJson()
	{
		return JsonSerializer.Serialize( this );
	}
}
