#region Copyright & License Information
/*
 * Copyright 2007-2010 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made 
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see LICENSE.
 */
#endregion

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System;

namespace OpenRA.FileFormats
{
	public class Palette
	{
		uint[] colors;
		public Color GetColor(int index)
		{
			return Color.FromArgb((int)colors[index]);
		}
		
		public void SetColor(int index, Color color)
		{
			colors[index] = (uint)color.ToArgb();
		}
		
		public void SetColor(int index, uint color)
		{
			colors[index] = (uint)color;
		}
				
		public uint[] Values
		{			
			get { return colors; }
		}

		public Palette(Stream s, bool remapTransparent)
		{
			colors = new uint[256];
			
			using (BinaryReader reader = new BinaryReader(s))
			{
				for (int i = 0; i < 256; i++)
				{
					byte r = (byte)(reader.ReadByte() << 2);
					byte g = (byte)(reader.ReadByte() << 2);
					byte b = (byte)(reader.ReadByte() << 2);
					colors[i] = (uint)((255 << 24) | (r << 16) | (g << 8) | b);
				}
			}

			colors[0] = 0;
			if (remapTransparent)
			{
				colors[3] = (uint)178 << 24;
				colors[4] = (uint)140 << 24;
			}
		}

		public Palette(Palette p, IPaletteRemap r)
		{
			colors = p.colors;
		}
		
		public Palette(Palette p)
		{
			colors = (uint[])p.colors.Clone();
		}
	}

	public interface IPaletteRemap { Color GetRemappedColor(Color original, int index);	}
}
