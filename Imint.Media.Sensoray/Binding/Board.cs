﻿// 
//  Board.cs
//  
//  Author:
//       Simon Mika <simon.mika@imint.se>
//  
//  Copyright (c) 2012-2013 Imint AB
// 
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//  the documentation and/or other materials provided with the distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using Kean.Core;
using Kean.Core.Extension;
using Collection = Kean.Core.Collection;
using Kean.Core.Collection.Extension;
using S2253 = Sensoray.S2253;

namespace Imint.Media.Sensoray.Binding
{
	public class Board :
		Collection.Abstract.ReadOnlyVector<Device>
	{
		Device[] devices;
		public override int Count { get { return this.devices.Length; } }
		public override Device this[int index]
		{
			get 
			{
				Device result = null;
				if (index < this.devices.Length && index >= 0)
				{
					result = this.devices[index];
					if (result.IsNull())
						this.devices[index] = result = Device.Open(this, index);
				}
				return result;
			}
		}
		Board(int deviceCount)
		{
			this.devices = new Device[deviceCount];
		}
		static Board board;
		public static Board Open()
		{
			try
			{
				if (Board.board.IsNull())
				{
					S2253.OpenBoard(-1);
					int deviceCount = 0;
					S2253.GetNumDevices(ref deviceCount);
					Board.board = new Board(deviceCount);
				}
			}
			catch (Exception)
			{
			}
			return Board.board;
		}
	}
}
