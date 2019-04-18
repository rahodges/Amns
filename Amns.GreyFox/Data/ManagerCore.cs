//===============================================================================
// AMNS GreyFox
// Core Library
//===============================================================================
// Copyright © Roy A.E. Hodges.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Reflection;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for ManagerCore.
	/// </summary>
	public class ManagerCore
	{
		/* *****************************************************************
		 * Plugin manager implements a fully lazy singleton pattern.
		 * ***************************************************************** */

		string _connectionString;

		bool _isInitialized;
        
		ExposedManagerCollection _exposedManagers = new ExposedManagerCollection();

		public ExposedManagerCollection ExposedManagers
		{
			get { return _exposedManagers; }
		}

		public string ConnectionString
		{
			get { return _connectionString; }
			set 
			{
				if(_connectionString != value)
				{
                    if (_isInitialized)
                    {
                        //throw new Exception("Manager core already initialized; " +
                        //    "cannot change connection string.");
                    }
                    
					_connectionString = value;
				}
			}
		}

		public bool IsInitialized
		{
			get { return _isInitialized; }
		}

		#region Singleton Core

		public ManagerCore() { }

		public static ManagerCore GetInstance()
		{
			return Nested.instance;
		}

		class Nested
		{
			static Nested()
			{
			}

			internal static readonly ManagerCore instance = new ManagerCore();
		}

		#endregion

		#region Initialize

		public void Initialize()
		{
			string directory;

			if(System.Web.HttpContext.Current != null)
			{
				directory = System.Web.HttpContext.Current.Server.MapPath("~/bin");
			}
			else
			{
				directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
			}
			
			Initialize(Directory.GetFiles(directory, "*.dll"));
		}
		
		public void Initialize(string[] fileNames)
		{
			FileInfo[] files = new FileInfo[fileNames.GetUpperBound(0)];
			for(int i = 0; i < fileNames.GetUpperBound(0); i++)
			{
				files[i] = new FileInfo(fileNames[i]);
			}
			Initialize(files);
		}

		public void Initialize(System.IO.FileInfo[] files)		
		{	
			Assembly assembly;
			Type[] assemblyTypes;
			object[] typeAttributes;
			ExposedManager eManager;
			IGreyFoxManager manager;
			object[] blankParams = new object[] { "" };
			
			if(_isInitialized)
			{
                //throw(new Exception("ManagerCore already initialized; cannot reinitialize."));
			}
			
			for(int i = 0; i < files.Length; i++)
			{
				assembly = Assembly.LoadFrom(files[i].FullName);
				assemblyTypes = assembly.GetTypes();

				foreach(Type type in assemblyTypes)
				{
					typeAttributes = 
						type.GetCustomAttributes(typeof(ExposedManager), false);
						
					if(typeAttributes.Length > 0)
					{
						eManager = (ExposedManager) typeAttributes[0];
					
						if(eManager.IsTableCoded)
						{
							manager = (IGreyFoxManager) Activator.CreateInstance(type);
						}
						else
						{
							manager = (IGreyFoxManager) Activator.CreateInstance(type, blankParams);
						}

						eManager.Manager = manager;
						
						eManager.Manager.Initialize(_connectionString);
						_exposedManagers.Add(eManager);
					}
				}
			}

			_isInitialized = true;
		}

		#endregion
	}
}