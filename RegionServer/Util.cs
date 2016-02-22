//
//  Util.cs
//
//  Author:
//       Dennis Lapchenko <>
//
//  Copyright (c) 2016 Dennis Lapchenko
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using RegionServer.Model.Interfaces;


namespace RegionServer
{
	public class Util
	{
		public static bool IsInShortRange(int distanceToForgetObject, IObject obj1, IObject obj2, bool includeZAxis)
		{
			if(obj1 == null || obj2 == null)
			{
				return false;
			}

			if(distanceToForgetObject == -1)
			{
				return false;
			}

			float dx = obj1.Position.X - obj2.Position.X;
			float dy = obj1.Position.Y - obj2.Position.Y;
			float dz = obj1.Position.Z - obj2.Position.Z;

			return ((dx*dx) + (dy*dy) + (includeZAxis ? (dz*dz) : 0 )) <= distanceToForgetObject * distanceToForgetObject;

		}
	}
}

