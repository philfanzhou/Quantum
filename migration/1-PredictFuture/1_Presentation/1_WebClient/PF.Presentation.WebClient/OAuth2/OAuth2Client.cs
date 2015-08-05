using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace PF.Presentation.WebClient
{
	public class OAuthe2Client : OpenPlatformClient
	{


		protected override string BaseAPIUrl
		{
			get { return ""; }
		}

		protected override string AuthorizeUrl
		{
            get { return "https://localhost/OAuth2/authorize"; }
		}

		protected override string AccessTokenUrl
		{
            get { return "https://localhost/OAuth2/access_token"; }
		}

		public OAuthe2Client(string clientID, string clientSecret, string callbackUrl)
			: base(clientID, clientSecret, callbackUrl)
		{

		}

		public OAuthe2Client(string clientID, string clientSecret, string accessToken, string refreshToken)
			: base(clientID, clientSecret, accessToken, refreshToken)
		{

		}

		public string GetAuthorizeUrl(ResponseType response = ResponseType.Code)
		{
			Dictionary<string, object> config = new Dictionary<string, object>()
			{
				{"client_id",ClientID},
				{"redirect_uri",CallbackUrl},
				{"response_type",response.ToString().ToLower()},
			};
			UriBuilder builder = new UriBuilder(AuthorizeUrl);
			builder.Query = Utility.BuildQueryString(config);

			return builder.ToString();
		}

		/// <summary>
		/// 使用code方式获取AccessToken
		/// </summary>
		/// <param name="code">Code</param>
		/// <returns></returns>
		public string GetAccessTokenByAuthorizationCode(string code)
		{
			return GetAccessToken(GrantType.AuthorizationCode, new Dictionary<string, string> { 
				{"code",code},
				{"redirect_uri", CallbackUrl}
			});
		}

		internal string GetAccessToken(GrantType type, Dictionary<string, string> parameters)
		{

			List<RequestParameter> config = new List<RequestParameter>()
			{
				new RequestParameter(){ Name= "client_id", Value= ClientID},
				new RequestParameter(){ Name="client_secret", Value=ClientSecret}
			};

			switch (type)
			{
				case GrantType.AuthorizationCode:
					{
						config.Add(new RequestParameter() { Name = "grant_type", Value = "authorization_code" });
						config.Add(new RequestParameter() { Name = "code", Value = parameters["code"] });
						config.Add(new RequestParameter() { Name = "redirect_uri", Value = parameters["redirect_uri"] });
					}
					break;
				case GrantType.Password:
					{
						config.Add(new RequestParameter() { Name = "grant_type", Value = "password" });
						config.Add(new RequestParameter() { Name = "username", Value = parameters["username"] });
						config.Add(new RequestParameter() { Name = "password", Value = parameters["password"] });
					}
					break;
				case GrantType.RefreshToken:
					{
						config.Add(new RequestParameter() { Name = "grant_type", Value = "refresh_token" });
						config.Add(new RequestParameter() { Name = "refresh_token", Value = parameters["refresh_token"] });
					}
					break;
			}

			var response = Request(AccessTokenUrl, RequestMethod.Post, config.ToArray());

			if (!string.IsNullOrEmpty(response))
			{
				var json = DynamicJson.Parse(response);
				AccessToken = json.access_token;
				return AccessToken;
			}
			else
			{
				return null;
			}
		}


		protected override string Request(string url, RequestMethod method = RequestMethod.Get, params RequestParameter[] parameters)
		{
			UriBuilder uri = new UriBuilder(url);
			string result = string.Empty;

			bool multipart = false;
#if !NET20
			multipart = parameters.Count(p => p.IsBinaryData) > 0;
#else
			foreach (var item in parameters)
			{
				if (item.IsBinaryData)
				{
					multipart = true;
					break;
				}
			}
#endif



			switch (method)
			{
				case RequestMethod.Get:
					{
						uri.Query = Utility.BuildQueryString(parameters);
					}
					break;
				case RequestMethod.Post:
					{
						if (!multipart)
						{
							uri.Query = Utility.BuildQueryString(parameters);
						}
					}
					break;
			}

			if (string.IsNullOrEmpty(AccessToken))
			{
				if (uri.Query.Length == 0)
				{
					uri.Query = "source=" + ClientID;
				}
				else
				{
					uri.Query += "&source=" + ClientID;
				}
			}


			HttpWebRequest http = WebRequest.Create(uri.Uri) as HttpWebRequest;
			http.ServicePoint.Expect100Continue = false;
			http.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
            http.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteCertificateValidationCallback);

			if (!string.IsNullOrEmpty(AccessToken))
			{
				http.Headers["Authorization"] = string.Format("OAuth2 {0}", AccessToken);
			}

			switch (method)
			{
				case RequestMethod.Get:
					{
						http.Method = "GET";
					}
					break;
				case RequestMethod.Post:
					{
						http.Method = "POST";

						if (multipart)
						{
							string boundary = Utility.GetBoundary();
							http.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
							http.AllowWriteStreamBuffering = true;
							using (Stream request = http.GetRequestStream())
							{
								try
								{
									var raw = Utility.BuildPostData(boundary, parameters);
									request.Write(raw, 0, raw.Length);
								}
								finally
								{
									request.Close();
								}
							}
						}
						else
						{
							http.ContentType = "application/x-www-form-urlencoded";

							using (StreamWriter request = new StreamWriter(http.GetRequestStream()))
							{
								try
								{
									request.Write(Utility.BuildQueryString(parameters));
								}
								finally
								{
									request.Close();
								}
							}
						}
					}
					break;
			}

			try
			{
				using (WebResponse response = http.GetResponse())
				{

					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						try
						{
							result = reader.ReadToEnd();
						}
						catch (System.Net.WebException)
						{
							throw;
						}
						finally
						{
							reader.Close();
						}
					}


					response.Close();
				}
			}
			catch (System.Net.WebException webEx)
			{
				if (webEx.Response != null)
				{
					using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
					{
						string errorInfo = reader.ReadToEnd();
#if DEBUG
						Debug.WriteLine(errorInfo);
#endif
						
						reader.Close();

						throw new OpenPlatformException(errorInfo);
					}
				}
				else
				{
					throw new OpenPlatformException(webEx.Message);
				}

			}
			catch
			{
				throw;
			}
			return result;

		}

		protected override string GetCommandUrl(string command)
		{
			string url = string.Empty;

			if (command.StartsWith("http://") || command.StartsWith("https://"))
			{
				url = command;
			}
			else
			{
				url = string.Format("{0}{1}.json", BaseAPIUrl, command);
			}
			return url;
		}

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

	}
}
