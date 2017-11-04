﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TomLabs.Shadowgem.Extensions
{
	/// <summary>
	/// XML related extensions
	/// </summary>
	public static class XmlExtensions
	{
		/// <summary>
		/// Formats given XML document
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public static string Beautify(this XDocument doc)
		{
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "\t",
				NewLineChars = Environment.NewLine,
				NewLineHandling = NewLineHandling.Replace,
				NewLineOnAttributes = true,
			};

			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				doc.Save(writer);
			}

			return sb.ToString();
		}

		public static XmlDocument ToXmlDocument(this XDocument xDocument)
		{
			var xmlDocument = new XmlDocument();
			using (var xmlReader = xDocument.CreateReader())
			{
				xmlDocument.Load(xmlReader);
			}
			return xmlDocument;
		}
		public static XDocument ToXDocument(this XmlDocument xmlDocument)
		{
			using (var nodeReader = new XmlNodeReader(xmlDocument))
			{
				nodeReader.MoveToContent();
				return XDocument.Load(nodeReader);
			}
		}
		public static XmlDocument ToXmlDocument(this XElement xElement)
		{
			var sb = new StringBuilder();
			var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
			using (var xw = XmlWriter.Create(sb, xws))
			{
				xElement.WriteTo(xw);
			}
			var doc = new XmlDocument();
			doc.LoadXml(sb.ToString());
			return doc;
		}
		public static Stream ToMemoryStream(this XmlDocument doc)
		{
			var xmlStream = new MemoryStream();
			doc.Save(xmlStream);
			xmlStream.Flush();//Adjust this if you want read your data 
			xmlStream.Position = 0;
			return xmlStream;
		}
	}
}
