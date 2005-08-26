using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Win32;
//TODO do we really want to load stuff using registry keys? I dont think so!!

namespace Alchemi.AppStart
{
	//perhaps best to use xml config saving. instead of this. TODO
	//**************************************************************
	// LoadRemoteConfigFile()
	// Used to access the appstart.config file.  
	// Use the static load method to instatiate an AppStartConfig object
	//**************************************************************
	public class AppStartConfig
	{
		public enum LaunchModes {AppDomain=0,Process=1};

		private string ConfigPath;

		private string _AppFolderName;
		public string AppFolderName 
		{ get { return _AppFolderName; } set { _AppFolderName = value; } }

		private string _AppExeName;
		public string AppExeName 
		{ get { return _AppExeName; } set { _AppExeName = value; } }
		
		private LaunchModes _AppLaunchMode;
		public LaunchModes AppLaunchMode 
		{ get { return _AppLaunchMode; } set { _AppLaunchMode = value; } }
		
		//Esssentially a calculated/readonly prop;
		public string AppExePath
		{
			get 
			{
				return (Path.Combine(Path.GetDirectoryName(ConfigPath),AppFolderName) + @"\" + AppExeName);
			}
			set {}
		}

		//Esssentially a calculated/readonly prop;
		public string AppPath
		{
			get 
			{
				return (Path.Combine(Path.GetDirectoryName(ConfigPath),AppFolderName)+ @"\");
			}
			set {}
		}

		//**************************************************************
		// AppStartConfig()	
		//**************************************************************
		public AppStartConfig(string filePath) 
		{
			ConfigPath = filePath;
		}

		//**************************************************************
		// Load()	
		//**************************************************************
		public static AppStartConfig Load(string filePath)
		{
			AppStartConfig Config = new AppStartConfig(filePath);
			
			try 
			{
				//Load the xml config file
				XmlDocument XmlDoc = new XmlDocument();
				try {
					XmlDoc.Load(filePath);
				}
				catch(Exception e) {
					throw new ConfigFileMissingException("Config file '" + filePath + "' is missing.", e);
				}

				//Parse out the redirection key
				string KeyValue = "";
				try 
				{
					XmlNode AppRedirNode = XmlDoc.SelectSingleNode(@"//AppRedirectionKey");
					KeyValue = AppRedirNode.InnerText;
				} 
				//The Key not present case
				catch (Exception)
				{
					KeyValue ="";
				}

				if (KeyValue != "")
				{
					return LoadRemoteConfigFile(KeyValue, filePath);
				}

				//Parse out the AppPath
				XmlNode AppPathNode = XmlDoc.SelectSingleNode(@"//AppFolderName");
				Config.AppFolderName = AppPathNode.InnerText;

				//Parse out the AppExeName
				XmlNode AppExeNode = XmlDoc.SelectSingleNode(@"//AppExeName");
				Config.AppExeName = AppExeNode.InnerText;

				//Parse out the AppLauchMode
				XmlNode AppLaunchModeNode = XmlDoc.SelectSingleNode(@"//AppLaunchMode");
				if (AppLaunchModeNode == null)
					//Default Value
					Config.AppLaunchMode = LaunchModes.Process;
				else 
				{
					if (AppLaunchModeNode.InnerText.ToLower(new CultureInfo("en-US")) == "appdomain")
						Config.AppLaunchMode = LaunchModes.AppDomain;
					else
						Config.AppLaunchMode = LaunchModes.Process;
				}

				return Config;
			
			} 
			catch (Exception e)
			{
				Debug.WriteLine("Failed to read the appstart config file at: " + filePath ); 
				Debug.WriteLine("Make sure that the config file is present & has a valid format");
				throw e;
			}
		}

		//**************************************************************
		// Update()	
		//**************************************************************
		public void Udpate()
		{
			try 
			{
				//Create a new xml config doc
				XmlDocument XmlDoc = new XmlDocument();

				//Create the root node
				XmlElement Root = XmlDoc.CreateElement("Config");
				XmlDoc.AppendChild(Root);

				//Insert the AppPath
				XmlElement XmlElem = XmlDoc.CreateElement("AppFolderName");
				XmlElem.InnerText = AppFolderName;
				Root.AppendChild(XmlElem);

				//Insert the AppExeName
				XmlElem = XmlDoc.CreateElement("AppExeName");
				XmlElem.InnerText = AppExeName;
				Root.AppendChild(XmlElem);

				//Insert the AppExeName
				XmlElem = XmlDoc.CreateElement("AppLaunchMode");
				if (AppLaunchMode == LaunchModes.AppDomain)
					XmlElem.InnerText = "appdomain";
				else
					XmlElem.InnerText = "process";
				Root.AppendChild(XmlElem);

				//Save the xml doc
				XmlDoc.Save(ConfigPath);
			
			} 
			catch (Exception e)
			{
				Debug.WriteLine("Failed to update appstart config file, make sure" 
					+ "that the config file is not locked");
				throw e;
			}
		}

		//**************************************************************
		// LoadRemoteConfigFile()	
		//**************************************************************
		private static AppStartConfig LoadRemoteConfigFile(string key, string orgConfigPath)
		{
			string NewConfigPath = "";

			try 
			{
				RegistryKey  r = Registry.LocalMachine.OpenSubKey(key);

				NewConfigPath = (string)r.GetValue("InstallDir"); 
				NewConfigPath = Path.Combine(NewConfigPath,Path.GetFileName(orgConfigPath));

				return AppStartConfig.Load(NewConfigPath);
			} 
			catch (Exception e)
			{
				Debug.WriteLine("Error loading config file at "+ NewConfigPath + " as specified by registry key.");
				throw e;
			}
		}
	}

	[Serializable()]
	public class ConfigFileMissingException : ApplicationException {
		public ConfigFileMissingException(string message, Exception innerException) : base(message, innerException){
		}
	}

}
