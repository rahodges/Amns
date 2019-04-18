using System;

namespace Amns.GreyFox.Data
{
	/// <summary>
	/// Summary description for DALAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ExposedManager : Attribute
	{		
		string name;
		string description;
		
		int versionMajor;
		int versionMinor;
		int versionBuild;

		bool isTableCoded;

		IGreyFoxManager manager;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public int VersionMajor
		{
			get { return versionMajor; }
			set { versionMajor = value; }
		}

		public int VersionMinor
		{
			get { return versionMinor; }
			set { versionMinor = value; }
		}

		public int VersonBuild
		{
			get { return versionBuild; }
			set { versionBuild = value; }
		}

		public IGreyFoxManager Manager
		{
			get { return manager; }
			set { manager = value; }
		}

		public bool IsTableCoded
		{
			get { return isTableCoded; }
			set { isTableCoded = value; }
		}

		public ExposedManager(string name, string description,
			int versionMajor, int versionMinor, int versionBuild)
		{
			this.name = name;
			this.description = description;
			this.isTableCoded = true;
			this.versionMajor = versionMajor;
			this.versionMinor = versionMinor;
			this.versionBuild = versionBuild;
		}

		public ExposedManager(string name, string description, bool isTableCoded,
			int versionMajor, int versionMinor, int versionBuild)
		{
			this.name = name;
			this.description = description;
			this.isTableCoded = isTableCoded;
			this.versionMajor = versionMajor;
			this.versionMinor = versionMinor;
			this.versionBuild = versionBuild;
		}
	}
}
