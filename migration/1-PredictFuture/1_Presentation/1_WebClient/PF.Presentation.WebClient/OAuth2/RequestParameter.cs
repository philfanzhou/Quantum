using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PF.Presentation.WebClient
{
	public class RequestParameter
	{

		public string Name
		{
			get;
			internal set;
		}

		public object Value
		{
			get;
			internal set;
		}

		public bool IsBinaryData
		{
			get
			{
				if (Value!=null && Value.GetType() == typeof(byte[]))
					return true;
				else
					return false;
			}
		}

		public RequestParameter()
		{ 
		
		}

		public RequestParameter(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public RequestParameter(string name, bool value)
		{
			Name = name;
			Value = value ? "1" : "0";
		}

		public RequestParameter(string name, int value)
		{
			Name = name;
			Value = string.Format("{0}",value);
		}

		public RequestParameter(string name, long value)
		{
			Name = name;
			Value = string.Format("{0}", value);
		}

		public RequestParameter(string name, float value)
		{
			Name = name;
			Value = string.Format("{0}", value);
		}

		public RequestParameter(string name, double value)
		{
			Name = name;
			Value = string.Format("{0}", value);
		}

		public RequestParameter(string name, byte[] value)
		{
			Name = name;
			Value = value;
		}

		internal RequestParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}


	}
}
