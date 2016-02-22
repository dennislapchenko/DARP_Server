//
//  ErrorEventForwardHandler.cs
//
//  Author:
//       Dennis Lapchenko <>
//
//  Copyright (c) 2015 Dennis Lapchenko
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
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;


namespace SubServerCommon.Handlers
{
	public class ErrorEventForwardHandler : DefaultEventHandler
	{
		public ErrorEventForwardHandler(PhotonApplication application) : base(application)
		{
		}

		public override MessageType Type {
			get {
				return MessageType.Async;
			}
		}
		
		public override byte Code {
			get {
				return (byte)(ClientOperationCode.Chat | ClientOperationCode.Region | ClientOperationCode.Login);
			}
		}

		public override int? SubCode {
			get {
				return null;
			}
		}

		protected override bool OnHandleMessage (IMessage message, PhotonServerPeer serverPeer)
		{
			Log.ErrorFormat("No existing Event Handler. Code {0}. SubCode {1}", ((ServerEventCode)message.Code).ToString(), ((MessageSubCode)message.SubCode).ToString());
			return true;
		}
	}
}

