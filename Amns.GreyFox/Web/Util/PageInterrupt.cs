using System;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for PageInterruptor.
	/// </summary>
	public enum PageInterruptType : int { 
		OK								= 200,
        BadRequest						= 400,
		Unauthorized					= 401,
		PaymentRequired					= 402,
		Forbidden						= 403,
		NotFound						= 404,
		MethodNotAllowed				= 405,
		NotAcceptable					= 406,
		ProxyAuthenticationRequired		= 407,
		RequestTimeout					= 408,
		Conflict						= 409,
		Gone							= 410,
		LengthRequired					= 411,
		PreconditionFailed				= 412,
		RequestEntityTooLarge			= 413,
		RequestUrlTooLong				= 414,
		UnsupportedMediaType			= 415,
		ExpectationFailed				= 417,
		InternalServerError				= 500,
		NotImplemeneted					= 501,
		BadGateway						= 502,
		ServiceUnavailable				= 503,
		GatewayTimeout					= 504,
		HttpVersionNotSupported			= 505,
	};
}
