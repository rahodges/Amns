using System;

namespace Amns.GreyFox.Imaging
{
	// Based on EXIF 2.1
	enum ExifPropertyTagType : short
	{
		PropertyTagTypeByte			= 1,
		PropertyTagTypeASCII		= 2,
		PropertyTagTypeShort		= 3,
		PropertyTagTypeLong			= 4,
		PropertyTagTypeRational		= 5,
		PropertyTagTypeUndefined	= 7,
		PropertyTagTypeSLONG		= 9,
		PropertyTagTypeSRational	= 10,
	}

	// Based on EXIF 2.1
	enum ExifPropertyName : int
	{
		//A. Tags relating to image data structure
		ImageWidth = 256,
		ImageLength = 257,
		BitsPerSample = 258,
		Compression = 259,
		PhotometricInterpretation = 262,
		Orientation = 274,
		SamplesPerPixel = 277,
		PlanarConfiguration = 284,
		YCbCrSubSampling = 530,
		YCbCrPositioning = 531,
		XResolution = 282,
		YResolution = 283,
		ResolutionUnit = 296,

		//B. Tags relating to recording offset
		StripOffsets = 273,
		RowsPerStrip = 278,
		StripByteCounts = 279,
		JPEGInterchangeFormat = 513,
		JPEGInterchangeFormatLength = 514,

		//C. Tags relating to image data characteristics
		TransferFunction = 301,
		WhitePoint = 318,
		PrimaryChromaticities = 319,
		YCbCrCoefficients = 529,
		ReferenceBlackWhite = 532,

		//D. Other tags
		DateTime = 306,
		ImageDescription = 270,
		Make = 271,
		Model = 272,
		Software = 305,
		Artist = 315,
		Copyright = 3432,

		//Tags Relating to Version
		ExifVersion = 36864,
		FlashPixVersion = 40960,

		//Tag Relating to Image Data Characteristics
		ColorSpace =40961,

		//Tags Relating to Image Configuration
		ComponentsConfiguration = 37121,
		CompressedBitsPerPixel = 37122,
		PixelXDimension = 40962,
		PixelYDimension = 40963,

		//Tags Relating to User Information
		MakerNote = 37500,
		UserComment = 37510,

		//Tag Relating to Related File Information
		RelatedSoundFile  = 40964,
		
		//Tags Relating to Date and Time
		DateTimeOriginal  = 36867,
		DateTimeDigitized = 36868,
		SubSecTime  = 37520,
		SubSecTimeOriginal = 37521,
		SubSecTimeDigitized = 37522,

		//Tags Relating to Picture -Taking Conditions
		ExposureTime =33434,
		FNumber =33437,
		ExposureProgram =34850,
		SpectralSensitivity =34852,
		ISOSpeedRatings =34855,
		OECF= 34856,
		ShutterSpeedValue =37377,
		ApertureValue= 37378,
		BrightnessValue= 37379,
		ExposureBiasValue =37380,
		MaxApertureValue =37381,
		SubjectDistance= 37382,
		MeteringMode =37383,
		LightSource= 37384,
		Flash =37385,
		FocalLength =37386,
		FlashEnergy =41483,
		SpatialFrequencyResponse =41484,
		FocalPlaneXResolution =41486,
		FocalPlaneYResolution =41487,
		FocalPlaneResolutionUnit = 41488,
		SubjectLocation = 41492,
		ExposureIndex = 41493,
		SensingMethod = 41495,
		FileSource = 41728,
		SceneType = 41729,
		CFAPattern = 41730
	}
}
