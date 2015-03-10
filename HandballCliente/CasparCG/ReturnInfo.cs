using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//Copyright by Media Support - Didi Kunz didi@mediasupport.ch
//The same license conditions apply as CasparCG server has.
//
[Serializable()]
public class ReturnInfo
{

	public int Number { get; set; }
	public string Message { get; set; }
	public string Data { get; set; }

	public ReturnInfo()
	{
	}

	public ReturnInfo(int Number, string Message, string Data)
	{
		this.Number = Number;
		this.Message = Message;
		this.Data = Data;
	}

}
