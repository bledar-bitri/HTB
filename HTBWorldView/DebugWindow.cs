using System;
using System.Windows.Forms;

namespace HTBWorldView
{
    public partial class DebugWindow
    {
        public DebugWindow()
        {
            InitializeComponent();
        }

        static DebugWindow Instance__instance = new DebugWindow();
        public static DebugWindow Instance
        {
            get
            {
                return Instance__instance;
            }
        }

        public void AddItem(string text)
        {
            if (DebugList.Items.Count > 1000)
            {
                DebugList.Items.RemoveAt(0);
            }
            DebugList.Items.Add(text);
            DebugList.SetSelected(DebugList.Items.Count - 1, true);
        }

        public void Clear()
        {
            DebugList.Items.Clear();
        }

        public void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        public void DebugWindow_Load(object sender, EventArgs e)
        {
            DebugList.UseCustomTabOffsets = true;
        }

        private void DebugList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
