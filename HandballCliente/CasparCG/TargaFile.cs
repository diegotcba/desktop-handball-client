using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//Copyright by Media Support - Didi Kunz didi@mediasupport.ch
//The same license conditions apply as CasparCG server has.
//
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class TargaFile
{

	//' TGA-Header for information only (remarks in German)
	//Private Structure TgaHeader
	//   ' Größe des Identblocks in Byte, der nach dem Header (18 Byte) folgt. 
	//   ' Normalerweise 0
	//   Dim IdentSize As Byte
	//   ' Palettentyp: 0 = Keine Palette vorhanden, 1 = Palette vorhanden
	//   Dim ColorMapType As Byte
	//   ' Bildtyp: 0 = none, 1 = Indexed, 2 = RGB, 3 = Grauskale, 
	//   ' 9 = Indexed (RLE), 10 = RGB (RLE), 11 = Grauskale (RLE)
	//   Dim ImageType As Byte
	//   ' erster Eintrag in der Farbtabelle
	//   Dim ColorMapStart As Short
	//   ' Anzahl der Farben in der Farbpalette
	//   Dim ColorMapLength As Short
	//   ' Bits Per Pixel der Farbtabelle 15, 16, 24, 32
	//   Dim ColorMapBits As Byte
	//   ' X-Position des Bildes in Pixel. Normalerweise 0
	//   Dim xStart As Short
	//   ' Y-Position des Bildes in Pixel. Normalerweise 0
	//   Dim yStart As Short
	//   Dim Width As Short          ' Breite des Bildes in Pixel
	//   Dim Height As Short         ' Höhe des Bildes in Pixel
	//   Dim Bits As Byte            ' Bits Per Pixel des Bildes 8, 16, 24, 32
	//   Dim Descriptor As Byte      ' Descriptor bits des Bildes
	//End Structure


	public static void SaveAsTarga(string Filename, Bitmap Picture)
	{
		if (Picture.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb) {
			throw new Exception("Must be a 32-Bit Image");
			return;
		}

		if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Filename))) {
			throw new Exception("Path to save file does not exist");
			return;
		}

		//System.Drawing.Bitmap have there pixels arranged from top to bottom, TGA's from bottom to top, so we flip the picture
		Picture.RotateFlip(RotateFlipType.RotateNoneFlipY);

		using (FileStream FS = new FileStream(Filename, FileMode.Create, FileAccess.Write)) {

			BinaryWriter bw = new BinaryWriter(FS);

			//Writing the Header
			int sh = 0;

			bw.Write(Convert.ToByte(0));
			//IdentSize
			bw.Write(Convert.ToByte(0));
			//ColorMapType
			bw.Write(Convert.ToByte(2));
			//ImageType

			bw.Write(sh);
			//ColorMapStart
			bw.Write(sh);
			//ColorMapLength
			bw.Write(Convert.ToByte(0));
			//ColorMapBits

			bw.Write(sh);
			//xStart
			bw.Write(sh);
			//yStart

			sh = Picture.Width;
			bw.Write(sh);
			//Width
			sh = Picture.Height;
			bw.Write(sh);
			//Height

			bw.Write(Convert.ToByte(32));
			//Bits Per Pixel
			bw.Write(Convert.ToByte(8));
			//Descriptor

			//Looking and writing the Bitmap
			BitmapData bmpData = Picture.LockBits(new Rectangle(0, 0, Picture.Width, Picture.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			IntPtr ptr = bmpData.Scan0;
			int bytes = bmpData.Stride * Picture.Height;
			byte[] rgbValues = new byte[bytes];

			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
			bw.Write(rgbValues);

			Picture.UnlockBits(bmpData);

			//Writing the Footer
			Int32 ln = 0;
			bw.Write(ln);
			bw.Write(ln);

			bw.Write("TRUEVISION-XFILE.");
			bw.Write(Convert.ToByte(0));

			//Clean Up
			bw.Flush();
			FS.Flush();
			bw.Close();
			FS.Close();

		}

	}

}
