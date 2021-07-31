using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Smod2;
using Smod2.Attributes;

namespace ExiledInstallerForSmod
{
	[PluginDetails(
		author = "0b10000",
		name = "EXILED Installer for Smod2",
		description = "Installs EXILED on a SMod2 server.",
		id = "0b10000.exiledinstallerforsmod",
		configPrefix = "eifs",
		langFile = "exiledinstallerforsmod",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 9,
		SmodRevision = 10
		)]
	public class InstallerPlugin : Plugin
	{
		public override void OnDisable()
		{
			Info(Details.name + " was disabled ):");
		}

		public override void OnEnable()
		{
			Info(Details.name + " has loaded :)");

			string platformSpecificString = null;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				platformSpecificString = "Win.exe";
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				platformSpecificString = "Linux";
			}

			if (platformSpecificString == null)
			{
				Error("Platform does not seem to be supported by EXILED.");
				return;
			}
			
			Info("Downloading EXILED...");
			using (var client = new WebClient())
			{ 
				client.DownloadFile(
					$"https://github.com/Exiled-Team/EXILED/releases/latest/download/Exiled.Installer-{platformSpecificString}",
					$"Exiled.Installer-{platformSpecificString}");
			}
			
			Info("Running installer...");

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Info("Marking installer as executable...");
				using (Process p = new Process())
				{
					p.StartInfo.FileName = "/bin/bash";
					p.StartInfo.Arguments = "-c \" chmod +x ./Exiled.Installer-Linux\" ";
					p.StartInfo.CreateNoWindow = true;

					p.Start();
					p.WaitForExit();
				}
			}

			using (Process p = new Process())
			{
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.FileName = $@"{Directory.GetCurrentDirectory()}/Exiled.Installer-{platformSpecificString}";
				p.Start();
				p.WaitForExit();
			}
			
			Info("Restarting server to enable EXILED...");
			
			Process.GetCurrentProcess().Kill();
		}
		
		public override void Register()
		{
			Info("why does this need to be implemented :weary:");
		}
		
	}
}