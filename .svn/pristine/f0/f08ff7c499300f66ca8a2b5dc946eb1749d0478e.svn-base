using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HTBAntColonyTSP;
using Microsoft.VisualBasic;

namespace HTBWorldView
{
    public partial class MainForm : Form
    {
        private Map m_Map;
        private System.Threading.Thread m_TspThread;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MapPicture_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (m_Map.FindCity(e.Location) == null)
                    {
                        m_Map.AddCity(e.Location);
                    }
                    break;
                case MouseButtons.Right:
                    m_Map.RemoveCity(e.Location);
                    break;
            }

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StopTsp();

            if (m_Map.CityCount < 4)
            {
                Interaction.MsgBox("At least 4 cities are needed.", MsgBoxStyle.Information, "Error");
                return;
            }

            StopButton.Enabled = true;
            StartButton.Enabled = false;
            m_TspThread = new System.Threading.Thread(StartTsp);
            m_TspThread.IsBackground = true;
            m_TspThread.Start();

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopTsp();
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            StopTsp();
            m_Map.Clear();
        }

        private void ShowLabelsCheck_CheckedChanged(object sender, EventArgs e)
        {
            m_Map.ShowLabels = ShowLabelsCheck.Checked;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTsp();
        }

        private void ShowDebugCheck_CheckedChanged(object sender, EventArgs e)
        {
            DebugWindow.Instance.Visible = ShowDebugCheck.Checked;
        }

        private void StartTsp()
        {
            Invoke(new Action(DebugWindow.Instance.Clear));

            var w = m_Map.ConstructTsp();
            w.Update += World_Update;
            var best_tour = w.FindTour(-1, false);
            Invoke(new Action<IEnumerable<TspCity>>(m_Map.DrawBestTour), best_tour);
            Invoke(new Action(StopTsp));
        }

        private void StopTsp()
        {
            StopButton.Enabled = false;
            if (m_TspThread != null && m_TspThread.IsAlive)
            {
                m_TspThread.Abort();
            }
            StartButton.Enabled = true;
        }

        private void World_Update(World sender, UpdateEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<World, UpdateEventArgs>(World_Update), sender, e);
                System.Threading.Thread.Sleep(100);
                return;
            }

            m_Map.Redraw(sender, e);
            DebugWindow.Instance.AddItem(e.ToString());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
//            OpenPictureDialog.Filter = SupportedPictureFilters()
//            StopButton.Enabled = False
            m_Map = new Map(MapPicture);
        }
    }
}
