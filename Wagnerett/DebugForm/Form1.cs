using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBLayer;

namespace DebugForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Poll l = DBInterface.GetPoll("f3dc779e56f249e3b2acf666755a35a6");
            listBox1.Items.Add(l.Question);
            listBox1.Items.Add(l.Disabled);
        }
    }
}
