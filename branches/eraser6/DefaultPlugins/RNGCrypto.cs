using System;
using System.Collections.Generic;
using System.Text;

using Eraser.Manager;
using System.Security.Cryptography;

namespace Eraser.DefaultPlugins
{
	public class RNGCrypto : PRNG
	{
		public override string Name
		{
			get { return "RNGCryptoServiceProvider"; }
		}

		public override Guid GUID
		{
			get { return new Guid("{6BF35B8E-F37F-476e-B6B2-9994A92C3B0C}"); }
		}

		public override void NextBytes(byte[] buffer)
		{
			lock (rand)
				rand.GetBytes(buffer);
		}

		RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
	}
}
