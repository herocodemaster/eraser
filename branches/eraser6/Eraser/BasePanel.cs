using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Eraser
{
	public partial class BasePanel : UserControl
	{
		public BasePanel()
		{
			InitializeComponent();
			Dock = DockStyle.Fill;
		}
	}
}
