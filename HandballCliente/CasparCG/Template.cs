using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//Copyright by Media Support - Didi Kunz didi@mediasupport.ch
//The same license conditions apply as CasparCG server has.
//
using System.Text;
using System.Xml;

[Serializable()]
public class Template
{

	#region "Properties"


	private List<TemplateField> _Fields = new List<TemplateField>();
	public string Name { get; set; }
	public string Author { get; set; }
	public string AuthorEMail { get; set; }
	public string Info { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public int FrameRate { get; set; }

	public List<TemplateField> Fields {
		get { return _Fields; }
	}

	#endregion

	#region "Methods"

	public String TemplateDataText()
	{
		//<templateData>
		//   <componentData id=\"f0\">
		//      <data id=\"text\" value=\"Niklas P Andersson\"></data>
		//   </componentData>
		//   <componentData id=\"f1\">
		//      <data id=\"text\" value=\"developer\"></data>
		//   </componentData>
		//   <componentData id=\"f2\">
		//      <data id=\"text\" value=\"Providing an example\"></data>
		//   </componentData>
		//</templateData>

		string af = "\\" + Strings.ChrW(0x22);
		StringBuilder sb = new StringBuilder();

		sb.Append("<templateData>");

		foreach (TemplateField tf in _Fields) {
			sb.AppendFormat("<componentData id={0}{1}{0}>", af, tf.Name);
			sb.AppendFormat("<data id={0}text{0} value={0}{1}{0}></data>", af, tf.Value);
			sb.Append("</componentData>");
		}

		sb.Append("</templateData>");

		return sb.ToString();
	}

	#endregion

	#region "Shared Methods"

	public static Template Parse(string XmlText)
	{
		//<?xml version="1.0" encoding="utf-8"?>
		//<template version="1.8.0" authorName="Didi Kunz" authorEmail="didi@mediasupport.ch" templateInfo="" originalWidth="1024" originalHeight="576" originalFrameRate="50">
		//   <components>
		//      <component name="CasparTextField">
		//         <property name="text" type="string" info="String data"/>
		//      </component>
		//   </components>
		//   <keyframes/>
		//   <instances>
		//      <instance name="f0" type="CasparTextField"/>
		//   </instances>
		//</template>

		Template ti = new Template();
		XmlDocument doc = new XmlDocument();

		doc.LoadXml(XmlText.TrimEnd(Strings.ChrW(0x0)));
		XmlNode nd = doc.SelectSingleNode("template");
		if (nd.Attributes.Count > 1) {
			ti.Author = nd.Attributes["authorName"].Value;
			ti.AuthorEMail = nd.Attributes["authorEmail"].Value;
			ti.Info = nd.Attributes["templateInfo"].Value;
			ti.Width = int.Parse(nd.Attributes["originalWidth"].Value);
			ti.Height = int.Parse(nd.Attributes["originalHeight"].Value);
			ti.FrameRate = int.Parse(nd.Attributes["originalFrameRate"].Value);
		}

		nd = doc.SelectSingleNode("template/instances");
		foreach (XmlNode fld in nd.ChildNodes) {
			ti.Fields.Add(new TemplateField(fld.Attributes["name"].Value));
		}

		return ti;

	}

	#endregion

	#region "Methods"

	public override string ToString()
	{
		return string.Format("{0}, {1}x{2}, {3}", this.Name, this.Width, this.Height, this.Author);
	}

	#endregion

}
