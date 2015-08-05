using System;
using System.Collections.Generic;
#if !NET20
using System.Linq;
#endif
using System.Text;
using System.Net;
using System.IO;

namespace PF.Presentation.WebClient
{
	public abstract class OpenPlatformClient
	{
		public string ClientID
		{
			get;
			set;
		}

		public string ClientSecret
		{
			get;
			set;
		}

		public string AccessToken
		{
			get;
			set;
		}

		public string RefreshToken
		{
			get;
			set;
		}

		public string CallbackUrl
		{
			get;
			set;
		}

		public OpenPlatformClient(string clientID, string clientSecret, string callbackUrl)
		{
			ClientID = clientID;
			ClientSecret = clientSecret;
			CallbackUrl = callbackUrl;
		}

		public OpenPlatformClient(string clientID, string clientSecret, string accessToken, string refreshToken)
		{
			ClientID = clientID;
			ClientSecret = clientSecret;
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}

		public virtual string SendCommand(string command, params RequestParameter[] parameters)
		{
			return SendCommand(command, RequestMethod.Get, parameters);
		}

		public virtual string SendCommand(string command, RequestMethod method= RequestMethod.Get, params RequestParameter[] parameters)
		{
			return Request(GetCommandUrl(command), method, parameters);
		}

		protected abstract string Request(string url, RequestMethod method = RequestMethod.Get, params RequestParameter[] parameters);
		protected abstract string GetCommandUrl(string command);

		protected abstract string BaseAPIUrl
		{
			get;
		}

		protected abstract string AuthorizeUrl
		{
			get;
		}

		protected abstract string AccessTokenUrl
		{
			get;
		}
	}
}
