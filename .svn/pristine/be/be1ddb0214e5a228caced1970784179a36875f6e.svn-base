using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TspAntColony
{
    public partial class DebugWindow : Form
    {
        private static readonly DebugWindow _instance = new DebugWindow();

        public static DebugWindow Instance
        {
            get { return _instance; }
        }

        public DebugWindow()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            DebugList.Items.Clear();
        }

        public void AddItem(string text)
        {
            if (DebugList.Items.Count > 1000)
            {
                DebugList.Items.Remove(0);
            }
            DebugList.Items.Add(text);
            DebugList.SetSelected(DebugList.Items.Count - 1, true);
        }
    }
}
