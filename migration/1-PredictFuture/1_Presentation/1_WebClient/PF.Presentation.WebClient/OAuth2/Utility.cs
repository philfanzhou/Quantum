using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PF.Presentation.WebClient
{
	internal static class Utility
	{

		internal static string GetBoundary()
		{
			string pattern = "abcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder boundaryBuilder = new StringBuilder();
			Random rnd = new Random();
			for (int i = 0; i < 10; i++)
			{ 
				var index = rnd.Next(pattern.Length);
				boundaryBuilder.Append(pattern[index]);
			}
			return boundaryBuilder.ToString();
		}

		internal static string BuildQueryString(params RequestParameter[] parameters)
		{
			List<string> pairs = new List<string>();
			foreach (var item in parameters)
			{
				if(item.IsBinaryData)
					continue;

				var value = string.Format("{0}", item.Value);

				if (!string.IsNullOrEmpty(value))
				{
					pairs.Add(string.Format("{0}={1}", Uri.EscapeDataString(item.Name), Uri.EscapeDataString(value)));
				}
			}
			return string.Join("&", pairs.ToArray());
		}

		internal static string BuildQueryString(IEnumerable<KeyValuePair<string, object>> parameters)
		{
			var reqeustParameters = new List<RequestParameter>();
			foreach (var item in parameters)
			{
				reqeustParameters.Add(new RequestParameter(item.Key, item.Value));
			}

			return BuildQueryString(reqeustParameters.ToArray());
		}


		internal static byte[] BuildPostData(string boundary, params RequestParameter[] parameters)
		{
			List<RequestParameter> pairs = new List<RequestParameter>(parameters);
			pairs.Sort(new RequestParameterComparer());

			MemoryStream buff = new MemoryStream();

			string header = string.Format("--{0}", boundary);
			string footer = string.Format("--{0}--", boundary);
			byte[] headerBuff = Encoding.ASCII.GetBytes(Environment.NewLine + header + Environment.NewLine);
			byte[] footerBuff = Encoding.ASCII.GetBytes(Environment.NewLine + footer);

			foreach (RequestParameter p in pairs)
			{

				if (!p.IsBinaryData)
				{
					var value = string.Format("{0}", p.Value);
					if (string.IsNullOrEmpty(value))
					{
						continue;
					}

					buff.Write(headerBuff, 0, headerBuff.Length);
					byte[] dispositonBuff = Encoding.UTF8.GetBytes(string.Format("content-disposition: form-data; name=\"{0}\"\r\n\r\n{1}", p.Name, p.Value.ToString()));
					buff.Write(dispositonBuff, 0, dispositonBuff.Length);


				}
				else
				{

					buff.Write(headerBuff, 0, headerBuff.Length);
					string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: \"image/unknow\"\r\nContent-Transfer-Encoding: binary\r\n\r\n";
					byte[] fileBuff = System.Text.Encoding.UTF8.GetBytes(string.Format(headerTemplate, p.Name, string.Format("upload{0}", BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0))));
					buff.Write(fileBuff, 0, fileBuff.Length);
					byte[] file = (byte[])p.Value;
					buff.Write(file,0,file.Length);
				}
			}


			buff.Write(footerBuff, 0, footerBuff.Length);
			buff.Position=0;

			byte[] contentBuff = new byte[buff.Length];
			buff.Read(contentBuff, 0, contentBuff.Length);
			buff.Close();
			buff.Dispose();
			return contentBuff;
		}


	}
}
