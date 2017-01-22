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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

        }


        private void StartTsp()
        {
            Invoke(new Action(DebugWindow.Instance.Clear));

            var w = m_Map.ConstructTsp();
            w.Update += World_Update;
            dynamic best_tour = w.FindTour();
            Invoke(new Action<IEnumerable<AntColonyTSP.City>>(m_Map.DrawBestTour), best_tour);
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
    }
}

