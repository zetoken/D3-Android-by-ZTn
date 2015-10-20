using System.Linq;

namespace ZTn.Pcl.D3Calculator.ViewModels
{
    class BnetAccountEditorViewModel
    {
        public string[] HostNames => App.Hosts.Select(h => $"{h.Name} ({h.Url})").ToArray();

        public string AccountEditorTitle => Resources.Lang.AccountEditorTitle.ToUpper();

        public int HostSelectedIndex { get; set; }

        public string BattleTagString { get; set; }
    }
}
