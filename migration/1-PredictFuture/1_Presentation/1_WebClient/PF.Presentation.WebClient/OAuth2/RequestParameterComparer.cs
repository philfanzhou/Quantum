using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Presentation.WebClient
{
	internal class RequestParameterComparer : IComparer<RequestParameter>
	{
		public int Compare(RequestParameter x, RequestParameter y)
		{
			return StringComparer.CurrentCulture.Compare(x.Name, y.Name);
		}
	}
}
