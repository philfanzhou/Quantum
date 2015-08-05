using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Presentation.WebClient
{
	public enum Connector : byte
	{
		Sina,
		Tencent
	}

	public enum RequestMethod
	{ 
		Get,
		Post
	}

	/// <summary>
	/// 授权认证返回类型
	/// </summary>
	public enum ResponseType
	{
		/// <summary>
		/// Code
		/// </summary>
		Code,
		/// <summary>
		/// Access Token
		/// </summary>
		Token
	}

	internal enum GrantType
	{
		AuthorizationCode,
		Password,
		RefreshToken
	}
}
