using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CQT.View
{
    public partial class MainMenu : Form
    {

        public MainMenu()
        {
            InitializeComponent();
            System.Console.Write("Hello world");
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainMenu());
        }

        private void Form_Load(object sender, EventArgs e)
        {
            System.Console.Write("Hello world");
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            //Begin a new game
            Game1 game = new Game1();
            game.Run();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
