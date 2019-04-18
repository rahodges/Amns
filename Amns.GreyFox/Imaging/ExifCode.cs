using System;

namespace Amns.GreyFox.Imaging
{
	public enum ExifCode
	{
		UnknownCode = 0x0,
		ImageDescription = 0x010e,
		Manufacturer = 0x010f ,
		Model  = 0x0110,
		//Orientation of camera to scene
		Orientation  = 0x0112,  
   
		//Display / Print resoution
		XResolution   = 0x011a, 
		YResolution  = 0x011b,      
		//1 == no unit, 2 == inch, 3 == centimeter. Default is 2
		ResolutionUnit  = 0x0128, 
		Software = 0x0131, 

		//Last modified time "YYYY:MM:DD HH:MM:SS
		DateTime  = 0x0132 , 

		WhitePoint  = 0x013e ,
		PrimaryChromaticities  = 0x013f,
		YCbCrCoefficients  =0x0211,
		YCbCrPositioning = 0x0213 ,
		ReferenceBlackWhite =0x0214,      

		Copyright = 0x8298,
		ExifOffset =0x8769,

		//1 / second
		ExposureTime =0x829a,
		//F-Stop
		FNumber = 0x829d,  

		/*
		* Exposure mode: 1 = manual, 2 = normal, 3 = aperture priority,
		 *  4 = shutter priority, 5 = program creative, 6 = program action
		* 7 = portrait, 8 = landscape
		*/
		ExposureProgram  =0x8822,

		ISOSpeedRatings =0x8827, 
		ExifVersion = 0x9000,
		DateTimeOriginal = 0x9003,
		DateTimeDigitized = 0x9004,

		ComponentsConfiguration =0x9101,
		CompressedBitsPerPixel =0x9102,
		ShutterSpeedValue = 0x9201,
		ApertureValue  =0x9202,
		BrightnessValue   =0x9203,
		ExposureBiasValue =0x9204 ,
		MaxApertureValue = 0x9205,
		SubjectDistance  = 0x9206,
		MeteringMode = 0x9207,
		LightSource  = 0x9208,
		Flash  = 0x9209,
		FocalLength = 0x920a,
		MakerNote =0x927c,

		UserComment =0x9286,
		SubsecTime  =0x9290,
		SubsecTimeOriginal =0x9291,
		SubsecTimeDigitized = 0x9292,

		Title = 0x9c9b,
		Comments = 0x9c9c,
		Author = 0x9c9d,
		Keywords = 0x9c9e,
		Subject = 0x9c9f,
   
		FlashPixVersion = 0xa000,
		ColorSpace = 0xa001,

		ExifImageWidth = 0xa002,
		ExifImageHeight  = 0xa003,
		RelatedSoundFile = 0xa004,
		ExifInteroperabilityOffset  = 0xa005,
		FocalPlaneXResolution =0xa20e,
		FocalPlaneYResolution = 0xa20f,
		FocalPlaneResolutionUnit =0xa210,
		ExposureIndex =0xa215,
		SensingMethod =0xa217 ,
		FileSource =0xa300,
		SceneType =0xa301,
		CFAPattern  =0xa302,
		InteroperabilityIndex = 0x0001,
		InteroperabilityVersion =0x0002 ,
		RelatedImageFileFormat = 0x1000,
		RelatedImageWidth  =0x1001,
		RelatedImageLength  = 0x1001,
		ImageWidth = 0x0100,
		ImageLength  = 0x0101,
		BitsPerSample =0x0102,
		Compression  =0x0103,
		PhotometricInterpretation  = 0x0106,
		StripOffsets = 0x0111,
		SamplesPerPixel  = 0x0115,
		RowsPerStrip = 0x0116,
		StripByteConunts = 0x0117 ,
		PlanarConfiguration  = 0x011c,
		JpegIFOffset  = 0x0201,
		JpegIFByteCount =0x0202,
		YCbCrSubSampling =0x0212 ,

	}
}
