using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZTn.BNet.BattleNet;
using ZTn.BNet.D3.Helpers;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    class BnetAccountEditorViewModel
    {
        public string[] HostNames => App.Hosts.Select(h => $"{h.Name} ({h.Url})").ToArray();

        public int HostSelectedIndex { get; set; }

        public string BattleTagString { get; set; }
    }
}
