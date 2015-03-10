using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//Copyright by Media Support - Didi Kunz didi@mediasupport.ch
//The same license conditions apply as CasparCG server has.
//
[Serializable()]
public class TemplateField : IComparable<TemplateField>
{

	#region "Enums"

	public enum enumFieldType
	{
		ftText,
		ftInteger,
		ftNumber,
		ftBoolean,
		ftDate,
		ftTime,
		ftColor,
		ftImageFilename,
		ftVideoFilename,
		ftCustomFilename
	}

	#endregion

	#region "Vars"


	private string _Value;
	#endregion

	#region "Properties"

	public string Name { get; set; }
	public enumFieldType FieldType { get; set; }
	public string CustomFileExtension { get; set; }

	public string Value {
		get { return _Value; }
		set { _Value = value; }
	}

	#endregion

	#region "Methods"

	public int GetValueAsInteger()
	{
		return int.Parse(_Value);
	}

	public void SetValueAsInteger(int value)
	{
		_Value = value.ToString();
		FieldType = enumFieldType.ftInteger;
	}

	public bool GetValueAsBoolean()
	{
		return bool.Parse(_Value);
	}

	public void SetValueAsBoolean(bool value)
	{
		_Value = value.ToString();
		FieldType = enumFieldType.ftBoolean;
	}

	public double GetValueAsNumber()
	{
		return double.Parse(_Value);
	}

	public void SetValueAsNumber(double value)
	{
		_Value = value.ToString();
		FieldType = enumFieldType.ftNumber;
	}

	public System.DateTime GetValueAsDate()
	{
		return System.DateTime.Parse(_Value);
	}

	public void SetValueAsDate(System.DateTime value)
	{
		_Value = value.ToString("d");
		FieldType = enumFieldType.ftDate;
	}

	public System.DateTime GetValueAsTime()
	{
		return System.DateTime.Parse(_Value);
	}

	public void SetValueAsTime(System.DateTime value)
	{
		_Value = value.ToString("t");
		FieldType = enumFieldType.ftTime;
	}

	public System.Drawing.Color GetValueAsColor()
	{
		string[] v = _Value.Split('|');
		return System.Drawing.Color.FromArgb(int.Parse(v[0]), int.Parse(v[1]), int.Parse(v[2]), int.Parse(v[3]));
	}

	public void SetValueAsColor(System.Drawing.Color value)
	{
		_Value = string.Format("{0}|{1}|{2}|{3}", value.A, value.R, value.G, value.B);
		FieldType = enumFieldType.ftColor;
	}

	public string GetFileDialogPattern()
	{
		switch (this.FieldType) {
			case enumFieldType.ftImageFilename:
				return "Image-Files (*.jpg, *.png)|*.jpg;*.png|All Files (*.*)|*.*||";
			case enumFieldType.ftVideoFilename:
				return "Flash-Video-Files (*.flv)|*.flv|All Files (*.*)|*.*||";
			case enumFieldType.ftCustomFilename:
				return string.Format("{0}-Files (*.{0})|*.{0}|All Files (*.*)|*.*||", this.CustomFileExtension);
			default:
				return "";
		}
	}

	public override string ToString()
	{
		return string.Format("{0} = '{1}'", this.Name, this.Value);
	}

	public int CompareTo(TemplateField other)
	{
		return Name.CompareTo(other.Name);
	}

	#endregion

	#region "Constructors"


	public TemplateField(string Name)
	{

		if (Name.ToUpper().EndsWith("_I")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftInteger;

		} else if (Name.ToUpper().EndsWith("_N")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftNumber;

		} else if (Name.ToUpper().EndsWith("_B")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftBoolean;

		} else if (Name.ToUpper().EndsWith("_D")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftDate;

		} else if (Name.ToUpper().EndsWith("_T")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftTime;

		} else if (Name.ToUpper().EndsWith("_C")) {
			this.Name = Name.Substring(1, Name.Length - 2);
			this.FieldType = enumFieldType.ftColor;

		} else if (Name.ToUpper().EndsWith("_FI")) {
			this.Name = Name.Substring(1, Name.Length - 3);
			this.FieldType = enumFieldType.ftImageFilename;

		} else if (Name.ToUpper().EndsWith("_FV")) {
			this.Name = Name.Substring(1, Name.Length - 3);
			this.FieldType = enumFieldType.ftVideoFilename;

		} else if (Name.ToUpper().EndsWith("_FC")) {
			this.Name = Name.Substring(1, Name.Length - 6).TrimEnd('_');
			this.FieldType = enumFieldType.ftCustomFilename;
			this.CustomFileExtension = Name.Substring(Name.Length - 6, 3);

		} else {
			this.Name = Name;

		}

		this.Value = "";

	}

	public TemplateField(string Name, string Value) : this(Name)
	{
		_Value = Value;
	}

	#endregion

}
