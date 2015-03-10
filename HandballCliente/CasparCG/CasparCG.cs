using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//Copyright by Media Support - Didi Kunz didi@mediasupport.ch
//The same license conditions apply as CasparCG server has.
//
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;

[Serializable()]
public class CasparCG
{

	public enum MediaTypes
	{
		All,
		Movie,
		Still,
		Audio
	}

	#region "Vars"


	private Socket _Caspar = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private string _ServerAdress = "localhost";

	private int _Port = 5250;

	private bool _KeepQuiet = false;
	private string _CasparExePath = "";

	private int _Retries = 1;
	private int _Version = 0;

	private Paths _ServerPaths;
	#endregion

	#region "Properties"

	public string Name { get; set; }

	public string ServerAdress {
		get { return _ServerAdress; }
		set { _ServerAdress = value.Trim(); }
	}

	public int Port {
		get { return _Port; }
		set { _Port = value; }
	}

	public bool KeepQuiet {
		get { return _KeepQuiet; }
		set { _KeepQuiet = value; }
	}

	public string CasparExePath {
		get { return _CasparExePath; }
		set { _CasparExePath = value; }
	}

	public int Retries {
		get { return _Retries; }
		set { _Retries = value; }
	}

	public bool Connected {
		get { return _Caspar.Connected; }
	}

	public int Version {
		get {
			if (_Version == 0) {
                String[] s = Execute("VERSION SERVER").Data.Split('.');
				_Version = (Convert.ToInt32(s[0]) * 10000) + (Convert.ToInt32(s[1]) * 100) + Convert.ToInt32(s[2]);
			}
			return _Version;
		}
	}

	[XmlRoot("paths")]
	public class Paths
	{

		[XmlElement(ElementName = "media-path")]
		public String MediaPath { get; set; }

		[XmlElement(ElementName = "log-path")]
		public String LogPath { get; set; }

		[XmlElement(ElementName = "data-path")]
		public String DataPath { get; set; }

		[XmlElement(ElementName = "template-path")]
		public String TemplatePath { get; set; }

		[XmlElement(ElementName = "initial-path")]
		public String InitialPath { get; set; }

	}

	public Paths ServerPaths {

		get {

            GetServerPaths();
			return _ServerPaths;
		}
	}

    private void GetServerPaths()
    {
        if (_ServerPaths == null)
        {
            string s = Execute("INFO PATHS").Data;
            byte[] b = Encoding.UTF8.GetBytes(Strings.Left(s, Strings.InStrRev(s, ">")));
            XmlSerializer ser = new XmlSerializer(typeof(Paths));
            _ServerPaths = (Paths)ser.Deserialize(new System.IO.MemoryStream(b));
        }
    }

	#endregion

	#region "Private Procs"

	#endregion

	#region "Methods"

	public ReturnInfo Execute(string Command)
	{


		if (_Caspar.Connected) {
			byte[] cmd = Encoding.UTF8.GetBytes(Command + Constants.vbCrLf);
			byte[] bytes = new byte[4 * 4096 + 1];
			try {
				// Blocks until send returns.
				int i = _Caspar.Send(cmd);

				// Get reply from the server.
				i = _Caspar.Receive(bytes, SocketFlags.None);
				string s = Encoding.UTF8.GetString(bytes);

				ReturnInfo ri = new ReturnInfo();

				if (Information.IsNumeric(Strings.Left(s, 3))) {
					ri.Number = int.Parse(Strings.Left(s, 3));
					int c = s.IndexOf(Constants.vbCrLf);
					ri.Message = s.Substring(4, c - 2).Trim();
					int d = s.IndexOf(Constants.vbCrLf, c + 1);
					if (d > 0) {
						ri.Data = s.Substring(c + 2).Trim();
						//ri.Data = s.Substring(c + 2, d - c - 2).Trim
					}
					return ri;
				} else {
					ri.Number = 0;
					ri.Message = "Data returned";
					ri.Data = s;
				}

				return ri;

			} catch (SocketException ex) {
				return new ReturnInfo(0, string.Format("{0} Error code: {1}.", ex.Message, ex.ErrorCode), "");
			}

		} else {
			return new ReturnInfo(0, "Not connected to CasparCG", "");
		}

	}

	public void Connect()
	{
		try {
			_Caspar.Connect(_ServerAdress, _Port);
            GetServerPaths();
		} catch (Exception ex) {

			if (_Retries > 1) {
				Process myProc = new Process();
				myProc.StartInfo.FileName = _CasparExePath;
				myProc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(_CasparExePath);
				myProc.StartInfo.CreateNoWindow = false;
				myProc.Start();

				int t = 0;
				do {
					try {
						_Caspar.Connect(_ServerAdress, _Port);
					} catch (Exception exp) {
					}
					if (_Caspar.Connected) {
						break; // TODO: might not be correct. Was : Exit Do
					} else {
						if (t > _Retries) {
							if (!_KeepQuiet) {
								throw new Exception("Could not start CasparCG-Server.");
							}
						} else {
							System.Threading.Thread.Sleep(1000);
							t += 1;
						}
					}
				} while (true);
			} else {
				if (!_KeepQuiet) {
					throw new Exception("CasparCG-Server has not been started.");
				}
			}
		}
	}

	public void Connect(string ServerAdress)
	{
		_ServerAdress = ServerAdress;
		this.Connect();
	}

	public void Connect(string ServerAdress, int Port)
	{
		_ServerAdress = ServerAdress;
		_Port = Port;
		this.Connect();
	}

	public void Connect(string ServerAdress, bool KeepQuiet)
	{
		_ServerAdress = ServerAdress;
		_KeepQuiet = KeepQuiet;
		this.Connect();
	}

	public void Connect(int Retries, string CasparExePath)
	{
		if (this.ServerAdress.ToLower() == "localhost" | this.ServerAdress == "127.0.0.1") {
			_CasparExePath = CasparExePath;
			_Retries = Retries;
			this.Connect();
		} else {
			throw new Exception("Not a local CasparCG-Server.");
		}
	}

	public void Disconnect()
	{
		if (_Caspar.Connected) {
			//If Not System.Diagnostics.Debugger.IsAttached Then
			//   Execute("KILL")
			//End If
			_Caspar.Disconnect(true);
		}
	}

	public List<string> GetMediaClipsNames()
	{
		return GetMediaClipsNames(MediaTypes.All);
	}

	public List<string> GetMediaClipsNames(MediaTypes ShowMedia)
	{

		List<string> lst = new List<string>();
        string[] Ret = Execute("CLS").Data.Split("\r\n".ToCharArray());
		int c = 1;
		int d = 0;

		foreach (string s in Ret) {
			if (s.Contains(Strings.Chr(0x22).ToString())) {
				c = s.IndexOf(Strings.Chr(0x22));
				d = s.IndexOf(Strings.Chr(0x22), 2);
				string clip = s.Substring(c + 1, d - (c + 1)).Replace("\\", "/");

				if (ShowMedia == MediaTypes.All) {
					lst.Add(clip);
				} else {
					if (ShowMedia == MediaTypes.Movie & s.Substring(d + 3, 5) == "MOVIE") {
						lst.Add(clip);
					}
					if (ShowMedia == MediaTypes.Still & s.Substring(d + 3, 5) == "STILL") {
						lst.Add(clip);
					}
					if (ShowMedia == MediaTypes.Audio & s.Substring(d + 3, 5) == "AUDIO") {
						lst.Add(clip);
					}
				}
			}
		}
		return lst;

	}

	//Public Class ClipInfo

	//End Class

	//Public Function GetMediaClipInfo(Mediafile As String) As ClipInfo
	//   Dim s As String = Execute(String.Format("CINF {0}", Mediafile)).Data

	//   Return New ClipInfo
	//End Function

	public List<string> GetTemplateNames()
	{

		List<string> lst = new List<string>();
        string[] Ret = Execute("TLS").Data.Split("\r\n".ToCharArray());

		foreach (string s in Ret) {
            if (s.Contains(Strings.Chr(0x22).ToString()))
            {
//				string tmpl = s.Substring(1, s.IndexOf(Strings.Chr(0x22), 2) - 1).Replace("\\", "/").Replace(Strings.Chr(0x22).ToString(), string.Empty);
                string tmpl = s.Substring(1, s.IndexOf(Strings.Chr(0x22), 2) - 1).Replace("\\", "/");
				lst.Add(tmpl);
			}
		}
		return lst;

	}

	public Template GetTemplate(string Name)
	{

		string info = Execute(string.Format("INFO TEMPLATE {0}", Name)).Data;
		Template tmpl = null;

		if (info != null) {
			tmpl = Template.Parse(info);
		} else {
			tmpl = new Template();
		}

		tmpl.Name = Name;
		return tmpl;

	}

	public List<Template> GetTemplates()
	{

		List<Template> tl = new List<Template>();
		List<string> lst = this.GetTemplateNames();

		foreach (string s in lst) {
			if (!string.IsNullOrEmpty(s)) {
				tl.Add(this.GetTemplate(s));
			}
		}

		return tl;

	}

	public Bitmap Grab(int channel)
	{
		return Grab(channel, -1);
	}

	public Bitmap Grab(int channel, int layer)
	{
		Bitmap functionReturnValue = null;

		if (this.ServerAdress.ToLower() != "localhost") {
			throw new Exception("The Grab-Function only works for local CasparCG-Server (localhost)");
			return null;
			return functionReturnValue;
		}

		//Server 2.0.4 Stable: Breaking change of PRINT command, use ADD IMAGE instead.
		if (Version >= 20004) {

			string PicPath = ServerPaths.MediaPath;

			if (PicPath.EndsWith("\\")) {
				PicPath = PicPath.Substring(0, PicPath.Length - 1);
			}

			if (!(PicPath.Substring(1, 1) == ":")) {
				throw new Exception("The Grab-Function needs to have absolute paths configured in CasparCG.config.");
				return null;
				return functionReturnValue;
			}

			//Delete old files once a day
			string delFn = string.Format("~{0:yyyyMMdd}.del", System.DateTime.Now);

			if (!System.IO.File.Exists(System.IO.Path.Combine(PicPath, delFn))) {
				string[] fn = System.IO.Directory.GetFiles(PicPath, "~*.del");
				for (int c = 0; c <= fn.Length - 1; c++) {
					System.IO.File.Delete(System.IO.Path.Combine(PicPath, fn[c]));
				}

				fn = System.IO.Directory.GetFiles(PicPath, "~GRAB_*.png");
				for (int c = 0; c <= fn.Length - 1; c++) {
					System.IO.File.Delete(System.IO.Path.Combine(PicPath, fn[c]));

					//If Math.Abs(DateDiff(DateInterval.Day, IO.File.GetCreationTime(IO.Path.Combine(PicPath, fn(c))), Date.Now)) > 1 Then
					//   IO.File.Delete(IO.Path.Combine(PicPath, fn(c)))
					//End If
				}

				System.IO.File.Create(System.IO.Path.Combine(PicPath, delFn));

			}

			System.Threading.Thread.Sleep(2000);

			int imgNum = 1;
			do {
				if (!System.IO.File.Exists(System.IO.Path.Combine(PicPath, string.Format("~GRAB_{0:00000}.png", imgNum))))
					break; // TODO: might not be correct. Was : Exit Do
				imgNum += 1;
			} while (true);

			ReturnInfo ri = default(ReturnInfo);
			if (layer == -1) {
				ri = Execute(string.Format("ADD {0} IMAGE ~GRAB_{1:00000}", channel, imgNum));
			} else {
				ri = Execute(string.Format("ADD {0}-{1}} IMAGE ~GRAB_{2:00000}", channel, layer, imgNum));
			}


			if (ri.Number == 202) {
				if (layer == -1) {
					ri = Execute(string.Format("REMOVE {0} IMAGE", channel));
				} else {
					ri = Execute(string.Format("REMOVE {0}-{1}} IMAGE", channel, layer));
				}

				string fn = System.IO.Path.Combine(PicPath, string.Format("~GRAB_{0:00000}.png", imgNum));
				do {
					System.Threading.Thread.Sleep(1000);
					if (System.IO.File.Exists(fn)) {
						break; // TODO: might not be correct. Was : Exit Do
					}
				} while (true);

				System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fn);

				return bm;

			} else {
				return null;
			}



		} else {
			string PicPath = ServerPaths.DataPath;

			if (PicPath.EndsWith("\\")) {
				PicPath = PicPath.Substring(0, PicPath.Length - 1);
			}

			if (!(PicPath.Substring(2, 1) == ":")) {
				throw new Exception("The Grab-Function needs to have absolute paths configured in CasparCG.config.");
				return null;
				return functionReturnValue;
			}

			string[] fn = null;
			fn = System.IO.Directory.GetFiles(PicPath, "*.png");
			for (int c = 0; c <= fn.Length - 1; c++) {
				System.IO.File.Delete(fn[c]);
			}

			System.Threading.Thread.Sleep(1000);

			ReturnInfo ri = default(ReturnInfo);
			if (layer == -1) {
				ri = Execute(string.Format("PRINT {0}", channel));
			} else {
				ri = Execute(string.Format("PRINT {0}-{1}", channel, layer));
			}


			if (ri.Number == 202) {
				do {
					System.Threading.Thread.Sleep(1000);
					fn = System.IO.Directory.GetFiles(PicPath, "*.png");
					if (fn.Length > 0) {
						break; // TODO: might not be correct. Was : Exit Do
					}
				} while (true);

				System.Drawing.Bitmap bm = new System.Drawing.Bitmap(fn[0]);

				return bm;

			} else {
				return null;
			}
		}
		return functionReturnValue;

	}


	public class LayerStatus
	{
		public string FilePlaying { get; set; }
		public string Status { get; set; }
		public long FrameNumber { get; set; }
		public long TotalFrames { get; set; }
		public bool BackGroundIsEmpty { get; set; }
	}

	public LayerStatus GetLayerStatus(string Layer)
	{

		XmlDocument doc = new XmlDocument();
		LayerStatus ls = new LayerStatus();

		ReturnInfo ri = Execute(string.Format("INFO {0}", Layer));
		doc.LoadXml(ri.Data.Substring(0, ri.Data.LastIndexOf(">") + 1));

		XmlNodeList nl = doc.SelectNodes("/layer/foreground/producer/filename");
		if (nl.Count > 0) {
			ls.FilePlaying = nl[0].FirstChild.Value;
		}

		nl = doc.SelectNodes("/layer/status");
		ls.Status = nl[0].FirstChild.Value;

		nl = doc.SelectNodes("/layer/frame-number");
		ls.FrameNumber = long.Parse(nl[0].FirstChild.Value);

		nl = doc.SelectNodes("/layer/nb_frames");
		ls.TotalFrames = long.Parse(nl[0].FirstChild.Value);

		nl = doc.SelectNodes("/layer/background/destination/producer/type");
		if (nl.Count == 0) {
			ls.BackGroundIsEmpty = true;
		} else {
			ls.BackGroundIsEmpty = (nl[0].FirstChild.Value == "empty-producer");
		}

		return ls;

	}

	public string SerializeToString()
	{

		XmlSerializer ser = new XmlSerializer(typeof(CasparCG));
		System.IO.StringWriter sw = new System.IO.StringWriter();
		ser.Serialize(sw, this);

		return sw.ToString();

	}

	#endregion

	#region "Contructors"


	public CasparCG()
	{
	}


	public CasparCG(string Serialized)
	{
		byte[] b = Encoding.UTF8.GetBytes(Serialized);
		XmlSerializer ser = new XmlSerializer(typeof(CasparCG));
		CasparCG ccg =(CasparCG) ser.Deserialize(new System.IO.MemoryStream(b));

		this.Name = ccg.Name;
		this.ServerAdress = ccg.ServerAdress;
		this.Port = ccg.Port;
		this.KeepQuiet = ccg.KeepQuiet;
		this.CasparExePath = ccg.CasparExePath;
		this.Retries = ccg.Retries;

		this.Connect();

	}

	#endregion

}
