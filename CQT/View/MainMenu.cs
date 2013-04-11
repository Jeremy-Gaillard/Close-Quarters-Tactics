using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

using CQT.Engine;

namespace CQT.View
{
    public partial class MainMenu : Form
    {

        public MainMenu()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainMenu());
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void bStart_Click(object sender, EventArgs e)
        {
            //Begin a new game
            ServerEngine si = new ServerEngine(int.Parse(tbPort.Text), 8/*TODO : change*/);
            Game1 game = new Game1(si);
            game.Run();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bJoin_Click(object sender, EventArgs e)
        {
            // TODO : better handling of exception
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(tbIp.Text), int.Parse(tbPort.Text));
            try
            {
                ClientEngine ci = new ClientEngine(server);
                Game1 game = new Game1(ci);
                game.Run();
            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
        }
    }
}
