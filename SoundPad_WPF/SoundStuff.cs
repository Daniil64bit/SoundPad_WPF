using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundPad_WPF
{
    internal class SoundStuff : Grid
    {
        private int myID;
        private static int _ID = 0;
        private string _link = "";
        private Key key;
        public static int ID { get => _ID; set => _ID = value; }
        public int MyID { get => myID; set => myID = value; }
        public string Link { get => _link; set => _link = value; }
        public Key Key { get => key; set => key = value; }

        public SoundStuff()
        {

        }

        public void IDUp()
        {
            myID = _ID;
            _ID++;
        }
    }
}
