﻿using System;
using System.IO;

namespace Android.Runtime
{
	public class InputStreamInvoker : Stream
	{
		public Java.IO.InputStream BaseInputStream {get; private set;}

		public InputStreamInvoker (Java.IO.InputStream stream)
		{
			this.BaseInputStream = stream;
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing && BaseInputStream != null) {
				BaseInputStream.Dispose ();
				BaseInputStream = null;
			}
		}

		public override void Flush ()
		{
			// No need to flush an input stream
		}

		public override int Read (byte[] buffer, int offset, int count)
		{
			int res = BaseInputStream.Read (buffer, offset, count);
			if (res == -1)
				return 0;
			return res;
		}

		// somewhat aggressive implementation
		public override long Seek (long offset, SeekOrigin origin)
		{
			long currentAvailable;
			switch (origin) {
			case SeekOrigin.Begin:
				BaseInputStream.Reset ();
				BaseInputStream.Skip (offset);
				return offset;
			case SeekOrigin.Current:
				long currentPosition = Position;
				BaseInputStream.Reset ();
				BaseInputStream.Skip (currentPosition + offset);
				return currentPosition + offset;
			case SeekOrigin.End:
				BaseInputStream.Reset ();
				long ret = BaseInputStream.Available () + offset;
				BaseInputStream.Skip (ret);
				return ret;
			}
			throw new NotSupportedException ($"Unexpected SeekOrigin: {(int) origin}");
		}

		public override void SetLength (long value)
		{
			throw new NotSupportedException ();
		}

		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException ();
		}

		public override bool CanRead { get { return true; } }

		// somewhat aggressive implementation
		public override bool CanSeek {
			get {
				try {
					BaseInputStream.Skip (0);
					return true;
				} catch {
					return false;
				}
			}
		}
		
		public override bool CanWrite { get { return false; } }

		// somewhat aggressive implementation
		public override long Length {
			get {
				long currentAvailable = BaseInputStream.Available ();
				BaseInputStream.Reset ();
				long length = BaseInputStream.Available ();
				long currentPosition = length - currentAvailable;
				BaseInputStream.Skip (currentPosition);
				return length;
			}
		}

		// somewhat aggressive implementation
		public override long Position {
			get {
				long currentAvailable = BaseInputStream.Available ();
				BaseInputStream.Reset ();
				long length = BaseInputStream.Available ();
				long currentPosition = length - currentAvailable;
				BaseInputStream.Skip (currentPosition);
				return currentPosition;
			}
			set {
				int currentAvailable = BaseInputStream.Available ();
				BaseInputStream.Reset ();
				BaseInputStream.Skip (value);
			}
		}
		
		[Preserve (Conditional=true)]
		public static Stream FromJniHandle (IntPtr handle, JniHandleOwnership transfer)
		{
			if (handle == IntPtr.Zero)
				return null;

			IJavaObject inst = Java.Lang.Object.PeekObject (handle);

			if (inst == null)
				inst = Java.Interop.TypeManager.CreateInstance (handle, transfer);
			else
				JNIEnv.DeleteRef (handle, transfer);

			return new InputStreamInvoker ((Java.IO.InputStream)inst);
		}
	}
}

