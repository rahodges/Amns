using System;

namespace Amns.GreyFox.Content
{
	public enum ContentLogEntryType : int 
	{
		ClipCreated=0,
		ClipUpdated=1,
		ClipDeleted=2,
		ClipOpenedForEdit=3,
		ClipUdatedByEditor=4,
		ClipRejectedByEditor=5,
		ClipApprovedByEditor=6,
		ClipOpenedForReview=7,
		ClipRejectedByReviewer=8,
		ClipApprovedByReviewer=9,
		ClipPublished=10,
		ClipRejectedByPublisher=11,

		ClipHit=12,
		ClipNotFound=13,
		ChildClipHit=14,
		ChildClipNotFound=15,
		ClipExpiredXRefLookup=16,
		ClipExpiredXRefLookupNotFound=17,
		ClipExpiredXRefLookupSuccess=18,
            
		TCSEventStart=19,
		TCSArchivedClip=20,
		TCSReplacedOccurences=21,
		TCSCreatedInterCatalogXRef=22,
		TCSCreatedXRefExpirationPointer=23,
		TCSCreatedXRefReplacementPointer=24,
		TCSComplete=25,
		TCSError=26,

		CatalogCreateSuccess=27,
		CatalogInit=28,
		CatalogStart=29,
		CatalogFailed=30,
		CatalogDbFailed=31,
		CatalogDsFailed=32,
		CatalogStop=33,
		CatalogReconfigure=34
	}

	public enum ContentType: byte 
	{
		Undefined = 0,
		Static = 100,
		Dynamic = 101
	}

	public enum ContentTrackChangesType: byte
	{
		Disabled = 0,
		ReplacementMode = 1,	//
		ExpirationMode = 2		//
	}

	public enum ContentHitCounterType: byte
	{
		Disabled = 0,			//
		Normal = 1,				//	
		Recursive = 2			//
	}

	public enum ContentStatusCode: byte
	{
		Authoring = 0,
		AwaitingReview = 10,
		AwaitingEdit = 20,
		AwaitingPublishAuthorization = 100,
		AwaitingPublishDate = 101,
		Published = 105,
		Unpublished = 108,
		Expired = 109,
		Trash = 255
	}
}
