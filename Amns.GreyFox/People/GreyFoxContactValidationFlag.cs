using System;

namespace Amns.GreyFox.People
{
	/// <summary>
	/// Contact method is 
	/// </summary>
    [Flags]
	public enum GreyFoxContactValidationFlag : byte
	{
		BadAddress = 0x01,                  // 0000 0001
		BadHomePhone = 0x02,                // 0000 0010
		BadWorkPhone = 0x04,                // 0000 0100
		BadMobilePhone = 0x08,              // 0000 1000
		BadEmail = 0x10,                    // 0001 0000
        BadUrl = 0x20,                      // 0010 0000
	}
}