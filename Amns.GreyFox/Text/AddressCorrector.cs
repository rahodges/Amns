using System;

namespace Amns.GreyFox.Text
{
	/// <summary>
	/// Summary description for AddressParser.
	/// </summary>
	public class AddressCorrector
	{
		/// <summary>
		/// Parses an address from the text provided. Applies capitalization and dictionary corrections.
		/// Dictionary corrections can be ignored by placing an explanation point before the text.
		/// </summary>
		/// <param name="text">Text to parse into an address.</param>
		/// <returns>Parsed address.</returns>
		public static string Parse(string text)
		{
			if(text != string.Empty || text != "")
			{
				if(text[0] == '!')
					return Parse(text.Substring(1), false);

				return Parse(text, true);
			}

			return text;
		}
		
		public static string Parse(string text, bool useDictionary)
		{		
			// NO CORRECTIONS!
			return text;

//			// Capitalize First Character of Each Word
//			// Capitalize Letters after Apostrophe's
//			// Correct trackText in dictionary if parameter is enabled
//
//			//			 * Here are the proper parsing rules:
//			//			 * - Do not correct text immediately after a number or cardinal direction.
//			//			 *	 123 Geoffery Highway		> 123 Geoffery Hwy.
//			//			 *	 123 Military Road			> 123 Military Rd.
//			//			 *	 144 east wayne avenue		> 123 E. Wayne Ave.					
//			//			 * - Always correct the last word.
//			//			 *	 123 butternut street nw	> 123 Butternut St. N.W. */
//
//			System.Text.StringBuilder s				= new System.Text.StringBuilder();
//						
//			char[] c								= text.Trim().ToCharArray();		// Char array to scan
//			bool caps								= true;
//			bool holdCaps							= false;							// Forces capitalization for symbols until whitespace
//			bool lockCaps							= false;
//			int x									= 0;								// Keeps track of index
//			int max									= c.GetUpperBound(0);
//			int length								= c.Length;
//			string lastWord							= string.Empty;
//
//			while(x < length)
//			{
//				// Correct Capitalizations & Trigger a DumpTrackText on detected spaces
//
//				if(char.IsNumber(c[x]) && (x + 3 == length || (x + 3 < length && char.IsWhiteSpace(c[x+3]))))
//				{
//					if(!((char.ToLower(c[x+1]) == 's' && char.ToLower(c[x+2]) == 't') |
//						(char.ToLower(c[x+1]) == 'r' && char.ToLower(c[x+2]) == 'd') |
//						(char.ToLower(c[x+1]) == 't' && char.ToLower(c[x+2]) == 'h')))
//					{
//						holdCaps = true;
//					}
//				}
//				else if(x + 1 ==  length || (x + 1 < length && (char.IsWhiteSpace(c[x+1]) | char.IsPunctuation(c[x+1]))))
//				{
//					if((c[x-1] == 'N' && char.ToUpper(c[x]) == 'E') |
//						(c[x-1] == 'N' && char.ToUpper(c[x]) == 'W') |
//						(c[x-1] == 'S' && char.ToUpper(c[x]) == 'E') |
//						(c[x-1] == 'S' && char.ToUpper(c[x]) == 'W'))
//					{
//						caps = true;
//					}
//				}
//				else if(char.IsPunctuation(c[x]))
//				{					
//					holdCaps = true;
//				}
//				else if(char.IsWhiteSpace(c[x]))
//				{
//					holdCaps = true;
//					lockCaps = false;
//				}
//				else if(c[x] == '#')
//				{
//					lockCaps = true;
//				}
//				else if(x > 0 && c[x-1] == 'M' && c[x] == 'c')	// Set capitalization flag on "Mc"
//				{
//					holdCaps = true;
//				}
//				else if(x > 0 && x < length && c[x-1] == 'L' && c[x] == 'e' && char.IsUpper(c[x+1]))	// Set capitalization flag on "Le"
//				{
//					holdCaps = true;
//				}
//
//				if(caps)
//				{
//					c[x] = Char.ToUpper(c[x]);
//				}
//				else
//				{
//					c[x] = Char.ToLower(c[x]);
//				}
//
//				// HoldCaps Off
//				caps = holdCaps | lockCaps;
//				holdCaps = false;
//
//				s.Append(c[x]);
//
//				x++;				
//			}
//
//			return s.ToString();
		}

		/// <summary>
		/// Street suffix array consisting of Primary Street Suffix Name(0),
		/// Commonly Used Street Suffix or Abbreviation (1), and
		/// Postal Service Standard Suffix Abbreviation (2).
		/// </summary>
		public static readonly String[,] AddressDictionary = new String[,]	
			{
				{"Alley",		"ALLEE",		"ALY."},
				{"Alley",		"ALLEY",		"ALY."},
				{"Alley",		"ALLY",			"ALY."},
				{"Alley",		"ALY",			"ALY."},
				{"Annex",		"ANEX",			"ANX."},
				{"Annex",		"ANNEX",		"ANX."},
				{"Annex",		"ANNX",			"ANX."},
				{"Annex",		"ANX",			"ANX."},
				{"Arcade",		"ARC",			"ARC."},
				{"Arcade",		"ARCADE",		"ARC."},
				{"Avenue",		"AV",			"AVE."},
				{"Avenue",		"AVE",			"AVE."},
				{"Avenue",		"AVEN",			"AVE."},
				{"Avenue",		"AVENU",		"AVE."},
				{"Avenue",		"AVENUE",		"AVE."},
				{"Avenue",		"AVN",			"AVE."},
				{"Avenue",		"AVNUE",		"AVE."},
				{"Bayoo",		"BAYOO",		"BYU."},
				{"Bayoo",		"BAYOU",		"BYU."},
				{"Beach",		"BCH",			"BCH."},
				{"Beach",		"BEACH",		"BCH."},
				{"Bend",		"BEND",			"BND."},
				{"Bend",		"BND",			"BND."},
				{"Bluff",		"BLF",			"BLF."},
				{"Bluff",		"BLUF",			"BLF."},
				{"Bluff",		"BLUFF",		"BLF."},
				{"Bluffs",		"BLUFFS",		"BLFS."},
				{"Bottom",		"BOT",			"BTM."},
				{"Bottom",		"BOTTM",		"BTM."},
				{"Bottom",		"BOTTOM",		"BTM."},
				{"Bottom",		"BTM",			"BTM."},
				{"Boulevard",	"BLVD",			"BLVD."},
				{"Boulevard",	"BOUL",			"BLVD."},
				{"Boulevard",	"BOULEVARD",	"BLVD."},
				{"Boulevard",	"BOULV",		"BLVD."},
				{"Branch",		"BR",			"BR."},
				{"Branch",		"BRANCH",		"BR."},
				{"Branch",		"BRNCH",		"BR."},
				{"Bridge",		"BRDGE",		"BRG."},
				{"Bridge",		"BRG",			"BRG."},
				{"Bridge",		"BRIDGE",		"BRG."},
				{"Brook",		"BRK",			"BRK."},
				{"Brook",		"BROOK",		"BRK."},
				{"Brooks",		"BROOKS",		"BRKS."},
				{"Burg",		"BURG",			"BG."},
				{"Burgs",		"BURGS",		"BGS."},
				{"Bypass",		"BYP",			"BYP."},
				{"Bypass",		"BYPA",			"BYP."},
				{"Bypass",		"BYPAS",		"BYP."},
				{"Bypass",		"BYPASS",		"BYP."},
				{"Bypass",		"BYPS",			"BYP."},
				{"Camp",		"CAMP",			"CP."},
				{"Camp",		"CMP",			"CP."},
				{"Camp",		"CP",			"CP."},
				{"Canyon",		"CANYN",		"CYN."},
				{"Canyon",		"CANYON",		"CYN."},
				{"Canyon",		"CNYN",			"CYN."},
				{"Canyon",		"CYN",			"CYN."},
				{"Cape",		"CAPE",			"CPE."},
				{"Cape",		"CPE",			"CPE."},
				{"Causeway",	"CAUSEWAY",		"CSWY."},
				{"Causeway",	"CAUSWAY",		"CSWY."},
				{"Causeway",	"CSWY",			"CSWY."},
				{"Center",		"CEN",			"CTR."},
				{"Center",		"CENT",			"CTR."},
				{"Center",		"CENTER",		"CTR."},
				{"Center",		"CENTR",		"CTR."},
				{"Center",		"CENTRE",		"CTR."},
				{"Center",		"CNTER",		"CTR."},
				{"Center",		"CNTR",			"CTR."},
				{"Center",		"CTR",			"CTR."},
				{"Centers",		"CENTERS",		"CTRS."},
				{"Circle",		"CIR",			"CIR."},
				{"Circle",		"CIRC",			"CIR."},
				{"Circle",		"CIRCL",		"CIR."},
				{"Circle",		"CIRCLE",		"CIR."},
				{"Circle",		"CRCL",			"CIR."},
				{"Circle",		"CRCLE",		"CIR."},
				{"Circles",		"CIRCLES",		"CIRS."},
				{"Cliff",		"CLF",			"CLF."},
				{"Cliff",		"CLIFF",		"CLF."},
				{"Cliffs",		"CLFS",			"CLFS."},
				{"Cliffs",		"CLIFFS",		"CLFS."},
				{"Club",		"CLB",			"CLB."},
				{"Club",		"CLUB",			"CLB."},
				{"Common",		"COMMON",		"CMN."},
				{"Corner",		"COR",			"COR."},
				{"Corner",		"CORNER",		"COR."},
				{"Corners",		"CORNERS",		"CORS."},
				{"Corners",		"CORS",			"CORS."},
				{"Course",		"COURSE",		"CRSE."},
				{"Course",		"CRSE",			"CRSE."},
				{"Court",		"COURT",		"CT."},
				{"Court",		"CRT",			"CT."},
				{"Court",		"CT",			"CT."},
				{"Courts",		"COURTS",		"CTS."},
				{"Courts",		"CT",			"CTS."},
				{"Cove",		"COVE",			"CV."},
				{"Cove",		"CV",			"CV."},
				{"Coves",		"COVES",		"CVS."},
				{"Creek",		"CK",			"CRK."},
				{"Creek",		"CR",			"CRK."},
				{"Creek",		"CREEK",		"CRK."},
				{"Creek",		"CRK",			"CRK."},
				{"Crescent",	"CRECENT",		"CRES."},
				{"Crescent",	"CRES",			"CRES."},
				{"Crescent",	"CRESCENT",		"CRES."},
				{"Crescent",	"CRESENT",		"CRES."},
				{"Crescent",	"CRSCNT",		"CRES."},
				{"Crescent",	"CRSENT",		"CRES."},
				{"Crescent",	"CRSNT",		"CRES."},
				{"Crest",		"CREST",		"CRST."},
				{"Crossing",	"CROSSING",		"XING."},
				{"Crossing",	"CRSSING",		"XING."},
				{"Crossing",	"CRSSNG",		"XING."},
				{"Crossing",	"XING",			"XING."},
				{"Crossroad",	"CROSSROAD",	"XRD."},
				{"Curve",		"CURVE",		"CURV."},
				{"Dale",		"DALE",			"DL."},
				{"Dale",		"DL",			"DL."},
				{"Dam",			"DAM",			"DM."},
				{"Dam",			"DM",			"DM."},
				{"Divide",		"DIV",			"DV."},
				{"Divide",		"DIVIDE",		"DV."},
				{"Divide",		"DV",			"DV."},
				{"Divide",		"DVD",			"DV."},
				{"Drive",		"DR",			"DR."},
				{"Drive",		"DRIV",			"DR."},
				{"Drive",		"DRIVE",		"DR."},
				{"Drive",		"DRV",			"DR."},
				{"Drives",		"DRIVES",		"DRS."},
				{"East",		"E.",			"\\uE."},			// Cardinal Direction
				{"Estate",		"EST",			"EST."},
				{"Estate",		"ESTATE",		"EST."},
				{"Estates",		"ESTATES",		"ESTS."},
				{"Estates",		"ESTS",			"ESTS."},
				{"Expressway",	"EXP",			"EXPY."},
				{"Expressway",	"EXPR",			"EXPY."},
				{"Expressway",	"EXPRESS",		"EXPY."},
				{"Expressway",	"EXPRESSWAY",	"EXPY."},
				{"Expressway",	"EXPW",			"EXPY."},
				{"Expressway",	"EXPY",			"EXPY."},
				{"Extension",	"EXT",			"EXT."},
				{"Extension",	"EXTENSION",	"EXT."},
				{"Extension",	"EXTN",			"EXT."},
				{"Extension",	"EXTNSN",		"EXT."},
				{"Extensions",	"EXTENSIONS",	"EXTS."},
				{"Extensions",	"EXTS",			"EXTS."},
				{"Fall",		"FALL",			"FALL."},
				{"Falls",		"FALLS",		"FLS."},
				{"Falls",		"FLS",			"FLS."},
				{"Ferry",		"FERRY",		"FRY."},
				{"Ferry",		"FRRY",			"FRY."},
				{"Ferry",		"FRY",			"FRY."},
				{"Field",		"FIELD",		"FLD."},
				{"Field",		"FLD",			"FLD."},
				{"Fields",		"FIELDS",		"FLDS."},
				{"Fields",		"FLDS",			"FLDS."},
				{"Flat",		"FLAT",			"FLT."},
				{"Flat",		"FLT",			"FLT."},
				{"Flats",		"FLATS",		"FLTS."},
				{"Flats",		"FLTS",			"FLTS."},
				{"Ford",		"FORD",			"FRD."},
				{"Ford",		"FRD",			"FRD."},
				{"Fords",		"FORDS",		"FRDS."},
				{"Forest",		"FOREST",		"FRST."},
				{"Forest",		"FORESTS",		"FRST."},
				{"Forest",		"FRST",			"FRST."},
				{"Forge",		"FORG",			"FRG."},
				{"Forge",		"FORGE",		"FRG."},
				{"Forge",		"FRG",			"FRG."},
				{"Forges",		"FORGES",		"FRGS."},
				{"Fork",		"FORK",			"FRK."},
				{"Fork",		"FRK",			"FRK."},
				{"Forks",		"FORKS",		"FRKS."},
				{"Forks",		"FRKS",			"FRKS."},
				{"Fort",		"FORT",			"FT."},
				{"Fort",		"FRT",			"FT."},
				{"Fort",		"FT",			"FT."},
				{"Freeway",		"FREEWAY",		"FWY."},
				{"Freeway",		"FREEWY",		"FWY."},
				{"Freeway",		"FRWAY",		"FWY."},
				{"Freeway",		"FRWY",			"FWY."},
				{"Freeway",		"FWY",			"FWY."},
				{"Garden",		"GARDEN",		"GDN."},
				{"Garden",		"GARDN",		"GDN."},
				{"Garden",		"GDN",			"GDN."},
				{"Garden",		"GRDEN",		"GDN."},
				{"Garden",		"GRDN",			"GDN."},
				{"Gardens",		"GARDENS",		"GDNS."},
				{"Gardens",		"GDNS",			"GDNS."},
				{"Gardens",		"GRDNS",		"GDNS."},
				{"Gateway",		"GATEWAY",		"GTWY."},
				{"Gateway",		"GATEWY",		"GTWY."},
				{"Gateway",		"GATWAY",		"GTWY."},
				{"Gateway",		"GTWAY",		"GTWY."},
				{"Gateway",		"GTWY",			"GTWY."},
				{"Glen",		"GLEN",			"GLN."},
				{"Glen",		"GLN",			"GLN."},
				{"Glens",		"GLENS",		"GLNS."},
				{"Green",		"GREEN",		"GRN."},
				{"Green",		"GRN",			"GRN."},
				{"Greens",		"GREENS",		"GRNS."},
				{"Grove",		"GROV",			"GRV."},
				{"Grove",		"GROVE",		"GRV."},
				{"Grove",		"GRV",			"GRV."},
				{"Groves",		"GROVES",		"GRVS."},
				{"Harbor",		"HARB",			"HBR."},
				{"Harbor",		"HARBOR",		"HBR."},
				{"Harbor",		"HARBR",		"HBR."},
				{"Harbor",		"HBR",			"HBR."},
				{"Harbor",		"HRBOR",		"HBR."},
				{"Harbors",		"HARBORS",		"HBRS."},
				{"Haven",		"HAVEN",		"HVN."},
				{"Haven",		"HAVN",			"HVN."},
				{"Haven",		"HVN",			"HVN."},
				{"Heights",		"HEIGHT",		"HTS."},
				{"Heights",		"HEIGHTS",		"HTS."},
				{"Heights",		"HGTS",			"HTS."},
				{"Heights",		"HT",			"HTS."},
				{"Heights",		"HTS",			"HTS."},
				{"Highway",		"HEIGHWAY",		"HWY."},
				{"Highway",		"HIGHWAY",		"HWY."},
				{"Highway",		"HIGHWY",		"HWY."},
				{"Highway",		"HIWAY",		"HWY."},
				{"Highway",		"HIWY",			"HWY."},
				{"Highway",		"HWAY",			"HWY."},
				{"Highway",		"HWY",			"HWY."},
				{"Hill",		"HILL",			"HL."},
				{"Hill",		"HL",			"HL."},
				{"Hills",		"HILLS",		"HLS."},
				{"Hills",		"HLS",			"HLS."},
				{"Hollow",		"HLLW",			"HOLW."},
				{"Hollow",		"HOLLOW",		"HOLW."},
				{"Hollow",		"HOLLOWS",		"HOLW."},
				{"Hollow",		"HOLW",			"HOLW."},
				{"Hollow",		"HOLWS",		"HOLW."},
				{"Inlet",		"INLET",		"INLT."},
				{"Inlet",		"INLT",			"INLT."},
				{"Island",		"IS",			"IS."},
				{"Island",		"ISLAND",		"IS."},
				{"Island",		"ISLND",		"IS."},
				{"Islands",		"ISLANDS",		"ISS."},
				{"Islands",		"ISLNDS",		"ISS."},
				{"Islands",		"ISS",			"ISS."},
				{"Isle",		"ISLE",			"ISLE."},
				{"Isle",		"ISLES",		"ISLE."},
				{"Junction",	"JCT",			"JCT."},
				{"Junction",	"JCTION",		"JCT."},
				{"Junction",	"JCTN",			"JCT."},
				{"Junction",	"JUNCTION",		"JCT."},
				{"Junction",	"JUNCTN",		"JCT."},
				{"Junction",	"JUNCTON",		"JCT."},
				{"Junctions",	"JCTNS",		"JCTS."},
				{"Junctions",	"JCTS",			"JCTS."},
				{"Junctions",	"JUNCTIONS",	"JCTS."},
				{"Key",			"KEY",			"KY."},
				{"Key",			"KY",			"KY."},
				{"Keys",		"KEYS",			"KYS."},
				{"Keys",		"KYS",			"KYS."},
				{"Knoll",		"KNL",			"KNL."},
				{"Knoll",		"KNOL",			"KNL."},
				{"Knoll",		"KNOLL",		"KNL."},
				{"Knolls",		"KNLS",			"KNLS."},
				{"Knolls",		"KNOLLS",		"KNLS."},
				{"Lake",		"LAKE",			"LK."},
				{"Lake",		"LK",			"LK."},
				{"Lakes",		"LAKES",		"LKS."},
				{"Lakes",		"LKS",			"LKS."},
				{"Land",		"LAND",			"LAND"},
				{"Landing",		"LANDING",		"LNDG."},
				{"Landing",		"LNDG",			"LNDG."},
				{"Landing",		"LNDNG",		"LNDG."},
				{"Lane",		"LA",			"LN."},
				{"Lane",		"LANE",			"LN."},
				{"Lane",		"LANES",		"LN."},
				{"Lane",		"LN",			"LN."},
				{"Light",		"LGT",			"LGT."},
				{"Light",		"LIGHT",		"LGT."},
				{"Lights",		"LIGHTS",		"LGTS."},
				{"Loaf",		"LF",			"LF."},
				{"Loaf",		"LOAF",			"LF."},
				{"Lock",		"LCK",			"LCK."},
				{"Lock",		"LOCK",			"LCK."},
				{"Locks",		"LCKS",			"LCKS."},
				{"Locks",		"LOCKS",		"LCKS."},
				{"Lodge",		"LDG",			"LDG."},
				{"Lodge",		"LDGE",			"LDG."},
				{"Lodge",		"LODG",			"LDG."},
				{"Lodge",		"LODGE",		"LDG."},
				{"Loop",		"LOOP",			"LOOP."},
				{"Loop",		"LOOPS",		"LOOP."},
				{"Mall",		"MALL",			"MALL."},
				{"Manor",		"MANOR",		"MNR."},
				{"Manor",		"MNR",			"MNR."},
				{"Manors",		"MANORS",		"MNRS."},
				{"Manors",		"MNRS",			"MNRS."},
				{"Meadow",		"MDW",			"MDW."},
				{"Meadow",		"MEADOW",		"MDW."},
				{"Meadows",		"MDWS",			"MDWS."},
				{"Meadows",		"MEADOWS",		"MDWS."},
				{"Meadows",		"MEDOWS",		"MDWS."},
				{"Mews",		"MEWS",			"MEWS."},
				{"Mill",		"MILL",			"ML."},
				{"Mill",		"ML",			"ML."},
				{"Mills",		"MILLS",		"MLS."},
				{"Mills",		"MLS",			"MLS."},
				{"Mission",		"MISSION",		"MSN."},
				{"Mission",		"MISSN",		"MSN."},
				{"Mission",		"MSN",			"MSN."},
				{"Mission",		"MSSN",			"MSN."},
				{"Motorway",	"MOTORWAY",		"MTWY."},
				{"Mount",		"MNT",			"MT."},
				{"Mount",		"MOUNT",		"MT."},
				{"Mount",		"MT",			"MT."},
				{"Mountain",	"MNTAIN",		"MTN."},
				{"Mountain",	"MNTN",			"MTN."},
				{"Mountain",	"MOUNTAIN",		"MTN."},
				{"Mountain",	"MOUNTIN",		"MTN."},
				{"Mountain",	"MTIN",			"MTN."},
				{"Mountain",	"MTN",			"MTN."},
				{"Mountains",	"MNTNS",		"MTNS."},
				{"Mountains",	"MOUNTAINS",	"MTNS."},
				{"North",		"N.",			"\\uN."}, 
				{"Northeast",	"NE",			"\\uN.E."},
				{"N.E.",		"NE",			"\\uN.E."},					// Direction
				{"Northwest",	"NW",			"\\uN.W."},
				{"N.W.",		"NW",			"\\uN.W."},					// Direction
				{"Neck",		"NCK",			"NCK."},
				{"Neck",		"NECK",			"NCK."},
				{"Orchard",		"ORCH",			"ORCH."},
				{"Orchard",		"ORCHARD",		"ORCH."},
				{"Orchard",		"ORCHRD",		"ORCH."},
				{"Oval",		"OVAL",			"OVAL."},
				{"Oval",		"OVL",			"OVAL."},
				{"Overpass",	"OVERPASS",		"OPAS."},
				{"Park",		"PARK",			"PARK"},
				{"Park",		"PK",			"PARK"},
				{"Park",		"PRK",			"PARK"},
				{"Parks",		"PARKS",		"PARK"},
				{"Parkway",		"PARKWAY",		"PKWY."},
				{"Parkway",		"PARKWY",		"PKWY."},
				{"Parkway",		"PKWAY",		"PKWY."},
				{"Parkway",		"PKWY",			"PKWY."},
				{"Parkway",		"PKY",			"PKWY."},
				{"Parkways",	"PARKWAYS",		"PKWY."},
				{"Parkways",	"PKWYS",		"PKWY."},
				{"Pass",		"PASS",			"PASS."},
				{"Passage",		"PASSAGE",		"PSGE."},
				{"Path",		"PATH",			"PATH"},
				{"Path",		"PATHS",		"PATH"},
				{"Pike",		"PIKE",			"PIKE"},
				{"Pike",		"PIKES",		"PIKE"},
				{"Pine",		"PINE",			"PNE."},
				{"Pines",		"PINES",		"PNES."},
				{"Pines",		"PNES",			"PNES."},
				{"Place",		"PL",			"PL."},
				{"Place",		"PLACE",		"PL."},
				{"Plain",		"PLAIN",		"PLN."},
				{"Plain",		"PLN",			"PLN."},
				{"Plains",		"PLAINES",		"PLNS."},
				{"Plains",		"PLAINS",		"PLNS."},
				{"Plains",		"PLNS",			"PLNS."},
				{"Plaza",		"PLAZA",		"PLZ."},
				{"Plaza",		"PLZ",			"PLZ."},
				{"Plaza",		"PLZA",			"PLZ."},
				{"Point",		"POINT",		"PT."},
				{"Point",		"PT",			"PT."},
				{"Points",		"POINTS",		"PTS."},
				{"Points",		"PTS",			"PTS."},
				{"Port",		"PORT",			"PRT."},
				{"Port",		"PRT",			"PRT."},
				{"Ports",		"PORTS",		"PRTS."},
				{"Ports",		"PRTS",			"PRTS."},
				{"Prairie",		"PR",			"PR."},
				{"Prairie",		"PRAIRIE",		"PR."},
				{"Prairie",		"PRARIE",		"PR."},
				{"Prairie",		"PRR",			"PR."},
				{"Radial",		"RAD",			"RADL."},
				{"Radial",		"RADIAL",		"RADL."},
				{"Radial",		"RADIEL",		"RADL."},
				{"Radial",		"RADL",			"RADL."},
				{"Ramp",		"RAMP",			"RAMP."},
				{"Ranch",		"RANCH",		"RNCH."},
				{"Ranch",		"RANCHES",		"RNCH."},
				{"Ranch",		"RNCH",			"RNCH."},
				{"Ranch",		"RNCHS",		"RNCH."},
				{"Rapid",		"RAPID",		"RPD."},
				{"Rapid",		"RPD",			"RPD."},
				{"Rapids",		"RAPIDS",		"RPDS."},
				{"Rapids",		"RPDS",			"RPDS."},
				{"Rest",		"REST",			"RST."},
				{"Rest",		"RST",			"RST."},
				{"Ridge",		"RDG",			"RDG."},
				{"Ridge",		"RDGE",			"RDG."},
				{"Ridge",		"RIDGE",		"RDG."},
				{"Ridges",		"RDGS",			"RDGS."},
				{"Ridges",		"RIDGES",		"RDGS."},
				{"River",		"RIV",			"RIV."},
				{"River",		"RIVER",		"RIV."},
				{"River",		"RIVR",			"RIV."},
				{"River",		"RVR",			"RIV."},
				{"Road",		"RD",			"RD."},
				{"Road",		"ROAD",			"RD."},
				{"Roads",		"RDS",			"RDS."},
				{"Roads",		"ROADS",		"RDS."},
				{"Route",		"ROUTE",		"RTE."},
				{"Row",			"ROW",			"ROW."},
				{"Rue",			"RUE",			"RUE."},
				{"Run",			"RUN",			"RUN."},
				{"South",		"S.",			"\\uS."},
				{"Southeast",	"SE",			"\\uS.E."},
				{"SE",			"SE",			"\\uS.E."},					// Direction
				{"Soutwest",	"SW",			"\\uS.W."},
				{"SW",			"SW",			"\\uS.W."},					// Direction
				{"Shoal",		"SHL",			"SHL."},
				{"Shoal",		"SHOAL",		"SHL."},
				{"Shoals",		"SHLS",			"SHLS."},
				{"Shoals",		"SHOALS",		"SHLS."},
				{"Shore",		"SHOAR",		"SHR."},
				{"Shore",		"SHORE",		"SHR."},
				{"Shore",		"SHR",			"SHR."},
				{"Shores",		"SHOARS",		"SHRS."},
				{"Shores",		"SHORES",		"SHRS."},
				{"Shores",		"SHRS",			"SHRS."},
				{"Skyway",		"SKYWAY",		"SKWY."},
				{"Spring",		"SPG",			"SPG."},
				{"Spring",		"SPNG",			"SPG."},
				{"Spring",		"SPRING",		"SPG."},
				{"Spring",		"SPRNG",		"SPG."},
				{"Springs",		"SPGS",			"SPGS."},
				{"Springs",		"SPNGS",		"SPGS."},
				{"Springs",		"SPRINGS",		"SPGS."},
				{"Springs",		"SPRNGS",		"SPGS."},
				{"Spur",		"SPUR",			"SPUR."},
				{"Spurs",		"SPURS",		"SPUR."},
				{"Square",		"SQ",			"SQ."},
				{"Square",		"SQR",			"SQ."},
				{"Square",		"SQRE",			"SQ."},
				{"Square",		"SQU",			"SQ."},
				{"Square",		"SQUARE",		"SQ."},
				{"Squares",		"SQRS",			"SQS."},
				{"Squares",		"SQUARES",		"SQS."},
				{"Station",		"STA",			"STA."},
				{"Station",		"STATION",		"STA."},
				{"Station",		"STATN",		"STA."},
				{"Station",		"STN",			"STA."},
				{"Stravenue",	"STRA",			"STRA."},
				{"Stravenue",	"STRAV",		"STRA."},
				{"Stravenue",	"STRAVE",		"STRA."},
				{"Stravenue",	"STRAVEN",		"STRA."},
				{"Stravenue",	"STRAVENUE",	"STRA."},
				{"Stravenue",	"STRAVN",		"STRA."},
				{"Stravenue",	"STRVN",		"STRA."},
				{"Stravenue",	"STRVNUE",		"STRA."},
				{"Stream",		"STREAM",		"STRM."},
				{"Stream",		"STREME",		"STRM."},
				{"Stream",		"STRM",			"STRM."},
				{"Street",		"ST",			"ST."},
				{"Street",		"STR",			"ST."},
				{"Street",		"STREET",		"ST."},
				{"Street",		"STRT",			"ST."},
				{"Streets",		"STREETS",		"STS."},
				{"Summit",		"SMT",			"SMT."},
				{"Summit",		"SUMIT",		"SMT."},
				{"Summit",		"SUMITT",		"SMT."},
				{"Summit",		"SUMMIT",		"SMT."},
				{"Terrace",		"TER",			"TER."},
				{"Terrace",		"TERR",			"TER."},
				{"Terrace",		"TERRACE",		"TER."},
				{"Throughway",	"THROUGHWAY",	"TRWY."},
				{"Trace",		"TRACE",		"TRCE."},
				{"Trace",		"TRACES",		"TRCE."},
				{"Trace",		"TRCE",			"TRCE."},
				{"Track",		"TRACK",		"TRAK."},
				{"Track",		"TRACKS",		"TRAK."},
				{"Track",		"TRAK",			"TRAK."},
				{"Track",		"TRK",			"TRAK."},
				{"Track",		"TRKS",			"TRAK."},
				{"Trafficway",	"TRAFFICWAY",	"TRFY."},
				{"Trafficway",	"TRFY",			"TRFY."},
				{"Trail",		"TR",			"TRL."},
				{"Trail",		"TRAIL",		"TRL."},
				{"Trail",		"TRAILS",		"TRL."},
				{"Trail",		"TRL",			"TRL."},
				{"Trail",		"TRLS",			"TRL."},
				{"Tunnel",		"TUNEL",		"TUNL."},
				{"Tunnel",		"TUNL",			"TUNL."},
				{"Tunnel",		"TUNLS",		"TUNL."},
				{"Tunnel",		"TUNNEL",		"TUNL."},
				{"Tunnel",		"TUNNELS",		"TUNL."},
				{"Tunnel",		"TUNNL",		"TUNL."},
				{"Turnpike",	"TPK",			"TPKE."},
				{"Turnpike",	"TPKE",			"TPKE."},
				{"Turnpike",	"TRNPK",		"TPKE."},
				{"Turnpike",	"TRPK",			"TPKE."},
				{"Turnpike",	"TURNPIKE",		"TPKE."},
				{"Turnpike",	"TURNPK",		"TPKE."},
				{"Underpass",	"UNDERPASS",	"UPAS."},
				{"Union",		"UN",			"UN."},
				{"Union",		"UNION",		"UN."},
				{"Unions",		"UNIONS",		"UNS."},
				{"Valley",		"VALLEY",		"VLY."},
				{"Valley",		"VALLY",		"VLY."},
				{"Valley",		"VLLY",			"VLY."},
				{"Valley",		"VLY",			"VLY."},
				{"Valleys",		"VALLEYS",		"VLYS."},
				{"Valleys",		"VLYS",			"VLYS."},
				{"Viaduct",		"VDCT",			"VIA."},
				{"Viaduct",		"VIA",			"VIA."},
				{"Viaduct",		"VIADCT",		"VIA."},
				{"Viaduct",		"VIADUCT",		"VIA."},
				{"View",		"VIEW",			"VW."},
				{"View",		"VW",			"VW."},
				{"Views",		"VIEWS",		"VWS."},
				{"Views",		"VWS",			"VWS."},
				{"Village",		"VILL",			"VLG."},
				{"Village",		"VILLAG",		"VLG."},
				{"Village",		"VILLAGE",		"VLG."},
				{"Village",		"VILLG",		"VLG."},
				{"Village",		"VILLIAGE",		"VLG."},
				{"Village",		"VLG",			"VLG."},
				{"Villages",	"VILLAGES",		"VLGS."},
				{"Villages",	"VLGS",			"VLGS."},
				{"Ville",		"VILLE",		"VL."},
				{"Ville",		"VL",			"VL."},
				{"Vista",		"VIS",			"VIS."},
				{"Vista",		"VIST",			"VIS."},
				{"Vista",		"VISTA",		"VIS."},
				{"Vista",		"VST",			"VIS."},
				{"Vista",		"VSTA",			"VIS."},
				{"Walk",		"WALK",			"WALK"},
				{"Walks",		"WALKS",		"WALK"},
				{"Wall",		"WALL",			"WALL"},
				{"Way",			"WAY",			"WAY"},
				{"Way",			"WY",			"WAY"},
				{"Ways",		"WAYS",			"WAYS"},
				{"Well",		"WELL",			"WL."},
				{"Wells",		"WELLS",		"WLS."},
				{"Wells",		"WLS",			"WLS."},
				{"West",		"W.",			"\\uW."}
			};
	}
}
