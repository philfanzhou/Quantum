using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Presentation.WebClient
{
	[Serializable]
	public class OpenPlatformException : System.Net.WebException
	{
		public OpenPlatformException(string message)
			: base(message)
		{
		}

	}
}
