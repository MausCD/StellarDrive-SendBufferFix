using System;
using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(SendBufferFix.BuildInfo.Description)]
[assembly: AssemblyDescription(SendBufferFix.BuildInfo.Description)]
[assembly: AssemblyCompany(SendBufferFix.BuildInfo.Company)]
[assembly: AssemblyProduct(SendBufferFix.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + SendBufferFix.BuildInfo.Author)]
[assembly: AssemblyTrademark(SendBufferFix.BuildInfo.Company)]
[assembly: AssemblyVersion(SendBufferFix.BuildInfo.Version)]
[assembly: AssemblyFileVersion(SendBufferFix.BuildInfo.Version)]
[assembly: MelonInfo(typeof(SendBufferFix.SendBufferFix), SendBufferFix.BuildInfo.Name, SendBufferFix.BuildInfo.Version, SendBufferFix.BuildInfo.Author, SendBufferFix.BuildInfo.DownloadLink)]
[assembly: MelonColor(255,0,255,255)]
// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("CuriousOwlGames", "StellarDrive")]