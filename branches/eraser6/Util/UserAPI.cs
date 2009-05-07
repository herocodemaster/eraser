/* 
 * $Id$
 * Copyright 2008 The Eraser Project
 * Original Author: Joel Low <lowjoel@users.sourceforge.net>
 * Modified By:
 * 
 * This file is part of Eraser.
 * 
 * Eraser is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software
 * Foundation, either version 3 of the License, or (at your option) any later
 * version.
 * 
 * Eraser is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
 * A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * 
 * A copy of the GNU General Public License can be found at
 * <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Eraser.Util
{
	public static class UserAPI
	{
		/// <summary>
		/// Gets the current position of the system caret.
		/// </summary>
		public static Point CaretPos
		{
			get
			{
				Point result = new Point();
				if (NativeMethods.GetCaretPos(out result))
					return result;
				return Point.Empty;
			}
		}

		/// <summary>
		/// Gets the cursor position fot the last message retrieved by GetMessage.
		/// </summary>
		public static uint MessagePos
		{
			get
			{
				return NativeMethods.GetMessagePos();
			}
		}

		/// <summary>
		/// Gets the message time for the last message retrieved by GetMessage.
		/// </summary>
		public static int MessageTime
		{
			get
			{
				return NativeMethods.GetMessageTime();
			}
		}

		/// <summary>
		/// Classes, structs and constants imported from User32.dll
		/// </summary>
		internal static class NativeMethods
		{
			/// <summary>
			/// The GetCaretPos function copies the caret's position to the specified
			/// POINT structure.
			/// </summary>
			/// <param name="lpPoint">[out] Pointer to the POINT structure that is to
			/// receive the client coordinates of the caret.</param>
			/// <returns>If the function succeeds, the return value is nonzero.
			/// If the function fails, the return value is zero. To get extended error
			/// information, call Marshal.GetLastWin32Error.</returns>
			[DllImport("User32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetCaretPos(out Point lpPoint);

			/// <summary>
			/// The GetMessagePos function retrieves the cursor position for the
			/// last message retrieved by the GetMessage function.
			/// </summary>
			/// <returns>The return value specifies the x- and y-coordinates of the
			/// cursor position. The x-coordinate is the low order short and the
			/// y-coordinate is the high-order short.</returns>
			[DllImport("User32.dll")]
			public static extern uint GetMessagePos();

			/// <summary>
			/// The GetMessageTime function retrieves the message time for the last
			/// message retrieved by the GetMessage function. The time is a long
			/// integer that specifies the elapsed time, in milliseconds, from the
			/// time the system was started to the time the message was created
			/// (that is, placed in the thread's message queue).
			/// </summary>
			/// <returns>The return value specifies the message time.</returns>
			[DllImport("User32.dll")]
			public static extern int GetMessageTime();
		}
	}
}
